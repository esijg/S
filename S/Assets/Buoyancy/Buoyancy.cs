using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (Rigidbody))]
public class Buoyancy : MonoBehaviour {
	
	
	public float density = .6f;
	public Vector3 cg;// = new Vector3(0,0,0);
	
	/*
	//global water variables. Edit to change physics properties of water. Part of waterplane instead?
	static float waterLevel = 0;//where the waterplane is located in world y
	static float waterDensity = 1f;//
	static float waterDrag = 1.5f;
	static float waterAngularDrag = 1f;
	*/
	
	private Vector3 c;//centroid for the submerged volume
	private float lSqr;//Approx square length of polyhedron
	private Vector3 I; //
	//private Mesh mesh;
	private Vector3[] verts;
	private int[] tris;
	private int triCount;
	private int vertCount;
	private float meshVolume;
	
	private int layerMask = 1 << 4;
	
	private float drag;
	private float angularDrag;
	 
	void OnEnable () {
		cg = Vector3.zero;
		/*
		//DEV
		rigidbody.AddTorque(Random.insideUnitSphere*10,ForceMode.VelocityChange);
		//
		*/
		try{
		rigidbody.SetDensity(density);
		}
		catch(System.Exception e){}
		rigidbody.centerOfMass = cg;
		
		lSqr = transform.localScale.magnitude * transform.localScale.magnitude;
		
		//Bounds b = ((Collider)GetComponent(typeof (Collider))).bounds;
		//lSqr = (b.size.x+b.size.y+b.size.z)/3f;
		//lSqr *= lSqr;
		
		Mesh mesh = ((MeshFilter)GetComponent(typeof (MeshFilter))).mesh;
		verts = mesh.vertices;
		tris = mesh.triangles;
		triCount = tris.Length/3;
		vertCount = verts.Length;
		meshVolume = ComputeVolume();
		
		I = rigidbody.mass/12f*Vector3.one;//(1.0f*body.mass/12.0f)*Vec3(1.0f, 1.0f, 1.0f);//body.I approximation
		
		drag = rigidbody.drag;
		angularDrag = rigidbody.angularDrag;
	}
	
	//TESTING
	//this enables you to change the cg variable for the object in the inspector to test the effect of changing center of gravity
	void Update(){
		SetCG(cg);	
	}
	//END TESTING - you probably want to remove this block eventualy
	
	void FixedUpdate () {
		ComputeBuoyancy();
	}
	
	public void SetCG(Vector3 _cg){
		rigidbody.centerOfMass = _cg;
	}
	
	//Buoyancy calculation
	
	// Returns the volume of a tetrahedron and updates the centroid accumulator.
	//static float TetrahedronVolume(Vector3& c, Vector3 p, Vector3 v1, Vector3 v2, Vector3 v3){
	private float TetrahedronVolume(Vector3 p, Vector3 v1, Vector3 v2, Vector3 v3){
		Vector3 a = v2 - v1;
		Vector3 b = v3 - v1;
		Vector3 r = p - v1;

		//float volume = (1.0f/6.0f)*(b % a) * r;
		float volume = (1f/6f)*Vector3.Dot(Vector3.Cross(b,a),r);
		c += .25f*volume*(v1 + v2 + v3 + p);
		return volume;
	}
	// Clips a partially submerged triangle and returns the volume of the resulting tetrahedrons and updates the centroid accumulator.
	// v's are vertices of a face and d's are depth of those vertices below the watersurface
	private float ClipTriangle(Vector3 p,Vector3 v1, Vector3 v2, Vector3 v3,float d1, float d2, float d3){
		//assert(d1*d2 < 0);
		
		Vector3 vc1 = v1 + (d1/(d1 - d2))*(v2 - v1);
		float volume = 0;

		if (d1 < 0){
			if (d3 < 0){
				// Case B - a quadrilateral or two triangles.
				Vector3 vc2 = v2 + (d2/(d2 - d3))*(v3 - v2);
				volume += TetrahedronVolume(p, vc1, vc2, v1);
				volume += TetrahedronVolume(p, vc2, v3, v1);
			} else {
				// Case A - a single triangle.
				Vector3 vc2 = v1 + (d1/(d1 - d3))*(v3 - v1);
				volume += TetrahedronVolume(p, vc1, vc2, v1);
			}
		} else {
			if (d3 < 0) {
				// Case B
				Vector3 vc2 = v1 + (d1/(d1 - d3))*(v3 - v1);
				volume += TetrahedronVolume(p, vc1, v2, v3);
				volume += TetrahedronVolume(p, vc1, v3, vc2);
			} else {
				// Case A
				Vector3 vc2 = v2 + (d2/(d2 - d3))*(v3 - v2);
				volume += TetrahedronVolume(p, vc1, v2, vc2);
			}
		}

		return volume;
	}
	
	// Computes the submerged volume and center of buoyancy of a polyhedron with the water surface defined as a value on world y axis (was plane).
	private float SubmergedVolume() {
		// Transform the plane into the polyhedron frame.(We do opposite and transfrom each vertex into world space for simplicity)
		
		/*
		Quaternion qt = q.Conjugate();
		Vec3 normal = qt.Rotate(plane.normal);
		float offset = plane.offset - plane.normal*x;
		*/
		
		// Compute the vertex heights relative to the surface.
		float TINY_DEPTH = -1e-6f;
		float[] ds = new float[vertCount];
		
		// Compute the depth of each vertex.
		int numSubmerged = 0;
		int sampleVert = 0;
		for (int i = 0; i < vertCount; ++i) {
			//ds[i] = normal*poly.verts[i] - offset;
			ds[i] = transform.TransformPoint(verts[i]).y - WaterPlane.instance.transform.position.y;
			if (ds[i] < TINY_DEPTH) {
				++numSubmerged;
				sampleVert = i;
				//Debug.DrawLine(Vector3.zero, transform.TransformPoint(verts[i]), Color.white);
			}
		}
		
		// Return early if no vertices are submerged
		if (numSubmerged == 0) {
			c = Vector3.zero;//c.SetZero();
			//ds = null;//delete [] ds;
			return 0;
		}
		
		// Find a point on the water surface. Project a submerged point to
		// get improved accuracy. This point serves as the point of origin for
		// computing all the tetrahedron volumes. Since this point is on the
		// surface, all of the surface faces get zero volume tetrahedrons. This
		// way the surface polygon does not need to be considered.
		Vector3 p = verts[sampleVert];// - ds[sampleVert]*Vector3.up;//Vec3 p = poly.verts[sampleVert] - ds[sampleVert]*normal;
		p.y = WaterPlane.instance.transform.position.y;
		
		// Initialize volume and centroid accumulators.
		float volume = 0;
		c = Vector3.zero;//c.SetZero();
		
		// Compute the contribution of each triangle.
		for (int i = 0; i < triCount; ++i) {
			int i1 = tris[i*3];
			int i2 = tris[i*3+1];
			int i3 = tris[i*3+2];
			
			Vector3 v1 = verts[i1];
			float d1 = ds[i1];

			Vector3 v2 = verts[i2];
			float d2 = ds[i2];

			Vector3 v3 = verts[i3];
			float d3 = ds[i3];
			
			if (d1 * d2 < 0) {
				// v1-v2 crosses the plane
				volume += ClipTriangle(p, v1, v2, v3, d1, d2, d3);
			} else if (d1 * d3 < 0) {
				// v1-v3 crosses the plane
				volume += ClipTriangle(p, v3, v1, v2, d3, d1, d2);
			} else if (d2 * d3 < 0) {
				// v2-v3 crosses the plane
				volume += ClipTriangle(p, v2, v3, v1, d2, d3, d1);
			} else if (d1 < 0 || d2 < 0 || d3 < 0){
				// fully submerged
				volume += TetrahedronVolume(p, v1, v2, v3);
			}
			
		}
		return volume;
	}
	
	//Compute volume of attached mesh
	private float ComputeVolume(){
		float volume = 0;
		Vector3 zero; //Vector3 c, zero;
		zero = Vector3.zero;
		c = Vector3.zero;

		// Compute the contribution of each triangle.
		//for (int i = 0; i < poly.numFaces; ++i)	{
		for (int i = 0; i < triCount; ++i)	{
						//volume += TetrahedronVolume(zero, verts[tris[i*3]], verts[tris[i*3+1]], verts[tris[i*3+2]]);
						volume += TetrahedronVolume(zero, transform.TransformPoint(verts[tris[i*3]]), transform.TransformPoint(verts[tris[i*3+1]]), transform.TransformPoint(verts[tris[i*3+2]]));
		}
	
		return volume;
	}
	
	/*
	Custom Unity-centered implementation
	*/
	void ComputeBuoyancy() {
		//Vector3 c;// Vec3 c;
		c = Vector3.zero;
		float gravity = Physics.gravity.magnitude;//FIX...
		
		float volume = SubmergedVolume()*meshVolume;//absolute volume
		if (volume > 0){
			
			Vector3 buoyancyForce = (WaterPlane.instance.waterDensity*volume*gravity)*Vector3.up;
			
			//return buoyancyForce;//add this at the center of bouyancy for free buoyancy torque
			float amountInWater = Mathf.Clamp01(volume / meshVolume); //use this to change drag & angularDrag
			float submergedMass = rigidbody.mass * amountInWater;
			Vector3 rc = c - rigidbody.centerOfMass;
			Vector3 vc = rigidbody.GetPointVelocity(transform.TransformPoint(c));//velocity at center of buoyancy
			Vector3 dragForce = (submergedMass*WaterPlane.instance.waterDrag)*(GetWaterCurrent() - vc);
			
			Vector3 totalForce = buoyancyForce + dragForce;
			//rigidbody.AddForce(totalForce);//body.F += totalForce;
			rigidbody.AddForceAtPosition(totalForce, transform.TransformPoint(c));
			//rigidbody.AddTorque(Vector3.Cross(rc,totalForce));//(buoyancy torque) -> unstable ??
			//body.T += rc % totalForce;
			//print(amountInWater);
			
			rigidbody.drag = Mathf.Lerp(drag,WaterPlane.instance.waterDrag,amountInWater);
			rigidbody.angularDrag = Mathf.Lerp(angularDrag,WaterPlane.instance.waterAngularDrag,amountInWater);
			
		}
	}
	/*
	GPG 6 implementation
	*/
	void ComputeBuoyancy2(){
		
		c = Vector3.zero;
		Vector3 omega = Vector3.Scale(rigidbody.angularVelocity, I);
		float gravity = Physics.gravity.magnitude;//FIX...
		
		float volume = SubmergedVolume();//0 - 1
		
		//print(transform.name + ", "+volume+ " / "+meshVolume);
		
		if (volume > 0)
		{
			Vector3 buoyancyForce = (WaterPlane.instance.waterDensity*volume*meshVolume*gravity)*Vector3.up;

			float partialMass = rigidbody.mass * volume * meshVolume;//rigidbody.mass * volume / meshVolume;
			Vector3 rc = c - rigidbody.centerOfMass;//c - body.x; Vector between center of mass & center of buoyancy
			Vector3 vc = rigidbody.GetPointVelocity(transform.TransformPoint(c));//body.v + body.omega % rc;
			Vector3 dragForce = (partialMass*WaterPlane.instance.waterDrag)*(GetWaterCurrent() - vc);
			
			Vector3 totalForce = buoyancyForce + dragForce;
			rigidbody.AddForce(totalForce);//body.F += totalForce;
			rigidbody.AddTorque(Vector3.Cross(rc,totalForce));//body.T += rc % totalForce;
			
			float length2 = lSqr;//poly.length*poly.length;
			Vector3 dragTorque = (-partialMass*WaterPlane.instance.waterAngularDrag*length2)*omega;
			rigidbody.AddTorque(dragTorque);//body.T += dragTorque;
			
		}
	}
	
	
	Vector3 GetWaterCurrent(){ //
	
		if(!WaterPlane.instance.currents)
			return Vector3.zero;
			
		Vector3 origin = transform.TransformPoint(c);
		origin.y = WaterPlane.instance.transform.position.y+1;
		Ray ray = new Ray(origin, -Vector3.up);
		
		RaycastHit hit;
    	if (!Physics.Raycast (ray, out hit, 2, layerMask))
        	return Vector3.zero;
		
		
			Vector2 uv = hit.textureCoord;
			Vector3 currentDir = Vector3.zero;
			Color dir = WaterPlane.instance.currents.GetPixelBilinear(uv.x,uv.y);
			float angle = dir.grayscale*360;
		
			currentDir.x = Mathf.Cos(angle*Mathf.Deg2Rad);
			currentDir.z = Mathf.Sin(angle*Mathf.Deg2Rad);
			
			currentDir = currentDir.normalized;
			//Debug.DrawLine(transform.position, transform.position + currentDir*dir.a);//Draw current
			return currentDir*dir.a*WaterPlane.instance.currentStrength;
	}

}
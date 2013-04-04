/* var projectile : Rigidbody; var speed = 20;
function Update() {
if( Input.GetButtonDown( "Fire1" ) ) {
var instantiatedProjectile : Rigidbody = Instantiate( projectile, transform.position, transform.rotation );
instantiatedProjectile.velocity = transform.TransformDirection( Vector3( 0, 0, speed ) );
Physics.IgnoreCollision( instantiatedProjectile. collider, transform.root.collider );
} } */
var projectile : Rigidbody;
var speed = 20;
public static var chargeLevel : float = 0; //Don't change this in the inspector.
var chargeSpeed : float = 1; //Default, the charge will go up 1 per second
var isCharging = false;

function Update () {
     if (Input.GetButtonDown("Fire1")) { //Did the user click?
          if(!isCharging) { //Some what unnecessary due to the way the Input is
          // setup
               isCharging = true;
               CalculateCharge();
          }
     }
}

function CalculateCharge () {
     while(Input.GetButton("Fire1") && chargeLevel < 1.5f) { //Add to the charge as long as the
     // user is holding the button
          chargeLevel += Time.deltaTime * chargeSpeed;
          yield; //Will cause a crash without this.
          transform.localScale = Vector3(chargeLevel*0.5, chargeLevel*0.5, chargeLevel*0.5);
      }

     //Fire Projectile
	var instantiatedProjectile : Rigidbody = Instantiate( projectile, transform.position, transform.rotation );
	
	instantiatedProjectile.velocity = transform.TransformDirection( Vector3( 0, 0, speed ) );
	Physics.IgnoreCollision( instantiatedProjectile. collider, transform.root.collider );
     Pitch.pitchLevel = chargeLevel;

     // Reset the vars.
     chargeLevel = 0.0;
     isCharging = false;
     transform.localScale = Vector3(0,0,0);
}


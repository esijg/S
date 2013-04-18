/********************/
/*   VERSION: 0.2   */
/********************/

public var speed = 6.0;
public var jumpSpeed = 8.0;
public var gravity = 20.0;

public var showMouseCursor = false;
public var enableKeyboardTurn = false;
public var turningSpeed = 150;
public var keyForLookingUp = "e";
public var keyForLookingDown = "q";
public var maxAngleLookingUp = 90;
public var maxAngleLookingDown = 90;
public var doubleTapTimeToResetLookAngle : float = 0.3;
public var flyingMode = false;
public var flyingUpDownSpeed = 10;

private var moveDirection = Vector3.zero;
private var grounded : boolean = false;
private var controller : CharacterController;
private var mainCamera : Transform;
private var lastTapTimeToResetLookUp : float;
private var lastTapTimeToResetLookDown : float;

// Use this for initialization
function Awake () {	
	// we need to disable the mouselook if we want to turn with the keyboard:
	if(enableKeyboardTurn){
		GetComponent("MouseLook").enabled = false;
		mainCamera = transform.Find("Main Camera");
		mainCamera.GetComponent("MouseLook").enabled = false;
	}
	if(!showMouseCursor){
		Screen.showCursor = false;
	}
	controller = GetComponent(CharacterController);
	maxAngleLookingUp = Mathf.Abs(Mathf.Clamp(maxAngleLookingUp, 0, 90));
	// we differentiate looking up and looking down by setting the "looking down" to a negative value
	maxAngleLookingDown = Mathf.Abs(Mathf.Clamp(maxAngleLookingDown, 0, 90)) * -1;
}

function FixedUpdate() {
	if (grounded || flyingMode) {
		// We are grounded, so recalculate movedirection directly from axes
		if(!enableKeyboardTurn){
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		}
		else{
			// if keyboardTurn is enabled, we just check the vertical axis
			moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
		}
		
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;
		
		if (Input.GetButton ("Jump") && !flyingMode) {
			moveDirection.y = jumpSpeed;
		}
	}
	
	// Apply gravity
	if(!flyingMode){
		moveDirection.y -= gravity * Time.deltaTime;
	}
	
	// Move the controller
	var flags = controller.Move(moveDirection * Time.deltaTime);
	grounded = (flags & CollisionFlags.CollidedBelow) != 0;
	
	// Make rotations in "enableKeyboardTurn" mode
	if(enableKeyboardTurn){
		// at first we rotate around the y axis -> turn left and right
		controller.transform.Rotate(0, Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime, 0);
		// now we rotate the camera around the x axis -> look up and down
		// we need to limit this according to our settings
		rotationInputValue = 0;		
		if(Input.GetKey(keyForLookingUp)) rotationInputValue--;
		if(Input.GetKey(keyForLookingDown)) rotationInputValue++;
		if(rotationInputValue!=0){
			localXAngle = convertAngle(mainCamera.localEulerAngles.x, mainCamera.localEulerAngles.y);										
			rotationDifference = rotationInputValue * turningSpeed * Time.deltaTime;
			newRotation = mainCamera.localEulerAngles.x + rotationDifference;
			newRotationConverted = convertAngle(newRotation, mainCamera.localEulerAngles.y);			
			if(newRotationConverted >= maxAngleLookingDown && newRotationConverted <= maxAngleLookingUp){
				mainCamera.Rotate(rotationDifference, 0, 0);
			}
		}
		// Check for double tap to reset the looking angle:
		if(Input.GetKeyUp(keyForLookingUp)) {
			if(Time.time - lastTapTimeToResetLookUp <= doubleTapTimeToResetLookAngle)
				mainCamera.localEulerAngles.x = 0;
			else
				lastTapTimeToResetLookUp = Time.time;
		}
		if(Input.GetKeyUp(keyForLookingDown)) {
			if(Time.time - lastTapTimeToResetLookDown <= doubleTapTimeToResetLookAngle)
				mainCamera.localEulerAngles.x = 0;
			else
				lastTapTimeToResetLookDown = Time.time;
		}
	}
	// Make movements in fylingMode
	if(flyingMode){
		var flyingMoveDirection = new Vector3(0, Input.GetAxis("Jump") * flyingUpDownSpeed, 0);
		controller.Move(flyingMoveDirection * Time.deltaTime);
	}
}

// we need to convert the given angle into an angle that fits to our maxAngles for looking up and down
// so they only range between 0 and 180 for both directions
// furthermore we want that looking down has a negative angle, and looking up a positive angle:	
private function convertAngle(xAngle, yAngle){
	if(yAngle == 0){
		localXAngle = xAngle;	
		if(localXAngle > 270) localXAngle = 360 - localXAngle;
		else localXAngle *= -1;				
	}
	else{
		localXAngle = 180 - xAngle;
		if(localXAngle <= 270) localXAngle *= -1;
	}
	return localXAngle;
}

@script RequireComponent(CharacterController)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMove : MonoBehaviour {

	//components
	public Rigidbody rb;
	private GameObject cam;
	private Camera camComponent;

	//movement
	[Header("Speed")]
	public float walkSpeed = 0.2f;
	public float sprintSpeed = 0.8f;

	[Header("Verticality")]
	public float jumpSpeed = 450f;
	public float flyHeight = 80f;
	public float gravity = 5f;
	public float groundedHeight = 0.2f;
	public float floatDistance = 4f;
	public float graceSpace = 0.5f;
	public float jumpDelayTime = 0.5f;
	public LayerMask mask;

	[Header("Acceleration")]
	public float speedChangeWalk = 2.0f;
	public float speedChangeSprint = 0.2f;
	public float speedChangeStop = 1.5f;
	public float directionChangeSpeed = 3f;
	public float airDampening = 0.2f;
	public float flyEase = 4f;
	public float flightSpeedMultiplier = 2f;
	public float flightControlMultiplier = 10f;

	[Header("FOV")]
	public float defaultFOV = 65f;
	public float fastFOV = 90f;
	public float flyingFOV = 100f;
	public float FOVease = 2f;

	[Header("Internals")]
	public bool flying = false;
	public float targetSpeed = 0f;
	public float targetFOV = 0f;
	public bool jumping = false;
	public bool sprinting = false;
	public Vector2 targetDirection = new Vector2(0f, 0f);
	private bool spacePressed = false;
	private bool shiftPressed = false;

	//story
	private bool ready = false;

	//audio
	private Sound soundSystem;
	public AudioMixer mix;
	
	void Start() {
		cam = GameObject.Find("Camera");
		camComponent = cam.GetComponent<Camera>();
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();

		targetFOV = defaultFOV;
	}

	void Update() {
		if (!ready) {
			if (GameObject.Find("Nox").GetComponent<Story>().introductionFinished) ready = true;
		} else {
			handleKeys();
		}

		setFOV();
		handleSound();
	}

	//needs to be in fixedupdate as we're modifying a rigidbody
	void FixedUpdate() {
		if (ready) move();
	}

	//this lets us handle key presses in the update loop, whereas they can be missed in FixedUpdate
	void handleKeys() {
		if (Input.GetKeyDown("escape")) Application.Quit();

		//switch to flying mode only if no mouse buttons are pressed
		if (Input.GetKeyDown(KeyCode.LeftAlt) && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)) {
			flying = !flying;
			//change speeds and dampening parameters when flying for quicker and snappier movement in-air
			if (flying) {
				walkSpeed *= flightSpeedMultiplier;
				sprintSpeed *= flightSpeedMultiplier;

				speedChangeWalk *= flightControlMultiplier;
				speedChangeSprint *= flightControlMultiplier;
				speedChangeStop *= flightControlMultiplier;
				directionChangeSpeed *= flightControlMultiplier;

				soundSystem.dynamicToggle("harmonies", true);
			} else {
				walkSpeed /= flightSpeedMultiplier;
				sprintSpeed /= flightSpeedMultiplier;

				speedChangeWalk /= flightControlMultiplier;
				speedChangeSprint /= flightControlMultiplier;
				speedChangeStop /= flightControlMultiplier;
				directionChangeSpeed /= flightControlMultiplier;

				soundSystem.dynamicToggle("harmonies", false);
				soundSystem.dynamicToggle("pads", false);
			}
		}

		if (Input.GetKeyDown(KeyCode.Space)) spacePressed = true;
		if (Input.GetKeyUp(KeyCode.Space)) spacePressed = false;

		if (Input.GetKeyDown(KeyCode.LeftShift)) shiftPressed = true;
		if (Input.GetKeyUp(KeyCode.LeftShift)) shiftPressed = false;
	}

	void handleSound() {
		if (flying && sprinting) soundSystem.addEnergy(2.4f);
		else if (flying && !sprinting) soundSystem.addEnergy(1.8f);
		else if (sprinting) soundSystem.addEnergy(1.4f);
		else if (getSpeed() > 1f) soundSystem.addEnergy(0.4f);

		//need listener specifically for a single event
		//repeated calls to dynamicToggle result in loss of functionality
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			soundSystem.dynamicToggle("percussion", true);
		}

		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			soundSystem.dynamicToggle("percussion", false);
		}

		if (jumping) {
			float cut;
			mix.GetFloat("Frequency_Cutoff", out cut);
			mix.SetFloat("Frequency_Cutoff", Mathf.Lerp(cut, 1100f, 1f * Time.deltaTime));
		} else {
			float cut;
			mix.GetFloat("Frequency_Cutoff", out cut);
			mix.SetFloat("Frequency_Cutoff", Mathf.Lerp(cut, 10f, 5f * Time.deltaTime));
		}
	}

	void setFOV() {
		if (GameObject.Find("Camera").GetComponent<SunClick>().transitioning == null) {
	 		if (!flying) targetFOV = Mathf.Lerp(targetFOV, Nox.remap(targetSpeed, walkSpeed, sprintSpeed, defaultFOV, fastFOV), FOVease * Time.deltaTime);
			camComponent.fieldOfView = targetFOV;
		}
	}

	void move() {
		//get input
		float horizontal = 0f;
		float vertical = 0f;
		if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f) horizontal = Input.GetAxis("Horizontal");
		if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f) vertical = Input.GetAxis("Vertical");
		Vector2 direction = getInput(horizontal, vertical);
		Vector3 newLoc = new Vector3(gameObject.transform.position.x + direction.x * Time.fixedDeltaTime, gameObject.transform.position.y, gameObject.transform.position.z + direction.y * Time.fixedDeltaTime);

		/*
		An important component of the movement is how, unless the player jumps, they are glued to the ground.
		This is done to create a feeling of 'skating' or 'gliding' across the landscape.
		*/

		//glue to ground, or add gravity while airborne
		if (!flying) {
			if (!jumping) newLoc = groundPlayer(newLoc);
			else rb.AddForce(-Vector3.up * gravity);
		} else {
			//fly
			rb.velocity = new Vector3(0, 0, 0);
			targetFOV = Mathf.Lerp(targetFOV, flyingFOV, FOVease * Time.fixedDeltaTime);

			if (transform.position.y < flyHeight - 0.001f) newLoc = new Vector3(newLoc.x, Mathf.Lerp(transform.position.y, flyHeight, flyEase * Time.fixedDeltaTime), newLoc.z);
			else newLoc = new Vector3(newLoc.x, flyHeight, newLoc.z);
		}

		//apply movement
		rb.MovePosition(newLoc);

		//limit player to bounds
		if (transform.position.z > 2900f) transform.position = new Vector3(transform.position.x, transform.position.y, 2899f);
		if (transform.position.z < -2900f) transform.position = new Vector3(transform.position.x, transform.position.y, -2899f);
		if (transform.position.x > 2900f) transform.position = new Vector3(2899f, transform.position.y, transform.position.z);
		if (transform.position.x < -2900f) transform.position = new Vector3(-2899f, transform.position.y, transform.position.z);

		//jump
		if (spacePressed && isGrounded() && !jumping && !flying) {
			jumping = true;
			rb.AddForce(Vector3.up * jumpSpeed);
			StartCoroutine(jumpDelay());
		}

		//no movement - stop all forces (excluding vertical force for jumping)
		if (horizontal == 0f && vertical == 0f && isGrounded()) {
			targetSpeed = Mathf.Lerp(targetSpeed, 0f, speedChangeStop * Time.fixedDeltaTime);
			rb.velocity = new Vector3(0, rb.velocity.y, 0);

		//sprint
		sprinting = false;
		} else if (shiftPressed) {
			sprinting = true;
			targetSpeed = Mathf.Lerp(targetSpeed, sprintSpeed, speedChangeSprint * Time.fixedDeltaTime);
			
		//walk
		} else {
			targetSpeed = Mathf.Lerp(targetSpeed, walkSpeed, speedChangeWalk * Time.fixedDeltaTime);
		}
	}

	Vector2 getInput(float horizontal, float vertical) {
		//calculating direction vector
		Vector3 direction = new Vector3(horizontal, 0.0f, vertical);

		//create rotated transform that is locked to avoid up/down camera angle affecting direction magnitude
		Quaternion cameraRotation = cam.transform.rotation;
		cam.transform.Rotate(Vector3.left, cam.transform.localRotation.eulerAngles.x);

		direction = cam.transform.TransformDirection(direction);
		direction.y = 0.0f;

		//revert camera's rotation
		cam.transform.rotation = cameraRotation;

		//limit input magnitude (to avoid high-magnitude input when moving diagonally)
		direction = Vector3.Normalize(direction);

		//ease direction for smoother movement (dampen direction change if in air)
		float changer = directionChangeSpeed;
		if (jumping) changer *= airDampening;

		targetDirection.x = Mathf.Lerp(targetDirection.x, direction.x, changer * Time.deltaTime);
		targetDirection.y = Mathf.Lerp(targetDirection.y, direction.z, changer * Time.deltaTime);

		//amplify normalized vector to desired speed
		return new Vector2(targetDirection.x, targetDirection.y) * targetSpeed;
	}

	Vector3 groundPlayer(Vector3 location) {
		
		//add small correction (offset upwards) so that a collider on a steep hill doesn't clip through
		RaycastHit hit;
		if (Physics.Raycast(new Vector3(location.x, location.y + graceSpace, location.z), -Vector3.up, out hit, floatDistance, mask)) {
			location = new Vector3(location.x, hit.point.y + GetComponent<Collider>().bounds.extents.y + graceSpace, location.z);

		//if distance is big enough (float), make player fall with gravity instead of forcing them to the ground
		} else if (!jumping) {
			jumping = true;
			StartCoroutine(jumpDelay());
		}

		return location;
	}

	bool isGrounded() {
		return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + groundedHeight);
	}

	IEnumerator jumpDelay() {
		yield return new WaitForSeconds(jumpDelayTime);

		while(jumping) {
			yield return new WaitForSeconds(0.01f);
			if (isGrounded()) jumping = false;
		}
	}

	public float getSpeed() {
		return targetSpeed / sprintSpeed;
	}
}
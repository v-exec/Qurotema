﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMove : MonoBehaviour {

	//components
	private GameObject cam;
	private Camera camComponent;

	//movement
	[Header("Speed")]
	public float walkSpeed = 20f;
	public float sprintSpeed = 80f;

	[Header("Verticality")]
	public float jumpSpeed = 200f;
	public float terminalVelocity = -20f;
	public float flyHeight = 200f;
	public float gravityEase = 5f;
	public float groundedHeight = 1f;
	public float floatDistance = 5f;
	public float graceSpace = 0.5f;
	public float jumpDelayTime = 0.5f;
	public LayerMask mask;

	[Header("Acceleration")]
	public float speedChangeWalk = 2f;
	public float speedChangeSprint = 2f;
	public float speedChangeStop = 2f;
	public float directionChangeSpeed = 3f;
	public float airDampening = 0.2f;
	public float flyEase = 4f;
	public float flightSpeedMultiplier = 2f;
	public float flightControlMultiplier = 3f;

	[Header("FOV")]
	public float defaultFOV = 65f;
	public float fastFOV = 90f;
	public float flyingFOV = 100f;
	public float FOVease = 2f;

	[Header("Internals")]
	public bool flying = false;
	public bool jumpTrigger = false;
	public bool jumping = false;
	public bool sprinting = false;
	public float targetSpeed = 0f;
	public float targetFOV = 0f;
	public float verticalForce = 0f;
	public float bottomDistanceFromCenter = 1f;
	public Vector2 targetDirection = new Vector2(0f, 0f);
	private bool ready = false; //story

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
			move();
		}

		setFOV();
		handleSound();
	}

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
	}

	void handleSound() {
		if (flying && sprinting) soundSystem.addEnergy(2.4f);
		else if (flying && !sprinting) soundSystem.addEnergy(1.8f);
		else if (sprinting) soundSystem.addEnergy(1.4f);
		else if (getSpeed() > 1f) soundSystem.addEnergy(0.4f);

		//need listener specifically for a single event
		//repeated calls to dynamicToggle result in loss of functionality
		if (Input.GetKeyDown(KeyCode.LeftShift)) soundSystem.dynamicToggle("percussion", true);
		if (Input.GetKeyUp(KeyCode.LeftShift)) soundSystem.dynamicToggle("percussion", false);

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
		Vector3 newLoc = new Vector3(transform.position.x + direction.x * Time.deltaTime, transform.position.y, transform.position.z + direction.y * Time.deltaTime);

		/*
		An important component of the movement is how, unless the player jumps, they are glued to the ground.
		This is done to create a feeling of 'skating' or 'gliding' across the landscape.
		*/

		//glue to ground, or add gravity while airborne
		if (!flying) {
			if (!jumping) newLoc = groundPlayer(newLoc);
			else {
				if (jumpTrigger) {
					verticalForce = jumpSpeed;
					jumpTrigger = false;
					StartCoroutine(jumpDelay());
				}

				if (verticalForce > 2f) verticalForce = Mathf.Lerp(verticalForce, 0f, gravityEase * Time.deltaTime);
				else verticalForce = Mathf.Lerp(verticalForce, terminalVelocity, gravityEase * Time.deltaTime);

				newLoc.y += verticalForce * Time.deltaTime;
				newLoc.y = preventClip(newLoc);
			}
		} else {
			//fly
			targetFOV = Mathf.Lerp(targetFOV, flyingFOV, FOVease * Time.deltaTime);

			if (transform.position.y < flyHeight - 0.001f) newLoc = new Vector3(newLoc.x, Mathf.Lerp(transform.position.y, flyHeight, flyEase * Time.deltaTime), newLoc.z);
			else newLoc = new Vector3(newLoc.x, flyHeight, newLoc.z);
		}

		//apply movement
		transform.position = newLoc;

		//limit player to bounds
		if (transform.position.z > 2900f) transform.position = new Vector3(transform.position.x, transform.position.y, 2899f);
		if (transform.position.z < -2900f) transform.position = new Vector3(transform.position.x, transform.position.y, -2899f);
		if (transform.position.x > 2900f) transform.position = new Vector3(2899f, transform.position.y, transform.position.z);
		if (transform.position.x < -2900f) transform.position = new Vector3(-2899f, transform.position.y, transform.position.z);

		//jump
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded() && !jumping && !flying) {
			jumping = true;
			jumpTrigger = true;
		}

		//no movement - stop all forces (excluding vertical force for jumping)
		if (horizontal == 0f && vertical == 0f && isGrounded()) {
			targetSpeed = Mathf.Lerp(targetSpeed, 0f, speedChangeStop * Time.deltaTime);

		//sprint
		} else if (Input.GetKey(KeyCode.LeftShift)) {
			sprinting = true;
			targetSpeed = Mathf.Lerp(targetSpeed, sprintSpeed, speedChangeSprint * Time.deltaTime);
			
		//walk
		} else {
			sprinting = false;
			targetSpeed = Mathf.Lerp(targetSpeed, walkSpeed, speedChangeWalk * Time.deltaTime);
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
			location = new Vector3(location.x, hit.point.y + bottomDistanceFromCenter + graceSpace, location.z);

		//if distance is big enough (float), make player fall with gravity instead of forcing them to the ground
		} else if (!jumping) {
			jumping = true;
			StartCoroutine(jumpDelay());
		}

		return location;
	}

	float preventClip(Vector3 location) {
		RaycastHit hit;
		if (Physics.Raycast(new Vector3(location.x, location.y + 20f, location.z), -Vector3.up, out hit, 50f, mask)) {
			if (hit.point.y-3f > location.y - bottomDistanceFromCenter) {
				return hit.point.y + bottomDistanceFromCenter + graceSpace;
			}
		}
		return location.y;
	}

	bool isGrounded() {
		return Physics.Raycast(transform.position - new Vector3(0f, bottomDistanceFromCenter, 0f), -Vector3.up, groundedHeight);
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
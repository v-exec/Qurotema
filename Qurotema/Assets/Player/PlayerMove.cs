﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	//components
	private Rigidbody rb;
	private GameObject cam;
	private Camera camComponent;

	//movement
	public float walkSpeed = 0.2f;
	public float sprintSpeed = 0.8f;

	private float jumpSpeed = 450f;
	private float flyHeight = 80f;
	private float gravity = 5f;

	private float speedChangeWalk = 2.0f;
	private float speedChangeSprint = 0.2f;
	private float speedChangeStop = 1.5f;
	private float directionChangeSpeed = 3f;
	private float airDampening = 0.2f;
	private float flyEase = 4f;

	private float jumpDelayTime = 0.1f;
	private float groundedHeight = 0.2f;
	private float floatDistance = 4f;
	private float flightSpeedMultiplier = 2f;
	private float flightControlMultiplier = 10f;

	public float defaultFOV = 65f;
	public float fastFOV = 90f;
	
	private float flyingFOV = 100f;
	private float FOVease = 2f;

	//internal
	public bool flying = false;
	public float targetSpeed = 0f;
	public float targetFOV = 0f;

	private bool jumping = false;
	private bool sprinting = false;
	private Vector2 targetDirection = new Vector2(0, 0);
	
	void Start() {
		//get components
		cam = GameObject.Find("Camera");
		camComponent = cam.GetComponent<Camera>();
		rb = GetComponent<Rigidbody>();

		//set fov to default
		targetFOV = defaultFOV;
	}

	//escape
	void Update() {
		handleKeys();
		setFOV();
	}

	void FixedUpdate() {
		move();
	}

	void handleKeys() {
		if (Input.GetKeyDown("escape")) {
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.LeftAlt)) {
			flying = !flying;
			if (flying) {
				walkSpeed *= flightSpeedMultiplier;
				sprintSpeed *= flightSpeedMultiplier;

				speedChangeWalk *= flightControlMultiplier;
				speedChangeSprint *= flightControlMultiplier;
				speedChangeStop *= flightControlMultiplier;
				directionChangeSpeed *= flightControlMultiplier;
			} else {
				walkSpeed /= flightSpeedMultiplier;
				sprintSpeed /= flightSpeedMultiplier;

				speedChangeWalk /= flightControlMultiplier;
				speedChangeSprint /= flightControlMultiplier;
				speedChangeStop /= flightControlMultiplier;
				directionChangeSpeed /= flightControlMultiplier;
			}
		}
	}

	void setFOV() {
		if (!flying) targetFOV = Nox.ease(targetFOV, Nox.remap(targetSpeed, walkSpeed, sprintSpeed, defaultFOV, fastFOV), FOVease);
		camComponent.fieldOfView = targetFOV;
	}

	void move() {
		//get input
		float horizontal = 0f;
		float vertical = 0f;
		if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f) horizontal = Input.GetAxis("Horizontal");
		if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f) vertical = Input.GetAxis("Vertical");
		Vector2 direction = getInput(horizontal, vertical);
		Vector3 newLoc = new Vector3(gameObject.transform.position.x + direction.x, gameObject.transform.position.y, gameObject.transform.position.z + direction.y);

		//glue to ground, or add gravity for jump
		if (!flying) {
			if (!jumping) {
				newLoc = groundPlayer(newLoc);
			} else {
				rb.AddForce(new Vector3(0, -gravity, 0));
			}
		} else {
			//fly
			rb.velocity = new Vector3(0, 0, 0);
			targetFOV = Nox.ease(targetFOV, flyingFOV, FOVease);

			if (transform.position.y < flyHeight - 0.001f) newLoc = new Vector3(newLoc.x, Nox.ease(transform.position.y, flyHeight, flyEase), newLoc.z);
			else newLoc = new Vector3(newLoc.x, flyHeight, newLoc.z);
		}

		//apply movement
		rb.MovePosition(newLoc);

		//jump
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded(groundedHeight) && !jumping && !flying) {
			jumping = true;
			rb.AddForce(Vector3.up * (jumpSpeed));
			StartCoroutine(jumpDelay());
		}

		sprinting = false;

		//no movement - stop all forces (excluding vertical force for jumping)
		if (horizontal == 0f && vertical == 0f && isGrounded(groundedHeight)) {
			targetSpeed = Nox.ease(targetSpeed, 0f, speedChangeStop);
			rb.velocity = new Vector3(0, rb.velocity.y, 0);
		//sprint
		} else if (Input.GetKey(KeyCode.LeftShift)) {
			sprinting = true;
			targetSpeed = Nox.ease(targetSpeed, sprintSpeed, speedChangeSprint);
		//walk
		} else {
			targetSpeed = Nox.ease(targetSpeed, walkSpeed, speedChangeWalk);
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

		targetDirection.x = Nox.ease(targetDirection.x, direction.x, changer);
		targetDirection.y = Nox.ease(targetDirection.y, direction.z, changer);

		//amplify normalized vector to desired speed
		return new Vector2(targetDirection.x, targetDirection.y) * targetSpeed;
	}

	Vector3 groundPlayer(Vector3 location) {
		RaycastHit hit;
		//offset raycast location to take into account collider height (add small correction for when collider and surface are perfectly aligned)
		Vector3 boundLocation = new Vector3(location.x, location.y - (GetComponent<Collider>().bounds.extents.y - 0.01f), location.z);
		Ray downRay = new Ray(boundLocation, -Vector3.up);

		if (Physics.Raycast(downRay, out hit)) {

			if (hit.collider.gameObject.layer != 9) { //PlayerNonColliders layer
				
				//if distance is big enough, make player fall with gravity instead of forcing them to the ground
				if (hit.distance > floatDistance && !jumping) {
					jumping = true;
					StartCoroutine(jumpDelay());
				} else if (hit.distance > groundedHeight) {
					//only apply transformation pushing player down if they're too high - otherwise this continuously pushes player downwards undesireably
					location = new Vector3(location.x, location.y - hit.distance + groundedHeight, location.z);
				}
			}
		}

		return location;
	}

	bool isGrounded(float extraDistance) {
		return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + extraDistance);
	}

	IEnumerator jumpDelay() {
		yield return new WaitForSeconds(jumpDelayTime);

		while(jumping) {
			yield return new WaitForSeconds(0.01f);
			if (isGrounded(groundedHeight)) jumping = false;
		}
	}

	public float getSpeed() {
		return targetSpeed / sprintSpeed;
	}
}
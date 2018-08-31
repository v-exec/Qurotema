using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	//camera and rigidbody
	private Transform cameraTransform;
	private Rigidbody rb;

	//movement
	private float walkSpeed = 10f;
	private float sprintSpeed = 40f;
	private float jumpSpeed = 3f;
	private float gravity = 600f;
	private float speedChangeWalk = 2.0f;
	private float speedChangeSprint = 0.2f;
	private float stopSpeed = 0.92f;

	//internal movement
	private float targetSpeed = 0f;
	private bool sprinting = false;
	private bool jumping = false;
	private Vector2 currentDirection = new Vector2(0, 0);

	//get camera
	void Start () {
		cameraTransform = GameObject.Find("Main Camera").transform;
		rb = GetComponent<Rigidbody>();
	}

	//escape
	void Update() {
		if (Input.GetKey("escape")) {
			Application.Quit();
		}
	}

	void FixedUpdate () {
		//additional gravity when not grounded
		rb.AddForce(new Vector3(0, -gravity, 0));
		if (!isGrounded(0.1f) && !jumping) {
			rb.AddForce(new Vector3(0, -gravity * 20f, 0));
		}

		//input
		float horizontal = 0f;
		float vertical = 0f;
		if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f) horizontal = Input.GetAxis("Horizontal");
		if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f) vertical = Input.GetAxis("Vertical");

		//calculating direction vector
		Vector3 direction = new Vector3(horizontal, 0.0f, vertical);
		Vector3 newDir = cameraTransform.forward;

		direction = cameraTransform.TransformDirection(direction);
		direction.y = 0.0f;

		//limit input magnitude (to avoid high-magnitude input when moving diagonally)
		direction = Vector3.Normalize(direction);
		currentDirection = new Vector2(direction.x, direction.z) * targetSpeed;

		//applied movement
		Vector3 newVel;
		newVel = new Vector3(currentDirection.x, rb.velocity.y, currentDirection.y);
		
		//only apply movement if grounded
		if (isGrounded(0.5f)) rb.velocity = newVel;

		//jump
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded(0.5f)) {
			jumping = true;
			rb.AddForce(Vector3.up * (jumpSpeed * 10000));
			StartCoroutine(JumpDelay());
		}

		//no movement
		if (Mathf.Abs(horizontal) < 0.1f && Mathf.Abs(vertical) < 0.1f && isGrounded(0.1f)) {
			sprinting = false;
			if (targetSpeed != 0) targetSpeed = 0f;
			
			if (isGrounded(0.1f) && rb.velocity.x > 0 || rb.velocity.z > 0) {
				rb.velocity *= stopSpeed;
				
				if (rb.velocity.x < 0.1f && rb.velocity.z < 0.1f) {
					rb.velocity = new Vector3(0, rb.velocity.y, 0);
				}
			}

		//sprint
		} else if (Input.GetKey(KeyCode.LeftShift)) {
			sprinting = true;
			if (targetSpeed != sprintSpeed) targetSpeed = ease(targetSpeed, sprintSpeed, speedChangeSprint);

		//walk
		} else {
			sprinting = false;
			if (targetSpeed != walkSpeed) targetSpeed = ease(targetSpeed, walkSpeed, speedChangeWalk);
		}
	}

	float ease (float val, float target, float ease) {
		float difference = target - val;
		difference *= ease * Time.deltaTime;
		return val + difference;
	}

	bool isGrounded (float extraDistance) {
		return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + extraDistance);
	}

	IEnumerator JumpDelay () {
		yield return new WaitForSeconds(0.2f);

		while(jumping) {
			yield return new WaitForSeconds(0.01f);
			if (isGrounded(0.3f)) jumping = false;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour {

	private float distanceFromCamera = 5f;
	private float followSpeed = 15f;
	private Rigidbody rb;

	private Vector3 red = new Vector3(1f, 0f, 0f);
	private Vector3 purple = new Vector3(0.05f, 0.05f, 1f);

	private Material mat;
	private Material trail;

	public float velocity = 0;

	void Start () {
		transform.position = Camera.main.transform.position + (Camera.main.transform.forward * distanceFromCamera);
		rb = GetComponent<Rigidbody>();
		mat = GetComponent<MeshRenderer>().material;
		trail = GetComponent<TrailRenderer>().material;
	}

	void Update () {
		if (!Input.GetMouseButton(1)) {
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<TrailRenderer>().enabled = false;
			makePassive();
		} else {
			GetComponent<MeshRenderer>().enabled = true;
			GetComponent<TrailRenderer>().enabled = true;

			if (Input.GetMouseButtonDown(0)) makeActive();
			if (Input.GetMouseButtonUp(0)) makePassive();
		}

		//override in movement and flight modes
		if (Input.GetMouseButton(2) || Nox.player.GetComponent<PlayerMove>().flying) {
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<TrailRenderer>().enabled = false;
		}
	}
	
	void FixedUpdate () {
		Vector3 targetPosition = Camera.main.transform.position + (Camera.main.transform.forward * distanceFromCamera);
		Vector3 newPosition = transform.position;

		newPosition.x = Nox.ease(newPosition.x, targetPosition.x, followSpeed);
		newPosition.y = Nox.ease(newPosition.y, targetPosition.y, followSpeed);
		newPosition.z = Nox.ease(newPosition.z, targetPosition.z, followSpeed);

		velocity = Vector3.Distance(transform.position, newPosition);

		rb.MovePosition(newPosition);
	}

	void makeActive() {
		mat.SetVector("_Color", red);
		trail.SetVector("_Color", red);
	}

	void makePassive() {
		mat.SetVector("_Color", purple);
		trail.SetVector("_Color", purple);
	}
}
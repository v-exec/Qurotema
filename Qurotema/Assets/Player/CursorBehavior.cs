using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour {

	public float distanceFromCamera = 5f;
	public float followSpeed = 15f;

	private Rigidbody rb;

	void Start () {
		transform.position = Camera.main.transform.position + (Camera.main.transform.forward * distanceFromCamera);
		rb = GetComponent<Rigidbody>();
	}

	void Update () {
		if (!Input.GetMouseButton(1)) {
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<TrailRenderer>().enabled = false;
		} else {
			GetComponent<MeshRenderer>().enabled = true;
			GetComponent<TrailRenderer>().enabled = true;
		}
	}
	
	void FixedUpdate () {
		Vector3 targetPosition = Camera.main.transform.position + (Camera.main.transform.forward * distanceFromCamera);
		Vector3 newPosition = transform.position;

		newPosition.x = Nox.ease(newPosition.x, targetPosition.x, followSpeed);
		newPosition.y = Nox.ease(newPosition.y, targetPosition.y, followSpeed);
		newPosition.z = Nox.ease(newPosition.z, targetPosition.z, followSpeed);

		rb.MovePosition(newPosition);
	}
}
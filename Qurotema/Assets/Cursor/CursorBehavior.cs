using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour {

	float distanceFromCamera = 5f;
	float followSpeed = 15f;

	void Start () {
		transform.position = Camera.main.transform.position + (Camera.main.transform.forward * distanceFromCamera);
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

		newPosition.x = ease(newPosition.x, targetPosition.x, followSpeed);
		newPosition.y = ease(newPosition.y, targetPosition.y, followSpeed);
		newPosition.z = ease(newPosition.z, targetPosition.z, followSpeed);

		transform.position = newPosition;
	}

	float ease (float val, float target, float ease) {
		float difference = target - val;
		difference *= ease * Time.deltaTime;
		return val + difference;
	}
}
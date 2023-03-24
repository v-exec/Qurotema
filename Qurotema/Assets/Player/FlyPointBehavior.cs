using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPointBehavior : MonoBehaviour {

	public LayerMask mask;
	private GameObject flyPoint;
	private Vector3 targetPoint;

	//audio
	private Sound soundSystem;

	void Start() {
		flyPoint = GameObject.Find("FlyPoint");
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();
	}

	void Update() {
		if (Nox.player.GetComponent<PlayerMove>().flying && Input.GetMouseButton(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) targetPoint = hit.point;

			soundSystem.addEnergy(1f);
		}

		if (Mathf.Abs(targetPoint.x - flyPoint.transform.position.x) > 1f && Mathf.Abs(targetPoint.z - flyPoint.transform.position.z) > 1f) {
			float newX = Nox.ease(flyPoint.transform.position.x, targetPoint.x, 1f);
			float newY = Nox.ease(flyPoint.transform.position.y, targetPoint.y, 1f);
			float newZ = Nox.ease(flyPoint.transform.position.z, targetPoint.z, 1f);
			flyPoint.transform.position = new Vector3(newX, newY, newZ);
		}

		//audio
		if (Nox.player.GetComponent<PlayerMove>().flying && Input.GetMouseButtonDown(0)) {
			soundSystem.dynamicToggle("pads", true);
		}

		if (Nox.player.GetComponent<PlayerMove>().flying && Input.GetMouseButtonUp(0)) {
			soundSystem.dynamicToggle("pads", false);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerBehavior : MonoBehaviour {

	public GameObject marker;
	public LayerMask mask;

	//audio
	private Sound soundSystem;
	private int markerCount = 0;

	void Start() {
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();
	}

	void Update() {
		if (Input.GetMouseButton(2) && !Input.GetMouseButton(1) && !Nox.player.GetComponent<PlayerMove>().flying) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
				Instantiate(marker, hit.point, Quaternion.identity);
				if (markerCount < 3) {
					soundSystem.shootSound("droplets");
					markerCount++;
				}

				if (Input.GetMouseButtonDown(0)) {
					Nox.player.GetComponent<PlayerMove>().targetFOV = 20f;
					Nox.player.GetComponent<PlayerMove>().rb.velocity = new Vector3(0,0,0);
					Nox.player.transform.position = new Vector3(hit.point.x, hit.point.y + 2f, hit.point.z);
					soundSystem.addEnergy(3f);
					soundSystem.shootSound("whips");
				}
			}
		}

		if (Input.GetMouseButtonDown(2)) markerCount = 0;
	}
}
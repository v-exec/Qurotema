using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerBehavior : MonoBehaviour {

	public GameObject marker;
	public LayerMask mask;

	void Update() {
		if (Input.GetMouseButton(2) && !Input.GetMouseButton(1) && !Nox.player.GetComponent<PlayerMove>().flying) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
				Instantiate(marker, hit.point, Quaternion.identity);

				if (Input.GetMouseButtonDown(0)) {
					Nox.player.GetComponent<PlayerMove>().targetFOV = 40f;
					Nox.player.GetComponent<PlayerMove>().rb.velocity = new Vector3(0,0,0);
					Nox.player.transform.position = new Vector3(hit.point.x, hit.point.y + 2f, hit.point.z);
				}
			}
		}
	}
}
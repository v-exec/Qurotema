using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPointBehavior : MonoBehaviour {

	public LayerMask mask;
	private bool ready = false;
	private float height = 200f;
	private IEnumerator routine;

	void Update() {
		if (Nox.player.GetComponent<PlayerMove>().flying && routine == null && !ready) {
			if (Mathf.Abs(Nox.flyPoint.y - getGroundHeight()) > 35f) {
				routine = Begin();
				StartCoroutine(routine);
			} else ready = true;
		}

		if (!Nox.player.GetComponent<PlayerMove>().flying && (routine != null || ready)) {
			if (routine != null) StopCoroutine(routine);
			routine = null;
			height = 200f;
			ready = false;
		}

		if (Nox.player.GetComponent<PlayerMove>().flying && ready) {
			float dist = getGroundHeight();
			Nox.flyPoint = new Vector3(transform.position.x, dist, transform.position.z);
		}
	}

	IEnumerator Begin() {
		while (!ready) {
			yield return new WaitForSeconds(0.01f);
			if (Nox.player.GetComponent<PlayerMove>().flying) {
				height -= 2f;
				Nox.flyPoint = new Vector3(transform.position.x, height, transform.position.z);
			}
			if (height <= getGroundHeight()) ready = true;
		}
	}

	float getGroundHeight() {
		RaycastHit hit;
		Ray ray = new Ray(transform.position, Vector3.down);

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
			return hit.point.y;
		} else return 1000f;
	}
}
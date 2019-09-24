using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stringer : MonoBehaviour {
	
	private Vector3 start = new Vector3(0,0,0);
	private Vector3 end = new Vector3(0,0,0);
	private bool stringing = false;
	private CursorBehavior cursor;

	public LineRenderer liner;
	public GameObject strings;
	public GameObject stringObject;
	public LayerMask mask;

	void Start() {
		cursor = GameObject.Find("Cursor").GetComponent<CursorBehavior>();
	}

	void Update() {
		if (!stringing) leftClick();
		else {
			drawPendingString();
			leftRelease();
		}
	}

	private void leftClick() {
		if (Input.GetMouseButton(1) && !Input.GetMouseButton(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~mask)) {
				if (hit.collider.tag == "String") {
					hit.collider.gameObject.GetComponent<String>().playSound(cursor.velocity);
				}
			}
		}

		if (Input.GetMouseButtonDown(0) && Input.GetMouseButton(1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~mask)) {
				if (hit.collider.tag == "StringsNode") {
					start = hit.collider.gameObject.transform.position;
					startString(start);
				}
			}
		}
	}

	private void leftRelease() {
		if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~mask)) {
				if (hit.collider.tag == "StringsNode") {
					end = hit.collider.gameObject.transform.position;
					if (end == start) cancelString();
					else endString(end);
				} else cancelString();
			} else cancelString();
		}
	}

	private void drawPendingString() {
		liner.SetPosition(0, start);
		liner.SetPosition(1, Camera.main.transform.position + (Camera.main.transform.forward * 10));
	}

	private float deletePendingString() {
		liner.SetPosition(0, new Vector3(0,0,0));
		liner.SetPosition(1, new Vector3(0,0,0));
		return Vector3.Distance(start, (Camera.main.transform.forward * 10));
	}

	private void startString(Vector3 s) {
		stringing = true;
		playStart();
	}

	private void endString(Vector3 e) {
		stringing = false;
		float snapDistance = deletePendingString();
		playEnd(Vector3.Distance(start, end));
		createString(start, end);
	}

	private void cancelString() {
		stringing = false;
		float snapDistance = deletePendingString();
		playSnap(snapDistance);
	}

	private void createString(Vector3 s, Vector3 e) {
		Vector3 pos = Vector3.Lerp(s, e, 0.5f);
		GameObject stringInstance = Instantiate(stringObject, pos, Quaternion.identity);
		stringInstance.transform.LookAt(e);
		stringInstance.transform.localScale += new Vector3(0, Vector3.Distance(s, e) * 50, 0);
		stringInstance.transform.Rotate(-90, 0, 0);
		String component = stringInstance.GetComponent<String>();
		component.init(s, e);
	}

	private void playSnap(float strength) {

	}

	private void playStart() {

	}

	private void playEnd(float length) {

	}
}
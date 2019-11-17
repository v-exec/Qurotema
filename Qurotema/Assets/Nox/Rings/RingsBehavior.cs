using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingsBehavior : MonoBehaviour {

	private Sound soundSystem;
	public bool inArea = false;
	public LayerMask mask;

	void Start() {
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();
	}

	void Update() {
		if (inArea && Input.GetMouseButton(1) && Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~mask)) {

				Debug.Log(hit.collider.tag);

				soundSystem.addEnergy(0.5f);
				soundSystem.shootSound("melodies", int.Parse(hit.collider.tag));
			}
		}
	}
}
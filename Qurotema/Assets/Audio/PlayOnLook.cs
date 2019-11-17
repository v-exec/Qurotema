﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnLook : MonoBehaviour {

	private Sound soundSystem;

	private bool lookingAtMonolith = false;
	private bool lookingAtGates = false;
	private bool lookingAtSun = false;
	private bool lookingAtRock = false;

	void Start() {
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();
	}

	void Update() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {

			//on conditions
			if (hit.collider.tag == "Sun" && !lookingAtSun) {
				disableAllFlags();
				lookingAtSun = true;

				soundSystem.ambienceToggle("sun", true);
			}

			if (hit.collider.tag == "Monolith" && !lookingAtMonolith) {
				disableAllFlags();
				lookingAtMonolith = true;

				soundSystem.ambienceToggle("whispers", true);
			}

			if (hit.collider.tag == "Gates" && !lookingAtGates) {
				disableAllFlags();
				lookingAtGates = true;

				soundSystem.ambienceToggle("vocals", true);
				soundSystem.ambienceToggle("whispers", true);
			}

			if (hit.collider.tag == "Rock" && !lookingAtRock) {
				disableAllFlags();
				lookingAtRock = true;

				soundSystem.ambienceToggle("whispers", true);
			}

			//off conditions
			if (hit.collider.tag != "Gates" && lookingAtGates) {
				lookingAtGates = false;
				soundSystem.ambienceToggle("vocals", false);
				if (hit.collider.tag != "Monolith" && hit.collider.tag != "Rock") soundSystem.ambienceToggle("whispers", false);
			}

			if (hit.collider.tag != "Monolith" && lookingAtMonolith) {
				lookingAtMonolith = false;
				if (hit.collider.tag != "Rock") soundSystem.ambienceToggle("whispers", false);
			}

			if (hit.collider.tag != "Sun" && lookingAtSun) {
				lookingAtSun = false;
				soundSystem.ambienceToggle("sun", false);
			}

			if (hit.collider.tag != "Rock" && lookingAtRock) {
				lookingAtRock = false;
				if (hit.collider.tag != "Monolith") soundSystem.ambienceToggle("whispers", false);
			}
		}
	}

	void disableAllFlags() {
		lookingAtGates = false;
		lookingAtMonolith = false;
		lookingAtSun = false;
		lookingAtRock = false;
	}
}

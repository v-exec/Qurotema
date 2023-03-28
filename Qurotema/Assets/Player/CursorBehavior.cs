﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour {

	private float distanceFromCamera = 5f;
	private float followSpeed = 15f;
	private Rigidbody rb;

	private Color red = new Color(100f, 0f, 0f);
	private Color purple = new Color(5f, 5f, 100f);

	private Material mat;
	private Material trail;

	//audio
	private Sound soundSystem;

	void Start () {
		transform.position = Camera.main.transform.position + (Camera.main.transform.forward * distanceFromCamera);
		rb = GetComponent<Rigidbody>();
		mat = GetComponent<MeshRenderer>().material;
		trail = GetComponent<TrailRenderer>().material;
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();
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

		//audio
		if (Input.GetMouseButton(1) && !Nox.player.GetComponent<PlayerMove>().flying && !Input.GetMouseButton(2)) {
			soundSystem.addEnergy(1f);
		}

		if (Input.GetMouseButtonDown(1) && !Nox.player.GetComponent<PlayerMove>().flying && !Input.GetMouseButton(2)) {
			soundSystem.dynamicToggle("rhythms", true);
		}

		if (Input.GetMouseButtonUp(1)) {
			soundSystem.dynamicToggle("rhythms", false);
		}
	}

	void FixedUpdate() {
		Vector3 targetPosition = Camera.main.transform.position + (Camera.main.transform.forward * distanceFromCamera);
		transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.fixedDeltaTime);
	}

	void makeActive() {
		mat.SetColor("_EmissiveColor", red);
		trail.SetColor("_EmissiveColor", red);
	}

	void makePassive() {
		mat.SetColor("_EmissiveColor", purple);
		trail.SetColor("_EmissiveColor", purple);
	}
}
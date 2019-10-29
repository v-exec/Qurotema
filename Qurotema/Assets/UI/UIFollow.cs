﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollow : MonoBehaviour {

	//parameters
	public float distanceFromCamera = 1.2f;
	public float followSpeed = 45f;
	public float fadeDelay = 0f;
	public string triggerLayer;

	private float targetOpacity = 0.9f;
	private float fadeSpeed = 10f;
	private float distanceFromCameraDifference = 0.3f;
	
	//internal
	private float opacity = 0f;
	private float minDistanceFromCamera = 0f;
	private float targetDistance = 0f;

	private Coroutine fader;
	private PlayerMove playerScript;

	void Start () {
		playerScript = Nox.player.GetComponent<PlayerMove>();

		transform.position = Camera.main.transform.position + (Camera.main.transform.forward * targetDistance);
		transform.rotation = Camera.main.transform.rotation;

		minDistanceFromCamera = distanceFromCamera - distanceFromCameraDifference;
	}

	void Update() {
		
		//modes
		switch (triggerLayer) {
			case "flight":
				if (Nox.player.GetComponent<PlayerMove>().flying) {
					if (fader != null) StopCoroutine(fader);
					if (opacity != targetOpacity) opacity = Nox.ease(opacity, targetOpacity, (fadeSpeed / 2f) * fadeDelay);
				} else {
					if (fader != null) StopCoroutine(fader);
					if (opacity != 0f) opacity = Nox.ease(opacity, 0f, fadeSpeed / 2f);
				}
				break;

			case "control":
				if (Input.GetMouseButtonDown(1)) {
					if (fader != null) StopCoroutine(fader);
					fader = StartCoroutine(Fade(targetOpacity));
				}

				if (Input.GetMouseButtonUp(1)) {
					if (fader != null) StopCoroutine(fader);
					fader = StartCoroutine(Fade(0f));
				}

				if (Input.GetMouseButtonDown(2)) {
					if (fader != null) StopCoroutine(fader);
					fader = StartCoroutine(Fade(0f));
				}

				if (Nox.player.GetComponent<PlayerMove>().flying) {
					if (fader != null) StopCoroutine(fader);
					if (opacity != 0f) opacity = Nox.ease(opacity, 0f, fadeSpeed / 2f);
				}
				break;

			case "movement":
				if (Input.GetMouseButtonDown(2)) {
					if (fader != null) StopCoroutine(fader);
					fader = StartCoroutine(Fade(targetOpacity));
				}

				if (Input.GetMouseButtonUp(2)) {
					if (fader != null) StopCoroutine(fader);
					fader = StartCoroutine(Fade(0f));
				}

				if (Input.GetMouseButtonDown(1)) {
					if (fader != null) StopCoroutine(fader);
					fader = StartCoroutine(Fade(0f));
				}

				if (Nox.player.GetComponent<PlayerMove>().flying) {
					if (fader != null) StopCoroutine(fader);
					if (opacity != 0f) opacity = Nox.ease(opacity, 0f, fadeSpeed / 2f);
				}
				break;
		}

		GetComponent<CanvasGroup>().alpha = opacity;
	}

	void FixedUpdate() {
		setDistance();
		follow();
	}

	void follow() {
		Vector3 targetPosition = Camera.main.transform.position + (Camera.main.transform.forward * targetDistance);
		Vector3 newPosition = transform.position;

		newPosition.x = Nox.ease(newPosition.x, targetPosition.x, followSpeed);
		newPosition.y = Nox.ease(newPosition.y, targetPosition.y, followSpeed);
		newPosition.z = Nox.ease(newPosition.z, targetPosition.z, followSpeed);

		transform.position = newPosition;
		transform.rotation = Camera.main.transform.rotation;
	}

	void setDistance() {
		targetDistance = Nox.remap(playerScript.targetFOV, playerScript.defaultFOV, playerScript.fastFOV, distanceFromCamera, minDistanceFromCamera);
	}

	IEnumerator Fade(float target) {
		yield return new WaitForSeconds(fadeDelay);

		while (Mathf.Abs(opacity - target) > 0.01f) {
			yield return new WaitForSeconds(0.01f);
			opacity = Nox.ease(opacity, target, fadeSpeed);
		}

		opacity = target;
	}
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour {

	public GameObject lightObject;
	public string tone;
	public int count;

	private Material light;
	private bool active;
	private bool ready = true;
	private Coroutine refreshRoutine;

	private float minAlpha = 0.1f;
	private float maxAlpha = 1f;
	private Coroutine glowRoutine;

	private Sound soundSystem;
	private Story s;

	void Start() {
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();
		s = GameObject.Find("Nox").GetComponent<Story>();
		light = lightObject.GetComponent<Renderer>().material;

		//move platform down and align to normal so that it matches terrain topology
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit)) {
			transform.position = hit.point + new Vector3(0f, -0.1f, 0f);

			//in order to avoid any grid pattern breaking on y axis, the pads parent needs to be aligned to the y axis (0, 180, -180, etc.)
			transform.up = hit.normal;
		}
	}

	void Update() {
		if (soundSystem.beat == count && active && soundSystem.beatChange) {
			if (glowRoutine != null) StopCoroutine(glowRoutine);
			glowRoutine = StartCoroutine(Glow());
			soundSystem.addEnergy(0.1f);
			soundSystem.shootSound(tone);
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && ready) {
			ready = false;
			active = !active;

			if (active) {
				if (glowRoutine != null) StopCoroutine(glowRoutine);
				light.SetColor("_BaseColor", new Color(1f, 1f, 1f, minAlpha));
				s.padPlayed();
			} else {
				if (glowRoutine != null) StopCoroutine(glowRoutine);
				light.SetColor("_BaseColor", new Color(1f, 1f, 1f, 0f));
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			if (refreshRoutine != null) StopCoroutine(refreshRoutine);
			refreshRoutine = StartCoroutine(Refresh());
		}
	}

	IEnumerator Glow() {
		float alpha = maxAlpha;
		light.SetFloat("_Alpha", alpha);

		while (alpha > minAlpha) {
			yield return new WaitForSeconds(0.01f);
			alpha = Mathf.Lerp(alpha, minAlpha, 2f * Time.deltaTime);
			light.SetColor("_BaseColor", new Color(1f, 1f, 1f, alpha));
		}
	}

	IEnumerator Refresh() {
		yield return new WaitForSeconds(0.5f);
		ready = true;
	}
}
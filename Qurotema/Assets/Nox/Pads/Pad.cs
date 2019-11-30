using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour {

	public GameObject lightObject;
	public string tone;
	public int count;

	private Material light;
	private bool active;
	private int time = 1;

	private float minAlpha = 0.1f;
	private float maxAlpha = 10f;
	private Coroutine glowRoutine;

	private Sound soundSystem;

	void Start() {
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();
		light = lightObject.GetComponent<Renderer>().material;
		StartCoroutine(Timer());
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			active = !active;

			if (active) {
				if (glowRoutine != null) StopCoroutine(glowRoutine);
				light.SetFloat("_Alpha", minAlpha);
			} else {
				if (glowRoutine != null) StopCoroutine(glowRoutine);
				light.SetFloat("_Alpha", 0f);
			}
		}
	}

	IEnumerator Timer() {
		while (true) {
			//bpm 120
			//beat/quarter = 0.5 of second
			//eight = 0.25 of second
			yield return new WaitForSeconds(0.25f);
			time++;
			if (time > 16) time = 1;

			if (time == count && active) {
				if (glowRoutine != null) StopCoroutine(glowRoutine);
				glowRoutine = StartCoroutine(Glow());
				soundSystem.addEnergy(0.1f);
				soundSystem.shootSound(tone);
			}
		}
	}

	IEnumerator Glow() {
		float alpha = maxAlpha;
		light.SetFloat("_Alpha", alpha);

		while (alpha > minAlpha) {
			yield return new WaitForSeconds(0.01f);
			alpha = Nox.ease(alpha, minAlpha, 2f);
			light.SetFloat("_Alpha", alpha);
		}
	}
}
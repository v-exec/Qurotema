using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour {

	public GameObject lightObject;
	public string tone;
	public int count;

	private Material light;
	private bool active;

	private float minAlpha = 0.1f;
	private float maxAlpha = 10f;
	private Coroutine glowRoutine;

	private Sound soundSystem;
	private Story s;

	void Start() {
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();
		s = GameObject.Find("Nox").GetComponent<Story>();
		light = lightObject.GetComponent<Renderer>().material;
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
		if (other.tag == "Player") {
			active = !active;

			if (active) {
				if (glowRoutine != null) StopCoroutine(glowRoutine);
				light.SetFloat("_Alpha", minAlpha);
				s.padPlayed();
			} else {
				if (glowRoutine != null) StopCoroutine(glowRoutine);
				light.SetFloat("_Alpha", 0f);
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
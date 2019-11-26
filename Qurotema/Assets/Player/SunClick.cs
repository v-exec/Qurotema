using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunClick : MonoBehaviour {

	private Sound soundSystem;
	private float FOV;

	public bool negative = false;
	public Coroutine transitioning;
	
	public LayerMask mask;
	public AudioMixer mix;

	void Start() {
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();
	}

	void Update() {
		if (Input.GetMouseButton(1) && Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~mask)) {
				if (hit.collider.tag == "Sun") {
					soundSystem.addEnergy(5f);
					if (transitioning != null) StopCoroutine(transitioning);
					transitioning = StartCoroutine(SwitchWorlds());
				}
			}
		}
	}

	IEnumerator SwitchWorlds() {
		negative = !negative;

		float cut;
		mix.GetFloat("Frequency_Cutoff", out cut);

		if (negative) {
			mix.SetFloat("Frequency_Cutoff", Nox.ease(cut, 500f, 0.5f));

			while (FOV < 140) {
				yield return new WaitForSeconds(0.01f);
				FOV += 0.5f;

			}

			mix.SetFloat("Frequency_Cutoff", Nox.ease(cut, 900f, 1f));

			while (FOV > 60) {
				yield return new WaitForSeconds(0.01f);
				FOV -= 0.5f;
				
			}
		} else {
			mix.SetFloat("Frequency_Cutoff", Nox.ease(cut, 500f, 0.5f));

			while (FOV < 140) {
				yield return new WaitForSeconds(0.01f);
				FOV += 0.5f;

			}

			mix.SetFloat("Frequency_Cutoff", Nox.ease(cut, 900f, 1f));

			while (FOV > 60) {
				yield return new WaitForSeconds(0.01f);
				FOV -= 0.5f;
				
			}
		}
	}
}
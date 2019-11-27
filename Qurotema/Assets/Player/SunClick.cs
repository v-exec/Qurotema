using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class SunClick : MonoBehaviour {

	private Sound soundSystem;
	private float FOV;
	private Camera camComponent;
	private bool routineEnded = false;

	public bool negative = false;
	public Coroutine transitioning;
	
	public LayerMask mask;
	public AudioMixer mix;
	public GameObject pp;

	void Start() {
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();
		camComponent = GameObject.Find("Camera").GetComponent<Camera>();
	}

	void Update() {
		if (routineEnded && transitioning != null) {
			StopCoroutine(transitioning);
			transitioning = null;
			routineEnded = false;
		}

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
		mix.GetFloat("LP_Freq", out cut);
		FOV = camComponent.fieldOfView;

		while (FOV > 10f) {
			yield return new WaitForSeconds(0.01f);
			FOV = Nox.ease(FOV, 9.9f, 10f);
			cut = Nox.ease(cut, 3000f, 0.1f);
			mix.SetFloat("LP_Freq", cut);
			camComponent.fieldOfView = FOV;
		}

		//filter cutoff
		if (negative) mix.SetFloat("LP_Freq", 1500f);
		else mix.SetFloat("LP_Freq", 22000f);

		//post-processing effects
		if (negative) pp.SetActive(true);
		else pp.SetActive(false);

		while (FOV < 65f) {
			yield return new WaitForSeconds(0.01f);
			FOV = Nox.ease(FOV, 66f, 5f);
			camComponent.fieldOfView = FOV;
		}

		routineEnded = true;
	}
}
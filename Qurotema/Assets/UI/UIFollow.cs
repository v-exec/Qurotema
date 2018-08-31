using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollow : MonoBehaviour {

	float distanceFromCamera = 0.9f;
	public float followSpeed = 1f;
	float opacity = 0f;
	float fadeSpeed = 0.4f;
	float targetOpacity = 0.2f;
	Coroutine fader;

	void Start () {
		transform.position = Camera.main.transform.position + (Camera.main.transform.forward * distanceFromCamera);
		transform.rotation = Camera.main.transform.rotation;
	}

	void Update () {
		if (Input.GetMouseButtonDown(1)) {
			if (opacity != targetOpacity) {
				if (fader != null) StopCoroutine(fader);
				fader = StartCoroutine(Fade(targetOpacity));
			}
		}

		if (Input.GetMouseButtonUp(1)) {
			if (opacity != 0f) {
				if (fader != null) StopCoroutine(fader);
				fader = StartCoroutine(Fade(0f));
			}
		}

		GetComponent<CanvasGroup>().alpha = opacity;
	}

	void FixedUpdate () {
		Vector3 targetPosition = Camera.main.transform.position + (Camera.main.transform.forward * distanceFromCamera);
		Vector3 newPosition = transform.position;

		newPosition.x = ease(newPosition.x, targetPosition.x, followSpeed);
		newPosition.y = ease(newPosition.y, targetPosition.y, followSpeed);
		newPosition.z = ease(newPosition.z, targetPosition.z, followSpeed);

		transform.position = newPosition;
		transform.rotation = Camera.main.transform.rotation;
	}

	float ease (float val, float target, float ease) {
		float difference = target - val;
		difference *= ease;
		return val + difference;
	}

	IEnumerator Fade (float target) {
		while (Mathf.Abs(opacity - target) > 0.01f) {
			yield return new WaitForSeconds(0.01f);
			opacity = ease(opacity, target, fadeSpeed);
		}

		opacity = target;
	}
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterBox : MonoBehaviour {

	//parameters
	public float aspectRatio = 2f;
	public float aspecRatioChangeSpeed = 4f;

	//internal
	private float currentAspect;
	private Coroutine letterBox;

	//components
	private RectTransform lowPanel;
	private RectTransform highPanel;
	private Camera cam;

	void Start() {
		//get components
		lowPanel = GameObject.Find("LowPanel").GetComponent<RectTransform>();
		highPanel = GameObject.Find("HighPanel").GetComponent<RectTransform>();
		cam = GameObject.Find("Camera").GetComponent<Camera>();

		currentAspect = cam.aspect;
		forceAspectRatio(cam.aspect);
	}

	void Update() {
		if (Input.GetMouseButtonDown(1)) {
			if (letterBox != null) StopCoroutine(letterBox);
			letterBox = StartCoroutine(changeAspectRatio(aspectRatio));
		}

		if (Input.GetMouseButtonUp(1)) {
			if (letterBox != null) StopCoroutine(letterBox);
			letterBox = StartCoroutine(changeAspectRatio(cam.aspect));
		}

		//override when flying
		if (Nox.player.GetComponent<PlayerMove>().flying) {
			if (letterBox != null) StopCoroutine(letterBox);
			if (currentAspect != cam.aspect) currentAspect = Mathf.Lerp(currentAspect, cam.aspect, aspecRatioChangeSpeed / 4f * Time.deltaTime);
			forceAspectRatio(currentAspect);
		}
	}

	//force aspect ratio with letterboxing
	void forceAspectRatio(float ratio) {
		if (cam.aspect <= 1.1f) return;

		float variance = (ratio / cam.aspect) - 1f;

		Vector2 resize = new Vector2(Screen.width + 10f, (variance * Screen.height) / 2f);

		lowPanel.sizeDelta = resize;
		lowPanel.anchoredPosition = new Vector2(0f, (lowPanel.rect.height / 2f) - 1f);

		highPanel.sizeDelta = resize;
		highPanel.anchoredPosition = new Vector2(0f, (-highPanel.rect.height / 2f) + 1f);
	}

	IEnumerator changeAspectRatio(float desiredRatio) {
		while (currentAspect != desiredRatio) {
			yield return new WaitForSeconds(0.01f);
			if (Mathf.Abs(currentAspect - desiredRatio) < 0.01f) currentAspect = desiredRatio;
			else {
				currentAspect = Mathf.Lerp(currentAspect, desiredRatio, aspecRatioChangeSpeed * Time.deltaTime);
				forceAspectRatio(currentAspect);
			}
		}
	}
}
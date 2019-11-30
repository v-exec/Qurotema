using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonolithBehavior : MonoBehaviour {

	private Story s;
	private Nox n;
	private Sound soundSystem;

	public GameObject tear;
	public CanvasGroup canvas;
	public Image text;
	public bool active = false;

	void Start() {
		s = GameObject.Find("Nox").GetComponent<Story>();
		n = GameObject.Find("Nox").GetComponent<Nox>();
		soundSystem = GameObject.Find("Nox").GetComponent<Sound>();
	}

	public void makeActive() {
		active = true;

		Sprite t = s.monolithTexts[s.monolithsRead];
		s.monolithsRead++;
		text.sprite = t;

		GetComponent<Renderer>().enabled = true;
		tear.GetComponent<Renderer>().enabled = true;
		canvas.alpha = 1f;

		soundSystem.addEnergy(5f);
		soundSystem.shootSound("sparkles");

		n.flashFeedback();
	}
}
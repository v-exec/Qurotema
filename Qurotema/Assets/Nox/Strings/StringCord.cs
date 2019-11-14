﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringCord : MonoBehaviour {

	public Vector3 start;
	public Vector3 end;
	public float amplitude;
	public float frequency;

	private AudioSource sound;
	private BoxCollider collider;
	private Material mat;
	private float distance;
	private float offset = 0f;
	private float offsetSpeed = 5f;

	private bool ready = true;
	private float waitTime = 1f;
	private float ringTick = 0.01f;
	private float sustainDecay = 0.0006f;

	IEnumerator ringRoutine;

	void Start () {
		sound = GetComponent<AudioSource>();
		collider = GetComponent<BoxCollider>();
		mat = GetComponent<MeshRenderer>().material;
		ringRoutine = Ring();
	}

	public void init (Vector3 s, Vector3 e) {
		start = s;
		end = e;
		distance = Vector3.Distance(s, e);
		frequency = distance * 30f;
	}
	
	public void playSound(float strength) {
		if (ready) {
			ready = false;
			amplitude = strength / 5f;
			//clamp amplitude to avoid extreme vibration / lack thereof
			if (amplitude > 0.05f) amplitude = 0.05f;
			if (amplitude < 0.01f) amplitude = 0.01f;

			StopCoroutine(ringRoutine);
			ringRoutine = Ring();
			StartCoroutine(ringRoutine);
			StartCoroutine(Refresh());
		}
	}

	IEnumerator Refresh() {
		yield return new WaitForSeconds(waitTime);
		ready = true;
	}

	IEnumerator Ring() {
		float f = frequency;
		float a = amplitude;

		while (a > 0f) {
			yield return new WaitForSeconds(ringTick);

			offset += offsetSpeed;
			mat.SetFloat("_Offset", offset);
			mat.SetFloat("_Amplitude", a);
			mat.SetFloat("_Frequency", f);
			a -= sustainDecay;

		}
	}
}
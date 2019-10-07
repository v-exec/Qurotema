using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkysphereRotate : MonoBehaviour {
	
	float rotateSpeed;
	float threshold;
	float thresholdNoiseSeed;
	float thresholdSpeed = 0.004f;

	void Start() {
		rotateSpeed = Random.Range(0.1f, 1f);
		if (Random.Range(0f, 1f) > 0.5f) rotateSpeed *= -1;
		thresholdNoiseSeed = Random.Range(0f, 1000f);
	}

	void Update() {
		gameObject.GetComponent<MeshRenderer>().material.SetVector("_SunLocation", Nox.sun.transform.position);
	}

	void FixedUpdate() {
		gameObject.transform.Rotate(0, 0, rotateSpeed * 0.02f, Space.Self);
		thresholdNoiseSeed += thresholdSpeed;
		gameObject.GetComponent<MeshRenderer>().material.SetFloat("_AlphaClip", remap(Mathf.PerlinNoise(thresholdNoiseSeed, 0), 0f, 1f, 0.035f, 0.15f));
	}

	float remap(float s, float a1, float a2, float b1, float b2) {
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}
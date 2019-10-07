using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class SkyboxRotate : MonoBehaviour {

	private Volume pp;
	public HDRISky sky;
	public float rotateSpeed;
	private float rotate = 0;

	void Start() {
		pp = gameObject.GetComponent<Volume>();
		HDRISky temp;
		if (pp.profile.TryGet<HDRISky>(out temp)) sky = temp;
	}

	void FixedUpdate() {
		rotate += rotateSpeed;
		if (rotate > 360) rotate = 0;
		sky.rotation.value = rotate;
	}
}

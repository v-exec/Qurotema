using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OribitingSun : MonoBehaviour {

	float orbitSpeed = 0.01f;
	
	void FixedUpdate () {
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + orbitSpeed, transform.localEulerAngles.z);
	}
}

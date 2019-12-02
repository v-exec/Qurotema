using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OribitingSun : MonoBehaviour {

	public float orbitSpeed = 1.2f;
	
	void Update () {
		transform.Rotate(0.0f, orbitSpeed * Time.deltaTime, 0.0f, Space.Self);
	}
}
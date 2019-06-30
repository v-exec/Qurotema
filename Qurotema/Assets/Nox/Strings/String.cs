using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class String : MonoBehaviour {

	GameObject firstPeer;
	GameObject secondPeer;
	AudioSource sound;
	float distance;

	void Start () {
		distance = Vector3.Distance(firstPeer.transform.position, secondPeer.transform.position);
		sound = GetComponent<AudioSource>();
	}
	
	void playSound() {

	}
}
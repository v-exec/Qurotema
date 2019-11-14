using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClip : MonoBehaviour {

	public bool isOneShot = false;
	public AudioClip[] clips;
	public AudioSource source;
	public string name;

	public void init(bool oneShot, AudioClip[] aud, string n) {
		isOneShot = oneShot;
		clips = aud;
		name = n;

		//create audio source
	}

	//start loopable audio
	public void kickstart() {
		//make clip loopable

		//start clip
	}

	//play random clip from array unless specified
	public void shootClip(int index = 100) {
		if (index == 100) {

		} else {

		}
	}

	IEnumerator toggleSound(AudioSource a, bool toggleFlag) {
		yield return new WaitForSeconds(1f);
	}
}

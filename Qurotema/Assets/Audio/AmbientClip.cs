using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientClip : MonoBehaviour {

	public string name;
	private AudioClip clip;
	private AudioSource source;

	void Awake() {
		source = GetComponent<AudioSource>();
	}

   	public void init(AudioClip audio, string n) {
   		clip = audio;
   		name = n;

   		source.clip = clip;
   		source.volume = 0f;
   	}

   	public void kickstart() {
   		source.Play();
   	}
}

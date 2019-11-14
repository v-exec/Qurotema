using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

	private float energy = 20f; //range: 0 - 100
	private float eneryFalloff = 1f;
	private bool atRoot = false;
	private SoundClip[] soundClips;
	
   	[System.Serializable]
   	public class Sonic { public bool oneShot; public AudioClip[] audios; public string name; }
   	public Sonic[] sounds;

	void Start() {
		//create sound clips from editor data

	}

	void Update() {
		//calculate energy falloff
		energy -= eneryFalloff * Time.deltaTime;

		//change note at random point in time

		if (energy > 60f) highEnergy();
		else if (energy > 10f) lowEnergy();
		else theme();
	}

	//play low energy set of ambient sounds
	private void lowEnergy() {

	}

	//play high energy set of ambient sounds
	private void highEnergy() {

	}

	//at random point in time, play theme
	private void theme() {

	}

	public void turnOffSound(string sound, float speed) {


	}

	//find clip and toggle sound
	public void turnOnSound(string sound, float speed) {


	}

	//play one-shot of soundclip
	public void shootSound(string sound) {

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

	public float energy = 20f; //range: 0 - 100
	private float eneryFalloff = 1f;
	private bool atRoot = false;
	private DynamicClip[] dynamicClips;
	private AmbientClip[] ambientClips;

	public GameObject ambientSoundEmitter;
	public GameObject dynamicSoundEmitter;
	
	[System.Serializable]
	public class Ambient { public AudioClip audio; public string name; }
	public Ambient[] ambiences;

   	[System.Serializable]
   	public class Sonic { public bool oneShot; public AudioClip[] audiosLo; public AudioClip[] audiosHi; public string name; }
   	public Sonic[] dynamics;

	void Start() {
		//create sound clips from editor data
		ambientClips = new AmbientClip[ambiences.Length];

		for (int i = 0; i < ambientClips.Length; i++) {
			GameObject temp = Instantiate(ambientSoundEmitter, Camera.main.transform);
			ambientClips[i] = temp.GetComponent<AmbientClip>();
			ambientClips[i].init(ambiences[i].audio, ambiences[i].name);
		}

		dynamicClips = new DynamicClip[dynamics.Length];

		for (int i = 0; i < dynamicClips.Length; i++) {
			GameObject temp = Instantiate(dynamicSoundEmitter, Camera.main.transform);
			dynamicClips[i] = temp.GetComponent<DynamicClip>();
			dynamicClips[i].init(dynamics[i].oneShot, dynamics[i].audiosLo, dynamics[i].audiosHi, dynamics[i].name);
		}

		//kickstart loop clips
		for (int i = 0; i < dynamicClips.Length; i++) {
			if (!dynamicClips[i].isOneShot) dynamicClips[i].kickstart();
		}

		for (int i = 0; i < ambientClips.Length; i++) {
			ambientClips[i].kickstart();
		}
	}

	void Update() {
		//calculate energy falloff
		energy -= eneryFalloff * Time.deltaTime;
		if (energy < 0f) energy = 0f;

		//change note at random point in time


		//play appropriate sound set for different energy
		if (energy > 60f) highEnergy();
		else if (energy > 5f) lowEnergy();
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

	public void addEnergy(float amount) {
		energy += amount * Time.deltaTime;
	}

	//toggle dynamic sound
	public void toggleSound(string sound, bool on, float speed = 0.1f) {
		if (energy > 60f) findDynamic(sound).toggleSound(on, speed, true);
		else findDynamic(sound).toggleSound(on, speed, false);
	}

	//toggle ambient sound
	public void ambienceToggle(string sound, bool on, float speed = 1f) {

	}

	//play one-shot of soundclip
	public void shootSound(string sound, int index = 100) {
		findDynamic(sound).shootClip(index);
	}

	private DynamicClip findDynamic(string sound) {
		for (int i = 0; i < dynamicClips.Length; i++) {
			if (dynamicClips[i].name == sound) return dynamicClips[i];
		}

		//too lazy to do proper exceptions
		Debug.Log("couldn't find dynamic sound");
		return new DynamicClip();
	}

	private AmbientClip findAmbient(string sound) {
		for (int i = 0; i < ambientClips.Length; i++) {
			if (ambientClips[i].name == sound) return ambientClips[i];
		}

		//too lazy to do proper exceptions
		Debug.Log("couldn't find ambient sound");
		return new AmbientClip();
	}
}
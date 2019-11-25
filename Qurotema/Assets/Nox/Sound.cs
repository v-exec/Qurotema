﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

	public float energy = 20f; //range: 0 - 100

	private float eneryFalloff = 1f;
	private int energyState = 1;
	private float highEnergyTreshold = 60f;
	private float lowEnergyThreshold = 10f;
	private bool atRoot = false;
	private bool playingTheme = false;

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

		findAmbient("theme").source.loop = false;
		findAmbient("theme").source.Stop();
		findAmbient("theme").source.volume = 1f;

		//activate ambience clips
		ambienceToggle("bass Em", true);
		ambienceToggle("bass ambience Em", true);
	}

	void FixedUpdate() {
		//change chord at random point in time (unless playing theme)
		if (!findAmbient("theme").source.isPlaying) changeChord();

		//play theme if low energy
		if (energy < lowEnergyThreshold && energyState != 0) {
			energyState = 0;
			ambienceToggle("vocals", true);
		}

		if (energy < lowEnergyThreshold) {
			theme();
		}
	}

	void Update() {
		//calculate energy falloff
		energy -= eneryFalloff * Time.deltaTime;
		energy = Mathf.Clamp(energy, 0f, 100f);

		//play appropriate sound set for different energy
		if (energy > highEnergyTreshold) {
			if (energyState != 2) {
				energyState = 2;
				ambienceToggle("clicks", true);
				if (atRoot) ambienceToggle("chord ambience CM", true);
				else ambienceToggle("chord ambience Em", true);

				//if dynamic clip is playing lo sound, switch to hi
				for (int i = 0; i < dynamicClips.Length; i++) {
					if (dynamicClips[i].playingLo) dynamicClips[i].toggleSound(true, 0.1f, true);
				}
			}
		} else if (energy > lowEnergyThreshold) {
			if (energyState != 1) {
				energyState = 1;
				ambienceToggle("clicks", false);
				ambienceToggle("chord ambience CM", false);
				ambienceToggle("chord ambience Em", false);
				ambienceToggle("vocals", false);

				//if dynamic clip is playing hi sound, switch to lo
				for (int i = 0; i < dynamicClips.Length; i++) {
					if (dynamicClips[i].playingHi) dynamicClips[i].toggleSound(true, 0.1f, false);
				}
			}
		}

		//force start on Em when theme is done playing
		if (playingTheme && !findAmbient("theme").source.isPlaying) {
			atRoot = false;
			playingTheme = false;
			ambienceToggle("bass Em", true);
			ambienceToggle("bass ambience Em", true);
			ambienceToggle("vocals", true, 0.5f);
		}
	}

	private void changeChord() {
		float chance = Random.Range(0f, 1f);
		if (chance < 0.001f) {
			atRoot = !atRoot;

			if (atRoot) {
				ambienceToggle("bass CM", true);
				ambienceToggle("bass ambience CM", true);

				ambienceToggle("bass Em", false);
				ambienceToggle("bass ambience Em", false);

				if (energyState == 2) {
					ambienceToggle("chord ambience Em", false);
					ambienceToggle("chord ambience CM", true);
				}
			} else {
				ambienceToggle("bass Em", true);
				ambienceToggle("bass ambience Em", true);

				ambienceToggle("bass CM", false);
				ambienceToggle("bass ambience CM", false);

				if (energyState == 2) {
					ambienceToggle("chord ambience CM", true);
					ambienceToggle("chord ambience Em", false);
				}
			}
		}
	}

	//at random point in time, play theme as one-shot
	private void theme() {
		float chance = Random.Range(0f, 1f);
		if (chance < 0.0003f && !findAmbient("theme").source.isPlaying) {
			ambienceToggle("bass Em", false);
			ambienceToggle("bass CM", false);
			ambienceToggle("bass ambience Em", false);
			ambienceToggle("bass ambience CM", false);
			ambienceToggle("vocals", false, 0.5f);
			findAmbient("theme").source.PlayOneShot(findAmbient("theme").clip);
			playingTheme = true;
		}
	}

	public void addEnergy(float amount) {
		energy += amount * Time.deltaTime;
	}

	//toggle dynamic sound
	public void dynamicToggle(string sound, bool on, float speed = 0.1f) {
		if (energy > highEnergyTreshold) findDynamic(sound).toggleSound(on, speed, true);
		else findDynamic(sound).toggleSound(on, speed, false);
	}

	//toggle ambient sound
	public void ambienceToggle(string sound, bool on, float speed = 0.1f) {
		findAmbient(sound).toggleSound(on, speed);
	}

	//play one-shot of soundclip
	public void shootSound(string sound, int index = 100) {
		if (energy > highEnergyTreshold) findDynamic(sound).shootClip(index, true);
		else findDynamic(sound).shootClip(index, false);
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
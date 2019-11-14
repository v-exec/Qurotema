using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

	private float energy = 20f; //range: 0 - 100
	private float eneryFalloff = 1f;
	private bool atRoot = false;
	private SoundClip[] soundClips;

	public GameObject soundEmitter;
	
   	[System.Serializable]
   	public class Sonic { public bool oneShot; public AudioClip[] audios; public string name; }
   	public Sonic[] sounds;

	void Start() {
		//create sound clips from editor data
		soundClips = new SoundClip[sounds.Length];

		for (int i = 0; i < soundClips.Length; i++) {
			GameObject temp = Instantiate(soundEmitter, Camera.main.transform);
			soundClips[i] = temp.GetComponent<SoundClip>();
			soundClips[i].init(sounds[i].oneShot, sounds[i].audios, sounds[i].name);
		}

		//kickstart loop clips
		for (int i = 0; i < soundClips.Length; i++) {
			if (!soundClips[i].isOneShot) soundClips[i].kickstart();
		}
	}

	void Update() {
		//calculate energy falloff
		energy -= eneryFalloff * Time.deltaTime;

		//change note at random point in time


		//play appropriate sound set for different energy
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

	public void modSound(string sound, float target, float speed) {
		findClip(sound).modSound(target, speed);
	}

	//play one-shot of soundclip
	public void shootSound(string sound, int index = 100) {
		findClip(sound).shootClip(index);
	}

	private SoundClip findClip(string sound) {
		for (int i = 0; i < soundClips.Length; i++) {
			if (soundClips[i].name == sound) return soundClips[i];
		}

		//too lazy to do proper exceptions
		return new SoundClip();
	}
}
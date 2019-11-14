using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
SoundClip is a class for playing sounds either as
	1. one-shots (either randomly or a select clip) from an array of clips
	2. loop whose volume can be modulated over time

These are separate functionalities as a this script is attached to an object with a single audio source.
This means that only one clip can be attached if the audio is to be looped.

In addition, a single-shot clip should never have its volume
modulated, as a single audio source can play multiple clips,
and a loop should never be toggled as a one-shot,
as it will be desynchronized from the rest of the track.
*/

public class SoundClip : MonoBehaviour {

	public string name;
	public bool isOneShot = false;
	private AudioClip[] clips;
	private AudioSource source;
	private Coroutine mod;

	void Start() {
		source = GetComponent<AudioSource>();
	}

	public void init(bool oneShot, AudioClip[] aud, string n) {
		isOneShot = oneShot;
		clips = aud;
		name = n;

		//fill audio source
		if (!isOneShot) {
			source.loop = true;
			source.clip = clips[0];
		}
	}

	//play random clip from array unless specified
	public void shootClip(int index = 100) {
		if (isOneShot) {
			if (index == 100) {
				int randomIndex = Random.Range(0, clips.Length);
				source.PlayOneShot(clips[randomIndex], 1f);
			} else source.PlayOneShot(clips[index], 1f);
		}
	}

	//start loop audio
	public void kickstart() {
		if (!isOneShot) {
			source.Play();
		}
	}

	public void modSound(float target, float speed) {
		if (!isOneShot) {
			if (mod != null) StopCoroutine(mod);
			mod = StartCoroutine(ModulateSound(target, speed));
		}
	}

	public bool isPlaying() {
		return source.isPlaying;
	}

	private IEnumerator ModulateSound(float target, float speed) {
		if (source.volume - target > 0) {
			while (source.volume > target) {
				yield return new WaitForSeconds(0.01f);
				source.volume -= speed * Time.deltaTime;
			}
		} else {
			while (source.volume < target) {
				yield return new WaitForSeconds(0.01f);
				source.volume += speed * Time.deltaTime;
			}
		}

		source.volume = target;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

	//drivers
	private float energy = 20f; //range: 0 - 100
	private float eneryFalloff = 1f;

	//samples
	public AudioSource[] ambiencesEm;
	public AudioSource[] ambiencesCM;
	
	public AudioSource[] clicks;

	public AudioSource[] dropletsLo;
	public AudioSource[] dropletsHi;

	public AudioSource sun;

	public AudioSource harmonyLo;
	public AudioSource harmonyHi;

	public AudioSource[] melodiesLo;
	public AudioSource[] melodiesHi;

	public AudioSource padLo;
	public AudioSource padHi;

	public AudioSource percussionLo;
	public AudioSource percussionHi;

	public AudioSource rhythmLo;
	public AudioSource rhythmHi;

	public AudioSource[] sparkles;

	public AudioSource vocalLo;
	public AudioSource vocalHi;

	public AudioSource[] whipsLo;
	public AudioSource[] whipsHi;

	public AudioSource whispers;

	public AudioSource theme;

	//instruments
	public AudioSource stringsSnap;
	public AudioSource stringsStart;
	public AudioSource stringsEnd;
	public AudioSource[] stringsPlay;

	void Start() {
		//samples
		ambiencesEm = new AudioSource[3];
		ambiencesCM = new AudioSource[3];
		
		clicks = new AudioSource[2];

		dropletsLo = new AudioSource[5];
		dropletsHi = new AudioSource[5];

		sun = new AudioSource();

		harmonyLo = new AudioSource();
		harmonyHi = new AudioSource();

		melodiesLo = new AudioSource[13];
		melodiesHi = new AudioSource[13];

		padLo = new AudioSource();
		padHi = new AudioSource();

		percussionLo = new AudioSource();
		percussionHi = new AudioSource();

		rhythmLo = new AudioSource();
		rhythmHi = new AudioSource();

		sparkles = new AudioSource[3];

		vocalLo = new AudioSource();
		vocalHi = new AudioSource();

		whipsLo = new AudioSource[4];
		whipsHi = new AudioSource[4];

		whispers = new AudioSource();

		theme = new AudioSource();

		//instruments
		stringsSnap = new AudioSource();
		stringsStart = new AudioSource();
		stringsEnd = new AudioSource();
		stringsPlay = new AudioSource[7];
	}

	void Update() {
		energy -= eneryFalloff * Time.deltaTime;

		if (energy > 60f) lowEnergy();
		else highEnergy();
	}

	private void lowEnergy() {

	}

	private void highEnergy() {

	}

	IEnumerator toggleSoundSlowly(AudioSource a, bool toggleFlag) {
		yield return new WaitForSeconds(1f);
	}

	IEnumerator toggleSoundQuickly(AudioSource audio) {
		yield return new WaitForSeconds(1f);
	}
}
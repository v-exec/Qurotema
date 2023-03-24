using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Story : MonoBehaviour {

	//media
	private Text storyText;
	private GameObject storyTextCanvas;
	private GameObject storyBackground;
	private float backgroundOpacity = 0f;
	public GameObject sun;
	public GameObject gates;
	private Sound soundSystem;
	private Nox n;

	//states
	public bool introductionFinished = false;

	//trackers
	public int monolithsRead = 0;
	public int stringsPlayed = 0;
	public int ringsPlayed = 0;
	public int padsPlayed = 0;

	//monolith texts
	public Sprite[] monolithTexts;

	//text
	private string[] talk;
	private int talkTracker = 0;
	private int randomTracker = 0;
	private Coroutine routine;

	void Start() {
		storyText = GameObject.Find("Story - Text/Text").GetComponent<Text>();
		storyTextCanvas = GameObject.Find("Story - Text");
		storyBackground = GameObject.Find("Story - Background");
		soundSystem = GetComponent<Sound>();
		n = GetComponent<Nox>();

		sun.SetActive(false);
		gates.SetActive(true);

		talk = new string[20];

		//intro
		talk[0] = "We have a stable port to Qurotema. RO, your vision should come in soon.";
		talk[1] = "We should remind you that because of the Gates, this is only a one-way communication channel.";
		talk[2] = "We will keep watch, and study Qurotema alongside you, Operator.";
		talk[3] = "Your vessel should be equipped with a control module.\nWe trust you will discover how it interacts with this world in time.";
		talk[4] = "Good luck with your research, RO. Make Sino proud.";

		//first monolith discovery
		talk[5] = "What you see in front of you is one of the Monoliths Dr. Sino spoke about. There should be a writing 'eminating' from it. Do you see anything?";

		//first monolith interaction
		talk[6] = "We have a feed of the characters. Sino called this script 'i-tema'. He claims there's no way to speak it, as it is a written-only language.";
		talk[7] = "This set of symbols in particular were already translated by Sino. They say: 'The night will flow when black time's sand knows foreign touch.'";
		talk[8] = "Nothing else was translated other than the writing on the gates, 'Quro' and 'Tema'.";
		talk[9] = "We are not sure how Sino felt so confident in his understanding of i-tema, but finding more Monoliths will help us fill a database for linguistic analysis.";

		//first instrument discovery
		talk[10] = "What is that? Nothing like that was documented in Sino's notes. It's not coming in clear.";

		//instrument playtime story progress
		talk[11] = "We have signals of faint acitivity away from your vessel's current position.";
		talk[12] = "We've lost sight of your feed, but we are still getting readings about your position. Keep up the research.";
		talk[13] = "Our connection is becoming unreliable. We are going to pull you out soon.";

		//ending
		talk[14] = "We are losing connection fast. Abandoning vessel.";
		talk[15] = "RO? Di? Your body in the lab is unresponsive. Initiate return protocol immediately so we can resuscitate.";
		talk[16] = "Di, I don't know what's going on but we have no connection to Qurotema, can't even find it with Sino's systems.";
		talk[17] = "It's possible that Qurotema is gone, and your vessel is in the Void.";
		talk[18] = "I'm going to be honest. I don't know what we're going to do.";
		talk[19] = "";

		if (!introductionFinished) {
			backgroundOpacity = 1f;
			storyBackground.GetComponent<CanvasGroup>().alpha = backgroundOpacity;
			talkTracker = 0;
			if (routine != null) StopCoroutine(routine);
			routine = StartCoroutine(PlayText(talkTracker));
		} else {
			//skip intro
			sun.SetActive(true);
			gates.SetActive(false);
		}
	}

	//set methods
	public void monolithDiscovered() {
		talkTracker = 5;
		if (routine != null) StopCoroutine(routine);
		routine = StartCoroutine(PlayText(talkTracker));
	}

	public void monolithActivated() {
		if (monolithsRead == 0) {
			talkTracker = 6;
			if (routine != null) StopCoroutine(routine);
			routine = StartCoroutine(PlayText(talkTracker));
		}

		monolithsRead++;
	}

	public void stringPlayed() {
		checkForInstrumentDiscovery();

		stringsPlayed++;
		if (stringsPlayed == 50) randomMessage();
	}

	public void ringPlayed() {
		checkForInstrumentDiscovery();

		ringsPlayed++;
		if (ringsPlayed == 40) randomMessage();
	}

	public void padPlayed() {
		checkForInstrumentDiscovery();

		padsPlayed++;
		if (padsPlayed == 30) randomMessage();
	}

	public void endGame() {
		talkTracker = 14;
		if (routine != null) StopCoroutine(routine);
		routine = StartCoroutine(PlayText(talkTracker));
	}

	private void randomMessage() {
		switch (randomTracker) {
			case 0:
				talkTracker = 11;
				break;

			case 1:
				talkTracker = 12;
				break;

			case 2:
				talkTracker = 13;
				break;
		}

		randomTracker++;

		soundSystem.shootSound("sparkles");
		n.terrain.flashFeedback();
		if (routine != null) StopCoroutine(routine);
		routine = StartCoroutine(PlayText(talkTracker));
	}

	private void checkForInstrumentDiscovery() {
		if (stringsPlayed == 0 && ringsPlayed == 0 && padsPlayed == 0) {
			talkTracker = 10;
			if (routine != null) StopCoroutine(routine);
			routine = StartCoroutine(PlayText(talkTracker));
		}
	}

	IEnumerator PlayText(int id) {
		//special actions
		switch (id) {
			//wait extra second for intro
			case 0:
				yield return new WaitForSeconds(2f);
				break;

			//allow vision
			case 1:
				StartCoroutine(FadeBackground(0f));
				sun.SetActive(true);
				break;

			//remove gates, allow movement
			case 2:
				gates.SetActive(false);
				introductionFinished = true;
				break;

			//show gates and remove map limit
			case 13:
				gates.SetActive(true);
				gates.GetComponent<GatesStory>().activateEnd();
				GameObject.Find("Map Limit (front)").SetActive(false);
				break;

			//slow player down and fade screen
			case 14:
				GameObject.Find("Player").GetComponent<PlayerMove>().sprintSpeed = 0.1f;
				GameObject.Find("Player").GetComponent<PlayerMove>().walkSpeed = 0.05f;
				StartCoroutine(FadeBackground(1f));
				break;

			//stop music
			case 15:
				soundSystem.silence();
				break;

			//end game
			case 19:
				Application.Quit();
				break;
		}

		float opacity = 0f;
		storyTextCanvas.GetComponent<CanvasGroup>().alpha = opacity;
		storyText.text = talk[id];

		while (opacity < 0.99f) {
			yield return new WaitForSeconds(0.01f);
			opacity += 0.01f;
			storyTextCanvas.GetComponent<CanvasGroup>().alpha = opacity;
		}

		yield return new WaitForSeconds(2f);

		while (opacity > 0.01f) {
			yield return new WaitForSeconds(0.01f);
			opacity -= 0.01f;
			storyTextCanvas.GetComponent<CanvasGroup>().alpha = opacity;
		}

		bool end = false;
		switch (talkTracker) {
			case 4:
			case 5:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 19:
				end = true;
				break;
		}

		talkTracker++;

		yield return new WaitForSeconds(2f);

		if (!end) StartCoroutine(PlayText(talkTracker));
		else storyText.text = "";
	}

	IEnumerator FadeBackground(float target) {
		while (Mathf.Abs(backgroundOpacity - target) > 0.02f) {
			yield return new WaitForSeconds(0.01f);
			backgroundOpacity = Nox.ease(backgroundOpacity, target, 0.1f);
			storyBackground.GetComponent<CanvasGroup>().alpha = backgroundOpacity;
		}

		backgroundOpacity = target;
		storyBackground.GetComponent<CanvasGroup>().alpha = backgroundOpacity;
	}
}
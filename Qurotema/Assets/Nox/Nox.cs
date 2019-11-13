using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nox : MonoBehaviour {

	//instruments
	static public GameObject player;
	static public GameObject sun;
	public Vector3 playerPosition;
	public List<String> strings = new List<String>();
	public float energy = 0f;

	//terrain manipulation
	private float waveHeightSpeed = 0.1f;
	private float waveHeightPerlin = 0f;
	static public GameObject flyPoint;

	private float[] mixers = new float[5];
	private float[] emissions = new float[2];
	private float[] smoothnesses = new float[1];
	private float mixerSpeed = 0.1f;

	//terrain material
	private Material terrainMaterial;

	void Start () {
		player = GameObject.Find("Player");
		flyPoint = GameObject.Find("FlyPoint");
		sun = GameObject.Find("SunSphere");
		playerPosition = player.transform.position;
		terrainMaterial = GameObject.Find("Terrain").GetComponent<MeshRenderer>().material;

		for (int i = 0; i < emissions.Length; i++) {
			emissions[i] = 0f;
		}

		//noise seeds
		waveHeightPerlin = Random.Range(0f, 1000f);

		for (int i = 0; i < mixers.Length; i++) {
			mixers[i] = Random.Range(0f, 1000f);
		}

		for (int i = 0; i < smoothnesses.Length; i++) {
			smoothnesses[i] = Random.Range(0f, 1000f);
		}
	}

	void Update () {
		playerPosition = player.transform.position;

		animateTerrain();
	}

	private void animateTerrain() {
		//calculate wave height
		waveHeightPerlin += waveHeightSpeed * Time.deltaTime;
		terrainMaterial.SetFloat("_WaveStrength", Mathf.PerlinNoise(waveHeightPerlin, 0f));

		//move flypoint upwards for fading effect
		if (flyPoint.transform.position.y < 500) flyPoint.transform.Translate((Vector3.up * 30) * Time.deltaTime);

		//communicate flypoint to terrain shader
		terrainMaterial.SetVector("_FlyPoint", flyPoint.transform.position);

		//update mixers
		//remap mixers from range to -1 to 2, and then clamp 0-1 so that mixers are typically in either extreme
		for (int i = 0; i < mixers.Length; i++) {
			mixers[i] += mixerSpeed * Time.deltaTime;
			
			float mix = Mathf.PerlinNoise(mixers[i], 0f);
			mix = remap(mix, 0, 1, -1, 2);
			mix = Mathf.Clamp(mix, 0, 1);

			terrainMaterial.SetFloat("_Blend" + i, mix);
		}

		for (int i = 0; i < smoothnesses.Length; i++) {
			smoothnesses[i] += mixerSpeed * Time.deltaTime;

			float smooth = Mathf.PerlinNoise(smoothnesses[i], 0f);
			smooth = remap(smooth, 0, 1, -1, 2);
			smooth = Mathf.Clamp(smooth, 0, 1);

			terrainMaterial.SetFloat("_SmoothBlend" + i, smooth);
		}
	}

	//feedback
	public void flashFeedback() {
		
	}

	//strings
	public void addString(GameObject o) {
		strings.Add(o.GetComponent<String>());
	}

	public bool stringExists(Vector3 s, Vector3 e) {
		for (int i = 0; i < strings.Count; i++) {
			if (strings[i].start == s && strings[i].end == e) return true;
			if (strings[i].start == e && strings[i].end == s) return true;
		}
		return false;
	}

	//audio


	//statics
	public static float ease(float val, float target, float ease) {
		if (val == target) return val;

		float difference = target - val;
		return val += ((difference * ease) * Time.deltaTime);
	}

	public static float remap(float val, float min1, float max1, float min2, float max2) {
		if (val < min1) val = min1;
		if (val > max1) val = max1;

		return (val - min1) / (max1 - min1) * (max2 - min2) + min2;
	}
}
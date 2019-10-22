using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nox : MonoBehaviour {

	//instruments
	static public GameObject player;
	static public GameObject sun;
	public Vector3 playerPosition;
	public List<String> strings = new List<String>();

	//terrain waves
	public float waveHeightSpeed = 0.1f;
	private float waveHeightPerlin = 0f;

	//terrain material
	private Material terrainMaterial;

	void Start () {
		player = GameObject.Find("Player");
		sun = GameObject.Find("SunSphere");
		playerPosition = player.transform.position;
		terrainMaterial = GameObject.Find("Terrain").GetComponent<MeshRenderer>().material;

		//perlin noise seed
		waveHeightPerlin = Random.Range(0f, 1000f);
	}

	void Update () {
		playerPosition = player.transform.position;

		moveWave();
	}

	private void moveWave() {
		//calculate wave height
		waveHeightPerlin += waveHeightSpeed * Time.deltaTime;
		terrainMaterial.SetFloat("_WaveStrength", Mathf.PerlinNoise(waveHeightPerlin, 0f));
	}

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
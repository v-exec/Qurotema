using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nox : MonoBehaviour {

	static public GameObject player;
	static public GameObject sun;
	public Vector3 playerPosition;
	public List<String> strings = new List<String>();

	public float waveHeightSpeed = 0.01f;
	public float waveOffsetSpeed = 0.02f;
	public float waveOffsetDampening = 0.0005f;

	private float waveHeightPerlin = 0f;
	private float waveOffsetPerlin = 0f;
	private float currentWaveOffset = 0f;

	private float voronoiOffset = 0f;
	public float voronoiOffsetSpeed = 0.1f;

	private Material terrainMaterial;
	private Material terrainOverheadMaterial;

	void Start () {
		player = GameObject.Find("Player");
		sun = GameObject.Find("SunSphere");
		playerPosition = player.transform.position;
		terrainMaterial = GameObject.Find("Terrain").GetComponent<MeshRenderer>().material;
		terrainOverheadMaterial = GameObject.Find("Terrain Overhead").GetComponent<MeshRenderer>().material;

		//perlin noise seed
		waveHeightPerlin = Random.Range(0f, 1000f);
		waveOffsetPerlin = Random.Range(0f, 1000f);
	}

	void Update () {
		playerPosition = player.transform.position;

		moveWave();
		displaceVoronoi();
	}

	private void displaceVoronoi() {
		voronoiOffset += voronoiOffsetSpeed;
		terrainMaterial.SetFloat("_VoronoiOffset", voronoiOffset);
	}

	private void moveWave() {
		//calculate wave height
		waveHeightPerlin += waveHeightSpeed * Time.deltaTime;
		terrainMaterial.SetFloat("_WaveStrength", Mathf.PerlinNoise(waveHeightPerlin, 0f));
		terrainOverheadMaterial.SetFloat("_WaveStrength", Mathf.PerlinNoise(waveHeightPerlin, 0f));

		//calculate wave offset
		waveOffsetPerlin += waveOffsetSpeed * Time.deltaTime;
		currentWaveOffset += Mathf.PerlinNoise(waveOffsetPerlin, 0f) * waveOffsetDampening;
		terrainMaterial.SetVector("_WaveOffset", new Vector4(0f, currentWaveOffset, 0f, 0f));
		terrainOverheadMaterial.SetVector("_WaveOffset", new Vector4(0f, currentWaveOffset, 0f, 0f));
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
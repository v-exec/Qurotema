using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nox : MonoBehaviour {

	static public GameObject player;
	public Vector3 playerPosition;

	public float waveHeightSpeed = 0.01f;
	public float waveOffsetSpeed = 0.02f;
	public float waveOffsetDampening = 0.0005f;

	private float waveHeightPerlin = 0f;
	private float waveOffsetPerlin = 0f;
	private float currentWaveOffset = 0f;

	private Material terrainMaterial;

	void Start () {
		player = GameObject.Find("Player");
		playerPosition = player.transform.position;
		terrainMaterial = GameObject.Find("Terrain").GetComponent<MeshRenderer>().material;

		//perlin noise seed
		waveHeightPerlin = Random.Range(0f, 1000f);
		waveOffsetPerlin = Random.Range(0f, 1000f);
	}

	void Update () {
		playerPosition = player.transform.position;

		moveWave();
	}

	void moveWave() {
		//calculate wave height
		waveHeightPerlin += waveHeightSpeed * Time.deltaTime;
		terrainMaterial.SetFloat("_WaveStrength", Mathf.PerlinNoise(waveHeightPerlin, 0f));

		//calculate wave offset
		waveOffsetPerlin += waveOffsetSpeed * Time.deltaTime;
		currentWaveOffset += Mathf.PerlinNoise(waveOffsetPerlin, 0f) * waveOffsetDampening;
		terrainMaterial.SetVector("_WaveOffset", new Vector4(0f, currentWaveOffset, 0f, 0f));
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
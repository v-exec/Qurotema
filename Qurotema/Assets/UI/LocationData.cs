using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationData : MonoBehaviour {

	Vector3 position;
	Text data;

	void Start () {
		position = GameObject.Find("System").GetComponent<Nox>().playerPosition;
		data = GetComponent<Text>();
	}
	
	void Update () {
		position = GameObject.Find("System").GetComponent<Nox>().playerPosition;
		data.text = position.x * 20 + "\n" + position.y * 20 + "\n" + position.z * 20;
	}
}
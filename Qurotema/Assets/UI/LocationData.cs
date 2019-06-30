﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationData : MonoBehaviour {

	Vector3 position;
	Text data;

	void Start () {
		position = GameObject.Find("Nox").GetComponent<Nox>().playerPosition;
		data = GetComponent<Text>();
	}
	
	void Update () {
		position = GameObject.Find("Nox").GetComponent<Nox>().playerPosition;
		var text = (position.x * 20).ToString("F2") + "\n" + (position.y * 20).ToString("F2") + "\n" + (position.z * 20).ToString("F2");
		data.text = text;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nox : MonoBehaviour {

	public GameObject player;
	public Vector3 playerPosition;

	void Start () {
		player = GameObject.Find("Player");
		playerPosition = player.transform.position;
	}

	void Update () {
		playerPosition = player.transform.position;
	}
}
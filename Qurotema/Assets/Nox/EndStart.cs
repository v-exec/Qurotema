using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStart : MonoBehaviour {

	private Story s;

    void Start() {
    	s = GameObject.Find("Nox").GetComponent<Story>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        s.endGame();
    }
}
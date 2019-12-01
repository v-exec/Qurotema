using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatesEnding : MonoBehaviour{
	public GameObject Quro;
	public GameObject Tema;

	void Start() {
		Quro.SetActive(false);
		Tema.SetActive(false);
	}

	public void activateEnd() {
		Quro.SetActive(true);
		Tema.SetActive(true);
	}
}
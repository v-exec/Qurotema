using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyFOV : MonoBehaviour {

	public Camera c;
	public Camera localC;

    void Update() {
       localC.fieldOfView = c.fieldOfView; 
    }
}
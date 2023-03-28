﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

	//mouse
	public float mouseSensitivity = 130f;
	public float clampAngle = 80f;
	public float easeSpeed = 2f;
	public float shakeSpeed = 1f;
	public float shakeQuantity = 1.4f;
	public float followSpeed = 8f;
 
	//internal mouse
	private float mouseX = 0f;
	private float mouseY = 0f;
	private float rotY = 0f;
	private float rotX = 0f;
	private float currentX = 0f;
	private float currentY = 0f;
	private float playerSpeed;

	//components
	private GameObject player;
	private Camera cam;

	//shake noise
	private float perlinX;
	private float perlinY;
	private float perlinZ;

	//story
	private bool ready = false;

	void Start() {
		//lock cursor
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		//get rotation
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
		currentY = rotY;
		currentX = rotX;

		//get perlin noise seed
		perlinX = Random.Range(0f, 1000f);
		perlinY = Random.Range(0f, 1000f);
		perlinZ = Random.Range(0f, 1000f);

		//get components
		player = GameObject.Find("Player");
		cam = gameObject.GetComponent<Camera>();

		//Application.targetFrameRate = 60;
	}

	void Update() {
		if (!ready) {
			if (GameObject.Find("Nox").GetComponent<Story>().introductionFinished) ready = true;
		}

		handleInput();
	}

	void FixedUpdate() {
		rotate();
		follow();
		shake();
	}

	void handleInput() {
		//get input
		mouseX = Input.GetAxis("Mouse X");
		mouseY = -Input.GetAxis("Mouse Y");

		if (!ready) {
			mouseX = 0;
			mouseY = 0;
		}

		//rotation manipulation (no need to scale by deltaTime as mouse axis are already frame deltas)
		rotY += mouseX * mouseSensitivity;
		rotX += mouseY * mouseSensitivity;
		rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
	}

	void rotate() {
		//ease rotation
		currentX = Mathf.Lerp(currentX, rotX, easeSpeed * Time.fixedDeltaTime);
		currentY = Mathf.Lerp(currentY, rotY, easeSpeed * Time.fixedDeltaTime);

		//apply rotation
		Quaternion localRotation = Quaternion.Euler(currentX, currentY, 0.0f);
		transform.rotation = localRotation;
	}

	void follow() {
		transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z), followSpeed * Time.deltaTime);
	}

	void shake() {
		//increment perlin 'cursor'
		perlinX += shakeSpeed * Time.fixedDeltaTime;
		perlinY += shakeSpeed * Time.fixedDeltaTime;
		perlinZ += shakeSpeed * Time.fixedDeltaTime;

		//remap to -1 to 1 and amplify according to shake quantity
		float x = Nox.remap(Mathf.PerlinNoise(perlinX, 0), 0f, 1f, -1f, 1f) * shakeQuantity;
		float y = Nox.remap(Mathf.PerlinNoise(perlinY, 0), 0f, 1f, -1f, 1f) * shakeQuantity;
		float z = Nox.remap(Mathf.PerlinNoise(perlinZ, 0), 0f, 1f, -1f, 1f) * shakeQuantity;

		//use player speed as subtraction to shake speed modifier
		//the faster the player moves, the less camera shake there is
		playerSpeed = player.GetComponent<PlayerMove>().getSpeed();

		float shakeSpeedModifier = 1f - (playerSpeed * 0.005f);
		if (shakeSpeedModifier < 0) shakeSpeedModifier = 0;

		//apply perlin noise as rotation
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + (x * shakeSpeedModifier), transform.localEulerAngles.y + (y * shakeSpeedModifier), transform.localEulerAngles.z + (z * shakeSpeedModifier));
	}
}
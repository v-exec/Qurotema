using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

	//mouse
	private float mouseSensitivity = 130.0f;
	private float clampAngle = 80.0f;
	private float easeSpeed = 1.5f;
	private float shakeSpeed = 1.0f;
	private float shakeQuantity = 1.4f;
	private float followSpeed = 5f;

	//internal mouse
	private float rotY = 0.0f;
	private float rotX = 0.0f;
	private float currentX = 0.0f;
	private float currentY = 0.0f;
	private GameObject player;
	private Rigidbody playerBody;
	private float playerSpeed;

	//shake noise
	private float perlinX;
	private float perlinY;
	private float perlinZ;

	void Start () {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;

		perlinX = Random.Range(0f, 1000f);
		perlinY = Random.Range(0f, 1000f);
		perlinZ = Random.Range(0f, 1000f);

		player = GameObject.Find("Player");
		playerBody = player.GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
		//get input
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = -Input.GetAxis("Mouse Y");

		//rotation manipulation
		rotY += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += mouseY * mouseSensitivity * Time.deltaTime;

		rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

		//ease rotation
		currentX = ease(currentX, rotX, easeSpeed);
		currentY = ease(currentY, rotY, easeSpeed);

		//apply rotation
		Quaternion localRotation = Quaternion.Euler(currentX, currentY, 0.0f);
		transform.rotation = localRotation;

		shake();

		follow();
	}

	float ease (float val, float target, float ease) {
		float difference = target - val;
		difference *= ease * Time.deltaTime;
		return val + difference;
	}

	void shake () {
		//increment perlin 'cursor'
		perlinX += shakeSpeed * Time.deltaTime;
		perlinY += shakeSpeed * Time.deltaTime;
		perlinZ += shakeSpeed * Time.deltaTime;

		//remap to -1 to 1 and amplify according to shake quantity
		float x = remap(Mathf.PerlinNoise(perlinX, 0), 0f, 1f, -1f, 1f) * shakeQuantity;
		float y = remap(Mathf.PerlinNoise(perlinY, 0), 0f, 1f, -1f, 1f) * shakeQuantity;
		float z = remap(Mathf.PerlinNoise(perlinZ, 0), 0f, 1f, -1f, 1f) * shakeQuantity;

		//use player speed as subtraction to shake speed modifier
		//the faster the player moves, the less camera shake there is
		playerSpeed = playerBody.velocity.magnitude;
		float shakeSpeedModifier = 1f - (playerSpeed * 0.005f);
		if (shakeSpeedModifier < 0) shakeSpeedModifier = 0;

		//apply perlin noise as rotation
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + (x * shakeSpeedModifier), transform.localEulerAngles.y + (y * shakeSpeedModifier), transform.localEulerAngles.z + (z * shakeSpeedModifier));
	}

	void follow() {
		float x = ease(transform.position.x, player.transform.position.x, followSpeed);
		float y = ease(transform.position.y, player.transform.position.y + 0.5f, followSpeed);
		float z = ease(transform.position.z, player.transform.position.z, followSpeed);
		transform.position = new Vector3(x, y, z);
	}

	float remap(float val, float min1, float max1, float min2, float max2) {
		return (val - min1) / (max1 - min1) * (max2 - min2) + min2;
	}
}
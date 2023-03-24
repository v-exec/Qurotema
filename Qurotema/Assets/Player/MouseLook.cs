using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

	//mouse
	public float mouseSensitivity = 130.0f;
	public float clampAngle = 80.0f;
	public float easeSpeed = 2.0f;
	public float shakeSpeed = 1.0f;
	public float shakeQuantity = 1.4f;
	public float followSpeed = 8f;
 
	//internal mouse
	private float rotY = 0.0f;
	private float rotX = 0.0f;
	private float currentX = 0.0f;
	private float currentY = 0.0f;
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
	}

	void Update() {
		if (!ready) {
			if (GameObject.Find("Nox").GetComponent<Story>().introductionFinished) ready = true;
		}

		handleInput();
		shake();
		follow();
	}

	void handleInput() {
		//get input
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = -Input.GetAxis("Mouse Y");

		if (!ready) {
			mouseX = 0;
			mouseY = 0;
		}

		//rotation manipulation
		rotY += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += mouseY * mouseSensitivity * Time.deltaTime;

		rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

		//ease rotation
		currentX = Nox.ease(currentX, rotX, easeSpeed);
		currentY = Nox.ease(currentY, rotY, easeSpeed);

		//apply rotation
		Quaternion localRotation = Quaternion.Euler(currentX, currentY, 0.0f);
		transform.rotation = localRotation;
	}

	void shake() {
		//increment perlin 'cursor'
		perlinX += shakeSpeed * Time.deltaTime;
		perlinY += shakeSpeed * Time.deltaTime;
		perlinZ += shakeSpeed * Time.deltaTime;

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

	void follow() {
		float x = Nox.ease(transform.position.x, player.transform.position.x, followSpeed);
		float y = Nox.ease(transform.position.y, player.transform.position.y + 0.5f, followSpeed);
		float z = Nox.ease(transform.position.z, player.transform.position.z, followSpeed);
		transform.position = new Vector3(x, y, z);
	}
}
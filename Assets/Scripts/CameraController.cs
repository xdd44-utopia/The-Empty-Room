using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private const float sensitivity = 5f;
	private float pitch = 0f;
	private float yaw = 0f;
	// Start is called before the first frame update
	void Start()
	{
		transform.position = new Vector3(0, 2.5f, 0);
	}

	// Update is called once per frame
	void Update()
	{
		pitch -= Input.GetAxis("Mouse Y") * sensitivity;
		pitch = (pitch <= 30 ? pitch : 30);
		pitch = (pitch >= -90 ? pitch : -90);
		yaw += Input.GetAxis("Mouse X") * sensitivity;
		yaw = Mathf.Repeat(yaw, 360);
		this.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
		if (Input.GetMouseButtonDown(0)) {
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
}

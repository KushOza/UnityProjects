using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MarioKartManager : NetworkBehaviour 
{	
	// Update is called once per frame
	void Update () 
	{
		if (!isLocalPlayer)
		{
			GetComponentInChildren<Camera> ().enabled = false;
			return;
		}

		float speed = 60.0f;
		Vector3 moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		moveDirection = transform.TransformDirection (moveDirection);
		moveDirection *= speed;
		this.GetComponentInChildren<CharacterController> ().Move (moveDirection * Time.deltaTime);

		/*
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);
		*/
	}

	void OnTriggerEnter(Collider other)
	{
		
	}
}

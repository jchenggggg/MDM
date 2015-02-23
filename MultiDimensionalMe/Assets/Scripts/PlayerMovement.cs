﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float moveSpeed;
	public float jumpSpeed;
	public float jumpTime;
	public float gravity = -40.0f;
	private bool jumping = false;
	public bool reversed;
	IEnumerator Jump() {
		yield return new WaitForSeconds (jumpTime);
		jumping = false;
		setVertSpeed (jumpSpeed/4);
	}


	// Use this for initialization
	void Start () {

		Camera.main.GetComponent<LockController> ().players.Add (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Cancel")) {
			Application.LoadLevel(Application.loadedLevelName);
		}

		if (jumping) {
			setVertSpeed(jumpSpeed);
		}
		if (IsGrounded() && reversed) {
			reversed = false;
			jumpSpeed = -jumpSpeed;
			print ("change jump direction");
		}
		if (Input.GetButtonDown("Jump")) {
			if (IsGrounded()) {
				//rigidbody.AddForce(Vector3.up * jumpSpeed);
				StartCoroutine("Jump");
				jumping = true;
				setVertSpeed(jumpSpeed);
			}
		}

		setHorizSpeed (Input.GetAxis("Horizontal") * moveSpeed);
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.GetComponent<LockSwitch>()) {
			lock_movement();
		}
		else if (coll.GetComponent<UnlockSwitch>()) {
			unlock_movement();
		}
		else if (coll.GetComponent<ReverseSwitch>()) {
			reverse_gravity();
		}
	}
	
	bool IsGrounded() {
		Cube[] cubes = GetComponentsInChildren<Cube> () as Cube[];
		foreach (Cube cube in cubes) {
			if (cube.IsGrounded())
				return true;
		}

		return false;
	}
	
	void setHorizSpeed(float x) {
		rigidbody.velocity = new Vector3(x, rigidbody.velocity.y, 0);
	}
	
	void setVertSpeed(float y) {
		rigidbody.velocity = new Vector3(rigidbody.velocity.x, y, 0);
	}

	void lock_movement() {
		Camera.main.GetComponent<LockController> ().lock_movement ();
	}

	void unlock_movement() {
		Camera.main.GetComponent<LockController> ().unlock_movement ();
	}

	void reverse_gravity() {
		gravity = -gravity;
		reversed = true;
		Physics.gravity = new Vector3(0, gravity, 0);
//		Quaternion newRotation;
//		if (gravity > 0)
//			newRotation = Quaternion.Euler(0,180,0);
//		else
//			newRotation = Quaternion.Euler(0,0,0);
		//transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime);
	}

}
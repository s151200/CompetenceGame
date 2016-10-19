﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public NavMeshAgent agent;
	public bool hatOn;
	public bool clockOn;
	public float speed = 3f;
	public float turnSmoothing = 3f;
	private float rotateAroundZ;
	public float rotateDegrees = 4f;
	private Animator animator;
	private bool isMoving;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
		animator = GetComponentInChildren<Animator>();
		animator.SetBool("NonCombat", true);
		animator.SetBool("Idling", true);
	}

	void Update() {
		// MOVING FORWARD
		if (Input.GetKey(KeyCode.UpArrow)) {
			// transform.forward is the forward direction in LOCAL SPACE and then the agent goes forward relative to the agent's angle.
			// Note: On the other hand Vector3.forward is in WORLD SPACE
			animator.SetBool("Idling", false); // Start moving
			transform.position += transform.forward * agent.speed * Time.deltaTime;
		}

		// STOP
		// Hammertime
		if (Input.GetKeyUp(KeyCode.UpArrow)) {
			animator.SetBool("Idling", true); // Stop moving
		}

		// ROTATING LEFT OR RIGHT
		RotatePlayer();
	}

	// Rotates target around the z-axis
	// using the left or right arrow keys
	private void RotatePlayer() {
		rotateAroundZ += Input.GetAxis("Horizontal") * rotateDegrees;
		Quaternion target = Quaternion.Euler(0, rotateAroundZ, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * turnSmoothing);
	}

	void OnCollisionEnter(Collision other) {
		if ( other.transform.tag == TagConstants.CLOCK ) {
			clockOn = true;
			other.gameObject.SetActive(false);
		}
		else if ( other.transform.tag == TagConstants.HAT ) {
			hatOn = true;
			other.gameObject.SetActive(false);
		}
	}
}
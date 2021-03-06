﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
	public NavMeshAgent agent;
	public bool hatOn;
	public bool clockOn;
	public float speed = 3f;
	public float turnSmoothing = 3f;
	private float rotateAroundZ;
	public float rotateDegrees = 4f;
	private Animator animator;
	public bool caught;
	private Timer time;
	public GameObject playerHat;
	public AudioSource clockTickingSound;
	public AudioSource hatPickupSound;

	void Start() {
		//clockTickingSound = GetComponent<AudioSource>();
		DisableHat(); // Hat is initilzed as hidden
		agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
		animator = GetComponent<Animator>();
		animator.SetBool("Idling", true);
		time = GameObject.FindGameObjectWithTag(TagConstants.TIME).GetComponent<Timer>();
	}

	void Update() {
		if(!caught) {
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
		} else {
			animator.SetBool("Idling", true);
		}
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
			time.AddExtraTime(2);
			clockTickingSound.Play();
			other.gameObject.SetActive(false);
		}

		else if ( other.transform.tag == TagConstants.HAT ) {
			hatOn = true;
			hatPickupSound.Play();
			other.gameObject.SetActive(false); // Collided hat in the scene disabled

			EnableHat(); // Hat is shown on top of player
			Invoke("DisableHat", EnemyFSM.hatOnTime); // Disable the hat after hatOnTime seconds
		}

		else if( other.transform.tag == TagConstants.WIN) {
			SceneManager.LoadScene("winScene");
		}
	}

	void EnableHat() {
		playerHat.active = true;
	}

	void DisableHat() {
		playerHat.active = false;
	}

}
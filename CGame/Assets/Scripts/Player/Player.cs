using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public NavMeshAgent agent;
	public bool hatOn;
	public bool clockOn;
	public float speed = 3f;
	public float turnSmoothing = 3f;
	float rotateAroundZ;
	float rotateDegrees = 6f;
	private Animator animator;
	private bool isMoving;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
		animator = GetComponentInChildren<Animator>();
		animator.SetBool("NonCombat", true);
		animator.SetBool("Idling", true);
	}

	// FixedUpdate is for physics based objects
	void FixedUpdate() {
		
		if ( Input.GetKey(KeyCode.UpArrow) ) {
			// transform.forward is the forward direction in LOCAL SPACE and then the agent goes forward relative to the agent's angle.
			// Note: On the other hand Vector3.forward is in WORLD SPACE
			transform.position += transform.forward * agent.speed * Time.deltaTime;
			isMoving = true;
			//Debug.Log("Up arrow");
		}
		if ( Input.GetKey(KeyCode.DownArrow) ) {
			transform.position -= transform.forward * agent.speed * Time.deltaTime;
			isMoving = true;
			//Debug.Log("Down arrow");
		}
		else if ( Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) ) {
			animator.SetBool("Idling", true);//stop moving
			isMoving = false;
		}
			
		RotatePlayer();

		if ( isMoving ) {
			animator.SetBool("Idling", false);
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
		}
		else if ( other.transform.tag == TagConstants.HAT ) {
			hatOn = true;
		}
	}
}
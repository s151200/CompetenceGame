using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public NavMeshAgent agent;
	public bool hatOn;
	public bool clockOn;
	public float speed = 3f;
	public float turnSmoothing = 15f;
	// A smoothing value for turning the player.

	void Start() {
		agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update() {
		if ( Input.GetKey(KeyCode.LeftArrow) ) {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		if ( Input.GetKey(KeyCode.RightArrow) ) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		if ( Input.GetKey(KeyCode.UpArrow) ) {
			transform.position += new Vector3(0, 0, 1) * speed * Time.deltaTime;
		}
		if ( Input.GetKey(KeyCode.DownArrow) ) {
			transform.position += new Vector3(0, 0, -1) * speed * Time.deltaTime;
		}
	}

	void FixedUpdate() {
		// Cache the inputs.
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		MovementManagement(h, v);
	}

	void MovementManagement(float horizontal, float vertical) {
		// If there is some axis input set the players rotation
		if ( horizontal != 0f || vertical != 0f ) {
			Rotating(horizontal, vertical);
		}
	}

	void Rotating(float horizontal, float vertical) {
		// Create a new vector of the horizontal and vertical inputs.
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);

		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);

		// Change the players rotation to this new rotation.
		GetComponent<Rigidbody>().MoveRotation(newRotation);
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

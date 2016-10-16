using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public NavMeshAgent agent;
	public bool hatOn;
	public bool clockOn;
	public float speed;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.position += new Vector3(0,0,1) * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.position += new Vector3(0,0,-1) * speed * Time.deltaTime;
		}
	}

	void OnCollisionEnter(Collision other){
		if ( other.transform.tag == TagConstants.CLOCK ) {
			clockOn = true;
		}
		else if ( other.transform.tag == TagConstants.HAT ) {
			hatOn = true;
		}
	}
}

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public NavMeshAgent agent;
	public bool hatOn;
	public bool clockOn;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update() {
		if ( Input.GetMouseButtonUp(0) ) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			if ( Physics.Raycast(ray, out hit, 10000) ) {
				var p = hit.point;
				agent.SetDestination(p);
			}
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

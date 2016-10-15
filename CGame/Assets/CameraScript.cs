using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private GameObject player;
	private Vector3 cameraPosition;
	public float offsetY = 2f;
	public float offsetZ = 5f;
	public float cameraDelayTime = 0.5f;

	void Start() {
		player = GameObject.FindGameObjectWithTag(TagConstants.PLAYER);
	}

	void Update() {
		StartCoroutine(DelayMove());
	}

	IEnumerator DelayMove() {
		Vector3 playerPos = player.transform.position;

		yield return new WaitForSeconds(cameraDelayTime);
		transform.position = new Vector3(playerPos.x , offsetY, playerPos.z - offsetZ);
	}

}
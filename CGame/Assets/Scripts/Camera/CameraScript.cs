using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private GameObject player;
	private Vector3 cameraPosition;
	public float offsetY = 4f;
	public float offsetZ = 9f;
	Vector3 playerPos;

	void Start() {
		player = GameObject.FindGameObjectWithTag(TagConstants.PLAYER);
	}

	void LateUpdate() {
		playerPos = player.transform.position;
		transform.position = new Vector3(playerPos.x, playerPos.y + offsetY, playerPos.z - offsetZ);
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class poolingSystem : MonoBehaviour {
	//private static int MAX_LIFES = 3;
	//private GameObject[] lifes;
	//private List<GameObject> balls = new List<GameObject>();
	//private int numberOfLifes;
	private List<GameObject> hats = new List<GameObject>();
	private List<GameObject> clocks = new List<GameObject>();
	private int numberOfHats;
	private int numberOfClocks;

	void Awake() {
		//MAX_LIFES = MAX_LIFES - 1;

		//lifes = GameObject.FindGameObjectsWithTag("Life");
		//numberOfLifes = lifes.Length;
		/*
		for (int i = 0; i < numberOfLifes; i++) {
			balls.Add((GameObject)Instantiate(Resources.Load("Ball"), this.transform.position, Quaternion.identity));
			balls[i].SetActive(false);
		}*/

		// Generate all hats in the scene
		for (int i = 0; i < numberOfHats; i++) {
			hats.Add((GameObject)Instantiate(Resources.Load("Hat"), this.transform.position, Quaternion.identity));
			hats[i].SetActive(false);
		}

		// Generate all clocks in the scene
		for (int i = 0; i < numberOfClocks; i++) {
			clocks.Add((GameObject)Instantiate(Resources.Load("Clock"), this.transform.position, Quaternion.identity));
			clocks[i].SetActive(false);
		}

	}

	// Hat ability
	public void InvisiblePlayer() {
		// TODO
	}

	// Clock ability
	public void GetMoreTime() {
		// TODO
	}

	public void KillPlayer(Collision other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.SetActive(false);

			Application.LoadLevel(Application.loadedLevel); // TODO load end screen
			/*
			if (GameObject.FindGameObjectsWithTag("Player").Length < 1 && MAX_LIFES > 0) {
				Application.LoadLevel(Application.loadedLevel);
			}
			else {
				Application.Quit();
			}*/
		}
	}

	// Enable a hat in the scene at position 'other'
	public void CreateNewHat(Vector3 other) {
		hats[numberOfHats].SetActive(true);
		hats[numberOfHats].transform.position = other;
	}

	// Enables a clock in the scene at position 'other'
	public void CreateNewClock(Vector3 other) {
		clocks[numberOfHats].SetActive(true);
		clocks[numberOfHats].transform.position = other;
	}

	/*
	public void createNewBall(Vector3 other) {
		numberOfLifes -= 1;
		balls[numberOfLifes].SetActive(true);
		balls[numberOfLifes].transform.position = other;
	}*/
}
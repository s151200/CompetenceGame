using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	private int timeLeft;
	private int startingTime;
	private Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> (); 
		startingTime = 60;
		timeLeft = startingTime;
	}

	// Update is called once per frame
	void FixedUpdate() {
		timeLeft = startingTime - (int)Time.timeSinceLevelLoad;
		text.text = "00:" + timeLeft;
		if (timeLeft < 10) {
			text.color = Color.red;
			text.text = "00:0" + timeLeft;
		}

		if ( timeLeft == 0 ) {
			// TODO load game over screen
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	/*
	public void AddExtraTime(int extraTime) {
		startingTime += extraTime;
	}
	*/
}


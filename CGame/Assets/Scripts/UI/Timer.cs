using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	private int timeLeft;
	private int startingTime;
	private Text text;
	private Player player;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> (); 
		startingTime = 90;
		timeLeft = startingTime;
		player = GameObject.FindGameObjectWithTag(TagConstants.PLAYER).GetComponent<Player>();
	}

	// Update is called once per frame
	void FixedUpdate() {

		if ( !player.clockOn ) {
			timeLeft = startingTime - (int)Time.timeSinceLevelLoad;

			if ( timeLeft >= 60 && (timeLeft - 60) >= 10 ) {
				text.text = "01:" + (timeLeft - 60);
			}
			else if ( timeLeft >= 60 && (timeLeft - 60) < 10 ) {
				text.text = "01:0" + (timeLeft - 60);
			}
			else {
				text.text = "00:" + timeLeft;
			}
			if ( timeLeft < 10 ) {
				text.color = Color.red;
				text.text = "00:0" + timeLeft;
			}
		}

		if ( timeLeft == 0 ) {
			// TODO load game over screen
			Application.LoadLevel(Application.loadedLevel);
		}
	}
		
	public void AddExtraTime(int extraTime) {
		startingTime += extraTime;
	}
}


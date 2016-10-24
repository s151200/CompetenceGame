using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour {
	public Button restartButton; 
	// Use this for initialization
	void Start () {
		Button btn = restartButton.GetComponent<Button>();
		btn.onClick.AddListener(OnClickFunc);
	}

	void OnClickFunc() {
		SceneManager.LoadScene("sindriScene");
	}
}

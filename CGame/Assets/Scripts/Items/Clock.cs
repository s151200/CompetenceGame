using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Clock : MonoBehaviour {
	//private Renderer rend;
	private Material mat;
	private float amount;
	private float inc;

	void Start() {
		mat = GetComponent<Renderer>().material;
		amount = -0.05f;
		inc = 0.003f;
	}

	void Update() {
		if ( amount > 0.07f || amount < -0.05f) {
			inc = inc * (-1);
		}
		amount += inc;
		mat.SetFloat("_Amount", amount);
	}
}


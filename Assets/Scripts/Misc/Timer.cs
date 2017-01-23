using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	
	static public bool running = true;

	static public float deltaTime() {
		//print (running.ToString ());
		if (running) {
			return Time.deltaTime;
		} else {
			return 0.0f;
			//return Mathf.Infinity;
		}

	}

	static public float time() {
		if (running) {
			return Time.time;
		} else {
			return Mathf.Infinity;
		}
	}

	// Use this for initialization
	void Start () {
		running = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

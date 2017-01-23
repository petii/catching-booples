using UnityEngine;
//using System.Collections;

public class Timer : MonoBehaviour {
	
	public static bool running = true;

	public static float deltaTime() {
		//print (running.ToString ());
		if (running) {
			return Time.deltaTime;
		} else {
			return 0.0f;
			//return Mathf.Infinity;
		}

	}

	public static float time() {
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

	void OnLevelWasLoaded() {
		running = true;
	}
	
	// Update is called once per frame
	void Update () {}
}

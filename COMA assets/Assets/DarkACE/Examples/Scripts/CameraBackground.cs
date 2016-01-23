using UnityEngine;
using System.Collections;

public class CameraBackground : MonoBehaviour {

	AudioEvents audioEvents;

	// Use this for initialization
	void Start () {
		audioEvents = GetComponent<AudioEvents>();
	}
	
	// Update is called once per frame
	void Update () {
		float r = audioEvents.GetCurrentValue(0);
		float g = audioEvents.GetCurrentValue(1);
		float b = audioEvents.GetCurrentValue(2);

		Camera.main.backgroundColor = new Color(r, g, b);
	}
}

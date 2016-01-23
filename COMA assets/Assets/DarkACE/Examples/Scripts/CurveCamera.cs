using UnityEngine;
using System.Collections;

public class CurveCamera : MonoBehaviour {

	AudioEvents events;

	// Use this for initialization
	void Start () {
		events = GetComponent<AudioEvents>();
	}
	
	// Update is called once per frame
	void Update () {
		Color color = new Color(events.GetCurrentValue(0), events.GetCurrentValue(1), events.GetCurrentValue(2));
		Camera.main.backgroundColor = color;
	}
}

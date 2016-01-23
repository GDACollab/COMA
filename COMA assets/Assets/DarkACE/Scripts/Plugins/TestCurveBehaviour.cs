using UnityEngine;
using System.Collections;

public class TestCurveBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TestCurve() {
		AudioEvents events = GetComponent<AudioEvents>();
		Debug.Log(events.GetValueAtTime(0, GetComponent<AudioSource>().time));
	}
}

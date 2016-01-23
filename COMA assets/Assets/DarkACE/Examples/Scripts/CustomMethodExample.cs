using UnityEngine;
using System.Collections;

public class CustomMethodExample : MonoBehaviour {

	void CustomMethod(){
		Debug.Log("Executed custom method at "+GetComponent<AudioSource>().time);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

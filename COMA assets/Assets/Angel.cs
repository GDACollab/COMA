using UnityEngine;
using System.Collections;

public class Angel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);

		Application.LoadLevel("title");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

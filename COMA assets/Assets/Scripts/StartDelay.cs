using UnityEngine;
using System.Collections;

public class StartDelay : MonoBehaviour {

	[SerializeField]
	public const float startDelayTime = 3f;

	// Use this for initialization
	void Start () {
		AudioSource audioSource = GetComponent<AudioSource> ();
		audioSource.PlayDelayed (startDelayTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

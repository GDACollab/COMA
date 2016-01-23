using UnityEngine;
using System.Collections;

public class DiscoBall : MonoBehaviour {

	public float rotationSpeed = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 rot = new Vector3 (0.0f, rotationSpeed, 0.0f);
		transform.rotation = Quaternion.Euler (rot);
	}
}

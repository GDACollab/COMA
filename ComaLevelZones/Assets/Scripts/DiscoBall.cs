using UnityEngine;
using System.Collections;

public class DiscoBall : MonoBehaviour {

	public float rotationSpeed = 0;
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 rot = new Vector3 (0.0f, rotationSpeed+=5, 0.0f);
		transform.rotation = Quaternion.Euler (rot);
	}
}

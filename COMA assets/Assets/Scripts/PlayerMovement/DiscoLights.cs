using UnityEngine;
using System.Collections;

public class DiscoLights : MonoBehaviour {

	public float speed = -0.01f; //the speed of disco flashing

	public float fadeIn = -0.01f;
	public float fadeOut = 0.01f;

	public bool shines = false;

	void Start(){
		for (int i = 0; i < transform.childCount; i++) {
			Transform obj = transform.GetChild (i);

			Color color = obj.GetComponent<SpriteRenderer> ().color;
			color.a += speed;

			obj.GetComponent<SpriteRenderer> ().color = color;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!shines) {
			SetAlpha (transform);
		} else {
			for (int i = 0; i < transform.childCount; i++) {
				SetAlpha(transform.GetChild(i));
			}
		}
	}

	private void SetAlpha(Transform obj){
		Color color = obj.GetComponent<SpriteRenderer> ().color;

		if (color.a <= 0)
			speed = fadeIn;

		if (color.a >= 1.0f)
			speed = fadeOut;

		color.a += speed;

		obj.GetComponent<SpriteRenderer> ().color = color;
	}
}

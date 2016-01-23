using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject health;
	public float speed = 0;
	public float playerHealth;

	private bool inSaveScreen = false;

	void Start()
	{
		playerHealth = SaveData.control.fullHealth;
		health.transform.localScale = new Vector3 (playerHealth, health.transform.localScale.y, health.transform.localScale.z);
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		//get phone accerometer data
		float horizontalMovement =  Input.GetAxis ("Horizontal");
		float verticalMovement =  Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (horizontalMovement, 0.0f, verticalMovement);

		//stop player movment when save screen displays
		if (!inSaveScreen)
			GetComponent<Rigidbody> ().velocity = movement * speed;
		else
			GetComponent<Rigidbody> ().velocity = movement * 0;

		GetComponent<Rigidbody>().position = new Vector3 
		(
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, -4.0f, 4.0f), 
			Mathf.Clamp (GetComponent<Rigidbody>().position.y, -0.5f, 5.0f), 
			0.0f
		);

		playerHealth = health.transform.localScale.x;

		if (playerHealth <= 0) {
			respawn ();
		}
	}

	void OnCollisionEnter(Collision obj)
	{
		if (obj.gameObject.tag == "enemy") {
			float dying = (float) (health.transform.localScale.x - 2);

			if(dying < 0) dying = 0;

			health.transform.localScale = new Vector3 (dying, health.transform.localScale.y, health.transform.localScale.z);
		}
	}

	void OnTriggerStay(Collider obj){
		if (obj.tag == "save" && !SaveData.control.done && Input.GetKeyUp ("space")) {
			inSaveScreen = true;
			setFullHealth ();
		}

		if (obj.tag == "save" && (SaveData.control.done)) {
			inSaveScreen = false;
		}
	}

	void respawn()
	{
		Application.LoadLevel ("LoadMenu");

		setFullHealth ();
	}

	void setFullHealth(){
		playerHealth = SaveData.control.fullHealth;
		health.transform.localScale = new Vector3 (playerHealth, health.transform.localScale.y, health.transform.localScale.z);
	}
}

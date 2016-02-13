using UnityEngine;
using System.Collections;

public class ChangeArea : MonoBehaviour {

	public string LeftLevelLoad = "";
	public string RightLevelLoad = "";

	private GameObject player;

	void Start(){
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (player.GetComponent<PlayerMovement> ().hitLeftWall && LeftLevelLoad != "") {
			Application.LoadLevel (LeftLevelLoad);
			player.GetComponent<PlayerMovement> ().hitLeftWall = false;
		}

		if (player.GetComponent<PlayerMovement> ().hitRightWall && RightLevelLoad != "") {
			Application.LoadLevel (RightLevelLoad);
			player.GetComponent<PlayerMovement> ().hitRightWall = false;
		}
	}
}

using UnityEngine;
using System.Collections;

public class ChangeArea : MonoBehaviour {

	public string LeftLevelLoad = "";
	public string RightLevelLoad = "";

	public GameObject player;

	// Update is called once per frame
	void FixedUpdate () {
		if (player != null && player.GetComponent<PlayerMovement> ().hitRightWall && RightLevelLoad != "") {
			Application.LoadLevel (RightLevelLoad);
			player.GetComponent<PlayerMovement> ().hitRightWall = false;
		}

		if (player != null && player.GetComponent<PlayerMovement> ().hitLeftWall && LeftLevelLoad != "") {
			Application.LoadLevel (LeftLevelLoad);
			player.GetComponent<PlayerMovement> ().hitLeftWall = false;
		}
	}
}

using UnityEngine;
using System.Collections;

public class ChangeArea : MonoBehaviour {

	public string LeftLevelLoad = "";
	public string RightLevelLoad = "";

	private GameObject player;

	void Start(){
		player = GameObject.FindGameObjectWithTag ("Player") as GameObject;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (player != null && player.GetComponent<PlayerMovement> ().hitLeftWall && LeftLevelLoad != "") {
			Angel.TransitionFromFieldToField(LeftLevelLoad);
			player.GetComponent<PlayerMovement> ().hitLeftWall = false;
		}

		if (player != null && player.GetComponent<PlayerMovement> ().hitRightWall && RightLevelLoad != "") {
			Angel.TransitionFromFieldToField(RightLevelLoad);
			player.GetComponent<PlayerMovement> ().hitRightWall = false;
		}
	}
}

using UnityEngine;
using System.Collections;

public class ChangeArea : MonoBehaviour {

	public string LeftLevelLoad = "";
	public string RightLevelLoad = "";

	private PlayerMovement player_movement;

	void Awake(){
		player_movement = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (player_movement.hitRightWall && RightLevelLoad != "") {
			Angel.TransitionFromFieldToField(RightLevelLoad);
			player_movement.hitRightWall = false;
		}

		if (player_movement.hitLeftWall && LeftLevelLoad != "") {
			Angel.TransitionFromFieldToField(LeftLevelLoad);
			player_movement.hitLeftWall = false;
		}
	}
}

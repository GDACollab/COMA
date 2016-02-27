using UnityEngine;
using System.Collections;

public class Angel : MonoBehaviour {

	void Start() {
		Angel.marshalled_state.Start();
		Angel.marshalled_state = null;
	}

//////////////////////////
// Start of static section

	// This is the thing that remembers stuff about the previous scene
	//   so that the current scene can initialize itself accordingly.
	private static MarshalledState marshalled_state = new NoState();

	// Other global state
	public static float hp;
//	public static List<Item> inventory;

	public static void TransitionFromBattleToField() {
//		marshalled_state = new BattleToField();
//
//		Application.LoadLevel(field_scene_name);
	}

	public static void TransitionFromFieldToBattle() {
//		field_scene_name = ???;
//		field_blue_pos = ???;

//		Application.LoadLevel(???);
	}

	public static void TransitionFromFieldToField(string destination) {
		marshalled_state = new FieldToField();
		Application.LoadLevel(destination);
	}

	interface MarshalledState {
		void Start();
	}

	class NoState : MarshalledState {
		public void Start() {}
	}

	class FieldToField : MarshalledState {
		string prev_field;

		public FieldToField() {
			prev_field = Application.loadedLevelName;
		}

		// This is called after transitioning from one field scene to another.
		//   Its purpose is to move Blue to an appropriate location based by
		//   taking into account the previous scene.
		public void Start() {
			GameObject blue = getBlue();

			Exit[] exits = Object.FindObjectsOfType<Exit>();
			foreach(Exit e in exits) {
				if(e.LevelLoad == prev_field) {
					blue.transform.position = Vector2.Scale(e.gameObject.GetComponent<BoxCollider2D>().offset, new Vector2(0.8f, 0.8f));
					return;
				}
			}

			ChangeArea ca = GameObject.FindGameObjectWithTag("Background").GetComponent<ChangeArea>();

			if(ca.LeftLevelLoad == prev_field) {
				blue.transform.position = new Vector3(-3, -1, 0);
				return;
			}
			if(ca.RightLevelLoad == prev_field) {
				blue.transform.position = new Vector3(3, -1, 0);
				return;
			}
		}
	}

	static private GameObject getBlue() {
		return GameObject.FindGameObjectWithTag ("Player") as GameObject;
	}
}

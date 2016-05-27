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
	public static Inventory inventory;
    public static Item Item0 = new HealingItem("Cherry.", "This is a Cherry.", 30, "Heals 30% of \nyour Health.", 1);
    public static Item Item1 = new QuestItem("Sword.", "This is a Sword.", "It has a cute \ntattoo on its hilt.", 2);
    public static Item Item2 = new QuestItem("Boob.", "It's a boob.", "Still a boob.", 1);
  
    //  quest info maybe?

    public static void TransitionFromBattleToField() {
		FieldToBattle x = (FieldToBattle) marshalled_state;
		Application.LoadLevel(x.field_name);
	}

	public static void TransitionFromFieldToBattle(string destination) {
		marshalled_state = new FieldToBattle();
		//Application.LoadLevel(destination);
		Application.LoadLevel(Application.loadedLevelName);
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

	class FieldToBattle : MarshalledState {
		public string field_name;
		Vector3 blue_pos;

		public FieldToBattle() {
			field_name = Application.loadedLevelName;
			blue_pos = getBlue().transform.position;
		}

		public void Start() {
			GameObject blue = getBlue();

			// I'm expecting this code to run ONLY when starting a FIELD scene!
			// The following assertion is an attempt to guarantee this ...
			if(blue == null)
				throw new System.Exception("Assertion failed");

			blue.transform.position = blue_pos;
			Debug.Log("returning from a random encounter");
		}
	}

	static private GameObject getBlue() {
		return GameObject.FindGameObjectWithTag ("Player") as GameObject;
	}
}

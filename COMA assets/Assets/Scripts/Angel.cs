using UnityEngine;
using System.Collections;

public class Angel {

	public static float hp;
//	public static List<Item> inventory;

//	Application.LoadLevel("title");

	// Field data to be remembered during battle scenes
	private static string field_scene_name;
	private static Vector3 field_blue_pos;

	public static void TransitionFromFieldToField(string destination) {
		Application.LoadLevel(destination);
	}
	public static void TransitionFromBattleToField() {
		Application.LoadLevel(field_scene_name);
	}
	public static void TransitionFromFieldToBattle() {
//		field_scene_name = ???;
//		field_blue_pos = ???;

//		Application.LoadLevel(???);
	}
	public static void StartFieldScene() {
//		??? = field_blue_pos;
	}
}

using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

	public Font pixel;

	void OnGUI ()
	{
		GUIStyle pixelStyle = new GUIStyle(GUI.skin.button);
		pixelStyle.fontSize = 10;
		pixelStyle.font = pixel;

		if (GUI.Button (new Rect (50, 20, 100, 30), "Level1", pixelStyle))
			StartCoroutine ("FadeToNextLevel", "savestuff");

		if(GUI.Button(new Rect(200, 20, 100, 30), "Level2", pixelStyle))
			StartCoroutine ("FadeToNextLevel", "TestSave");

		if(GUI.Button(new Rect(350, 20, 100, 30), "StartMenu", pixelStyle))
			StartCoroutine ("FadeToNextLevel", "LoadMenu");
	}

	//coroutine to fade between scenes
	IEnumerator FadeToNextLevel(string scene){
		float fadetime = GameObject.Find ("SaveData").GetComponent<FadingScenes> ().BeginFade (1);
		yield return new WaitForSeconds(fadetime);
		Application.LoadLevel(scene);
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadMenu : MonoBehaviour {

	public Font pixel;

	// internal data
	private SelectionHandler selectionHandler;

	private string[] fileNames = new string[] {"/ComaPlayerData0.dat", "/ComaPlayerData1.dat", "/ComaPlayerData2.dat"};
	private string[] loadAreas;
	private bool hasNonEmptySlot = false;
	private bool loadFileB = false;

	void Start(){
		loadAreas = new string[fileNames.Length];
		CheckIfLoadFileExists ();
	}

	void OnGUI ()
	{
		GUIStyle normPixelStyle = new GUIStyle(GUI.skin.box);
		normPixelStyle.font = pixel;

		GUIStyle boldPixelStyle = new GUIStyle(GUI.skin.box);
		boldPixelStyle.font = pixel;
		boldPixelStyle.fontSize = 32;

		if (!loadFileB && GUI.Button (new Rect (200, 20, 200, 30), "New Game", normPixelStyle))
			StartCoroutine ("FadeToNextLevel", "savestuff");
		
		if (!loadFileB && GUI.Button(new Rect(200, 50, 200, 30), "Load Game", normPixelStyle) && hasNonEmptySlot) {
			loadFileB = true;
		}

		if (loadFileB) {
			for (int i = 0; i < loadAreas.Length; i++) {
				if (selectionHandler.GetSelectedIndex () == i) {
					GUI.Box (new Rect (Screen.width / 4, 50 * i, Screen.width / 2, 40), loadAreas [i], boldPixelStyle);
				} else {
					GUI.Box (new Rect (Screen.width / 4, 50 * i, Screen.width / 2, 40), loadAreas [i], normPixelStyle);
				}
			}

			LoadSlot ();

			if (Input.GetKeyUp ("up")) {
				selectionHandler.Previous ();
			}

			if (Input.GetKeyUp ("down")) {
				selectionHandler.Next ();
			}
		}
	}

	public void Load(string loadFile)
	{
		if (File.Exists (Application.persistentDataPath + loadFile)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + loadFile, FileMode.Open);
			PlayerSaveData data = (PlayerSaveData) bf.Deserialize(file);
			file.Close();

			StartCoroutine ("FadeToNextLevel", data.level);
		}
	}

	public void CheckIfLoadFileExists(){
		List<string> temp = new List<string> ();
		for (int i = 0; i < fileNames.Length; i++) {
			if (!File.Exists (Application.persistentDataPath + fileNames[i])) {
				temp.Add("Empty Slot");
				loadAreas[i] = "Empty Slot";
			} else {
				hasNonEmptySlot = true;
				temp.Add(fileNames [i]);
				GetSaveFileAreaData (fileNames[i], i);
			}
		}

		selectionHandler = new SelectionHandler (temp);
	}

	private void GetSaveFileAreaData(string fileName, int i){
		if (File.Exists (Application.persistentDataPath + fileName)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + fileName, FileMode.Open);
			PlayerSaveData data = (PlayerSaveData) bf.Deserialize(file);
			file.Close();
			
			loadAreas[i] = data.level;
		}
	}

	public void LoadSlot(){
		if (Input.GetKeyUp ("space")) {
			Load(selectionHandler.GetOptionListString());
		}
	}

	IEnumerator FadeToNextLevel(string scene){
		float fadetime = GameObject.Find ("SaveData").GetComponent<FadingScenes> ().BeginFade (1);
		yield return new WaitForSeconds(fadetime);
		SceneManager.LoadScene(scene);
	}
}

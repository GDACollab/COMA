using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveData : MonoBehaviour {

	public static SaveData control;

	public float fullHealth;
	
	public Font pixel;

	public bool openSaveSlots = false;
	public bool done = false;

	private bool playerCollision = false;
	private bool overrideRequest = false;

	// internal data
	private SelectionHandler selectionHandler;

	private string[] fileNames = new string[] {"/ComaPlayerData0.dat", "/ComaPlayerData1.dat", "/ComaPlayerData2.dat"};
	private string[] loadAreas;

	// Use this for initialization
	void Awake ()
	{
		//keep
		if(control == null)
		{
			DontDestroyOnLoad(gameObject);
			control = this;
		}
		else if(control != this)
		{
			Destroy(gameObject);
		}
	}

	void Start(){
		//determine what files already exist
		loadAreas = new string[fileNames.Length];
		CheckIfLoadFileExists ();
	}

	void OnGUI ()
	{
		//gui style for buttons
		GUIStyle pixelStyleButton = new GUIStyle(GUI.skin.button);
		pixelStyleButton.fontSize = 10;
		pixelStyleButton.font = pixel;

		//gui style for text statements
		GUIStyle pixelStyleLabel = new GUIStyle(GUI.skin.textArea);
		pixelStyleLabel.fontSize = 10;
		pixelStyleLabel.font = pixel;

		//gui style for save slots unselected
		GUIStyle normPixelStyle = new GUIStyle(GUI.skin.box);
		normPixelStyle.font = pixel;

		//gui style for selected save slots
		GUIStyle boldPixelStyle = new GUIStyle(GUI.skin.box);
		boldPixelStyle.font = pixel;
		boldPixelStyle.fontSize = 32;

		//for programmer to know what level is loaded
		GUI.Label(new Rect(50, 400, 100, 30), "Level: " + Application.loadedLevelName, pixelStyleLabel);

		//when player collides with the player and presses space ask if they wish to save
		if (playerCollision) {
			//if player has chosen to save stop diaplying the ask to save text
			if(!openSaveSlots) GUI.Label(new Rect(100, 180, 240, 30), "Health restored\nWould You Like to Save?", pixelStyleLabel);

			//if player has chosen to save stop displaying yes button
			if(!openSaveSlots && GUI.Button(new Rect(100, 220, 100, 30), "Yes", pixelStyleButton)){
				openSaveSlots =	true;
			}

			//if player has chosen to save stop displaying no button
			if (!openSaveSlots && GUI.Button (new Rect (230, 220, 100, 30), "No", pixelStyleButton)) {
				done = true;
			}

			//if yes to save is true display the save slots
			if (openSaveSlots) {
				//loop through each file to show already saved or empty files
				for (int i = 0; i < loadAreas.Length; i++) {
					if (selectionHandler.GetSelectedIndex () == i) {
						GUI.Box (new Rect (Screen.width / 4, 50 * i, Screen.width / 2, 40), loadAreas [i], boldPixelStyle);
					} else {
						GUI.Box (new Rect (Screen.width / 4, 50 * i, Screen.width / 2, 40), loadAreas [i], normPixelStyle);
					}
				}

				//if choosing a slot to save on then check if that slot is empty
				if ((Input.GetKeyUp ("space") && SaveSlots (fileNames[selectionHandler.GetSelectedIndex()])) || overrideRequest) {
					overrideRequest = true;

					//label for question statement ask to verify save choice
					GUI.Label(new Rect(100, 180, 200, 30), "Would you like to override this file?", pixelStyleLabel);

					//dislay yes\no to see if player does wish to override
					if(GUI.Button(new Rect(100, 220, 100, 30), "Yes", pixelStyleButton)){
						Save (fileNames[selectionHandler.GetSelectedIndex()]);
						done =	true;
						overrideRequest = false;
						openSaveSlots = false;
					}
					if(GUI.Button (new Rect (230, 220, 100, 30), "No", pixelStyleButton)) {
						overrideRequest = false;
					}
				}

				//allow player to cancel or exit the save slots
				if (!overrideRequest && GUI.Button (new Rect (230, 220, 100, 30), "Cancel", pixelStyleButton)){
					done = true;
					openSaveSlots = false;
					overrideRequest = false;
				}

				//cycle upwards through save slots
				if (!overrideRequest && Input.GetKeyUp ("up")) {
					selectionHandler.Previous ();
				}

				//cycle downwards through save slots
				if (!overrideRequest && Input.GetKeyUp ("down")) {
					selectionHandler.Next ();
				}
			}
		}
	}

	void OnTriggerStay(Collider obj)
	{
		//check if player has collided with the save point
		if (obj.tag == "Player" && !done && Input.GetKeyUp ("space")) {
			playerCollision = true;
		} else if(done)
			playerCollision = false;
	}

	void OnTriggerExit(Collider obj){
		if (obj.tag == "Player") {
			done = false;
			CheckIfLoadFileExists ();
		}
	}

	//Look through possible save files to see if they exist and pull from that the zone file was saved in
	public void CheckIfLoadFileExists(){
		List<string> temp = new List<string> ();
		for (int i = 0; i < fileNames.Length; i++) {
			if (!File.Exists (Application.persistentDataPath + fileNames[i])) {
				temp.Add("Empty Slot");
				loadAreas[i] = "Empty Slot";
			} else {
				temp.Add(fileNames [i]);
				GetSaveFileAreaData (fileNames[i], i);
			}
		}

		selectionHandler = new SelectionHandler (temp);
	}

	//get the zone data from the files
	private void GetSaveFileAreaData(string fileName, int i){
		if (File.Exists (Application.persistentDataPath + fileName)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + fileName, FileMode.Open);
			PlayerSaveData data = (PlayerSaveData) bf.Deserialize(file);
			file.Close();

			loadAreas[i] = data.level;
		}
	}

	//check if the file exists already before saving
	public bool SaveSlots(string saveFile){
		bool exists = false;
		if (File.Exists (Application.persistentDataPath + saveFile) && selectionHandler.GetOptionListString() != "Empty Slot") {
			exists = true;
		} else {
			Save (fileNames[selectionHandler.GetSelectedIndex()]);
			done = true;
			openSaveSlots = false;
			overrideRequest = false;
		}
		return exists;
	}

	//save the game to the selected save file slot
	public void Save(string saveFile)
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + saveFile);

		PlayerSaveData data = new PlayerSaveData ();
		data.level = Application.loadedLevelName;

		bf.Serialize (file, data);
		file.Close ();
	}
}

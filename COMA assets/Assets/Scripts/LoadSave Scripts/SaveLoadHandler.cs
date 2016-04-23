using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadHandler : MonoBehaviour {

	public Font pixel;
	public static SaveLoadHandler slHandler;
	public static int SaveSlotNumber = 0;
	private PlayerSaveData playerData;

	private string currLevel = "saveTestScene";
	private List<string> itemNames;
	private List<int> itemNums;

	private const int MAX_SLOTS = 5;

	void Start() {
		if (slHandler == null) {
			slHandler = this;
			playerData = new PlayerSaveData();
		}
		else if (slHandler != this) {
			Destroy (gameObject);
		}
		for(int i = 0; i < MAX_SLOTS; ++i) {
			String newFileName = Application.persistentDataPath + "/ComaPlayerData" + i.ToString() + ".dat";
			if(!File.Exists(newFileName))
			   File.Create(newFileName);
		}
		Load (SaveSlotNumber);
	}

	void OnGUI ()
	{
		GUIStyle pixelStyle = new GUIStyle(GUI.skin.button);
		pixelStyle.font = pixel;

		if(GUI.Button(new Rect(300, 20, 200, 30), "Save Slot 0", pixelStyle))
			Save (SaveSlotNumber);

		if(GUI.Button(new Rect(300, 60, 200, 30), "Load Slot 0", pixelStyle))
			Load (SaveSlotNumber);

		if (GUI.Button (new Rect (300, 100, 200, 30), "Add Corn", pixelStyle))
			itemNums [0] += 1;

		if (GUI.Button (new Rect (300, 140, 200, 30), "Reload Level", pixelStyle))
			LoadLevel ();
	}

	public void Load(int num)
	{
		if (num > MAX_SLOTS || num < 0)
			print ("Load file number not in range");
		else {
			String fileName = Application.persistentDataPath + "/ComaPlayerData" + num.ToString () + ".dat";
			if (File.Exists (fileName)) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (fileName, FileMode.Open);
				PlayerSaveData data = (PlayerSaveData)bf.Deserialize (file);
				file.Close ();

				playerData = data;
			}
		}
	}

	public void Save(int num)
	{
		if (num > MAX_SLOTS || num < 0)
			print ("Save file number not in range");
		else {
			String fileName = Application.persistentDataPath + "/ComaPlayerData" + num.ToString () + ".dat";
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file;
			if (!File.Exists (fileName))
				file = File.Create (fileName);
			else {
				file = File.Open (fileName, FileMode.Open);
			}

			bf.Serialize (file, playerData);
			file.Close ();
		}
	}

	public void LoadLevel() {
		Application.LoadLevel (currLevel);
	}

	public void SetSlotNum(int num) {
		if (num < 0 || num > MAX_SLOTS) {
			print ("slot number does not exist");
		} else {
			SaveSlotNumber = num;
		}
	}

}

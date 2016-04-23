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
	private List<string> itemNames = new List<string> ();

	private string currLevel = "saveTestScene";

	private const int MAX_SLOTS = 5;

	void Start() {
		if (slHandler == null) {
			slHandler = this;
			PlayerPrefs.SetInt("corn",0);
			itemNames.Add("corn");
		}
		else if (slHandler != this) {
			Destroy (gameObject);
		}
		for(int i = 0; i < MAX_SLOTS; ++i) {
			String newFileName = Application.persistentDataPath + "/ComaPlayerData" + i.ToString() + ".dat";
			if(!File.Exists(newFileName))
			   File.Create(newFileName);
		}
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
			addItem ("corn", 1);

		if (GUI.Button (new Rect (300, 140, 200, 30), "Reload Level", pixelStyle))
			LoadLevel ();

		if (GUI.Button (new Rect (0, 140, 200, 30), "Clear All Save Data", pixelStyle))
			clearAllData ();
	}

	public static void Load(int num)
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

				currLevel = data.level;
				itemNames = data.itemNames;
				data.setPrefs();
				print (data.itemNums[0]);
			}
		}
	}

	public static void Save(int num)
	{
		if (num > MAX_SLOTS || num < 0)
			print ("Save file number not in range");
		else {
			String fileName = Application.persistentDataPath + "/ComaPlayerData" + num.ToString () + ".dat";
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file;
			file = File.Open (fileName, FileMode.Open);

			PlayerSaveData data = new PlayerSaveData();
			data.level = currLevel;
			data.itemNames = itemNames;
			data.getPrefs();
			bf.Serialize (file, data);
			file.Close ();
		}
	}

	private void LoadLevel() {
		Application.LoadLevel (currLevel);
	}

	public int addItem(string name, int num) {
		PlayerPrefs.SetInt (name, PlayerPrefs.GetInt (name) + num);
		return PlayerPrefs.GetInt (name);
	}

	private void SetSlotNum(int num) {
		if (num < 0 || num > MAX_SLOTS) {
			print ("slot number does not exist");
		} else {
			SaveSlotNumber = num;
		}
	}

	private void clearAllData() {
		for (int i = 0; i < MAX_SLOTS; ++i) {
			String newFileName = Application.persistentDataPath + "/ComaPlayerData" + i.ToString () + ".dat";
			FileStream file;
			File.Delete(newFileName);
			File.Create (newFileName);
		}
	}

}

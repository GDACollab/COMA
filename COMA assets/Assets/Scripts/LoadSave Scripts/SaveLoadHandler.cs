using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadHandler : MonoBehaviour {

	public Font pixel;

	private string currLevel = "SaveStuff";
	public float fullHealth;

	private const int MAX_SLOTS = 5;

	void Start() {
		for(int i = 0; i < MAX_SLOTS; ++i) {
			String newFileName = Application.persistentDataPath + "/ComaPlayerData" + i.ToString() + ".dat";
			if(!File.Exists(newFileName))
			   File.Create(newFileName);
		}
		Save (0);
	}

	void OnGUI ()
	{
		GUIStyle pixelStyle = new GUIStyle(GUI.skin.button);
		pixelStyle.font = pixel;

		if(GUI.Button(new Rect(300, 20, 200, 30), "Save Slot 0", pixelStyle))
			Save (0);

		if(GUI.Button(new Rect(300, 60, 200, 30), "Load Slot 0", pixelStyle))
			Load (0);
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

				currLevel = data.level;
				Application.LoadLevel(currLevel);

				GameObject.FindGameObjectWithTag("Player").transform.localPosition = 
					new Vector3(PlayerPrefs.GetFloat("objectX"),PlayerPrefs.GetFloat("objectY"));
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
			
			PlayerSaveData data = new PlayerSaveData ();
			data.level = currLevel;
			PlayerPrefs.SetFloat("objectX", GameObject.FindGameObjectWithTag("Player").transform.localPosition.x);
			PlayerPrefs.SetFloat("objectY", GameObject.FindGameObjectWithTag("Player").transform.localPosition.y);
			
			bf.Serialize (file, data);
			file.Close ();
		}
	}

}

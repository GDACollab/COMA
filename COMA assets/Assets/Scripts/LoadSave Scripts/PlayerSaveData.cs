using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
class PlayerSaveData {

	public string level;
	public List<string> itemNames;
	public List<int> itemNums;

	public void setPrefs() {
		for(int i = 0; i < itemNames.Count; ++i) {
			PlayerPrefs.SetInt(itemNames[i], itemNums[i]);
		}
	}

	public void getPrefs() {
		itemNums = new List<int> ();
		for (int i = 0; i < itemNames.Count; ++i) {
			itemNums.Add(PlayerPrefs.GetInt (itemNames [i]));
		}
	}
}

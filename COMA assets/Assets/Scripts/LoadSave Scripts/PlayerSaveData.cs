using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
class PlayerSaveData {

	public string level;
	public Item[] itemList;

}

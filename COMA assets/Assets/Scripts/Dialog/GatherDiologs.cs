using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GatherDiologs : MonoBehaviour {

	Dictionary<GameObject, List<string>> dialogsInOrder = new Dictionary<GameObject, List<string>>();
	public TextAsset csvFile;

	//for quests
	private string ACCEPT_QUEST = "Accept";
	private string DECLINE_QUEST = "Decline";

	//for converstaions
	private string QUESTION = "Question";
	private string WALK_AWAY = "Walk Away";

	// Use this for initialization
	void Start () {
		ReadSpreadSheets read = new ReadSpreadSheets ();
		read.ParseCSV (csvFile);

		GameObject[] allNPC = GameObject.FindGameObjectsWithTag ("NPC");
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		Dictionary<GameObject, List<Row>> listOfCharacterDialogs = new Dictionary<GameObject, List<Row>> ();

		for (int i = 0; i < allNPC.Length; i++) {
			if(!listOfCharacterDialogs.ContainsKey(allNPC[i]))
				listOfCharacterDialogs.Add(allNPC[i], read.FindAll_ACTOR(allNPC[i].name));
		}

		if(!listOfCharacterDialogs.ContainsKey(player))
			listOfCharacterDialogs.Add(player, read.FindAll_ACTOR(player.name));

		bool firstFound = false;
		foreach (KeyValuePair<GameObject, List<Row>> dialog in listOfCharacterDialogs) {
			for (int i = 0; i < dialog.Value.Count && dialog.Value.Count > 0; i++) {
				if(dialog.Value[i].LOCATION.Equals(Application.loadedLevelName)){
					if(dialog.Value[i].CONTEXT.Contains("1st approach")){
					dialog.Key.GetComponent<makeText> ().dialogue.Add(dialog.Value [i].CUE);

					//remove each dialog once added to the dialodsInOrder dict to reduce unneccessary searching again
					if (!dialogsInOrder.ContainsKey (dialog.Key)) {
						dialogsInOrder.Add (dialog.Key, new List<string>(){dialog.Value [i].CUE});
						dialog.Value.RemoveAt (i);
					} else {
						dialogsInOrder [dialog.Key].Add(dialog.Value [i].CUE);
						dialog.Value.RemoveAt (i);
					}

					firstFound = true;
					i = 0;
				}

				if (firstFound && dialog.Value[i].LOCATION.Equals(Application.loadedLevelName)) {
					//search string for approach if this is found then just append second
					//if not found then check choice_type
					//check also if value -1 if so then end checking
					string temp = dialog.Value[i].Conversation_Path_Chain;

					//check for choice paths
					int bar = temp.IndexOf ("|");
					if (bar != -1) {
							/*for (int k = 0; k < temp.Length; k++) {
								int pathNum = int.Parse (temp.Substring (0, bar));
								if(pathNum == -1) pathNum = int.Parse (temp);

								if (pathNum != -1) {
									if (dialog.Key.GetComponent<makeText> ().dialogue < 4) {
										dialogsInOrder [dialog.Key].Add ("CHOICE");
										dialog.Key.GetComponent<makeText> ().dialogue.Add ("CHOICE");

										if (dialog.Value [i].Choice_Type.Equals ("Quest")) {
											dialogsInOrder [dialog.Key].Add (ACCEPT_QUEST);
											dialogsInOrder [dialog.Key].Add (DECLINE_QUEST);
											dialog.Key.GetComponent<makeText> ().dialogue.Add (ACCEPT_QUEST);
											dialog.Key.GetComponent<makeText> ().dialogue.Add (DECLINE_QUEST);
										} else if (dialog.Value [i].Choice_Type.Equals ("Converstaion")) {
											dialogsInOrder [dialog.Key].Add (QUESTION);
											dialogsInOrder [dialog.Key].Add (WALK_AWAY);
											dialog.Key.GetComponent<makeText> ().dialogue.Add (QUESTION);
											dialog.Key.GetComponent<makeText> ().dialogue.Add (WALK_AWAY);
										}
									}



									if (dialogsInOrder [dialog.Key] [i].GameObject_Interacted_With.Equals ("Player") && listOfCharacterDialogs [player] [pathNum] != null) {
										if (dialog.Key.GetComponent<makeText> ().path1.Count < 4) {
											dialog.Key.GetComponent<makeText> ().path1.Add (listOfCharacterDialogs [player] [pathNum].CUE);
										}
										dialog.Value.RemoveAt (pathNum);
									}
								}

								int barIndex = temp.IndexOf ('|');
								if (barIndex == -1)
									barIndex = 0;
								temp = temp.Substring (barIndex, temp.Length);
							}*/
					}
					
					if(dialog.Value [i].CONTEXT.EndsWith("approach"))
							Debug.Log (dialog.Value [i].CONTEXT);
						dialogsInOrder [dialog.Key].Add ("SECOND");
						dialog.Key.GetComponent<makeText> ().dialogue.Add ("SECOND");
						dialog.Key.GetComponent<makeText> ().nextDialog.Add(listOfCharacterDialogs [dialog.Key] [i].CUE);
						dialog.Value.RemoveAt (i);
					}
				}

			}

			if (dialogsInOrder.ContainsKey (dialog.Key) && dialog.Value.Count == 0)
				dialogsInOrder [dialog.Key].Add("END");
			
			firstFound = false;
		}

		//after getting all the npc diologs get blues remaining diologs

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

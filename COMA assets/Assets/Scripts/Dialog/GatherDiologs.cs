﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GatherDiologs : MonoBehaviour
{

	private Dictionary<GameObject, List<string>> dialogsInOrder = new Dictionary<GameObject, List<string>> ();
	public TextAsset csvFile;

	//for quests
	private string ACCEPT_QUEST = "Accept";
	private string DECLINE_QUEST = "Decline";

	//for converstaions
	private string QUESTION = "Question";
	private string WALK_AWAY = "Walk Away";

	// Use this for initialization
	void Start ()
	{
		ReadSpreadSheets read = new ReadSpreadSheets ();
		read.ParseCSV (csvFile);

		GameObject[] allNPC = GameObject.FindGameObjectsWithTag ("NPC");
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		Dictionary<GameObject, List<Row>> listOfCharacterDialogs = new Dictionary<GameObject, List<Row>> (); //get all the npc gameobjects row diologs
		int[] newPathNums = new int[read.getRowsLength()]; //length is identifier, 2nd int for new idex value that goes with it's new list assignment
		newPathNums = read.FindDiologIndexes();

		for (int i = 0; i < allNPC.Length; i++) {
			if (!listOfCharacterDialogs.ContainsKey (allNPC [i])) 
				listOfCharacterDialogs.Add (allNPC [i], read.FindAll_ACTOR (allNPC [i].name));
		}

		if (!listOfCharacterDialogs.ContainsKey (player))
			listOfCharacterDialogs.Add (player, read.FindAll_ACTOR (player.name));
		

		bool firstFound = false;
		foreach (KeyValuePair<GameObject, List<Row>> dialog in listOfCharacterDialogs) {
			for (int i = 0; i < dialog.Value.Count && dialog.Value.Count > 0; i++) {
				if (dialog.Value [i].LOCATION.Equals (Application.loadedLevelName)) {
					if (!firstFound && dialog.Value [i].CONTEXT.Contains ("1st approach")) {
						dialog.Key.GetComponent<makeText> ().dialogue.Add (dialog.Value [i].CUE);
						ChoicePath (dialog, player, listOfCharacterDialogs, newPathNums, i, true);

						dialog.Value.RemoveAt (i);
						firstFound = true;
						i = -1;
					} else if (firstFound) {
						//search string for approach if this is found then just append second
						//if not found then check choice_type
						//check also if value -1 if so then end checking
						//remove each dialog once added to the dialodsInOrder dict to reduce unneccessary searching again
						if (!dialogsInOrder.ContainsKey (dialog.Key)) {
							dialogsInOrder.Add (dialog.Key, new List<string> (){ dialog.Value [i].CUE });
						} else {
							dialogsInOrder [dialog.Key].Add (dialog.Value [i].CUE);
						}

						string temp = dialog.Value [i].Conversation_Path_Chain;

						//check for choice paths
						if (temp != string.Empty) {
							ChoicePath (dialog, player, listOfCharacterDialogs, newPathNums, i, false);
						}

						//get next approach diolog
						if (dialog.Value [i].CONTEXT.EndsWith ("approach")) {
							if (!dialogsInOrder.ContainsKey (dialog.Key)) {
								dialogsInOrder [dialog.Key].Add (dialog.Value [i].CUE);
								dialogsInOrder [dialog.Key].Add ("SECOND");
							} 

							if (!dialog.Key.GetComponent<makeText> ().dialogue [dialog.Key.GetComponent<makeText> ().dialogue.Count - 1].Equals ("SECOND")) {
								dialog.Key.GetComponent<makeText> ().dialogue.Add ("SECOND");
								dialog.Key.GetComponent<makeText> ().nextDialog.Add (listOfCharacterDialogs [dialog.Key] [i].CUE);
							}

							dialog.Value.RemoveAt (i);
							i = -1;
						} else {
							if (!dialogsInOrder.ContainsKey (dialog.Key)) {
								dialogsInOrder.Add (dialog.Key, new List<string> (){ dialog.Value [i].CUE });
							} else {
								dialogsInOrder [dialog.Key].Add (dialog.Value [i].CUE);
							}
							dialog.Value.RemoveAt (i);
							i = -1;
						}
					}
				}
			}

			if (dialogsInOrder.ContainsKey (dialog.Key) && dialog.Value.Count == 0) {
				//dialog.Key.GetComponent<makeText> ().nextDialog.Add ("END");
				dialogsInOrder [dialog.Key].Add ("END");
			}

			firstFound = false;
		}

		//after getting all the npc diologs get blues remaining diologs
		//these are already set under blues diolog just need to set them to the correct objects
		List<Row> playerDiologs = read.FindAll_ACTOR (player.name);
		
		for (int i = 0; i < playerDiologs.Count; i++) {
			if (playerDiologs [i].LOCATION.Equals (Application.loadedLevelName)) {
				GameObject tempG = GameObject.Find (playerDiologs [i].GameObject_Interacted_With);

				if (tempG != null && !tempG.tag.Equals ("NPC")) {
					//List<Row> noNPC = read.FindAll_ACTOR (tempG.name);
					if (tempG.GetComponent<makeText> ().dialogue.Count < 1) {
						tempG.GetComponent<makeText> ().dialogue.Add (playerDiologs [i].CUE);
					} else {
						tempG.GetComponent<makeText> ().nextDialog.Add (playerDiologs [i].CUE);
					}
					//there seems to be an issue where the player was not inserted possibly into the list of character diologs need to double check this
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//here check when diolog == path1 if it does then check for next diolg sequence also check first if END exists
	}

	private void ChoicePath(KeyValuePair<GameObject, List<Row>> dialog, GameObject player, Dictionary<GameObject, List<Row>> listOfCharacterDialogs, int[] newPathNums, int i, bool useDiolog){
		//remove each dialog once added to the dialodsInOrder dict to reduce unneccessary searching again
		if (!dialogsInOrder.ContainsKey (dialog.Key)) {
			dialogsInOrder.Add (dialog.Key, new List<string> (){ dialog.Value [i].CUE });
		} else {
			dialogsInOrder [dialog.Key].Add (dialog.Value [i].CUE);
		}

		string temp = dialog.Value [i].Conversation_Path_Chain;

		//check for choice paths
		if (temp != string.Empty) {
			//get the next diologs path indexes
			int bar = temp.IndexOf ("|");
			int pathNum1 = -1;
			int pathNum2 = -1;

			if (bar != -1) {
				if (int.TryParse (temp.Substring (0, bar), out pathNum1)) {
					pathNum1 = int.Parse (temp.Substring (0, bar));
					if(pathNum1 != -1) pathNum1 = newPathNums [pathNum1];
					temp = temp.Substring (bar + 1);
				}

				bar = temp.IndexOf ("|"); //there is a third path for eyestravagent that needs to be considered
				if(bar == -1) bar = temp.Length - 1;
				if (int.TryParse (temp.Substring (0, bar), out pathNum2)) {
					pathNum2 = int.Parse (temp.Substring (0, bar));
					if(pathNum2 != -1) pathNum2 = newPathNums [pathNum2];
					temp = temp.Substring (bar + 1);
				}
			} else if (int.TryParse (temp, out pathNum1)) {
				pathNum1 = int.Parse (temp);
				if(pathNum1 != -1) pathNum1 = newPathNums [pathNum1];
			}

			if (pathNum1 != -1) {
				dialogsInOrder [dialog.Key].Add ("CHOICE");
				if(useDiolog) dialog.Key.GetComponent<makeText> ().dialogue.Add ("CHOICE");

				GameObject tempG = GameObject.Find(dialog.Value [i].GameObject_Interacted_With);
				
				//issue if the conversations are not in order then the pathnum finder will not work how to check value with out known index value?
				//possibly create a functon in readspreadsheet that will allow this special access find?
				//this is for a quest
				if (listOfCharacterDialogs.ContainsKey(tempG) && listOfCharacterDialogs [tempG].Count > pathNum1 && pathNum1 != -1 && listOfCharacterDialogs [tempG].Count > pathNum2 && pathNum2 != -1) {
					//insert the path1 diolog choice
					Row path1Diolog = listOfCharacterDialogs [tempG] [pathNum1];
					if(useDiolog) dialog.Key.GetComponent<makeText> ().dialogue.Add (path1Diolog.CUE);
					dialogsInOrder [dialog.Key].Add (path1Diolog.CUE);

					//insert the path2 diolog choice
					Row path2Diolog = listOfCharacterDialogs [tempG] [pathNum2];
					if(useDiolog) dialog.Key.GetComponent<makeText> ().dialogue.Add (path2Diolog.CUE);
					dialogsInOrder [dialog.Key].Add (path2Diolog.CUE);

					//get next diologs
					if (int.TryParse (temp, out pathNum1)) {
						pathNum1 = int.Parse (path1Diolog.Conversation_Path_Chain);
						if(pathNum1 != -1) pathNum1 = newPathNums [pathNum1];
						listOfCharacterDialogs [tempG].Remove (path1Diolog);
					}

					if (int.TryParse (temp, out pathNum2)) {
						pathNum2 = int.Parse (path2Diolog.Conversation_Path_Chain);
						if(pathNum2 != -1) pathNum2 = newPathNums [pathNum2];
						listOfCharacterDialogs [tempG].Remove (path2Diolog);
					}

					//insert to path1 the diolog that follows this choice
					if (dialog.Value.Count > pathNum1 && pathNum1 != -1) {
						if(useDiolog) dialog.Key.GetComponent<makeText> ().path1.Add (dialog.Value [pathNum1].CUE);
						dialogsInOrder [dialog.Key].Add (dialog.Value [pathNum1].CUE);
						dialog.Value.RemoveAt (pathNum1);
						pathNum2--;
					}

					//insert to path2 the diolog that follows this choice
					if (dialog.Value.Count > pathNum2 && pathNum2 != -1) {
						if(useDiolog) dialog.Key.GetComponent<makeText> ().path2.Add (dialog.Value [pathNum2].CUE);
						if(useDiolog) dialog.Key.GetComponent<makeText> ().path2.Add ("RESET");
						dialogsInOrder [dialog.Key].Add (dialog.Value [pathNum2].CUE);
						dialogsInOrder [dialog.Key].Add ("RESET");
						dialog.Value.RemoveAt (pathNum2);
					}
				}
				//this is for a conversation
				else if(listOfCharacterDialogs [tempG].Count > pathNum1 && pathNum1 != -1){
					//add basic diolog for generic characters
					if(useDiolog) dialog.Key.GetComponent<makeText> ().dialogue.Add (QUESTION);
					dialogsInOrder [dialog.Key].Add (QUESTION);
					if(useDiolog) dialog.Key.GetComponent<makeText> ().dialogue.Add (WALK_AWAY);
					dialogsInOrder [dialog.Key].Add (WALK_AWAY);

					//add the diolog that follows these choices
					if (int.TryParse (listOfCharacterDialogs [tempG] [pathNum1].Conversation_Path_Chain, out pathNum1)) {
						pathNum1 = int.Parse (listOfCharacterDialogs [tempG] [pathNum1].Conversation_Path_Chain);
						if(pathNum1 != -1) pathNum1 = newPathNums [pathNum1];
						temp = temp.Substring (bar + 1);
					}

					if (dialog.Value.Count > pathNum1 && pathNum1 != -1) {
						//question
						if(useDiolog) dialog.Key.GetComponent<makeText> ().path1.Add (dialog.Value [pathNum1].CUE);
						dialogsInOrder [dialog.Key].Add (dialog.Value [pathNum1].CUE);
						dialog.Value.RemoveAt (pathNum1);

						//walk away
						if(useDiolog) dialog.Key.GetComponent<makeText> ().path2.Add ("Bye!");
						if(useDiolog) dialog.Key.GetComponent<makeText> ().path2.Add ("RESET");
						dialogsInOrder [dialog.Key].Add ("Bye");
						dialogsInOrder [dialog.Key].Add ("RESET");
					}
				}
			}
		}
	}
}

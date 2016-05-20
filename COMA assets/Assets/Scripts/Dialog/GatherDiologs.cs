using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GatherDiologs : MonoBehaviour
{

	public Dictionary<GameObject, List<string>> dialogsInOrder = new Dictionary<GameObject, List<string>> ();
	public TextAsset csvFile;
	public List<int> tetp;

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

		GameObject[] interactable = GameObject.FindGameObjectsWithTag ("Interactable");
		GameObject[] allNPC = GameObject.FindGameObjectsWithTag ("NPC");
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		GameObject boss = GameObject.FindGameObjectWithTag ("Boss");

		Dictionary<GameObject, List<Row>> listOfCharacterDialogs = new Dictionary<GameObject, List<Row>> (); //get all the npc gameobjects row diologs
		int[] newPathNums = new int[read.getRowsLength()]; //length is identifier, 2nd int for new idex value that goes with it's new list assignment
		newPathNums = read.FindDiologIndexes();

		//get diolag for all the NPC characters
		for (int i = 0; i < allNPC.Length; i++) {
			if (!listOfCharacterDialogs.ContainsKey (allNPC [i])) 
				listOfCharacterDialogs.Add (allNPC [i], read.FindAll_ACTOR (allNPC [i].name));
		}

		//get dialog for all the interactable object characters
		for (int i = 0; i < interactable.Length; i++) {
			if (!listOfCharacterDialogs.ContainsKey (interactable [i])) {
				listOfCharacterDialogs.Add (interactable [i], read.FindAll_ACTOR (interactable [i].name));
			}
		}

		//get dialog for the boss (should be essentially the same for NPC)
		bool isBoss = false;
		if (boss != null) {
			listOfCharacterDialogs.Add (boss, read.FindAll_ACTOR (boss.name));
			isBoss = true;
		}

		//get dialog for the player (this should be just for conversation response purposes)
		if (player != null && !listOfCharacterDialogs.ContainsKey (player))
			listOfCharacterDialogs.Add (player, read.FindAll_ACTOR (player.name));

		bool splitPath = false;
		foreach (KeyValuePair<GameObject, List<Row>> dialog in listOfCharacterDialogs) {
			if (!dialog.Key.tag.Equals ("Player") && dialog.Value [0].LOCATION.Equals (Application.loadedLevelName)) {
				//get the next conversation identifier number or numbers if they exist
				int pathChainIndex = -1;
				List<int> values = GetPathSplitIndex (dialog.Value [0]);
				pathChainIndex = values.Count > 0 ? values[0] : -1;
				if(pathChainIndex != -1 && values.Count > 1) splitPath = true;

				//set the first dialog
				dialog.Key.GetComponent<makeText> ().dialogue.Add (dialog.Value [0].CUE);

				//GameObject tempActor = null; //this is for when blues secondary dialog is needed
				int count = 0;
				while (pathChainIndex != -1) {
					if (dialog.Key.name.Equals ("Person1"))
						Debug.Log (pathChainIndex);
					//if is for when path ways for dialog are needed
					if (dialog.Value.Count > newPathNums [pathChainIndex] && splitPath) {
						pathChainIndex = newPathNums [pathChainIndex];
						if(listOfCharacterDialogs[player].Count > pathChainIndex) {
							dialog.Key.GetComponent<makeText> ().dialogue.Add ("CHOICE"); //initiate the choice for blue to be able to respond
							dialog.Key.GetComponent<makeText> ().dialogue.Add (listOfCharacterDialogs[player][pathChainIndex].CUE); //add blues first response option

							//get new values for the players choice path dialog for choice 1
							List<int> newValues = GetPathSplitIndex (listOfCharacterDialogs[player][pathChainIndex]);
							tetp = newValues;
							pathChainIndex = newValues.Count > 0 ? newValues[0] : -1;
							if (pathChainIndex != -1) {
								pathChainIndex = newPathNums [pathChainIndex];
								dialog.Key.GetComponent<makeText> ().path1.Add (dialog.Value [pathChainIndex].CUE);
							}

							//get next choice 2 dialog for blue
							pathChainIndex = values.Count > 1 ? values[1] : -1;
							if (pathChainIndex != -1) {
								pathChainIndex = newPathNums [pathChainIndex];
								dialog.Key.GetComponent<makeText> ().dialogue.Add (listOfCharacterDialogs [player] [pathChainIndex].CUE);
							}

							//get new values for the players choice path dialog for choice 2
							newValues = GetPathSplitIndex (listOfCharacterDialogs[player][pathChainIndex]);
							pathChainIndex = newValues.Count > 0 ? newValues[0] : -1;
							if (pathChainIndex != -1) {
								pathChainIndex = newPathNums [pathChainIndex];
								dialog.Key.GetComponent<makeText> ().path2.Add (dialog.Value [pathChainIndex].CUE);
							}
						}
					} else if(pathChainIndex != -1 && dialog.Value.Count > newPathNums [pathChainIndex]){ //else is for when the dialog is a continuous chat
						pathChainIndex = newPathNums [pathChainIndex];
						dialog.Key.GetComponent<makeText> ().dialogue.Add (dialog.Value [pathChainIndex].CUE);
					}

					if (pathChainIndex != -1 && dialog.Value.Count > pathChainIndex) {
						splitPath = false;
						int lastVal = pathChainIndex;

						//if statment is for if dialog for the player becomes available
						if (listOfCharacterDialogs[player].Count > pathChainIndex && player.name == read.ActorAtIndex(pathChainIndex)) {
							values = GetPathSplitIndex (listOfCharacterDialogs[player][pathChainIndex]);
							pathChainIndex = values.Count > 1 ? values[0] : -1;
							if(pathChainIndex != -1 && values.Count > 1) splitPath = true;
						} else if(pathChainIndex != -1){ //else statment is for if the dialog is just a simple conversation chain
							values = GetPathSplitIndex (dialog.Value [pathChainIndex]);
							pathChainIndex = values.Count > 1 ? values[0] : -1;
							if(pathChainIndex != -1 && values.Count > 1) splitPath = true;
						}

						dialog.Value.RemoveAt (lastVal);
					} 
				}

				if(dialog.Value.Count != 0 && dialog.Value[0].CUE == dialog.Key.GetComponent<makeText> ().dialogue[0]) dialog.Value.RemoveAt (0);
			}
		}
	}

	private List<int> GetPathSplitIndex(Row dialogStuff){
		List<int> values = new List<int> ();
		int dummyVal = 0;
		int barIndex = 0;
		int startIndex = 0;

		foreach (char c in dialogStuff.Conversation_Path_Chain) {
			if (c.Equals ('|')) {
				if (int.TryParse (dialogStuff.Conversation_Path_Chain.Substring (startIndex, barIndex), out dummyVal)){
					values.Add(int.Parse (dialogStuff.Conversation_Path_Chain.Substring (startIndex, barIndex))); //still need to do somthing with the value that is after bar
					startIndex = barIndex + 1;
				}
			}

			barIndex++;
		}

		//for the last path or for a singlton
		if (int.TryParse (dialogStuff.Conversation_Path_Chain.Substring (startIndex), out dummyVal)) {
			values.Add(int.Parse (dialogStuff.Conversation_Path_Chain.Substring (startIndex))); //still need to do somthing with the value that is after bar
		}

		return values;
	}
}

/*Check actor to see which gameObject will be given the dialg
 * Follow path chain till -1 is reached
 * if a new chat is found that is from an old actor then add to next dialog
 * 		if right below first instance player and path chain does not lead to each other then this is a second approach dialog (nextDialog)
 * look for when the converation path chain leads to Blue (or a character that's not the actor) then set that next path dialog to path1 and path2
 * the first time a character is encountered then have that be their first dialog
 * */
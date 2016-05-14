using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GatherDiologs : MonoBehaviour
{

	public Dictionary<GameObject, List<string>> dialogsInOrder = new Dictionary<GameObject, List<string>> ();
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
		GameObject boss = GameObject.FindGameObjectWithTag ("Boss");

		Dictionary<GameObject, List<Row>> listOfCharacterDialogs = new Dictionary<GameObject, List<Row>> (); //get all the npc gameobjects row diologs
		int[] newPathNums = new int[read.getRowsLength()]; //length is identifier, 2nd int for new idex value that goes with it's new list assignment
		newPathNums = read.FindDiologIndexes();

		for (int i = 0; i < allNPC.Length; i++) {
			if (!listOfCharacterDialogs.ContainsKey (allNPC [i])) 
				listOfCharacterDialogs.Add (allNPC [i], read.FindAll_ACTOR (allNPC [i].name));
		}

		bool isBoss = false;
		if (boss != null) {
			listOfCharacterDialogs.Add (boss, read.FindAll_ACTOR (boss.name));
			isBoss = true;
		}
		
		if (player != null && !listOfCharacterDialogs.ContainsKey (player))
			listOfCharacterDialogs.Add (player, read.FindAll_ACTOR (player.name));
		

		bool firstFound = false;
		foreach (KeyValuePair<GameObject, List<Row>> dialog in listOfCharacterDialogs) {
			for (int i = 0; i < dialog.Value.Count && dialog.Value.Count > 0; i++) {
				if (dialog.Key.tag.Equals ("Boss")) break;

				if (dialog.Value [i].LOCATION.Equals (Application.loadedLevelName)) {
					if (!firstFound && dialog.Value [i].CONTEXT.Contains ("1st approach")) {
						dialog.Key.GetComponent<makeText> ().dialogue.Add (dialog.Value [i].CUE);
						if (!dialog.Value [i].Choice_Type.Equals ("Continue"))
							ChoicePath (dialog, player, listOfCharacterDialogs, newPathNums, i, true);
						else {
							int pathNum1 = 0;
							int.TryParse (dialog.Value [i].Conversation_Path_Chain, out pathNum1);
							GameObject tempG = GameObject.Find (dialog.Value [i].GameObject_Interacted_With);

							if(pathNum1 != -1) ConversationSetter (dialog, player, tempG, listOfCharacterDialogs, newPathNums, pathNum1, -1, i, true);
						}
							
						if (dialog.Value.Count > i && dialog.Value [i] != null)
							dialog.Value.RemoveAt (i);
						firstFound = true;
						i = -1;
					} else if (firstFound) {
						//search string for approach if this is found then just append second
						//if not found then check choice_type
						if (dialog.Value [i].Choice_Type.Equals ("Quest"))
							dialog.Key.GetComponent<makeText> ().thanks.Add (dialog.Value [i].CUE);
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
						if (dialog.Value.Count > i && dialog.Value [i].CONTEXT.EndsWith ("approach")) {
							if (!dialogsInOrder.ContainsKey (dialog.Key)) {
								dialogsInOrder [dialog.Key].Add (dialog.Value [i].CUE);
								dialogsInOrder [dialog.Key].Add ("SECOND");
							} 

							if (!dialog.Key.GetComponent<makeText> ().dialogue.Contains ("SECOND") && !dialog.Key.GetComponent<makeText> ().dialogue.Contains ("CONVERSATION") &&
									!dialog.Key.GetComponent<makeText> ().dialogue.Contains ("CHOICE")) {
								dialog.Key.GetComponent<makeText> ().dialogue.Add ("SECOND");
								dialog.Key.GetComponent<makeText> ().nextDialog.Add (listOfCharacterDialogs [dialog.Key] [i].CUE);
							} else {
								//dialog.Key.GetComponent<makeText> ().extraDialog.Add ("SECOND");
								dialog.Key.GetComponent<makeText> ().nextDialog.Add (listOfCharacterDialogs [dialog.Key] [i].CUE);
							}

							dialog.Value.RemoveAt (i);
							i = -1;
						} else {
							if (i > dialog.Value.Count)
								i = -1;
						}
					} else if (!firstFound && i + 1 == dialog.Value.Count && dialog.Key.name != "Blue") {
						if(!dialogsInOrder.ContainsKey(dialog.Key)) dialogsInOrder.Add (dialog.Key, new List<string> {dialog.Value [i].CUE});
						dialog.Key.GetComponent<makeText> ().dialogue.Add (dialog.Value [i].CUE);
						dialog.Value.RemoveAt (i);
						firstFound = true;
						i = -1;
					}
				}
			}

			if (!dialog.Key.tag.Equals ("Boss") && dialogsInOrder.ContainsKey (dialog.Key) && dialog.Value.Count == 0) {
				//dialog.Key.GetComponent<makeText> ().nextDialog.Add ("END");
				dialogsInOrder [dialog.Key].Add ("END");
			}

			firstFound = false;
		}

		//boss dialog
		if(isBoss){
			BossCheck (read, boss);
		}

		//after getting all the npc diologs get blues remaining diologs
		//these are already set under blues diolog just need to set them to the correct objects
		InteractablesCheck(read, player, boss);
	}

	private void BossCheck(ReadSpreadSheets read, GameObject boss){
		List<Row> bossDiologs = read.FindAll_ACTOR (boss.name);
		if(!dialogsInOrder.ContainsKey(boss)) dialogsInOrder.Add(boss, new List<string>());

		for (int i = 0; i < bossDiologs.Count; i++) {
			if (bossDiologs [i].LOCATION.Equals (Application.loadedLevelName)) {
				if (bossDiologs [i].CONTEXT.Contains ("Blue enters") && boss.GetComponent<makeText> ().dialogue.Count < 1) {
					boss.GetComponent<makeText> ().dialogue.Add (bossDiologs [i].CUE);
					boss.GetComponent<makeText> ().dialogue.Add ("END");
					dialogsInOrder [boss].Add (bossDiologs [i].CUE);
					dialogsInOrder [boss].Add ("END");
				} else if (bossDiologs [i].CONTEXT.Contains ("<Good/True Path>")) {
					boss.GetComponent<makeText> ().path1.Add (bossDiologs [i].CUE);
					dialogsInOrder [boss].Add (bossDiologs [i].CUE);
				}else if (bossDiologs [i].CONTEXT.Contains ("<Evil Path/ Terrible finish>")) {
					boss.GetComponent<makeText> ().path2.Add (bossDiologs [i].CUE);
					dialogsInOrder [boss].Add (bossDiologs [i].CUE);
				}
				else {
					boss.GetComponent<makeText> ().nextDialog.Add (bossDiologs [i].CUE);
					dialogsInOrder [boss].Add (bossDiologs [i].CUE);
				}
			}
		}

		boss.GetComponent<makeText> ().nextDialog.Add ("BATTLE");
		dialogsInOrder [boss].Add ("BATTLE");
		dialogsInOrder [boss].Add ("END");
	}

	private void InteractablesCheck(ReadSpreadSheets read, GameObject player, GameObject boss){
		List<Row> playerDiologs = read.FindAll_ACTOR (player.name);
		if(!dialogsInOrder.ContainsKey(player)) dialogsInOrder.Add(player, new List<string>());

		for (int i = 0; i < playerDiologs.Count; i++) {
			if (playerDiologs [i].LOCATION.Equals (Application.loadedLevelName)) {
				GameObject tempG = GameObject.Find (playerDiologs [i].GameObject_Interacted_With);

				if (tempG != null && !tempG.tag.Equals ("NPC") && !tempG.tag.Equals ("Boss")) {
					//List<Row> noNPC = read.FindAll_ACTOR (tempG.name);
					if (tempG.GetComponent<makeText> ().dialogue.Count < 1) {
						tempG.GetComponent<makeText> ().dialogue.Add (playerDiologs [i].CUE);
						dialogsInOrder [player].Add (playerDiologs [i].CUE);
					} else {
						tempG.GetComponent<makeText> ().nextDialog.Add (playerDiologs [i].CUE);
						dialogsInOrder [player].Add (playerDiologs [i].CUE);
					}
					//there seems to be an issue where the player was not inserted possibly into the list of character diologs need to double check this
				}

				if(tempG != null && tempG.tag.Equals ("Boss")){
					boss.GetComponent<makeText> ().dialogue[boss.GetComponent<makeText> ().dialogue.Count - 1] = "CONTINUE";
					dialogsInOrder [boss][dialogsInOrder.Count - 1] = "CONTINUE";
					tempG.GetComponent<makeText> ().response = playerDiologs [i].CUE;
					dialogsInOrder [player].Add (playerDiologs [i].CUE);
				}
			}

			dialogsInOrder [player].Add ("END");
		}
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
				if(dialog.Value[i].Choice_Type.Equals("Quest") || dialog.Value[i].Choice_Type.Equals("Conversation")){
					dialogsInOrder [dialog.Key].Add ("CHOICE");
					if (useDiolog)
						dialog.Key.GetComponent<makeText> ().dialogue.Add ("CHOICE");
				}

				GameObject tempG = GameObject.Find (dialog.Value [i].GameObject_Interacted_With);

				//issue if the conversations are not in order then the pathnum finder will not work how to check value with out known index value?
				//possibly create a functon in readspreadsheet that will allow this special access find?

				//this is for a quest
				if (listOfCharacterDialogs.ContainsKey (tempG) && listOfCharacterDialogs [tempG].Count > pathNum1 && pathNum1 != -1 && listOfCharacterDialogs [tempG].Count > pathNum2 && pathNum2 != -1) {
					QuestSetter (dialog, player, tempG, listOfCharacterDialogs, newPathNums, pathNum1, pathNum2, i, useDiolog);
				}
				//this is for a conversation
				else if (listOfCharacterDialogs [dialog.Key].Count > pathNum1 && pathNum1 != -1) {
					ConversationSetter (dialog, player, tempG, listOfCharacterDialogs, newPathNums, pathNum1, pathNum2, i, useDiolog);
				} 
				//add basic text choice diolog for generic characters
				else if (pathNum1 != -1) {
						if (useDiolog)
							dialog.Key.GetComponent<makeText> ().dialogue.Add (QUESTION);
						dialogsInOrder [dialog.Key].Add (QUESTION);
						if (useDiolog)
							dialog.Key.GetComponent<makeText> ().dialogue.Add (WALK_AWAY);
						dialogsInOrder [dialog.Key].Add (WALK_AWAY);
					
					//dialogsInOrder [dialog.Key].Add ("SAME");
				}

			}
		}
	}

	private void QuestSetter(KeyValuePair<GameObject, List<Row>> dialog, GameObject player, GameObject tempG, Dictionary<GameObject, List<Row>> listOfCharacterDialogs, int[] newPathNums, int pathNum1, int pathNum2, int i, bool useDiolog){
		//insert the path1 diolog choice
		Row path1Diolog = listOfCharacterDialogs [tempG] [pathNum1];
		if (useDiolog)
			dialog.Key.GetComponent<makeText> ().dialogue.Add (path1Diolog.CUE);
		dialogsInOrder [dialog.Key].Add (path1Diolog.CUE);

		//insert the path2 diolog choice
		Row path2Diolog = listOfCharacterDialogs [tempG] [pathNum2];
		if (useDiolog)
			dialog.Key.GetComponent<makeText> ().dialogue.Add (path2Diolog.CUE);
		dialogsInOrder [dialog.Key].Add (path2Diolog.CUE);

		//get next diologs
		if (int.TryParse (path1Diolog.Conversation_Path_Chain, out pathNum1)) {
			pathNum1 = int.Parse (path1Diolog.Conversation_Path_Chain);
			if (pathNum1 != -1)
				pathNum1 = newPathNums [pathNum1];
			listOfCharacterDialogs [tempG].Remove (path1Diolog);
		}

		if (int.TryParse (path2Diolog.Conversation_Path_Chain, out pathNum2)) {
			pathNum2 = int.Parse (path2Diolog.Conversation_Path_Chain);
			if (pathNum2 != -1)
				pathNum2 = newPathNums [pathNum2];
			listOfCharacterDialogs [tempG].Remove (path2Diolog);
		}

		//insert to path1 the diolog that follows this choice
		if (dialog.Value.Count > pathNum1 && pathNum1 != -1) {
			if (useDiolog) {
				dialog.Key.GetComponent<makeText> ().path1.Add (dialog.Value [pathNum1].CUE);
				dialog.Key.GetComponent<makeText> ().path1.Add ("QUEST");
			}

			dialogsInOrder [dialog.Key].Add (dialog.Value [pathNum1].CUE);
			dialogsInOrder [dialog.Key].Add ("QUEST");
			dialog.Value.RemoveAt (pathNum1);

			pathNum2--;
		}

		//insert to path2 the diolog that follows this choice
		if (dialog.Value.Count > pathNum2 && pathNum2 != -1) {
			if (useDiolog)
				dialog.Key.GetComponent<makeText> ().path2.Add (dialog.Value [pathNum2].CUE);
			if (useDiolog)
				dialog.Key.GetComponent<makeText> ().path2.Add ("RESET");
			if (dialog.Key.name == "System")
				Debug.Log (dialog.Value [pathNum2].CUE);
			dialogsInOrder [dialog.Key].Add (dialog.Value [pathNum2].CUE);
			dialogsInOrder [dialog.Key].Add ("RESET");
			dialog.Value.RemoveAt (pathNum2);
		}
	}

	private void ConversationSetter(KeyValuePair<GameObject, List<Row>> dialog, GameObject player, GameObject tempG, Dictionary<GameObject, List<Row>> listOfCharacterDialogs, int[] newPathNums, int pathNum1, int pathNum2, int i, bool useDiolog){
		if (pathNum1 > dialog.Value.Count)
			pathNum1 = newPathNums [pathNum1];
		
		//add basic diolog for generic characters
		if (dialog.Value [pathNum1].GameObject_Interacted_With.Equals ("Blue") && !dialog.Value[i].Choice_Type.Equals("Continue")) {
			if (useDiolog)
				dialog.Key.GetComponent<makeText> ().dialogue.Add (QUESTION);
			dialogsInOrder [dialog.Key].Add (QUESTION);
			if (useDiolog)
				dialog.Key.GetComponent<makeText> ().dialogue.Add (WALK_AWAY);
			dialogsInOrder [dialog.Key].Add (WALK_AWAY);

			Row path1Diolog = listOfCharacterDialogs [dialog.Key] [pathNum1];

			if (dialog.Value.Count > pathNum1 && pathNum1 != -1) {
				//question
				if (useDiolog)
					dialog.Key.GetComponent<makeText> ().path1.Add (dialog.Value [pathNum1].CUE);
				if (useDiolog)
					dialog.Key.GetComponent<makeText> ().path1.Add ("SECOND");
				dialogsInOrder [dialog.Key].Add (dialog.Value [pathNum1].CUE);
				dialogsInOrder [dialog.Key].Add ("SECOND");
				dialog.Value.RemoveAt (pathNum1);

				//walk away
				if (useDiolog)
					dialog.Key.GetComponent<makeText> ().path2.Add ("Bye!");
				if (useDiolog)
					dialog.Key.GetComponent<makeText> ().path2.Add ("SECOND");
				dialogsInOrder [dialog.Key].Add ("Bye");
				dialogsInOrder [dialog.Key].Add ("SECOND");
			}
		} else if(dialog.Value[i].Choice_Type.Equals("Continue")){
			if (useDiolog) {
				dialog.Key.GetComponent<makeText> ().dialogue.Add ("CONVERSATION");
				if (!dialogsInOrder.ContainsKey (dialog.Key))
					dialogsInOrder.Add (dialog.Key, new List<string>());
				dialogsInOrder [dialog.Key].Add ("CONVERSATION");
			}

			//GameObject nextActor = dialog [dialog.Value [pathNum1].ACTOR];
			Row nextDialogRow = dialog.Value [pathNum1];

			while (!dialog.Value [pathNum1].Choice_Type.Equals ("END")) {
				dialog.Key.GetComponent<makeText> ().extraDialog.Add (dialog.Value [pathNum1].CUE);
				dialogsInOrder [dialog.Key].Add (dialog.Value [pathNum1].CUE);

				if (dialog.Value.Count > pathNum1 && dialog.Value [pathNum1] != null) {
					int.TryParse (dialog.Value [pathNum1].Conversation_Path_Chain, out pathNum1);

					dialog.Value.RemoveAt (pathNum1);

					if (pathNum1 != -1) {
						pathNum1 = newPathNums [pathNum1];
					}
				}
			}
			dialog.Key.GetComponent<makeText> ().extraDialog.Add (dialog.Value [pathNum1].CUE);
			dialogsInOrder [dialog.Key].Add (dialog.Value [pathNum1].CUE);

			dialog.Key.GetComponent<makeText> ().extraDialog.Add ("SECOND");
			dialogsInOrder [dialog.Key].Add ("SECOND");

			if (dialog.Value.Count > pathNum1 && dialog.Value [pathNum1] != null) {
				dialog.Value.RemoveAt (pathNum1);
			}

		}
	}
}

//still need to setup for a possible three path choice system of dialog
//also need to do next diolog inserts once one set has been gone through
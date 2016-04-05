using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/*USAGE NOTES
 * Type your text in the dialogue tree.
 * If the player gets a choice, type one box as "CHOICE"
 * then the next two boxes as the two choices, respectively.
 * Type the text following the choice in the two path trees,
 * the first choice will correspond to the path1 tree.
 * If the player accepts a quest from a character, type "QUEST"
 * as the last box of that tree.
 * If the player denies a quest, or otherwise needs to restart the conversation,
 * type "RESET" as the last box of that tree.*/

//current issue if you are collising with more then one npc then the diolog box will not open

public class makeText : MonoBehaviour {

	new GameObject textObject;
	new GameObject choiceObject;
	new GameObject choiceObject2;
	public GameObject player;
	public GameObject dialogCSVParserObject;
	Text choice1;
	Text choice2;
	Image ChBG;
	Text words;
	Image BG;
	public List<string> dialogue = new List<string>();
	public List<string> path1 = new List<string> ();
	public List<string> path2 = new List<string> ();
	public List<string> nextDialog = new List<string>();
	public string response = string.Empty;
	List<string> storage = new List<string> ();
	/*public List<string> waiting = new List<string> ();
	public List<string> thanks = new List<string> ();
	static int quest = 0;*/
	static int i = 0;
	static int j = 0;

	private bool nearCharacter = false;
	private bool inConversation = false;

	// Use this for initialization
	void Start () {
		storage = dialogue;
		textObject = GameObject.Find ("Text");
		choiceObject = GameObject.Find ("Ctext1");
		choiceObject2 = GameObject.Find ("Ctext2");
		words = textObject.GetComponent<Text> ();
		BG = textObject.GetComponentInParent<Image> ();
		choice1 = choiceObject.GetComponent<Text> ();
		ChBG = choiceObject.GetComponentInParent<Image> ();
		choice2 = choiceObject2.GetComponent<Text> ();
		words.enabled = false;
		BG.enabled = false;
		choice1.enabled = false;
		choice2.enabled = false;
		ChBG.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (nearCharacter) {
			if (choice1.enabled == false) {
				if (words.enabled == true) {
					words.text = dialogue [i];
				}
				if (Input.GetKeyDown (KeyCode.Space) && words.enabled == true) {
					if(i < dialogue.Count) i++;

					if (i > dialogue.Count - 1) {
						words.enabled = false;
						BG.enabled = false;
						player.GetComponent<PlayerMovement> ().inDialog = false;
					} else {
						if (dialogue [i].CompareTo ("CHOICE") == 0) {
							choice1.enabled = true;
							choice2.enabled = true;
							ChBG.enabled = true;
							if (i < dialogue.Count)
								i++;
							choice1.text = dialogue [i];
							if (i < dialogue.Count)
								i++;
							choice2.text = dialogue [i];
						}
						if (dialogue [i].CompareTo ("CONTINUE") == 0) {
							choice1.enabled = true;
							ChBG.enabled = true;
							choice1.text = response;
						}
						/*if (dialogue[i].CompareTo("QUEST") == 0){
						  this.quest = 1;
						  dialogue = waiting;
						  }*/
						if (dialogue [i].CompareTo ("RESET") == 0) {
							dialogue = storage;
							i = 0;
							words.enabled = false;
							BG.enabled = false;
							inConversation = false;
							player.GetComponent<PlayerMovement> ().inDialog = false;
						}
						if (dialogue [i].CompareTo ("SECOND") == 0) {
							dialogue = nextDialog;
							i = 0;
							words.enabled = false;
							BG.enabled = false;
							inConversation = false;
							player.GetComponent<PlayerMovement> ().inDialog = false;
						}
						if (dialogue [i].CompareTo ("BATTLE") == 0) {
							words.enabled = false;
							BG.enabled = false;
							inConversation = false;
							player.GetComponent<PlayerMovement> ().inDialog = false;
							//start the boss battle
						}
					}
				} else if (Input.GetKeyDown (KeyCode.Space)) {
					words.enabled = true;
					BG.enabled = true;
					player.GetComponent<PlayerMovement> ().inDialog = true;
					i = 0;
					/*if (this.quest == 1 && inventory.questItem.obtained == true){
					inventory.questItem.obtained = false;
					this.quest = 2;
					dialogue = thanks;
				}*/
				}
			} else {
				if (j == 0) {
					choice1.color = new Color (176.0f / 255.0f, 160.0f / 255.0f, 254.0f / 255.0f);
					choice2.color = new Color (124.0f / 255.0f, 110.0f / 255.0f, 195.0f / 255.0f);
				} else {
					choice1.color = new Color (124.0f / 255.0f, 110.0f / 255.0f, 195.0f / 255.0f);
					choice2.color = new Color (176.0f / 255.0f, 160.0f / 255.0f, 254.0f / 255.0f);
				}
				if (Input.GetKeyDown (KeyCode.DownArrow))
					j = 1;
				if (Input.GetKeyDown (KeyCode.UpArrow))
					j = 0;
				if (Input.GetKeyDown (KeyCode.Space)) {
					if (choice1.enabled && choice2.enabled) {
						if (j == 0) {
							if (dialogue != path1)
								dialogue = path1;
							else
								dialogue = nextDialog;
						}  else
							dialogue = path2;
					}
					else {
						dialogue = nextDialog;
					}
					i = 0;
					choice1.enabled = false;
					choice2.enabled = false;
					ChBG.enabled = false;
				}
			}
		}

		//this is for each characters dialogs
		if (dialogCSVParserObject.GetComponent<GatherDiologs> ().dialogsInOrder.ContainsKey (gameObject)) {
			List<string> characterDialog = dialogCSVParserObject.GetComponent<GatherDiologs> ().dialogsInOrder [gameObject];

			//here check when diolog == path1 if it does then check for next diolg sequence also check first if END exists

			//check if next diolag has been set then change last SECOND path string to RESET
			if (nextDialog.Equals (dialogue) && path1.Count > 0 && path2.Count > 0) {
			
				bool setNextDialog = false;
				for (int i = 0; i < characterDialog.Count; i++) {
					if (characterDialog [i].Equals (nextDialog [0])) {
						dialogue = new List<string> ();
						setNextDialog = true;
					}
					if (setNextDialog && !characterDialog [i].Equals ("END")) {
						dialogue.Add (characterDialog [i]);
					}
				}
				storage = dialogue;
				path1 [path1.Count - 1] = "RESET";
				path2 [path2.Count - 1] = "RESET";
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D obj){
		if (obj.tag.Equals ("Player") && obj.isTrigger)
			nearCharacter = true;
	}

	public void OnTriggerExit2D(Collider2D obj){
		if (obj.tag.Equals ("Player") && obj.isTrigger) {
			nearCharacter = false;
			obj.GetComponent<PlayerMovement> ().inDialog = false;
		}
	}
}
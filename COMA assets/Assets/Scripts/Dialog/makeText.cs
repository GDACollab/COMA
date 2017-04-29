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

public class makeText : MonoBehaviour
{

    GameObject textObject;
    GameObject choiceObject;
    GameObject choiceObject2;
    GameObject playerObject;
    Text choice1;
    Text choice2;
    Image ChBG;
    Text words;
    Image BG;
    public List<string> dialogue = new List<string>();
    public List<string> path1 = new List<string>();
    public List<string> path2 = new List<string>();
    List<string> storage = new List<string>();
    /*public List<string> waiting = new List<string> ();
	public List<string> thanks = new List<string> ();
	static int quest = 0;*/
    static int i = 0;
    static int j = 0;
    bool person = false;

    // Use this for initialization
    void Start()
    {
        storage = dialogue;
        textObject = GameObject.Find("Text");
        choiceObject = GameObject.Find("Ctext1");
        choiceObject2 = GameObject.Find("Ctext2");
        playerObject = GameObject.Find("Blue");
        words = textObject.GetComponent<Text>();
        BG = textObject.GetComponentInParent<Image>();
        choice1 = choiceObject.GetComponent<Text>();
        ChBG = choiceObject.GetComponentInParent<Image>();
        choice2 = choiceObject2.GetComponent<Text>();
        words.enabled = false;
        BG.enabled = false;
        choice1.enabled = false;
        choice2.enabled = false;
        ChBG.enabled = false;

    }
    void OnCollisionStay2D(Collider2D other)
    {
        if (other == playerObject)
        {
            person = true;
        }
    }
    void OnCollisionExit2D(Collider2D other)
    {
        if (other == playerObject)
        {
            person = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (choice1.enabled == false)
        {
            if (words.enabled == true)
            {
                words.text = dialogue[i];
            }
            if (Input.GetKeyDown(KeyCode.Space) && words.enabled == true)
            {
                i++;
                if (i > dialogue.Count - 1)
                {
                    words.enabled = false;
                    BG.enabled = false;
                }
                if (dialogue[i].CompareTo("CHOICE") == 0)
                {
                    choice1.enabled = true;
                    choice2.enabled = true;
                    ChBG.enabled = true;
                    i++;
                    choice1.text = dialogue[i];
                    i++;
                    choice2.text = dialogue[i];
                }
                /*if (dialogue[i].CompareTo("QUEST") == 0){
				 * this.quest = 1;
				 * dialogue = waiting;
				 * }*/
                if (dialogue[i].CompareTo("RESET") == 0)
                {
                    dialogue = storage;
                    i = 0;
                    words.enabled = false;
                    BG.enabled = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space) && )
            {
                words.enabled = true;
                BG.enabled = true;
                i = 0;
                /*if (this.quest == 1 && inventory.questItem.obtained == true){
					inventory.questItem.obtained = false;
					this.quest = 2;
					dialogue = thanks;
				}*/
            }
        }
        else {
            if (j == 0)
            {
                choice1.color = new Color(176.0f / 255.0f, 160.0f / 255.0f, 254.0f / 255.0f);
                choice2.color = new Color(124.0f / 255.0f, 110.0f / 255.0f, 195.0f / 255.0f);
            }
            else {
                choice1.color = new Color(124.0f / 255.0f, 110.0f / 255.0f, 195.0f / 255.0f);
                choice2.color = new Color(176.0f / 255.0f, 160.0f / 255.0f, 254.0f / 255.0f);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
                j = 1;
            if (Input.GetKeyDown(KeyCode.UpArrow))
                j = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (j == 0)
                    dialogue = path1;
                else
                    dialogue = path2;
                i = 0;
                choice1.enabled = false;
                choice2.enabled = false;
                ChBG.enabled = false;
            }
        }
    }
}
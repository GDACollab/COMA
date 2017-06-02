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
 * Type BOSS as the last box of a tree to enter a boss battle after a conversation.
 * If the player denies a quest, or otherwise needs to restart the conversation,
 * type "RESET" as the last box of that tree.*/

public class makeText : MonoBehaviour
{

    GameObject textObject;
    GameObject choiceObject;
    GameObject choiceObject2;
    public GameObject playerObject;
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
    static int i;
    static int j = 0;
    bool person;
    string boss = "Starlet battle normal";

    // Use this for initialization
    void Start()
    {
        i = 0;
        storage = dialogue;
        textObject = GameObject.Find("Text");
        choiceObject = GameObject.Find("Ctext1");
        choiceObject2 = GameObject.Find("Ctext2");
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
        person = false;

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == playerObject)
        {
            person = true;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject == playerObject)
        {
            person = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(i);
        Debug.Log(words.enabled);
        if (person){
            if (choice1.enabled == false)
            {
                if (words.enabled == true)
                {
                    words.text = dialogue[i];
                }
                if (Input.GetKeyDown(KeyCode.Space) && words.enabled == true)
                {
                    Debug.Log("increment");
                    i++;
                    Debug.Log(dialogue[i]);
                    if (i > dialogue.Count - 1)
                    {
                        Debug.Log("shutting down");
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
                    if (dialogue[i].CompareTo("BOSS") == 0){
                        Angel.TransitionFromFieldToBattle(boss);
                    }
                    if (dialogue[i].CompareTo("RESET") == 0)
                    {
                        dialogue = storage;
                        i = 0;
                        words.enabled = false;
                        BG.enabled = false;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("space");
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
}
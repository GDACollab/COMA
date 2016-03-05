using UnityEngine;
using System.Collections;

public class spawnJ : MonoBehaviour {
	
	public GameObject note;
    public Sprite noteS;
	public Sprite noteD;
	public GameObject[] Jscore = new GameObject[10];
	public int bottom = -1;
	public int top = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void kill(){
		Destroy (Jscore [top]);
		Jscore [top] = null;
		top = (top == 9) ? 0 : top + 1;
	}
	public void JNoteS(){
		GameObject temp = Instantiate (note);
		temp.transform.position = new Vector3 (1, 0, 2);
		temp.name = "Note";
        temp.GetComponent<SpriteRenderer>().sprite = noteS;
		bottom = (bottom == 9) ? 0:bottom+1;
		Jscore [bottom] = temp;
	}
	public void JNoteD(){
        GameObject temp = Instantiate(note);
        temp.transform.position = new Vector3(1, 0, 2);
        temp.name = "Note";
        temp.GetComponent<SpriteRenderer>().sprite = noteD;
        bottom = (bottom == 9) ? 0 : bottom + 1;
        Jscore[bottom] = temp;
    }
}

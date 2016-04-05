using UnityEngine;
using System.Collections;

public class spawnC : MonoBehaviour {
	
	public GameObject note;
    public Sprite noteS;
	public Sprite noteD;
	public GameObject[] Cscore = new GameObject[10];
	public int bottom = -1;
	public int top = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void kill(){
		Destroy (Cscore [top]);
		Cscore [top] = null;
		top = (top == 9) ? 0 : top + 1;
	}
	public void CNoteS(){
		GameObject temp = Instantiate (note);
		temp.transform.position = new Vector3 (7, 0, 2);
		temp.name = "Note";
        temp.GetComponent<SpriteRenderer>().sprite = noteS;
		bottom = (bottom == 9) ? 0:bottom+1;
		Cscore [bottom] = temp;
	}
	public void CNoteD(){
        GameObject temp = Instantiate(note);
        temp.transform.position = new Vector3(7, 0, 2);
        temp.name = "Note";
        temp.GetComponent<SpriteRenderer>().sprite = noteD;
        bottom = (bottom == 9) ? 0 : bottom + 1;
        Cscore[bottom] = temp;
    }
}

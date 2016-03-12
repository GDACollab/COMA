using UnityEngine;
using System.Collections;

public class spawnA : MonoBehaviour {

	public GameObject note;
    public Sprite noteS;
    public Sprite noteD;
	public GameObject[] Ascore = new GameObject[10];
	public int bottom = -1;
	public int top = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void kill(){
		Destroy (Ascore [top]);
		Ascore [top] = null;
		top = (top == 9) ? 0 : top + 1;
	}
	public void ANoteS(){
		GameObject temp = Instantiate (note);
		temp.transform.position = new Vector3 (-7, 0, 2);
		temp.name = "Note";
        temp.GetComponent<SpriteRenderer>().sprite = noteS;
		bottom = (bottom == 9) ? 0:bottom+1;
		Ascore [bottom] = temp;
	}
	public void ANoteD(){
		GameObject temp = Instantiate (note);
        temp.transform.position = new Vector3(-7, 0, 2);
        temp.name = "Note";
        temp.GetComponent<SpriteRenderer>().sprite = noteD;
        bottom = (bottom == 9) ? 0 : bottom + 1;
        Ascore[bottom] = temp;
	}
}

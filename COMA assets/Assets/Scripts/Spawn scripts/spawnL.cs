using UnityEngine;
using System.Collections;

public class spawnL : MonoBehaviour {
	
	public GameObject noteS;
	public GameObject noteD;
	public GameObject[] Lscore = new GameObject[10];
	public int bottom = -1;
	public int top = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void kill(){
		Destroy (Lscore [top]);
		Lscore [top] = null;
		top = (top == 9) ? 0 : top + 1;
	}
	public void LNoteS(){
		GameObject temp = Instantiate (noteS);
		temp.transform.position = new Vector3 (5, 0, 2);
		temp.name = "singleNote_mother";
		bottom = (bottom == 9) ? 0:bottom+1;
		Lscore [bottom] = temp;
	}
	public void LNoteD(){
		Instantiate (noteD);
	}
}

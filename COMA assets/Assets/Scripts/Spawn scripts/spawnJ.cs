using UnityEngine;
using System.Collections;

public class spawnJ : MonoBehaviour {
	
	public GameObject noteS;
	public GameObject noteD;
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
		GameObject temp = Instantiate (noteS);
		temp.transform.position = new Vector3 (1, 0, 2);
		temp.name = "singleNote_mother";
		bottom = (bottom == 9) ? 0:bottom+1;
		Jscore [bottom] = temp;
	}
	public void JNoteD(){
		Instantiate (noteD);
	}
}

using UnityEngine;
using System.Collections;

public class spawnC : MonoBehaviour {
	
	public GameObject noteS;
	public GameObject noteD;
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
		GameObject temp = Instantiate (noteS);
		temp.transform.position = new Vector3 (7, 0, 2);
		temp.name = "singleNote_mother";
		bottom = (bottom == 9) ? 0:bottom+1;
		Cscore [bottom] = temp;
	}
	public void CNoteD(){
		Instantiate (noteD);
	}
}

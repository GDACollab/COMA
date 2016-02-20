using UnityEngine;
using System.Collections;

public class spawnS : MonoBehaviour {
	
	public GameObject noteS;
	public GameObject noteD;
	public GameObject[] Sscore = new GameObject[10];
	public int bottom = -1;
	public int top = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void kill(){
		Destroy (Sscore [top]);
		Sscore [top] = null;
		top = (top == 9) ? 0 : top + 1;
	}
	public void SNoteS(){
		GameObject temp = Instantiate (noteS);
		temp.transform.position = new Vector3 (-5, 0, 2);
		temp.name = "singleNote_mother";
		bottom = (bottom == 9) ? 0:bottom+1;
		Sscore [bottom] = temp;
	}
	public void SNoteD(){
		Instantiate (noteD);
	}
}

using UnityEngine;
using System.Collections;

public class TitleInput : MonoBehaviour {

	public int selectionNumber;
	public const int maxSelection = 10;
	public int numChild;
	[SerializeField] GameObject[] childList;
	[SerializeField] GameObject[] desList;
	// Use this for initialization
	void Start () {
		selectionNumber = 0;
		numChild = 0;
		childList = new GameObject[maxSelection];
		desList = new GameObject[maxSelection];

		foreach (Transform t in transform) {
			//Debug.Log(t.gameObject);
			childList [numChild] = t.gameObject;
			desList[numChild] = t.gameObject.GetComponentInChildren<DeselectSel>().gameObject;
			//Debug.Log(t.gameObject.GetComponent<DeselectSel>());
			if(t.gameObject.GetComponent<DeselectSel>() == null) {
				childList[numChild].GetComponent<SpriteRenderer>().enabled = false;
				numChild++;
				if(numChild > maxSelection) {
					//error 
					numChild = maxSelection;
				}
			}
			else
				t.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}
		if (transform.parent && transform.parent.GetComponent<TitleInput>()) {
			//Debug.Log(GetComponentInParent<TitleInput>());
			hideMenu ();
			GetComponent<TitleInput>().enabled = false;
			selectionNumber = 1;
		}
		else
			childList [0].GetComponent<SpriteRenderer> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		int tempSel = selectionNumber;
		checkInput ();
		updateChoice (tempSel);
	}

	private void checkInput () {
		if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
			if (selectionNumber == numChild - 1) {
				//play error beep noise maybe
				
			} else
				selectionNumber++;
		} else if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
			if (selectionNumber == 0) {
				//play error beep noise maybe
				
			} else
				selectionNumber--;
		} else if (Input.GetKeyDown (KeyCode.Return)) {
			nextMenu ();
		} else if (Input.GetKeyDown (KeyCode.Backspace)) {
			prevMenu ();
		}
	}

	private void updateChoice (int oldNum) {
		if (selectionNumber != oldNum) {
			//childList[oldNum].GetComponent<Renderer>().material.color = Color.red;
			//childList[selectionNumber].GetComponent<Renderer>().material.color = Color.blue;
			childList[oldNum].GetComponent<SpriteRenderer>().enabled = false;
			desList[oldNum].GetComponent<SpriteRenderer>().enabled = true;
			childList[selectionNumber].GetComponent<SpriteRenderer>().enabled = true;
			desList[selectionNumber].GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	public void prevMenu() {
		if (this.transform.parent && this.transform.parent.GetComponent<TitleInput> ()) {
			TitleInput temp = this.transform.parent.GetComponent<TitleInput> ();
			//Debug.Log (temp.ToString ());
			hideMenu ();
			selectionNumber = 1;
			temp.enabled = true;
			temp.showMenu ();
			this.enabled = false;
		}
	}

	private void nextMenu() {
		if(childList[selectionNumber].GetComponent<TitleInput>()) {
			TitleInput temp = childList[selectionNumber].GetComponent<TitleInput>();
			hideMenu ();
			temp.showMenu ();
			temp.enabled = true;
			this.enabled = false;
		}
	}

	public void hideMenu() {
		//Vector3 scaleVar = new Vector3 (0, 0, 100);
		for (int i = 0; i < numChild; i++) {
			//childList [i].transform.position -= scaleVar;
			childList[i].GetComponent<SpriteRenderer>().enabled = false;
			desList[i].GetComponent<SpriteRenderer>().enabled = false;	
		}	
	}

	public void showMenu() {
		//Vector3 scaleVar = new Vector3 (0, 0, 100);
		for(int i = 0; i < numChild; i++) {
			//childList[i].transform.position += scaleVar;
			//childList[i].GetComponent<SpriteRenderer>().enabled = false;
			desList[i].GetComponent<SpriteRenderer>().enabled = true;
		}
		childList [selectionNumber].GetComponent<SpriteRenderer> ().enabled = true;
		desList[selectionNumber].GetComponent<SpriteRenderer>().enabled = false;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinkedTitle : MonoBehaviour {

	public class LinkedT {

		private List<GameObject> buttonList = new List<GameObject>();
		private List<LinkedT> subMenuList = new List<LinkedT> ();
		public GameObject gob;
		public GameObject des;
		protected LinkedT parentLT;
		protected int totalNumChoices;
		private int currC;

		LinkedT() {
		}

		public LinkedT(GameObject gob) {
			this.gob = gob;
			createList();
		}

		LinkedT(GameObject gob, LinkedT lt) {
			this.gob = gob;
			this.parentLT = lt;
			createList();
		}

		private void createList() {
			totalNumChoices = 0;
			currC = 0;

			bool isDes = false;
			foreach (Transform t in gob.transform) {
				if(isDes == false) {
					//found deselect sprite
					des = t.gameObject;
					isDes = true;
				}
				else {
					buttonList.Add(t.gameObject);
					++ totalNumChoices;
					if(t.childCount > 1) {
						print (t.gameObject);
						LinkedT linT = new LinkedT(t.gameObject, this);
						linT.hideLevel();
						subMenuList.Add(linT);
					}
					else subMenuList.Add(null);
				}
			}
		}

		public GameObject getCurrObject() {
			return buttonList [currC];
		}

		public void prevObject() {
			if (buttonList [currC - 1] != null) {
				buttonList[currC].GetComponentInChildren<SpriteRenderer>().enabled = false;
				buttonList[--currC].GetComponentInChildren<SpriteRenderer>().enabled = true;
			}
		}

		public void nextObject() {
			if (buttonList [currC + 1] != null) {
				buttonList[currC].GetComponentInChildren<SpriteRenderer>().enabled = false;
				buttonList[++currC].GetComponentInChildren<SpriteRenderer>().enabled = true;
			}
		}

		public LinkedT selectChoice() {
			if (subMenuList [currC] != null) {
				nextLevel ();
				return subMenuList [currC];
			} else if (currC == 0) {
				return prevLevel ();
			} else
				return this;
		}

		private void nextLevel() {
			this.hideLevel ();
			subMenuList [currC].showLevel ();
			print ("nextlevel");
		}

		public LinkedT prevLevel() {
			this.hideLevel ();
			parentLT.showLevel ();
			return parentLT;
		}

		private void showLevel() {
			foreach (GameObject gob in buttonList) {
				gob.GetComponent<SpriteRenderer>().enabled = false;
				getDesObject(gob).enabled = true;
			}
			buttonList [currC].GetComponent<SpriteRenderer> ().enabled = true;
		}

		private void hideLevel() {
			foreach (GameObject gob in buttonList) {
				gob.GetComponent<SpriteRenderer>().enabled = false;
				getDesObject(gob).enabled = false;
			}
			print ("hides");
		}

		private SpriteRenderer getDesObject(GameObject gob) {
			return gob.transform.GetChild(0).GetComponent<SpriteRenderer>();
		}

		public void printList() {
			foreach (GameObject gob in buttonList) {
				print (gob);
			}
		}

		public void init() {
			this.showLevel ();
			parentLT = this;
		}

	}
	
	LinkedT curr;

	void Start() {
		curr = new LinkedT (gameObject);
		curr.init ();
	}

	void Update() {
		checkInput();
	}

	private void checkInput () {
		if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
			curr.nextObject();
		} else if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
			curr.prevObject();
		} else if (Input.GetKeyDown (KeyCode.Return)) {
			curr = curr.selectChoice();
		} else if (Input.GetKeyDown (KeyCode.Backspace)) {
			curr = curr.prevLevel();
		}
	}

}


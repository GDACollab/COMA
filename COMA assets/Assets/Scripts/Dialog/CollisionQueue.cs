using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionQueue : MonoBehaviour {

	private List<GameObject> dialogQueue = new List<GameObject> ();

	void OnTriggerEnter2D(Collider2D obj){
		if (obj.gameObject.GetComponent<makeText> () != null)
			dialogQueue.Add (obj.gameObject);
	}

	void OnTriggerStay2D(Collider2D obj){
		//if(dialogQueue.Count == 0 && obj.gameObject.GetComponent<makeText> () != null && !dialogQueue.Contains(obj.gameObject) && !GetComponent<PlayerMovement>().inDialog)
		//	dialogQueue.Add (obj.gameObject);

		if (Input.GetKeyUp (KeyCode.Space) && obj.gameObject.GetComponent<makeText> () != null && !GetComponent<PlayerMovement>().inDialog) {
				float minDist = float.MaxValue;
				Vector2 playerLoc = new Vector2 (transform.position.x, transform.position.y);
				int closestObjIndex = 0;

				//determine the closest collider to use that as the one to open up the dialog box
				for (int i = 0; i < dialogQueue.Count; i++) {
					Vector2 gameObjectXY = new Vector2 (dialogQueue [i].transform.position.x, dialogQueue [i].transform.position.y);
					dialogQueue [i].GetComponent<makeText> ().useThisConversation = false;

					float dist = Vector2.Distance (playerLoc, gameObjectXY);
					if (dist < minDist) {
						minDist = dist;
						closestObjIndex = i;
					}
				}

				if (dialogQueue.Count > closestObjIndex) {
					dialogQueue [closestObjIndex].GetComponent<makeText> ().useThisConversation = true;
				}
				
			dialogQueue = new List<GameObject> ();
		}
	}
}

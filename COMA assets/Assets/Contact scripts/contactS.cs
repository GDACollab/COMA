using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class contactS : MonoBehaviour {
	
	public Sprite greyNote;
	int missed = 0;
	spawnS source;
	new GameObject textObject;
	Text words;
	
	// Use this for initialization
	void Start () {
		source = GameObject.Find ("Broodmother S").GetComponent<spawnS> ();
		textObject = GameObject.Find ("TextS");
		words = textObject.GetComponent<Text> ();
		words.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.S)) {
			if ( missed == 0){
				if (Vector3.Distance(this.transform.position, source.Sscore[source.top].transform.position) < .16){
					source.kill ();
					words.enabled = true;
					words.text = "Good";
				}else if (Vector3.Distance(this.transform.position, source.Sscore[source.top].transform.position)<.33){
					source.kill ();
					words.enabled = true;
					words.text = "Bad";
					//lower health
					Health.hp -= 2.5f;
				}else{
					source.Sscore[source.top].GetComponent<SpriteRenderer>().sprite = greyNote;
					missed = 1;
				}
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D other){
		source.kill ();
		missed = 0;
		words.enabled = true;
		words.text = "Miss";
		//lower health a lot
		Health.hp -= 5f;
	}
}

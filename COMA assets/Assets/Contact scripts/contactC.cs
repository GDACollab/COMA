using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class contactC : MonoBehaviour {
	
	public Sprite greyNote;
	int missed = 0;
	spawnC source;
	new GameObject textObject;
	Text words;
	
	// Use this for initialization
	void Start () {
		source = GameObject.Find ("Broodmother C").GetComponent<spawnC> ();
		textObject = GameObject.Find ("TextC");
		words = textObject.GetComponent<Text> ();
		words.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Semicolon)) {
			if ( missed == 0){
				if (Vector3.Distance(this.transform.position, source.Cscore[source.top].transform.position) < 2.01){
					source.kill ();
					words.enabled = true;
					words.text = "Good";
				}else if (Vector3.Distance(this.transform.position, source.Cscore[source.top].transform.position)<2.1){
					source.kill ();
					words.enabled = true;
					words.text = "Bad";
					//lower health
					Health.hp -= 2.5f;
				}else{
					source.Cscore[source.top].GetComponent<SpriteRenderer>().sprite = greyNote;
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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class contactA : MonoBehaviour {

	public Sprite greyNote;
	int missed = 0;
	spawnA source;
	new GameObject textObject;
	Text words;

	// Use this for initialization
	void Start () {
		source = GameObject.Find ("Broodmother A").GetComponent<spawnA> ();
		textObject = GameObject.Find ("TextA");
		words = textObject.GetComponent<Text> ();
		words.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			if ( missed == 0){
				if (Vector3.Distance(transform.position, source.Ascore[source.top].transform.position)<2.01){
					source.kill ();
					words.enabled = true;
					words.text = "Good";
				}else if (Vector3.Distance(transform.position, source.Ascore[source.top].transform.position)<2.1){
					source.kill ();
					words.enabled = true;
					words.text = "Bad";
					//lower health
					Health.hp -= 2.5f;
				}else{
					source.Ascore[source.top].GetComponent<SpriteRenderer>().sprite = greyNote;
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

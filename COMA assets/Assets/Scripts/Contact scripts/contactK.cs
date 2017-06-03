using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class contactK : MonoBehaviour {
	
	public Sprite greyNote;
    public GameObject glow;
	int missed = 0;
	spawnK source;
	new GameObject textObject;
	Text words;
	
	// Use this for initialization
	void Start () {
		source = GameObject.Find ("Broodmother K").GetComponent<spawnK> ();
		textObject = GameObject.Find ("TextK");
		words = textObject.GetComponent<Text> ();
		words.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.K)) {
            GameObject temp = Instantiate(glow);
            temp.transform.position = new Vector3(3, -1, 2);
            if ( missed == 0){
				if (Vector3.Distance(this.transform.position, source.Kscore[source.top].transform.position) < 2.01){
					source.kill ();
					//words.enabled = true;
					words.text = "Good";
				}else if (Vector3.Distance(this.transform.position, source.Kscore[source.top].transform.position)<2.1){
					source.kill ();
					//words.enabled = true;
					words.text = "Bad";
					//lower health
					//Health.hp -= 2.5f;
				}else{
					source.Kscore[source.top].GetComponent<SpriteRenderer>().sprite = greyNote;
					missed = 1;
				}
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D other){
		source.kill ();
		missed = 0;
		//words.enabled = true;
		words.text = "Miss";
		//lower health a lot
		Health.hp -= 2.5f;
	}
}

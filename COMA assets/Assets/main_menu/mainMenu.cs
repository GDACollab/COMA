using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour {
    
    int buttonNumber = 0;
    public GameObject startButton;
    public GameObject exitButton;
    public Sprite starterOn;
    public Sprite starterOff;
    public Sprite exiterOn;
    public Sprite exiterOff;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.DownArrow)){
            buttonNumber = 1;
        }
        if (Input.GetKeyDown (KeyCode.UpArrow)){
            buttonNumber = 0;
        }
        
        if (buttonNumber == 0){
            startButton.GetComponent<SpriteRenderer>().sprite = starterOn;
            exitButton.GetComponent<SpriteRenderer>().sprite = exiterOff;
        }
        else {
            startButton.GetComponent<SpriteRenderer>().sprite = starterOff;
            exitButton.GetComponent<SpriteRenderer>().sprite = exiterOn;
        }
        
        if (Input.GetKeyDown (KeyCode.Space)){
            if (buttonNumber == 0){
                SceneManager.LoadScene("Disco");
            }
            else {
                Application.Quit();
            }
        }
	}
}

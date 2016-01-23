using UnityEngine;
using System.Collections;

public class FadingScenes : MonoBehaviour {

	public Texture2D fadeOutTexture; //the texture that will overlay the screen
	public float fadeSpeed = 0.8f;

	private int drawDepth = -1000; //texture's order in the draw heirarchy
	private float alpha = 1.0f;
	private int fadeDir = -1; //direction to fade: in = -1, out = 1

	void OnGUI(){
        //fade out/in the alpha value using direction, speed, and time to convert the opertaion to seconds
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
        //force(clamp) the number bewteen 0 and 1
		alpha = Mathf.Clamp01 (alpha);
        
        //set alpha of GUI texture
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

    //set fadeDir to the direction parameter making the scene fade in and out
	public float BeginFade(int direction){
		fadeDir = direction;
		return(fadeSpeed); //return the fadeSpeed variable to easily time the level load
	}

	void OnLevelWasLoaded(){
		BeginFade (-1);
	}
}

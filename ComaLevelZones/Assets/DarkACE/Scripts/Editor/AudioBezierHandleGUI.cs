using UnityEngine;
using System.Collections;

public class AudioBezierHandleGUI : AudioPointGUI
	// This class just has the capability of being dragged in GUI - it could be any type of generic data class
{	
	public AudioBezierHandleGUI (AudioBezierHandle point) : base (point){
	}
	
	override protected void LoadTexture(){
		tex = Resources.Load("Textures/handle") as Texture2D;
	}
}
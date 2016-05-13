using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class life : MonoBehaviour {

	public Rect lifebarRect;
	public Texture2D lifebar;
	private float width = 700f;
	public static life instance;

	private RectTransform lifeBar;

	// Use this for initialization
	void Start ()
	{
		instance = this;
		lifeBar = this.GetComponent <RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/*void OnGUI(){
		instance.lifebarRect.width = width * (Health.hp / 100);
		instance.lifebarRect.height = 20;
		//instance.lifebarBGRect.width = width;
		//instance.lifebarBGRect.height = 10;
		//GUI.DrawTexture (lifebarBGRect, lifebarBG);
		GUI.DrawTexture (lifebarRect, lifebar);
	}*/
}

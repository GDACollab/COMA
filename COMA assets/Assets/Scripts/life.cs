using UnityEngine;
using System.Collections;

public class life : MonoBehaviour {

	public Rect lifebarRect;
	//public Rect lifebarBGRect;
	public Texture2D lifebar;
	//public Texture2D lifebarBG;
	//private Health heart;
	//public GameObject PlayerData;
	private float width = 700f;
	public static life instance;

	// Use this for initialization
	void Start () {
		instance = this;
		//heart = PlayerData.GetComponent ("Health") as Health;
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnGUI(){
		instance.lifebarRect.width = width * (Health.hp / 100);
		instance.lifebarRect.height = 20;
		//instance.lifebarBGRect.width = width;
		//instance.lifebarBGRect.height = 10;
		//GUI.DrawTexture (lifebarBGRect, lifebarBG);
		GUI.DrawTexture (lifebarRect, lifebar);
	}
}

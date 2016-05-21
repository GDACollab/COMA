using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class life : MonoBehaviour
{
	private float pixelScale = 4.16f;

	public static life instance;

	private RectTransform lifeBar;

	// Use this for initialization
	void Start ()
	{
		instance = this;
		lifeBar = this.GetComponent <RectTransform>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		lifeBar.sizeDelta = new Vector2 ((-100+Health.hp)*pixelScale,0);
        print(Health.hp);
        if (Health.hp <= 0) Application.LoadLevel("Death");
	}

}

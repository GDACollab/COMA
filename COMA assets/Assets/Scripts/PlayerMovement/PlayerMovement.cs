using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float speed;
	public float rightX = 0.2f;
	public float leftX = -0.1f;
	public GameObject background;

	private Animator anim;

	//consider making a rectangle to reduce how many individual variables there are here
	private float backgroundSides = 0;

	public bool hitLeftWall = false;
	public bool hitRightWall = false;

	// Use this for initialization
	void Start ()
    {
		if (background == null)
			background = GameObject.FindGameObjectWithTag ("Background");
		
        anim = GetComponent<Animator>();

		//the set of variables assigned to know where the end of each walkable area is
		backgroundSides = background.GetComponent<SpriteRenderer>().bounds.extents.x - transform.localScale.x;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float input_x = 0;
		float input_y = Input.GetAxisRaw ("Vertical");

		//to make sure the player does not go beyond the walking space
		if (transform.position.x > leftX - backgroundSides && Input.GetAxisRaw ("Horizontal") < 0) 
			input_x = Input.GetAxisRaw ("Horizontal");
		else if (transform.position.x < backgroundSides + rightX && Input.GetAxisRaw ("Horizontal") > 0) 
			input_x = Input.GetAxisRaw ("Horizontal");

		CheckIfHitWall ();

		bool isWalking = (Mathf.Abs(input_x) + Mathf.Abs(input_y)) > 0;


		anim.SetBool("isWalking", isWalking);
		anim.SetFloat("x", input_x);
		anim.SetFloat("y", input_y);

		if (isWalking)
        {
			transform.position += new Vector3(input_x, input_y, 0).normalized * Time.deltaTime * speed;
        }
    }

	void CheckIfHitWall()
	{
		if (transform.position.x <= leftX - backgroundSides)
			hitLeftWall = true;
		else if (transform.position.x >= backgroundSides + rightX)
			hitRightWall = true;
		else {
			hitLeftWall = false;
			hitRightWall = false;
		}
	}

	void OnTriggerStay2D(Collider2D obj){
		if (obj.tag != "Background" && obj.transform.position.y <= transform.position.y)
			transform.GetComponent<SpriteRenderer> ().sortingOrder = 9;
	}
}

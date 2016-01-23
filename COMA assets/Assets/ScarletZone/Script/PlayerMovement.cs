using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    Animator anim;
    public float speed;
	public float bottomBound = 0.51f;
	public GameObject background;

	//consider making a rectangle to reduce how many individual variables there are here
	private float backgroundRightSide = 0;
	private float backgroundLeftSide = 0;
	private float backgroundBottom = 0;

	public bool hitLeftWall = false;
	public bool hitRightWall = false;

	//private float prevPosition = new Vector3(0,0,0);

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();

		//the set of variables assigned to know where the end of each walkable area is
		backgroundRightSide = background.GetComponent<SpriteRenderer>().bounds.extents.x - transform.localScale.x - 0.05f;
		backgroundLeftSide = background.GetComponent<SpriteRenderer>().bounds.extents.x - transform.localScale.x - 0.2f;
		backgroundBottom = background.GetComponent<SpriteRenderer>().bounds.extents.y - transform.localScale.y - bottomBound;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float input_x = 0;
		float input_y = Input.GetAxisRaw ("Vertical");

		//to make sure the player does not go beyond the walking space
		if (transform.position.x > -backgroundLeftSide && Input.GetAxisRaw ("Horizontal") < 0) 
			input_x = Input.GetAxisRaw ("Horizontal");
		else if (transform.position.x < backgroundRightSide && Input.GetAxisRaw ("Horizontal") > 0) 
			input_x = Input.GetAxisRaw ("Horizontal");

		if (transform.position.y < -backgroundBottom && Input.GetAxisRaw ("Vertical") < 0)
			input_y = 0;

		CheckIfHitWall ();

		bool isWalking = (Mathf.Abs(input_x) + Mathf.Abs(input_y)) > 0;


		anim.SetBool("isWalking", isWalking);
		anim.SetFloat("x", input_x);
		anim.SetFloat("y", input_y);

		if (isWalking)
        {
			//prevPosition = transform.position;
			transform.position += new Vector3(input_x, input_y, 0).normalized * Time.deltaTime * speed;
        }
    }

	void CheckIfHitWall()
	{
		if (transform.position.x <= -backgroundLeftSide)
			hitLeftWall = true;
		else if (transform.position.x >= backgroundRightSide)
			hitRightWall = true;
		else {
			hitLeftWall = false;
			hitRightWall = false;
		}
	}

	void OnTriggerStay2D(Collider2D obj){
		if (obj.tag == "Boundry")
			transform.GetComponent<SpriteRenderer> ().sortingOrder = 9;
	}

	void OnTriggerExit2D(Collider2D obj){
		if (obj.tag == "Boundry")
			transform.GetComponent<SpriteRenderer> ().sortingOrder = 100;
	}
}
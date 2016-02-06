using UnityEngine;
using System.Collections;

public class PlayerControllerV3 : MonoBehaviour {

    // Use this for initialization
    public Rigidbody2D rb2D;
	void Start () {
        rb2D = GetComponent<Rigidbody2D>();
	}

    public float defaultSpeed, moveSpeedU, moveSpeedD, moveSpeedL, moveSpeedR;
    public float collisionDistance, width, height;

    //Goals
    //Change origins of raycasts to center of box collider rather than transform.position
	
    // Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            RaycastHit2D hitU = Physics2D.Raycast(transform.position, Vector3.up, collisionDistance);
            if (hitU.collider != null)
            {
                moveSpeedU = 0f;
            }
            else
            {
                moveSpeedU = defaultSpeed;
            }
            transform.Translate(Vector3.up * moveSpeedU * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            RaycastHit2D hitD = Physics2D.Raycast(transform.position, Vector3.down, collisionDistance); 
            if (hitD.collider != null)
            {
                moveSpeedD = 0f;
            }
            else
            {
                moveSpeedD = defaultSpeed;
            }
            transform.Translate(Vector3.down * moveSpeedD * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            RaycastHit2D hitL = Physics2D.Raycast(transform.position, Vector3.left, collisionDistance); 
            if (hitL.collider != null)
            {
                moveSpeedL = 0f;
            }
            else
            {
                moveSpeedL = defaultSpeed;
            }
            transform.Translate(Vector3.left * moveSpeedL * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            RaycastHit2D hitR = Physics2D.Raycast(transform.position, Vector3.right, collisionDistance);
            if (hitR.collider != null)
            {
                moveSpeedR = 0f;
            }
            else
            {
                moveSpeedR = defaultSpeed;
            }
            transform.Translate(Vector3.right * moveSpeedR * Time.deltaTime);
        }
	}

    
}

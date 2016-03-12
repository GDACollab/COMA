using UnityEngine;
using System.Collections;

public class vanishG : MonoBehaviour {

    float next = 0;
    float wait = .2f;

	// Use this for initialization
	void Start () {
        next = Time.time + wait;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > next)
            Destroy(gameObject);
	}
}

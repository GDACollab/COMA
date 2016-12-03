using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour {

    public GameObject sign;
    float next = 0;
    float wait = .2f;
    bool on = false;
    Object holder;

    // Use this for initialization
    void Start () {
        float next = 0;
        float wait = Random.value * (float).5;
        next = Time.time + wait;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > next && on == false)
        {
            GameObject temp = Instantiate(sign);
            temp.transform.position = new Vector3((float)-0.0207462, (float)0.0222096, 0);
            holder = temp;
            wait = Random.value * (float).5;
            next = Time.time + wait;
            on = true;
        }
        else if(Time.time > next && on == true)
        {
            Destroy(holder);
            wait = Random.value * (float).5;
            next = Time.time + wait;
            on = false;
        }
    }
}

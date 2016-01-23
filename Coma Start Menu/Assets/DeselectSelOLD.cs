using UnityEngine;
using System.Collections;

public class DeselectSel : MonoBehaviour {

	private Vector3 tempV;
	// Use this for initialization
	void Start () {
		tempV = new Vector3 (0, 0, 10);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void hit() {
		this.gameObject.transform.localPosition += tempV;
	}

	public void unhit() {
		this.gameObject.transform.localPosition -= tempV;
	}
}

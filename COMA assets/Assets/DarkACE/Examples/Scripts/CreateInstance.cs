using UnityEngine;
using System.Collections;

public class CreateInstance : MonoBehaviour {

	public GameObject original;

	void DoInstance(){
		GameObject.Instantiate(original, new Vector3(Random.value, Random.value, Random.value), Quaternion.identity);
	}
}

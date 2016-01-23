using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AudioPosTrigger : AudioPoint {
	public string methodName;

	public AudioPosTrigger(float audioPos, string methodName) : base(audioPos){
		this.methodName = methodName;
	}
}

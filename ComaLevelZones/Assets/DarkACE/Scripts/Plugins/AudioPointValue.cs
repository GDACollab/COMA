using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AudioPointValue : AudioPoint {
	public float speed;

	public AudioPointValue(float audioPos, float speed) : base(audioPos){
		this.speed = speed;
	}
	
	public AudioPointValue(AudioPointValue original) : base(original.audioPos){
		this.speed = original.speed;
	}
}

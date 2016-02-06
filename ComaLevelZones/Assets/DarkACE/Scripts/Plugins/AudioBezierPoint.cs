using UnityEngine;
using System.Collections;
using System;



[Serializable]
public class AudioBezierPoint : AudioPointValue {
	public AudioBezierHandle[] handles;

	public AudioBezierPoint(float audioPos, float speed, AudioBezierHandle lh, AudioBezierHandle rh): base(audioPos, speed){
		handles = new AudioBezierHandle[2];
		handles[0] = lh;
		handles[1] = rh;
	}
	public AudioBezierPoint(AudioBezierPoint original):base(original.audioPos, original.speed){
		handles = new AudioBezierHandle[2];
		handles[0] = new AudioBezierHandle(original.handles[0]);
		handles[1] = new AudioBezierHandle(original.handles[1]);
	}
}

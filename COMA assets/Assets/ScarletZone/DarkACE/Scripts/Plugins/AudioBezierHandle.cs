using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AudioBezierHandle : AudioPointValue {
	public AudioBezierHandle(float audioPos, float speed): base(audioPos, speed){}
	public AudioBezierHandle(AudioBezierHandle original): base(original.audioPos, original.speed){}
}

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AudioBezierPointList {
	public List<AudioBezierPoint> list;
	public AudioBezierPointList(){
		list = new List<AudioBezierPoint>();
	}
}

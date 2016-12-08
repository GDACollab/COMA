using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioEvents : MonoBehaviour {
	
	[SerializeField]
	public List<AudioBezierPointList> curves = new List<AudioBezierPointList>();
	
	[SerializeField]
	public List<string> names = new List<string>();
	
	[SerializeField]
	public List<bool> soloCurves = new List<bool>();
	
	[SerializeField]
	public List<bool> mutedCurves = new List<bool>();
	
	public bool dirty;
	
	[SerializeField]
	public List<Color> colors = new List<Color>();
	
	[SerializeField]
	public List<AudioPosTrigger> triggers = new List<AudioPosTrigger>();
	
	Hashtable nameMapping = new Hashtable();
	
	int currentEvent = 0;
	
	
	public float GetCurrentValue(int curve) {
		return GetValueAtTime(curve, GetComponent<AudioSource>().time);
	}
	public float GetCurrentValue(string curve) {
		int curveIdx = (int)nameMapping[curve];
		return GetValueAtTime(curveIdx, GetComponent<AudioSource>().time);
	}
	
	public float GetValueAtTime(int curve, float time) {
		int startIndex = BinarySearchFirstPoint(curve, time);
		return BinarySearchCurveValue(curve, startIndex, time);
	}
	public float GetValueAtTime(string curve, float time) {
		int curveIdx = (int)nameMapping[curve];
		int startIndex = BinarySearchFirstPoint(curveIdx, time);
		return BinarySearchCurveValue(curveIdx, startIndex, time);
	}
	
	public float BinarySearchCurveValue(int curve, int index, float time) {
		return BinarySearchCurveValue(curve, index, time, 0f, 1f);
	}
	
	public float BinarySearchCurveValue(int curve, int index, float time, float lPos, float rPos) {
		float mPos = (lPos + rPos)/2;
		Vector2 bezierRes = CalculateBezier(
			new Vector2(curves[curve].list[index].audioPos, curves[curve].list[index].speed),
			new Vector2(curves[curve].list[index].handles[1].audioPos, curves[curve].list[index].handles[1].speed),
			new Vector2(curves[curve].list[index+1].handles[0].audioPos, curves[curve].list[index+1].handles[0].speed),
			new Vector2(curves[curve].list[index+1].audioPos, curves[curve].list[index+1].speed),
			mPos);
		
		if(Mathf.Abs(bezierRes.x - time) < .01f){
			return bezierRes.y;
		}
		
		//Go right
		if(bezierRes.x < time){
			return BinarySearchCurveValue(curve, index, time, mPos, rPos);
		}
		//Go left
		else{
			return BinarySearchCurveValue(curve, index, time, lPos, mPos);
		}
	}
	
	public static Vector2 CalculateBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t){
		float ti = 1-t;
		return Mathf.Pow(ti, 3)*p0 + 3*Mathf.Pow(ti, 2)*t*p1 + 3*ti*Mathf.Pow(t, 2)*p2 + Mathf.Pow(t, 3)*p3;
	}
	
	
	
	public int BinarySearchFirstTrigger(float time){
		return BinarySearchFirstTrigger(time, 0, triggers.Count-1);
	}
	
	public int BinarySearchFirstTrigger(float time, int lTriggerIndex, int rTriggerIndex) {
		if(triggers.Count <= lTriggerIndex || triggers[lTriggerIndex].audioPos > time){
			return -1;
		}
		if(triggers[rTriggerIndex].audioPos < time){
			return rTriggerIndex;
		}
		if(lTriggerIndex+1 >= rTriggerIndex){
			return lTriggerIndex;
		}
		int mPos = (rTriggerIndex + lTriggerIndex)/2;
		//Go right
		if(triggers[mPos].audioPos <= time){
			return BinarySearchFirstTrigger(time, mPos, rTriggerIndex);
		}
		//Go left
		else{
			return BinarySearchFirstTrigger(time, lTriggerIndex, mPos);
		}
	}
	
	public int BinarySearchFirstPoint(int curve, float time){
		return BinarySearchFirstPoint(curve, time, 0, curves[curve].list.Count-1);
	}
	
	public int BinarySearchFirstPoint(int curve, float time, int lPointIndex, int rPointIndex) {
		if(lPointIndex+1 >= rPointIndex){
			return lPointIndex;
		}
		int mPos = (rPointIndex + lPointIndex)/2;
		//Go right
		if(curves[curve].list[mPos].audioPos <= time){
			return BinarySearchFirstPoint(curve, time, mPos, rPointIndex);
		}
		//Go left
		else{
			return BinarySearchFirstPoint(curve, time, lPointIndex, mPos);
		}
	}
	
	public override int GetHashCode ()
	{
		int hashCode=0;
		for(int i=0; i<curves.Count; ++i){
			if(curves[i].list != null){
				for(int j=0; j<curves[i].list.Count; ++j){
					hashCode += j*(curves[i].list[j].audioPos.GetHashCode() + curves[i].list[j].speed.GetHashCode());
				}
			}
		}
		
		for(int i=0; i<triggers.Count; ++i){
			hashCode += triggers[i].audioPos.GetHashCode();
			hashCode += triggers[i].methodName.GetHashCode();
		}
		return hashCode;
	}
	
	public void TimeChangedManually () {
		currentEvent = BinarySearchFirstTrigger(GetComponent<AudioSource>().time);
	}
	
	void Start () {
		//Pfft :D
		for(int i=0; i<names.Count; ++i){
			nameMapping.Add(names[i], i);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		/* Old code
		int prevEvent = currentEvent;
		currentEvent = BinarySearchFirstTrigger(GetComponent<AudioSource>().time);
		
		//Passed an event
		if(prevEvent != currentEvent && currentEvent>=0){
			SendMessage(triggers[currentEvent].methodName, SendMessageOptions.DontRequireReceiver);
		}
		*/

		// triggers is a List of the events
		// currentEvent is just an index into that List

		float currentTime = GetComponent<AudioSource> ().time; // Current time
		// If we still have triggers to run, and we've just passed one
		if (currentEvent < triggers.Count &&
			currentTime > triggers [currentEvent].audioPos) {
			// Send a message to Maestro to spawn a note
			SendMessage(triggers[currentEvent].methodName, 
				        SendMessageOptions.DontRequireReceiver);
			currentEvent++; // Go to next event
		}
	}
}

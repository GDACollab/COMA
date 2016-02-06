using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

[CustomEditor(typeof(AudioEvents))]
public class AudioEventsGUI : Editor {

	bool curvesFoldout = false;
	bool eventsFoldout = false;

	public override void OnInspectorGUI () {
		bool dirty = false;
		AudioEvents target = this.target as AudioEvents;
		curvesFoldout = EditorGUILayout.Foldout(curvesFoldout, "Curves");
		if(curvesFoldout){
			for(int i=0; i<target.curves.Count; ++i){
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(target.names[i]);
				target.colors[i] = EditorGUILayout.ColorField(target.colors[i]);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("+")){
				int newPos = target.curves.Count+1;
				target.colors.Add(new Color(newPos%2, (newPos>>1)%2, (newPos>>2)%2));

				AudioBezierPointList list = new AudioBezierPointList();
				//Special values for non-existing handles
				list.list.Add(new AudioBezierPoint(0, 0, new AudioBezierHandle(-10f, -10f), new AudioBezierHandle(target.GetComponent<AudioSource>().clip.length*.05f, 0)));
				list.list.Add(new AudioBezierPoint(target.GetComponent<AudioSource>().clip.length, 0, new AudioBezierHandle(target.GetComponent<AudioSource>().clip.length*.95f, 0), new AudioBezierHandle(-10f, -10f)));
				target.curves.Add( list );

				dirty = true;
			}
			if(GUILayout.Button("-")){
				target.colors.RemoveAt(target.curves.Count-1);
				target.curves.RemoveAt(target.curves.Count-1);
				target.names.RemoveAt(target.curves.Count-1);
				target.soloCurves.RemoveAt(target.curves.Count-1);
				target.mutedCurves.RemoveAt(target.curves.Count-1);
				dirty = true;
			}
			EditorGUILayout.EndHorizontal();
		}
		eventsFoldout = EditorGUILayout.Foldout(eventsFoldout, "Events");
		if(eventsFoldout){
			for(int i=0; i<target.triggers.Count; ++i){
				EditorGUILayout.BeginHorizontal();
				float prevPos = target.triggers[i].audioPos;
				target.triggers[i].audioPos = EditorGUILayout.FloatField(target.triggers[i].audioPos);
				if(prevPos != target.triggers[i].audioPos){
					dirty = true;
				}
				target.triggers[i].methodName = EditorGUILayout.TextField(target.triggers[i].methodName);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("+")){
				target.triggers.Add(new AudioPosTrigger(0, ""));
				//Sort triggers
				target.triggers = target.triggers.OrderBy(x => x.audioPos).ToList();
				dirty = true;
			}
			if(GUILayout.Button("-")){
				//if too many triggers, remove last
				target.triggers.RemoveAt(target.triggers.Count-1);
				dirty = true;
			}
			EditorGUILayout.EndHorizontal();
		}
		if(dirty){
			target.dirty = true;
		}
		if (GUI.changed){
			EditorUtility.SetDirty (target);
		}
	}
}

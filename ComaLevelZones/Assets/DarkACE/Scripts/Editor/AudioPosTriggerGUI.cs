using UnityEngine;
using System.Collections;
using System;

public class AudioPosTriggerGUI : GUIDraggableObject {
	public GUIStyle style;

	public AudioPosTrigger trigger;
	public Texture2D eventTexture;

	public Rect drawRect;

	public AudioPosTriggerGUI(AudioPosTrigger trigger){
		this.trigger = trigger;
		eventTexture = Resources.Load("Textures/trigger") as Texture2D;
		vLocked = true;
	}

	public void CalculateGUIPosition(float zoomFactor, float hScrollPosition, Rect guiRect, AudioClip clip){
		float pointx = ACEEditor.CalculateGUIAudioPosition(zoomFactor, hScrollPosition, guiRect, clip, trigger.audioPos);
		float pointy = guiRect.height+15f;
		m_Position = new Vector2(pointx, pointy);
	}
	
	public float CalculateAudioPosition(float zoomFactor, float hScrollPosition, Rect guiRect, AudioClip clip){
		float pos = ACEEditor.CalculateAudioPosition(zoomFactor, hScrollPosition, guiRect, clip, m_Position.x);
		trigger.audioPos = pos;
		return pos;
	}

	public bool OnGUI(float zoomFactor, float hScrollPosition, Rect guiRect, AudioClip clip){
		bool prevDragging = Dragging;
		bool shouldRepaint = false;

		if(style == null){
			style = new GUIStyle( GUI.skin.GetStyle ("Box"));
			style.normal.background = eventTexture;
		}

		drawRect = new Rect (m_Position.x-eventTexture.width/2, m_Position.y, eventTexture.width, eventTexture.height);
		
		GUILayout.BeginArea (drawRect, style);
		GUILayout.EndArea ();
		Drag (drawRect);

		//Dragging. should repaint
		if(Dragging){
			shouldRepaint = true;
		}
		//Stopped dragging. Calculate audio position
		if(prevDragging && !Dragging){
			CalculateAudioPosition(zoomFactor, hScrollPosition, guiRect, clip);
			if(trigger.audioPos < 0){
				trigger.audioPos = 0f;
			}
			shouldRepaint = true;
		}
//		
		//Not dragging. Recalculate position to account for zoom and scroll changes
		else if(!Dragging){
			CalculateGUIPosition(zoomFactor, hScrollPosition, guiRect, clip);
		}

		return shouldRepaint;
	}
}

using UnityEngine;
using System.Collections;
using System;

public class AudioPosLine : GUIDraggableObject {
	public GUIStyle style;

	public AudioPosLine(){
	}

	public bool OnGUI(Rect guiPos){
		bool prevDragging = Dragging;

		if(!Dragging){
			m_Position.x = guiPos.x;
		}
		m_Position.y = guiPos.y;

		if(style == null){
			Texture2D tex = new Texture2D(1, 1);
			tex.SetPixel(0, 0, new Color(.8f, .8f, .8f, .9f));
			tex.Apply();
			style = new GUIStyle( GUI.skin.GetStyle ("Box"));
			style.normal.background = tex;
			style.border = new RectOffset(0, 0, 0, 0);
		}
		Rect dragRect = new Rect (m_Position.x-4f, m_Position.y, guiPos.width+8f, guiPos.height);
		Rect drawRect = new Rect (m_Position.x, m_Position.y, guiPos.width, guiPos.height);
		
		GUILayout.BeginArea (drawRect, style);
		GUILayout.EndArea ();
		Drag (dragRect);
		return Dragging || prevDragging;
	}
}

using UnityEngine;
using UnityEditor;
using System.Collections;

public class AudioBezierPointGUI : AudioPointGUI
	// This class just has the capability of being dragged in GUI - it could be any type of generic data class
{
	public bool selected = false;
	public bool h_Locked = false;

	public bool DraggedChildren = false;

	public AudioBezierHandleGUI[] handlesGUI;
	
	public AudioBezierPointGUI (AudioBezierPoint point) : base (point){
		handlesGUI = new AudioBezierHandleGUI[2];

		for(int i=0; i<point.handles.Length; ++i){
			if(point.handles[i].audioPos != -10f){
				handlesGUI[i] = new AudioBezierHandleGUI(point.handles[i]);
			}
		}

		LoadTexture();
	}


	protected override void LoadTexture(){
		tex = Resources.Load("Textures/point") as Texture2D;
	}
	
	public bool OnGUI (float zoomFactor, float hScrollPosition, Rect guiRect, AudioClip clip, bool disabled)
	{
		this.disabled = disabled;
		bool shouldRepaint = false;
		bool previousState = Dragging;
		Vector2 previousPos = Position;

		base.OnGUI();

		//Currently dragging. Make sure to repaint next frame and update children using movement deltas
		if(Dragging){
			shouldRepaint = true;
			for(int i=0; i<handlesGUI.Length; ++i){
				if(handlesGUI[i] != null){
					handlesGUI[i].Position += Position - previousPos;
				}
			}
		}

		//Stopped dragging. Make sure to repaint and update audio positions for this and children.
		else if(previousState && !Dragging){
			shouldRepaint = true;
			CalculateAudioPosition(zoomFactor, hScrollPosition, guiRect, clip);
			CalculateSpeed(guiRect, clip);
			for(int i=0; i<handlesGUI.Length; ++i){
				if(handlesGUI[i] != null){
					handlesGUI[i].CalculateAudioPosition(zoomFactor, hScrollPosition, guiRect, clip);
					handlesGUI[i].CalculateSpeed(guiRect, clip);
				}
			}
		}

		//Not dragging. Recalculate position to account for zoom and scroll changes
		else if(!Dragging){
			CalculateGUIPosition(zoomFactor, hScrollPosition, guiRect, clip);
		}

		DraggedChildren = false;
		//Handlers operations
		for(int i=0; i<handlesGUI.Length; ++i){
			if(handlesGUI[i] != null){
				bool handlePrevState = handlesGUI[i].Dragging;

				//if(handlesGUI[i].Position.x > ACEEditor.leftSidebarWidth && handlesGUI[i].Position.x < ACEEditor.ScreenSize.x){
				handlesGUI[i].disabled = disabled;
					handlesGUI[i].OnGUI();
				//}
				if(handlesGUI[i].Dragging){
					DraggedChildren = true;
					shouldRepaint = true;
				}

				//Stopped dragging handle. Recalculate audio pos
				if(!handlesGUI[i].Dragging && handlePrevState){
					handlesGUI[i].CalculateAudioPosition(zoomFactor, hScrollPosition, guiRect, clip);
					handlesGUI[i].CalculateSpeed(guiRect, clip);
					shouldRepaint = true;
				}
				else if (!handlesGUI[i].Dragging && !Dragging){
					handlesGUI[i].CalculateGUIPosition(zoomFactor, hScrollPosition, guiRect, clip);
				}

				//Make sure left handle stays left, and right handle stays right!
				if(i == 0) {
					handlesGUI[i].Position = new Vector2( Mathf.Min (handlesGUI[i].Position.x, Position.x), handlesGUI[i].Position.y);
				}

				if(i == 1) {
					handlesGUI[i].Position = new Vector2( Mathf.Max (handlesGUI[i].Position.x, Position.x), handlesGUI[i].Position.y);
				}

				//Draw line from point to handle
				if(!disabled){
					Handles.BeginGUI();
					Color handlesColor = Handles.color;
					Handles.color = new Color(1, 1, 1, .8f);
					Handles.DrawLine(Position, handlesGUI[i].Position);
					Handles.color = handlesColor;
					Handles.EndGUI();
				}
			}
		}

		return shouldRepaint;


//		if(style == null){
//			style = new GUIStyle( GUI.skin.GetStyle ("Box"));
//			style.normal.background = tex;
//		}
//		Rect drawRect = new Rect (m_Position.x-4f, m_Position.y-4f, 8.0f, 8.0f);
//
//		GUILayout.BeginArea (drawRect, style);
//		GUILayout.EndArea ();
//
//		Vector2 prevPos = m_Position;
//
//		Drag (drawRect);
//
//		if(prevState && !Dragging){
//
//		}
//
//		DraggedChildren = Dragging;
//
//		if(Dragging){
//			for(int i=0; i<handlesGUI.Length; ++i){
//				if(handlesGUI[i] != null){
//					handlesGUI[i].Position += m_Position - prevPos;
//				}
//			}
//		}
//
//		if(selected){
//			for(int i=0; i<handlesGUI.Length; ++i){
//				if(handlesGUI[i] != null){
//					AudioBezierHandleGUI data = handlesGUI[i];
//					
//					bool prevState = data.Dragging;
//					data.OnGUI ();
//					
//					if (prevState && !data.Dragging)
//					{
//						data.CalculateSpeed(guiRect, clip);
//						data.CalculateSpeed(guiRect, clip);
//					}
//				}
//			}
//		}
	}
}
using UnityEngine;
using System.Collections;

public class AudioPointGUI : GUIDraggableObject
	// This class just has the capability of being dragged in GUI - it could be any type of generic data class
{
	protected Texture2D tex;
	protected GUIStyle style;

	public bool disabled = false;
	
	public AudioPointValue point;
	
	public AudioPointGUI (AudioPointValue point) : base (){
		this.point = point;
		LoadTexture();
	}
	
	public void CalculateGUIPosition(float zoomFactor, float hScrollPosition, Rect guiRect, AudioClip clip){
		float pointx = ACEEditor.CalculateGUIAudioPosition(zoomFactor, hScrollPosition, guiRect, clip, point.audioPos);
		float pointy = (guiRect.height - guiRect.height * point.speed) * .5f;
		m_Position = new Vector2(pointx, pointy);
	}
	
	public float CalculateAudioPosition(float zoomFactor, float hScrollPosition, Rect guiRect, AudioClip clip){
		float pos = ACEEditor.CalculateAudioPosition(zoomFactor, hScrollPosition, guiRect, clip, m_Position.x);
		point.audioPos = pos;
		return pos;
	}
	
	public float CalculateSpeed(Rect guiRect, AudioClip clip){
		float speed = ACEEditor.CalculateSpeed(guiRect, clip, Position.y);
		point.speed = speed;
		return speed;
	}
	
	protected virtual void LoadTexture(){
		tex = Resources.Load("Textures/point") as Texture2D;
	}
	
	public void OnGUI ()
	{
		if(!disabled){
			if(style == null){
				style = new GUIStyle( GUI.skin.GetStyle ("Box"));
				style.normal.background = tex;
			}
			Rect drawRect = new Rect (m_Position.x-4f, m_Position.y-4f, 8.0f, 8.0f);
			
			GUILayout.BeginArea (drawRect, style);
			GUILayout.EndArea ();
			
			Drag (drawRect);
		}
//		if(Dragging){
//			return true;
//		}
//		return false;
	}
}
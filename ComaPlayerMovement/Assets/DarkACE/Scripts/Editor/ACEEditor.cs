using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ACEEditor  : EditorWindow
{
	Texture2D musicTex;
	AudioClip clip;

	float zoomFactor = 1f;
	float pitch = 1f;
	float hScrollPosition;
	Vector2 sidebarScrollPos;
	AudioEvents audioEvents;
	AudioPosLine posLine;

	
	public Rect triggerWindowRect;
//	int selectedHandle = 0;

	Texture2D bgTex;
	Texture2D sidebarBgTex;
	Texture2D trigBgTex;

	public static int leftSidebarWidth = 190;
	public static int leftScaleIndicators = 25;

	private List<List< AudioBezierPointGUI >> points;
	private List<AudioPosTriggerGUI> triggerPoints;

	int selectedCurve = 0;
	private bool doRepaint = false;
	private bool triggerWasSelected = false;
	private bool wasPlaying = false;


	private int selectedTrigger = -1;
	private string sendMessageContent;

	private int hash;

	int soloCurves = 0;
	
	private float oldSpeed;
	private float oldAudioPos;

	public AudioPointValue oldBezierData;
	public AudioPointValue oldBezierLHandle;
	public AudioPointValue oldBezierRHandle;

	public static Vector2 ScreenSize{
		get {
			return new Vector2(Screen.width, Screen.height);
		}
	}

	public ACEEditor(){
		posLine = new AudioPosLine();
		GenerateTextures();
	}

	[MenuItem("Window/ACE Editor")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		ACEEditor speedEditor = EditorWindow.GetWindow(typeof(ACEEditor )) as ACEEditor;
		speedEditor.OnSelectionChange();
	}

	void GenerateTextures()
	{
		bgTex = new Texture2D(1, 1);
		bgTex.SetPixel(0, 0, new Color(0.22f, 0.22f, 0.22f));
		bgTex.Apply();

		sidebarBgTex = new Texture2D(1, 1);
		sidebarBgTex.SetPixel(0, 0, new Color(0.22f, 0.22f, 0.44f));
		sidebarBgTex.Apply();
	}

	void GenerateAudioTexture()
	{
		float[] data = new float[clip.samples * clip.channels];
		clip.GetData(data, 0);
		musicTex = new Texture2D(2048, 512, TextureFormat.RGBA32, false);
		musicTex.filterMode = FilterMode.Point;
		
		Color[] cols = new Color[musicTex.width * musicTex.height];
		for( int i = 0; i < musicTex.width * musicTex.height; ++i ) {
			cols[i] = new Color(0, 0, 0, 0);
		}
		musicTex.SetPixels(cols);
		
		int parts = data.Length / musicTex.width;
		
		for(int i=0; i<musicTex.width; ++i){
			float min = 0;
			float max = 0;
			int j;
			
			for(j=i*parts; j<(i+1)*parts; ++j){
				if(data[j] < min){
					min = data[j];
				}
				if(data[j] > max){
					max = data[j];
				}
			}
			
			int pixels = (int)(max*(musicTex.height/2));
			for(j=0; j<pixels; ++j){
				musicTex.SetPixel(i, (musicTex.height/2)+j, Color32.Lerp(new Color32(255, 253, 189, 255), new Color32(252, 174, 5, 255), (float)j/(float)pixels));
			}
			pixels = (int)(min*(-(musicTex.height/2)));
			for(j=0; j<pixels; ++j){
				musicTex.SetPixel(i, (musicTex.height/2)-j, Color32.Lerp(new Color32(255, 253, 189, 255), new Color32(252, 174, 5, 255), (float)j/(float)pixels));
			}
		}
		musicTex.Apply();
	}

	public static float CalculateAudioPosition(float zoomFactor, float hScrollPosition, Rect guiRect, AudioClip clip, float guiPos){
		return clip.length*(hScrollPosition*(guiRect.width-leftSidebarWidth)*(zoomFactor-1)+guiPos - leftSidebarWidth)/((guiRect.width-leftSidebarWidth)*zoomFactor);
	}
	
	public static float CalculateGUIAudioPosition(float zoomFactor, float hScrollPosition, Rect guiRect, AudioClip clip, float audioPos){
		return leftSidebarWidth + zoomFactor * (guiRect.width-leftSidebarWidth) * audioPos / clip.length - hScrollPosition*(zoomFactor-1)*(guiRect.width-leftSidebarWidth);
	}

	public static float CalculateSpeed(Rect guiRect, AudioClip clip, float guiPos){
		return (guiRect.height/2 - guiPos) / (guiRect.height/2);
	}

	void ClearCurvePoints()
	{
		points = new List<List<AudioBezierPointGUI>>();
		for(int i=0; i<audioEvents.curves.Count; ++i){
			points.Add ( new List<AudioBezierPointGUI>());
		}
	}

	void ClearTriggers()
	{
		triggerPoints = new List<AudioPosTriggerGUI>();
	}

	void AddTriggerPoint(AudioPosTrigger point){
		triggerPoints.Add(new AudioPosTriggerGUI(point));
	}
	
	void AddCurvePoint(AudioBezierPoint p, int curve)
	{
		AddCurvePoint(p, curve, false);
	}

	void AddCurvePoint(AudioBezierPoint p, int curve, bool hLocked)
	{
		points[curve].Add (new AudioBezierPointGUI (p));
		points[curve][points[curve].Count-1].hLocked = hLocked;
	}

	void OnSelectionChange()
	{
		if(Selection.activeGameObject != null){
			AudioEvents tmpAudioEvents = Selection.activeGameObject.GetComponent<AudioEvents>();
			if(tmpAudioEvents != null && tmpAudioEvents.GetComponent<AudioSource>().clip != null){
				audioEvents = tmpAudioEvents;
				clip = audioEvents.GetComponent<AudioSource>().clip;
				RecreateCurvePoints();
				CreateTriggers();
				GenerateAudioTexture();
			}
			else{
				audioEvents = null;
			}
		}
		doRepaint = true;
	}

	void CreateTriggers(){
		ClearTriggers();

		audioEvents.triggers = audioEvents.triggers.OrderBy(x => x.audioPos).ToList();
		
		//For each audio trigger
		for(int i=0; i<audioEvents.triggers.Count; ++i){
			AddTriggerPoint(audioEvents.triggers[i]);
		}
		doRepaint = true;
	}

	void RecreateCurvePoints(){
//
//		//If curves container null, generate it.
//		if(audioCurves.curves == null){
//			audioCurves.curves = new List<AudioBezierPointList>();
//		}
//
//		//if not enough curves, generate more!
//		if(audioCurves.curves.Count < audioCurves.curvesCount){
//			audioCurves.colors = new List<Color>();
//			for(int i=0; i< audioCurves.curvesCount; ++i){
//				int icpy = i+1;
//				if(audioCurves.colors.Count <= i){
//					audioCurves.colors.Add(new Color(icpy&4, icpy&2, icpy&1));
//				}
//				if(audioCurves.curves.Count <= i){
//					audioCurves.curves.Add( new AudioBezierPointList() );
//				}
//			}
//		}
//		//if too many curves, remove last
//		while(audioCurves.curves.Count > audioCurves.curvesCount){
//			audioCurves.curves.RemoveAt(audioCurves.curves.Count-1);
//		}
//		
//		//also, colors
//		while(audioCurves.colors.Count > audioCurves.curvesCount){
//			audioCurves.colors.RemoveAt(audioCurves.colors.Count-1);
//		}
//

		ClearCurvePoints();

		//For each curve contained
		for(int i=0; i<audioEvents.curves.Count; ++i){
			
			//Add booleans if not existing
			if(audioEvents.soloCurves.Count < audioEvents.curves.Count){
				audioEvents.soloCurves.Add(false);
			}
			
			if(audioEvents.mutedCurves.Count < audioEvents.curves.Count){
				audioEvents.mutedCurves.Add(false);
			}

			//Add name if not exists
			if(audioEvents.names.Count < audioEvents.curves.Count){
				audioEvents.names.Add((audioEvents.names.Count+1)+"");
			}

			//Generate points if not existing
//			if(audioEvents.curves[i].list == null || audioEvents.curves[i].list.Count < 2){
//				audioEvents.curves[i].list = new List<AudioBezierPoint>();
//				audioEvents.curves[i].list.Add(new AudioBezierPoint(0, 0, null, new AudioBezierHandle(5, 0)));
//				audioEvents.curves[i].list.Add(new AudioBezierPoint(clip.length, 0, new AudioBezierHandle(clip.length - 5, 0), null));
//			}
//			else{
				audioEvents.curves[i].list = audioEvents.curves[i].list.OrderBy(x => x.audioPos).ToList();
//			}
			
			AddCurvePoint(audioEvents.curves[i].list[0], i, true);
			for(int j=1; j<audioEvents.curves[i].list.Count-1; ++j){
				AddCurvePoint(audioEvents.curves[i].list[j], i);
			}
			AddCurvePoint(audioEvents.curves[i].list[audioEvents.curves[i].list.Count-1], i, true);
		}
		selectedCurve = Mathf.Clamp(selectedCurve, 0, audioEvents.curves.Count-1);
		doRepaint = true;
	}
	
	void OnDisable(){
//		if(tmpObject != null){
//			DestroyImmediate(tmpObject);
//		}
	}

	void OnDestroy(){
//		if(tmpObject != null){
//			DestroyImmediate(tmpObject);
//		}
	}

	public void Update ()
	{
		if(sidebarBgTex == null){
			GenerateTextures();
		}
		if(audioEvents != null){
			if(audioEvents.dirty){
				OnSelectionChange();
				if(audioEvents != null){
					audioEvents.dirty = false;
				}
			}
		}
		if(points == null){
			OnSelectionChange();
		}
		if(!Application.isPlaying && wasPlaying){
			posLine = new AudioPosLine();
		}
		if (doRepaint)
		{
			Repaint ();
		}
		wasPlaying = Application.isPlaying;
	}

	public void OnInspectorUpdate(){
		Repaint ();
	}
	
	void OnGUI()
	{

		doRepaint = false;
		if(audioEvents != null){
			soloCurves = 0;
			for(int i=0; i<audioEvents.curves.Count; ++i){
				for(int j=0; j<audioEvents.curves[i].list.Count; ++j){
					for(int k=0; k<2; ++k){
						if(audioEvents.curves[i].list[j].handles[k] == null){
							Debug.Log("Curve "+i+" has a point with missing handle "+k);
						}
					}
				}
				if(audioEvents.soloCurves[i]){
					++soloCurves;
				}
			}
			GUILayout.BeginVertical();

			Rect guiRect = new Rect(0, 0, Screen.width-3, Screen.height - 85f);
			GUILayoutUtility.GetRect(guiRect.width, guiRect.height);
			
			Rect labelsRect = GUILayoutUtility.GetRect(Screen.width, 15f);
			Rect triggersRect = GUILayoutUtility.GetRect(Screen.width, 25f);

			float texWidth = (guiRect.width - leftSidebarWidth) * zoomFactor;
			GUI.DrawTexture(new Rect(leftSidebarWidth - (hScrollPosition * (texWidth - guiRect.width + leftSidebarWidth)), 0, texWidth, guiRect.height), musicTex);

			//Main Audio GUI
			Handles.BeginGUI();

			Handles.color = new Color(.8f, .8f, .8f, .8f);

			//Vertical line
			float pointx = CalculateGUIAudioPosition(zoomFactor, hScrollPosition, guiRect, clip, audioEvents.GetComponent<AudioSource>().time);

			bool lineUpdating = posLine.OnGUI(new Rect(pointx, 0, 1, guiRect.height));
			if(lineUpdating){
				audioEvents.GetComponent<AudioSource>().time = CalculateAudioPosition(zoomFactor, hScrollPosition, guiRect, clip, posLine.Position.x);
			}
			doRepaint = lineUpdating || doRepaint;

			Handles.color = new Color(.5f, .5f, .5f, .5f);

			//Vertical lines

			float startx = ACEEditor.CalculateGUIAudioPosition(zoomFactor, hScrollPosition, guiRect, clip, 0);
			float dist = ACEEditor.CalculateGUIAudioPosition(zoomFactor, hScrollPosition, guiRect, clip, 1) - startx;
			for(int i=0; i<clip.length; i+=5){
				float xpoint = startx + dist * i;
				if(xpoint > leftSidebarWidth && xpoint < Screen.width){
					Handles.DrawLine(new Vector2(xpoint, 0), new Vector2(xpoint, guiRect.height + 15f));
					GUI.Label(new Rect(xpoint, labelsRect.y, 100, 15f), i+"");
				}
			}
			if(dist > 30){
				for(int i=0; i<clip.length; ++i){
					float xpoint = startx + dist * i;
					if(xpoint > leftSidebarWidth && xpoint < Screen.width){
						Handles.DrawLine(new Vector2(xpoint, 0), new Vector2(xpoint, guiRect.height + 15f));
						GUI.Label(new Rect(xpoint, labelsRect.y, 100, 15f), i+"");
					}
				}
			}

			//We must go deeper
			if(dist > 100){
				for(float i=0; i<clip.length * 10; i+=.1f){
					float xpoint = startx + dist * i;
					if(xpoint > leftSidebarWidth && xpoint < Screen.width){
						Handles.DrawLine(new Vector2(xpoint, 0), new Vector2(xpoint, guiRect.height + 15f));
					}
				}
			}

			
			//Bezier points

			for(int i=0; i<points.Count; ++i){
				for (int j=0; j<points[i].Count; ++j)
				{
					AudioBezierPointGUI data = points[i][j];

					bool prevDragging = data.Dragging;
					bool prevDraggingChildren = data.DraggedChildren;

					doRepaint = data.OnGUI(zoomFactor, hScrollPosition, guiRect, clip, audioEvents.mutedCurves[i] || (soloCurves>0 && !audioEvents.soloCurves[i])) || doRepaint;
					
					// data.selected = false;

					//Started dragging, check if alt was held down, and remove if necessary
					if(!prevDragging && data.Dragging){

						//Also set curve as current
						selectedCurve = i;

						if(Event.current.control){
							//Undo.RecordObject(audioEvents, "Remove Curve Point");
							audioEvents.curves[i].list.RemoveAt(j);
							RecreateCurvePoints();
							return;
						}
					}
					//Dropped curve point
					else if(prevDragging && !data.Dragging){
//						AudioPointValue tmpData = new AudioPointValue(data.point);
//						AudioPointValue tmpLHandle = new AudioPointValue(data.handlesGUI[0].point);
//						AudioPointValue tmpRHandle = new AudioPointValue(data.handlesGUI[1].point);
//						data.point = oldBezierData;
//						data.handlesGUI[0].point = oldBezierLHandle;
//						data.handlesGUI[1].point = oldBezierRHandle;
//						Debug.Log("Change curve pos");
//						Undo.RecordObject(audioCurves, "Changed Curve Point Position");
//						data.point = tmpData;
//						data.handlesGUI[0].point = tmpLHandle;
//						data.handlesGUI[1].point = tmpRHandle;
//						oldBezierData = oldBezierLHandle = oldBezierRHandle = null;
					}

					//Started dragging handles.
					if(!prevDraggingChildren && data.DraggedChildren){
//						oldBezierLHandle = new AudioPointValue(data.handlesGUI[0].point);
//						oldBezierRHandle = new AudioPointValue(data.handlesGUI[1].point);
					}
					//Stopped dragging handles
					else if(prevDraggingChildren && !data.DraggedChildren){
//						AudioPointValue tmpLHandle = new AudioPointValue(data.handlesGUI[0].point);
//						AudioPointValue tmpRHandle = new AudioPointValue(data.handlesGUI[1].point);
//						data.handlesGUI[0].point = oldBezierLHandle;
//						data.handlesGUI[1].point = oldBezierRHandle;
//						Debug.Log("Change handle pos");
//						Undo.RecordObject(audioCurves, "Changed Curve Point Handle Position");
//						data.handlesGUI[0].point = tmpLHandle;
//						data.handlesGUI[1].point = tmpRHandle;
//
//						oldBezierLHandle = oldBezierRHandle = null;
					}
				}
			}

			//Check if new points should be added
			if(Event.current.type == EventType.MouseDown && guiRect.Contains(Event.current.mousePosition) && Event.current.shift && audioEvents.curves.Count>0 && selectedCurve < audioEvents.curves.Count){

				float audioPos = ACEEditor.CalculateAudioPosition(zoomFactor, hScrollPosition, guiRect, clip, Event.current.mousePosition.x);
				float speed = ACEEditor.CalculateSpeed(guiRect, clip, Event.current.mousePosition.y);

				//We should add the new stuff
				audioEvents.curves[selectedCurve].list.Add(new AudioBezierPoint(
					audioPos,
					speed,
					new AudioBezierHandle(Mathf.Max (audioPos- (0.03f * clip.length / zoomFactor ), 0), speed),
					new AudioBezierHandle(Mathf.Min (audioPos+ (0.03f * clip.length / zoomFactor ), clip.length), speed))
				);
//				Undo.RecordObject(audioCurves, "New curve point");
				
				RecreateCurvePoints();
				return;
			}


			//Bezier curves

			for(int i=0; i<points.Count; ++i){
				if(audioEvents.mutedCurves[i]){
					continue;
				}
				Color color = audioEvents.colors[i];
				if(soloCurves>0 && !audioEvents.soloCurves[i]){
					color.a = 0.4f;
				}
				for(int j=1; j<points[i].Count; ++j){
					Handles.DrawBezier(points[i][j-1].Position, points[i][j].Position, points[i][j-1].handlesGUI[1].Position, points[i][j].handlesGUI[0].Position, color, null, 2f);
				}
			}
			
			//Horizontal lines
//			GUI.DrawTexture(new Rect(0, 0, 24f, Screen.height), bgTex);
			float offset = guiRect.height/20f;
			GUI.DrawTexture(new Rect(leftSidebarWidth-leftScaleIndicators, 0, leftScaleIndicators, Screen.height-70f), bgTex);
			for(int i=0; i<=10; ++i){
				float currOffset = offset * i;
				GUI.Label(new Rect(leftSidebarWidth-leftScaleIndicators-2, guiRect.height/2 + currOffset - 8, 40f, 20), -i*.1f+"");
				Handles.DrawLine(new Vector2(leftSidebarWidth, guiRect.height/2 + currOffset), new Vector2(guiRect.width, guiRect.height/2 + currOffset));
				if(i == 10){
					GUI.Label(new Rect(leftSidebarWidth-leftScaleIndicators-2, guiRect.height/2 - currOffset, 40f, 20), i*.1f+"");
				}
				else{
					GUI.Label(new Rect(leftSidebarWidth-leftScaleIndicators-2, guiRect.height/2 - currOffset - 8, 40f, 20), i*.1f+"");
				}
				Handles.DrawLine(new Vector2(leftSidebarWidth, guiRect.height/2 - currOffset), new Vector2(guiRect.width, guiRect.height/2 - currOffset));
			}
			Handles.DrawLine(new Vector2(leftSidebarWidth, 0), new Vector2(leftSidebarWidth, Screen.height-70f));
			Handles.DrawLine(new Vector2(leftSidebarWidth-leftScaleIndicators, 0), new Vector2(leftSidebarWidth-leftScaleIndicators, Screen.height-70f));

			Handles.EndGUI();


			//Triggers GUI

			//background
			Rect triggersRectMod = triggersRect;
			triggersRectMod.x = leftSidebarWidth;
			triggersRectMod.width = Screen.width;
			if(trigBgTex == null){
				trigBgTex = new Texture2D(1, 1);
				trigBgTex.SetPixel(0, 0, new Color(0.32f, 0.72f, 1f, .2f));
				trigBgTex.Apply();
			}
			GUI.DrawTexture(triggersRectMod, trigBgTex);


			for(int i=0; i<triggerPoints.Count; ++i){
				bool prevDragging = triggerPoints[i].Dragging;
				doRepaint = triggerPoints[i].OnGUI(zoomFactor, hScrollPosition, guiRect, clip) || doRepaint;

				//Handle removal of trigger
				if(!prevDragging && triggerPoints[i].Dragging){
					if(Event.current.alt){
//						Undo.RecordObject(audioCurves, "Remove Trigger");
						audioEvents.triggers.RemoveAt(i);
						CreateTriggers();
						audioEvents.TimeChangedManually();
						doRepaint = true;
						return;
					}
				}

				//Handle position update
				if(prevDragging && !triggerPoints[i].Dragging){
					audioEvents.TimeChangedManually();
				}

				//Handle trigger right click
				if(Event.current.type == EventType.ContextClick && triggerPoints[i].drawRect.Contains(Event.current.mousePosition)){
					selectedTrigger = i;
					sendMessageContent = triggerPoints[i].trigger.methodName;
					doRepaint = true;
				}
			}

			//Handle adding of triggers
			if(Event.current.type == EventType.MouseDown && triggersRect.Contains(Event.current.mousePosition) && Event.current.shift){
				AudioPosTrigger newTrigger = new AudioPosTrigger(ACEEditor.CalculateAudioPosition(zoomFactor, hScrollPosition, guiRect, clip, Event.current.mousePosition.x), "");
//				Undo.RecordObject(audioCurves, "Add Trigger");
				audioEvents.triggers.Add(newTrigger);
				AddTriggerPoint(newTrigger);
				audioEvents.TimeChangedManually();
				doRepaint = true;
			}
			
			GUI.DrawTexture(new Rect(leftSidebarWidth-leftScaleIndicators, Screen.height-71f, leftScaleIndicators, 25f), bgTex);
			
			
			//Sidebar
			Rect sidebarRect = new Rect(0, 0, leftSidebarWidth-leftScaleIndicators, Screen.height-65f);
			Rect sidebarContentRect = new Rect(0, 0, leftSidebarWidth-leftScaleIndicators-15, audioEvents.curves.Count * 17f);
			sidebarScrollPos = GUI.BeginScrollView(sidebarRect, sidebarScrollPos, sidebarContentRect);
			GUI.DrawTexture(sidebarRect, bgTex);
			
			for(int i=0; i<audioEvents.curves.Count; ++i){
				Rect curveButtonRect = new Rect(0, i*17f, leftSidebarWidth-leftScaleIndicators, 17f);
				if(Event.current != null && Event.current.isMouse && Event.current.type == EventType.mouseDown && curveButtonRect.Contains(Event.current.mousePosition)){
					selectedCurve = i;
				}
				if(i == selectedCurve){
					Color bgColor = GUI.backgroundColor;
					GUI.backgroundColor = new Color(1f, 1f, 1f, 1f);
					GUI.DrawTexture(curveButtonRect, sidebarBgTex);
					GUI.backgroundColor = bgColor;
				}
				Rect labelRect = new Rect(curveButtonRect);
				labelRect.width = 2*curveButtonRect.width/3;
				string newName = GUI.TextField(labelRect, audioEvents.names[i]);
				audioEvents.names[i] = newName;
				
				Rect soloRect = new Rect(curveButtonRect);
				soloRect.width = 30;
				soloRect.x = 2*(curveButtonRect.width/3);
				
				Rect mutedRect = new Rect(soloRect);
				mutedRect.x += 25;
				mutedRect.width += 5;
				
				audioEvents.soloCurves[i] = GUI.Toggle(soloRect, audioEvents.soloCurves[i], "S");
				audioEvents.mutedCurves[i] = GUI.Toggle(mutedRect, audioEvents.mutedCurves[i], "M");
			}
			GUI.EndScrollView();
			GUI.DrawTexture(new Rect(0, Screen.height-65f, leftSidebarWidth-leftScaleIndicators, 20f), bgTex);
			
			//Add and remove buttons
			if(GUI.Button(new Rect(0, Screen.height-65f, (leftSidebarWidth-leftScaleIndicators)/2, 15f), "+")){
				int newPos = audioEvents.curves.Count+1;
				audioEvents.colors.Add(new Color(newPos%2, (newPos>>1)%2, (newPos>>2)%2));
				AudioBezierPointList list = new AudioBezierPointList();
				//Special values for non-existing handles
				list.list.Add(new AudioBezierPoint(0, 0, new AudioBezierHandle(-10f, -10f), new AudioBezierHandle(audioEvents.GetComponent<AudioSource>().clip.length*.05f, 0)));
				list.list.Add(new AudioBezierPoint(audioEvents.GetComponent<AudioSource>().clip.length, 0, new AudioBezierHandle(audioEvents.GetComponent<AudioSource>().clip.length*.95f, 0), new AudioBezierHandle(-10f, -10f)));
				audioEvents.curves.Add( list );
				
				audioEvents.dirty = true;
			}
			if(GUI.Button(new Rect((leftSidebarWidth-leftScaleIndicators)/2, Screen.height-65f, (leftSidebarWidth-leftScaleIndicators)/2, 15f), "-")){
				bool result = EditorUtility.DisplayDialog("Removing curve", "Are you sure you want to remove the selected curve?", "Yes", "No");
				if(result){
					audioEvents.colors.RemoveAt(selectedCurve);
					audioEvents.curves.RemoveAt(selectedCurve);
					audioEvents.names.RemoveAt(selectedCurve);
					audioEvents.soloCurves.RemoveAt(selectedCurve);
					audioEvents.mutedCurves.RemoveAt(selectedCurve);
					this.RecreateCurvePoints();
				}
			}

			
			//Bottom GUI
			GUILayout.BeginHorizontal();
			GUILayout.Label("Spd:", GUILayout.Width(30));
			pitch = GUILayout.HorizontalSlider(pitch, 0.05f, 1f, GUILayout.Width(100));
			audioEvents.GetComponent<AudioSource>().pitch = pitch;
			GUILayout.Label(audioEvents.GetComponent<AudioSource>().time+"", GUILayout.Width(50));
			if(GUILayout.Button("Play", GUILayout.Width(80))){
				audioEvents.GetComponent<AudioSource>().Play();
			}
			if(GUILayout.Button(audioEvents.GetComponent<AudioSource>().isPlaying?"Pause":"Stop", GUILayout.Width(80))){
				if(audioEvents.GetComponent<AudioSource>().isPlaying){
					audioEvents.GetComponent<AudioSource>().Pause();
				}
				else{
					audioEvents.GetComponent<AudioSource>().Stop();
					audioEvents.GetComponent<AudioSource>().time = 0f;
				}
			}
			
			if(audioEvents.colors.Count > selectedCurve && selectedCurve >= 0){
				audioEvents.colors[selectedCurve] = EditorGUILayout.ColorField(audioEvents.colors[selectedCurve], GUILayout.Width(60));
			}

			hScrollPosition = GUILayout.HorizontalScrollbar(hScrollPosition, .1f, 0f, 1.1f);
			zoomFactor = GUILayout.HorizontalSlider(zoomFactor, 1, 10, GUILayout.MaxWidth(100f));

			GUILayout.EndHorizontal();

			GUILayout.EndVertical();

			if(audioEvents.GetComponent<AudioSource>().isPlaying){
				doRepaint = true;
			}
			//Modal window for triggers
			
			if(selectedTrigger != -1){
				if(!triggerWasSelected){
					triggerWindowRect = new Rect(triggerPoints[selectedTrigger].drawRect.x - 80, guiRect.height-100, 170, 100);
					triggerWasSelected = true;
				}
				BeginWindows();
				triggerWindowRect = GUI.Window(0, triggerWindowRect, TriggerEditWindow, "Trigger "+selectedTrigger+" Edit");
				EndWindows();
			}
		}
		else{
			GUILayout.Label("Please select an object that has an AudioEvents script attached.");
		}
	}

	
	void TriggerEditWindow(int windowID) {
		GUI.Label(new Rect(10, 20, 150, 20), "Send Message:");
		sendMessageContent = GUI.TextField(new Rect(10, 40, 150, 20), sendMessageContent);
		if(GUI.Button (new Rect(5, 65, 75, 20), "Save" )){
			triggerPoints[selectedTrigger].trigger.methodName = sendMessageContent;
			selectedTrigger = -1;
			triggerWasSelected = false;
			
			Undo.RecordObject(audioEvents, "Changed method name");
		}
		if(GUI.Button (new Rect(85, 65, 75, 20), "Close" )){
			selectedTrigger = -1;
			triggerWasSelected = false;
		}
		GUI.DragWindow(new Rect(0, 0, 10000, 10000));
	}
}
using UnityEngine;
using System.Collections;

public class GUIDraggableObject
{
	protected Vector2 m_Position;
	private Vector2 m_DragStart;
	private bool m_Dragging;

	private bool m_hLocked;
	private bool m_vLocked;
	
	
	public GUIDraggableObject ()
	{
	}

	public GUIDraggableObject (Vector2 position)
	{
		m_Position = position;
	}
	
	public bool Dragging
	{
		get
		{
			return m_Dragging;
		}
	}
	
	public Vector2 Position
	{
		get
		{
			return m_Position;
		}
		
		set
		{
			m_Position = value;
		}
	}
	
	public bool hLocked
	{
		get
		{
			return m_hLocked;
		}
		
		set
		{
			m_hLocked = value;
		}
	}

	public bool vLocked
	{
		get
		{
			return m_vLocked;
		}
		
		set
		{
			m_vLocked = value;
		}
	}
	
	public void Drag (Rect draggingRect)
	{
		if (Event.current.type == EventType.MouseUp)
		{
			m_Dragging = false;
		}
		else if (Event.current.type == EventType.MouseDown && draggingRect.Contains (Event.current.mousePosition))
		{
			m_Dragging = true;
			m_DragStart = Event.current.mousePosition - m_Position;
			Event.current.Use();
		}
		
		if (m_Dragging)
		{
			Vector2 prevPosition = m_Position;
			m_Position = Event.current.mousePosition - m_DragStart;
			if(m_hLocked){
				m_Position.x = prevPosition.x;
			}
			if(m_vLocked){
				m_Position.y = prevPosition.y;
			}
		}
	}
}
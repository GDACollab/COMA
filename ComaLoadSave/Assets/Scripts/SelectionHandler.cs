using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionHandler
{
	private int selectedIndex;
	private string optionText;

	private List<string> optionLabels = new List<string>();

	public SelectionHandler(List<string> labels)
	{
		selectedIndex = 0;
		optionLabels.Clear ();
		optionLabels = labels;
		optionText = optionLabels [0];
	}

	public void Reset()
	{
		selectedIndex = 0;
	}

	public void Next()
	{
		selectedIndex = selectedIndex > 0 ? selectedIndex-1 : optionLabels.Count - 1;
		optionText = optionLabels [selectedIndex];
	}

	public void Previous(){
		selectedIndex = (selectedIndex + 1) % optionLabels.Count;
		optionText = optionLabels [selectedIndex];
	}

	public void ChangeListItem(int index, string value){
		optionLabels[index] = value;
	}

	public int GetListSize(){
		return optionLabels.Count;
	}

	public string GetOptionListString()
	{
		return optionText;
	}

	public int GetSelectedIndex()
	{
		return selectedIndex;
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 

[SerializeField]
public class ReadSpreadSheets 
{
	private List<Row> parsedRows = new List<Row>();

	public void ParseCSV(TextAsset csvFile)
	{
		int numRows = -1;
		/*for (int i = 0; i < csvFile.text.Length; i++) {
			if (csvFile.text [i].Equals ('\n'))
				numRows++;
		}*/

		//Identifier,ACTOR,CUE,CONTEXT,Conversation Path Chain,Choice Type,GameObject Interacted With,INFLECTION,LOCATION,AREA,EFFECT
		int commaPair = 0;
		Row row = new Row ();
		for (int i = 0, j = 0; i < csvFile.text.Length ; i++) {
			switch(j){
				case 0:
					if (csvFile.text [i].Equals (',')) {
						row.Identifier = row.Identifier.Trim ();
						j++;
					}
					else row.Identifier += csvFile.text [i];
					break;
				case 1:
					if (csvFile.text [i].Equals (',')) {
						row.ACTOR = row.ACTOR.Trim ();
						row.ACTOR = row.ACTOR;
						j++;
					}
					else row.ACTOR += csvFile.text [i];
					break;
			case 2:
				string temp = row.CUE + "";
				if ((temp.EndsWith("\",") && temp[0] == '"') || (temp.EndsWith(",") && temp[0] != '"') || temp.Contains("CUE,")) {
						row.CUE = row.CUE.Remove (row.CUE.Length - 1);
						row.CUE = row.CUE.Trim ();
						j++;
						i--;
					} else {
						row.CUE += csvFile.text [i];
					}
					break;
				case 3:
					if (csvFile.text [i].Equals (',')) {
						row.CONTEXT = row.CONTEXT.Trim ();
						j++;
					}
					else row.CONTEXT += csvFile.text [i];
					break;
				case 4:
					if (csvFile.text [i].Equals (',')) {
						row.Conversation_Path_Chain = row.Conversation_Path_Chain.Trim ();
						j++;
					}
					else row.Conversation_Path_Chain += csvFile.text [i];
					break;
				case 5:
					if (csvFile.text [i].Equals (',')) {
						row.Choice_Type = row.Choice_Type.Trim ();
						j++;
					}
					else row.Choice_Type += csvFile.text [i];
					break;
				case 6:
					if (csvFile.text [i].Equals (',')) {
						row.GameObject_Interacted_With = row.GameObject_Interacted_With.Trim ();
						j++;
					}
					else row.GameObject_Interacted_With += csvFile.text [i];
					break;
				case 7:
					if (csvFile.text [i].Equals (',')) {
						row.INFLECTION = row.INFLECTION.Trim ();
						j++;
					}
					else row.INFLECTION += csvFile.text [i];
					break;
				case 8:
					if (csvFile.text [i].Equals (',')) {
						row.LOCATION = row.LOCATION.Trim ();
						j++;
					}
					else row.LOCATION += csvFile.text [i];
					break;
				case 9:
					if (csvFile.text [i].Equals (',')) {
						row.AREA = row.AREA.Trim ();
						j++;
					}
					else row.AREA += csvFile.text [i];
					break;
				case 10:
					row.EFFECT += csvFile.text [i];
					break;
			}

			if (csvFile.text [i].Equals ('\n')) {
				row.EFFECT = row.EFFECT.Trim ();
				parsedRows.Add(row);
				row = new Row ();
				commaPair = 0;
				j = 0;
			}
		}

		if (parsedRows [0] != null)
			parsedRows.RemoveAt (0);
	}

	public List<Row> getParsedRows(){
		return parsedRows;
	}

	public List<Row> FindAll_ACTOR(string name){
		List<Row> actorsRows = new List<Row> ();
		for(int i = 0; i < parsedRows.Count; i++){
			if (parsedRows [i].ACTOR.Equals (name))
				actorsRows.Add (parsedRows [i]);
		}

		return actorsRows;
	}
}
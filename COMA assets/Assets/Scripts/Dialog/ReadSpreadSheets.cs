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

		//Identifier,ACTOR,CUE,Conversation Path Chain,LOCATION
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
						row.Conversation_Path_Chain = row.Conversation_Path_Chain.Trim ();
						j++;
					}
					else row.Conversation_Path_Chain += csvFile.text [i];
					break;
				case 4:
					if (csvFile.text [i].Equals (',')) {
						row.LOCATION = row.LOCATION.Trim ();
						j++;
					}
					else row.LOCATION += csvFile.text [i];
					break;
			}

			if (csvFile.text [i].Equals ('\n')) {
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

	public int getRowsLength(){
		return parsedRows.Count;
	}

	public List<Row> FindAll_ACTOR(string name){
		List<Row> actorsRows = new List<Row> ();

		for(int i = 0; i < parsedRows.Count; i++){
			if (parsedRows [i].ACTOR.Equals (name)) {
				actorsRows.Add (parsedRows [i]);
			}
		}

		return actorsRows;
	}

	public int[] FindDiologIndexes(){
		List<Row> tempParsedRows = new List<Row>(parsedRows);
		int[] indexRows = new int[tempParsedRows.Count];
		int index = 0;

		while(tempParsedRows.Count > 0){
			List<Row> actorsDiolog = FindAll_ACTOR (tempParsedRows [index].ACTOR);

			for (int j = 0; j < actorsDiolog.Count; j++) {
				int temp = 0;
				if(int.TryParse(actorsDiolog[j].Identifier, out temp)){
					temp = int.Parse (actorsDiolog [j].Identifier);
					indexRows [temp] = j;
					tempParsedRows.Remove (actorsDiolog [j]);
				}
			}
		}

		return indexRows;
	}

	public string ActorAtIndex(int index){
		return parsedRows [index].ACTOR;
	}

	private List<Row> RemoveAll_ACTOR(string name, List<Row> pR){
		for(int i = 0; i < pR.Count; i++){
			if (pR [i].ACTOR.Equals (name))
				pR.RemoveAt (i);
		}

		return pR;
	}

	private bool actorExists(string name){
		for(int i = 0; i < parsedRows.Count; i++){
			if (parsedRows [i].ACTOR.Equals (name))
				return true;
		}

		return false;
	}
}
// This is the 

using UnityEngine;
using System.Collections;

public class menuStart : MenuCode {
	public override void on_select() {
		Application.LoadLevel("Home");
	}
}

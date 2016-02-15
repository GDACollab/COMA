// This script is attached to buttons that don't do anything.
// If a button has no script attached, then this one is automatically attached
// by default.

using UnityEngine;
using System.Collections;

public class DoNothing : MenuCode {
	public override void on_select() {
	}
}

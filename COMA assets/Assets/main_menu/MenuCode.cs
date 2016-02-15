// Each menu button should have its own script implementing this interface.
// The script should be attached to the appropriate button as a component.

using UnityEngine;
using System.Collections;

public abstract class MenuCode : MonoBehaviour {
	public abstract void on_select();
}

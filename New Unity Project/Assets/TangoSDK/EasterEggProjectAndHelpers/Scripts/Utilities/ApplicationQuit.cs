using UnityEngine;
using System.Collections;

public class ApplicationQuit : MonoBehaviour {
	// temp solution when the application 
	// goes into background
	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus)
			Application.Quit ();
	}
}

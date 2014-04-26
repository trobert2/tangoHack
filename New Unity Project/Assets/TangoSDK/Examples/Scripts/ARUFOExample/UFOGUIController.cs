using UnityEngine;
using System.Collections;

public class UFOGUIController : MonoBehaviour {

	public GameObject arcamera;

	void OnGUI ()
	{
		GUI.Box(new Rect(Screen.width - 300, 5, 280, 90), "");

		GUI.Label (new  Rect (Screen.width - 280, 10, 300, 20), 
		           "Screen size: " + Screen.width + "x" + Screen.height);
		
		GUI.Label (new  Rect (Screen.width - 280, 30, 300, 20), 
		           "Position(m): " + arcamera.transform.position.x.ToString("######0.0000")+" "
		           + arcamera.transform.position.y.ToString("######0.0000")+" "
		           + arcamera.transform.position.z.ToString("######0.0000")+" ");
		
		GUI.Label (new  Rect (Screen.width - 280, 50, 300, 20), 
		           "Rotation(deg): " + arcamera.transform.rotation.eulerAngles.x.ToString("######0.0000")+" "
		           + arcamera.transform.rotation.eulerAngles.y.ToString("######0.0000")+" "
		           + arcamera.transform.rotation.eulerAngles.z.ToString("######0.0000")+" ");
		
		Camera cam = arcamera.GetComponent<Camera>();
		GUI.Label (new  Rect (Screen.width - 280,70, 300, 20), "FOV:" +cam.fieldOfView.ToString("######0.0"));
		GUI.Label (new  Rect (Screen.width - 280, Screen.height - 30, 200, 30), Tango.Version.versionString);

	}
}

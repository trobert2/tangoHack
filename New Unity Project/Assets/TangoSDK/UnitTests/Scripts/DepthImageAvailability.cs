using UnityEngine;
using System.Collections;

public class DepthImageAvailability : MonoBehaviour {

	public float timeout = 1.0f;
	private bool result = false;
	public GameObject depthImageSource;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.captureFramerate < timeout) {
			if(result == false){
				Tango.Synchronizer depthImageAccessor = depthImageSource.GetComponent<Tango.Synchronizer>();
				int [] depthImageBuffer = new int[ depthImageAccessor.getDepthBufferSize()];
				double timestamp = 0;
				result = depthImageAccessor.getDepthBuffer(ref depthImageBuffer, ref timestamp);
				if (result)
					Debug.Log("Depth buffer is available. Reported timestamp:" + timestamp);
			}
		}
	}
}

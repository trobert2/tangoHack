using UnityEngine;
using System.Collections;

public class ARController : MonoBehaviour {

	public Transform arcamera;
	public GameObject tutorialObject;

	public GameObject newObject;
	
	public void AddEgg()
	{
		Vector3 objectPosition = new Vector3 (arcamera.transform.position.x, 
		                                      arcamera.transform.position.y, 
		                                      arcamera.transform.position.z);
		int egg = Random.Range (1, 6);
		//TODO(ADI): Add random values for vector for egg spawn. If position is behind object, oclusion!!! 
		//Get somehow left/right front back walls limits relative to objectPosition.
 
		if (egg == 1) {
			GameObject newObject = (GameObject)Instantiate (Resources.Load ("Prefabs/knob1"), objectPosition, Quaternion.identity);
		} else if (egg == 2) {
			GameObject newObject = (GameObject)Instantiate (Resources.Load ("Prefabs/knob2"), objectPosition, Quaternion.identity);
		} else if (egg == 3) {
			GameObject newObject = (GameObject)Instantiate (Resources.Load ("Prefabs/knob3"), objectPosition, Quaternion.identity);
		} else if (egg == 4) {
			GameObject newObject = (GameObject)Instantiate (Resources.Load ("Prefabs/knob4"), objectPosition, Quaternion.identity);
		}else if (egg == 5) {
			GameObject newObject = (GameObject)Instantiate (Resources.Load ("Prefabs/knob5"), objectPosition, Quaternion.identity);
		}

		newObject.transform.parent = arcamera.gameObject.transform;
		newObject.transform.localPosition = new Vector3 (0.5f, 0f, 0.5f);
		newObject.transform.parent = gameObject.transform;
	}

	// Destory all Eggs
	public void DestroyEggs()
	{
		if (transform.childCount == 0)
			return;
		#if UNITY_ANDROID
		Handheld.Vibrate ();
		#endif
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}

	public int GetScore (int time){
		int score = 50;
		if (time < 25 && time > 20) {
			score = 40;
		} else if (time < 20 && time > 10) {
			score = 30;
		} else if (time < 10){
			score = 20;
		}
		return score;
		}
}

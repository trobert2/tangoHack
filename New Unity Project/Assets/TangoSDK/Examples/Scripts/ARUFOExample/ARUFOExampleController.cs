using UnityEngine;
using System.Collections;

public class ARUFOExampleController : MonoBehaviour {

	public Transform arcamera;
	public GameObject tutorialObject;
	public GameObject explosionFlashObject;

	private GameObject newUFOObject;

	void Start()
	{
		StartCoroutine (StartTutorial ());
		explosionFlashObject.guiTexture.pixelInset = 
			new Rect (-Screen.width / 2, -Screen.height / 2, Screen.width, Screen.height);
	}

	// Blink the tutorial text
	IEnumerator StartTutorial()
	{
		while (!UFOGlobals.isShowedTouchToAddText) 
		{
			tutorialObject.SetActive(!tutorialObject.activeInHierarchy);
			yield return new WaitForSeconds(0.7f);
		}
		tutorialObject.SetActive (false);
		yield return null;
	}

	// Destory all UFOs
	public void DestoryUFOs()
	{
		if (transform.childCount == 0)
			return;
		audio.Play ();
		StartCoroutine (FadeOutGUITexture (explosionFlashObject.guiTexture));
#if UNITY_ANDROID
		Handheld.Vibrate ();
#endif
		foreach (Transform child in transform) 
		{
			Destroy(child.gameObject);
		}
	}

	IEnumerator FadeOutGUITexture(GUITexture tex)
	{
		const float totalTime = 0.7f;
		float counter = 0.0f;
		Color tempColor = Color.white;
		while (counter<=totalTime) 
		{
			tempColor.a = Mathf.Lerp(1.0f, 0.0f, counter/totalTime);
			tex.color = tempColor;
			counter += Time.deltaTime;
			yield return null;
		}
	}

	// 
	public void AddKey()
	{
		Vector3 objectPosition = new Vector3 (arcamera.transform.position.x, 
		                                      arcamera.transform.position.y, 
		                                      arcamera.transform.position.z);
		
		GameObject newUFOObject = (GameObject)Instantiate(Resources.Load("Prefabs/key"), 
		                                                  objectPosition, 
		                                                  Quaternion.identity); 
		newUFOObject.transform.parent = arcamera.gameObject.transform;
		newUFOObject.transform.localPosition = new Vector3 (0f, -0.1f, 0.5f);
		newUFOObject.transform.parent = gameObject.transform;
	}

	public void AddKnob()
	{
		Vector3 objectPosition = new Vector3 (arcamera.transform.position.x, 
		                                      arcamera.transform.position.y, 
		                                      arcamera.transform.position.z);
		
		GameObject newUFOObject = (GameObject)Instantiate(Resources.Load("Prefabs/knob"), 
		                                                  objectPosition, 
		                                                  Quaternion.identity); 
		newUFOObject.transform.parent = arcamera.gameObject.transform;
		newUFOObject.transform.localPosition = new Vector3 (0f, -0.1f, 0.5f);
		newUFOObject.transform.parent = gameObject.transform;
	}
}

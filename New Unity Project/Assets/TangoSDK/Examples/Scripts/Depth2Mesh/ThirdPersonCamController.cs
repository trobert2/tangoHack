//-----------------------------------------------------------------------
// <copyright file="ThirdPersonCamController.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// Controll camera logics.
/// </summary>
public class ThirdPersonCamController : MonoBehaviour 
{
	public GameObject camObj;
	public GameObject ar_CamObj;
	public GameObject thirdPersonCamAnchor;

	public Transform thirdPersonCamPos;

	public GameObject axisObject;
	public MeshGenerate meshGenerate;

	public bool is_ThirdPersonCamOn = false;
	public float zoomFactor;

	private Vector3 startPosition;
	private Vector3 startRotation;
	
	private Vector3 unitVectorY = new Vector3(0, 1, 0);
	private Vector3 unitVectorX = new Vector3(1, 0, 0); 

	private float startFov;
	private float zoomStartDis;

	private bool is_FirstTouched = false;

	/// <summary>
	/// Update is called once per frame.
	/// </summary>
	private void Update() 
	{
		thirdPersonCamAnchor.transform.position = ar_CamObj.transform.position;
		if (Input.GetKeyDown(KeyCode.A)) 
		{
			SwitchCamera();
		}
		if (!is_ThirdPersonCamOn) 
		{
			camObj.transform.LookAt(ar_CamObj.transform.position + ar_CamObj.transform.forward);
		}
		else
		{
			camObj.transform.LookAt(ar_CamObj.transform.position);
			if (Input.touchCount == 1) 
			{
				if (!is_FirstTouched) 
				{
					startPosition = Input.mousePosition;
					startRotation = thirdPersonCamAnchor.transform.rotation.eulerAngles;
					is_FirstTouched = true;
				} 
				if (Input.GetMouseButton(0)) 
				{
					float scaleX = 90.0f / (Screen.width / 2.0f);
					float scaleY = 90.0f / (Screen.height / 2.0f);

					Vector3 rotEuler = startRotation + (unitVectorX * scaleY * (startPosition.y - Input.mousePosition.y))
						+ (unitVectorY * scaleX * (Input.mousePosition.x - startPosition.x));
					thirdPersonCamAnchor.transform.rotation = Quaternion.Euler(rotEuler);
				} 
			}

			if (Input.touchCount == 2) 
			{
				if (Input.GetTouch(0).phase == TouchPhase.Began ||
					Input.GetTouch(1).phase == TouchPhase.Began)
				{
					camObj.camera.enabled = true;
					camObj.transform.rotation = Quaternion.Euler(startRotation);
					zoomStartDis = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
					startFov = camObj.camera.fieldOfView;
				}
				if (Input.GetTouch(0).phase == TouchPhase.Moved ||
					Input.GetTouch(1).phase == TouchPhase.Moved)
				{
					float curDis = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
					camObj.camera.fieldOfView = Mathf.Clamp(startFov - ((curDis - zoomStartDis) * zoomFactor), 10, 100);
				}
				is_FirstTouched = false;
			}
			if (Input.touchCount == 0) 
			{
				is_FirstTouched = false;
			}
		}
	}

	/// <summary>
	/// Switch camera's position.
	/// </summary>
	private void SwitchCamera()
	{
		is_ThirdPersonCamOn = !is_ThirdPersonCamOn;
		if (is_ThirdPersonCamOn) 
		{
			StopAllCoroutines();
			axisObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			camObj.transform.parent = thirdPersonCamAnchor.transform;
			StartCoroutine(CamMoveOut(camObj, camObj.transform.position, 1.0f));
		} 
		else 
		{
			StopAllCoroutines();
			axisObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			camObj.transform.parent = ar_CamObj.transform;
			StartCoroutine(CamMoveIn(camObj, camObj.transform.position, 1.0f));
		}
		thirdPersonCamAnchor.transform.rotation = Quaternion.identity;
	}

	/// <summary>
	/// Camera zoom out movement cooroutine.
	/// </summary>
	/// <returns> Returns when yield return call.</returns>
	/// <param name="obj">Object to move.</param>
	/// <param name="startPos">Start position.</param>
	/// <param name="totalTime">Time to finish the track.</param>
	private IEnumerator CamMoveOut(GameObject obj, Vector3 startPos, float totalTime)
	{
		float timeCounter = 0.0f;
		Vector3 forward = new Vector3(ar_CamObj.transform.forward.x, 0.0f, ar_CamObj.transform.forward.z);
		Vector3 endPosition = ar_CamObj.transform.position - 
				(5 * forward.normalized) + 
				(5 * Vector3.up);
		while (timeCounter <= totalTime) 
		{
			obj.transform.position = Vector3.Lerp(startPos, endPosition, timeCounter / totalTime);
			timeCounter += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}

	/// <summary>
	/// Camera zoom in movement cooroutine.
	/// </summary>
	/// <returns> Returns when yield return call.</returns>
	/// <param name="obj">Object to move.</param>
	/// <param name="startPos">Start position.</param>
	/// <param name="totalTime">Time to finish the track.</param>
	private IEnumerator CamMoveIn(GameObject obj, Vector3 startPos, float totalTime)
	{
		float timeCounter = 0.0f;
		while (timeCounter <= totalTime) 
		{
			obj.transform.position = Vector3.Lerp(startPos, thirdPersonCamPos.position, timeCounter / totalTime);
			timeCounter += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}

	/// <summary>
	/// Unity GUI function.
	/// </summary>
	private void OnGUI()
	{
		GUI.Label(new Rect(10, 30, 500, 20), "device position: " + transform.position.ToString());
		GUI.Label(new Rect(10, 50, 500, 20), "fov: " + camObj.camera.fieldOfView.ToString());
		GUI.Label(new Rect(10, 70, 500, 20), "render mode: " + meshGenerate.renderMode.ToString());
		GUI.Label(new Rect(10, 90, 500, 20), "step size: " + meshGenerate.stepSize.ToString());

		if (GUI.Button(new Rect(1000, 150, 150, 70), "switch render mode"))
		{
			if (meshGenerate.renderMode == MeshTopology.Triangles) 
			{
				meshGenerate.renderMode = MeshTopology.Points;
			}
			else
			{
				meshGenerate.renderMode = MeshTopology.Triangles;
			}
		}
		if (GUI.Button(new Rect(10, 150, 70, 70), "step-"))
		{
			meshGenerate.stepSize--;
			meshGenerate.stepSize = Mathf.Clamp(meshGenerate.stepSize, 1, 10);
			meshGenerate.Start();
		}
		if (GUI.Button(new Rect(90, 150, 70, 70), "step+"))
		{
			meshGenerate.stepSize++;
			meshGenerate.stepSize = Mathf.Clamp(meshGenerate.stepSize, 1, 10);
			meshGenerate.Start();
		}	
		if (GUI.Button(new Rect(1000, 70, 150, 70), "switch Camera"))
		{
			SwitchCamera();
		}
		GUI.Label(new Rect(1000, 50, 500, 20), "touches:" + Input.touchCount.ToString());
	}
}

//-----------------------------------------------------------------------
// <copyright file="DebugCamera.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// Does the same thing as GUI_Debug, but with Unity GUI button instead of custom buttons.
/// Shows top down camera.
/// </summary>
public class DebugCamera : MonoBehaviour 
{
    // this should be your prefab camera (ARCamera/VRCamera)
    public GameObject m_camObj;

    // flag to check if this camera is active
    private bool m_isDebugActive = false;
	
    // start position of touch input
    private Vector2 m_startPosScreen;

    // position of this game object when view shifts starts
    private Vector3 m_startPosWorld;

    // starting position for zoom in
    private float m_startDis = 0f;

    // used for zoom in_out
    private float m_startZoomValue = 0.0f;
    private float m_zoomSensitivity = 0.05f;

    /// <summary>
    /// Use this for initlialization.
    /// </summary>
    private void Start()
    {
        m_startPosScreen = new Vector2();
	}

    /// <summary>
    /// Use this for GUI related calls.
    /// </summary>
    private void OnGUI()
	{
        GUILayout.BeginHorizontal();

		// debug button disables and enables debug camera
        if (GUILayout.Button("Top Down Camera", GUILayout.Height(120), GUILayout.Width(120)))
		{
            if (!m_isDebugActive)
			{
                m_camObj.camera.enabled = false;
                transform.position = new Vector3(m_camObj.transform.position.x,
                    transform.position.y,
                    m_camObj.transform.position.z);
				this.camera.enabled = true;
                m_isDebugActive = true;
			}
			else
			{
                m_camObj.camera.enabled = true;
				this.camera.enabled = false;
                m_isDebugActive = false;
			}
		}
		GUILayout.EndHorizontal();
	}

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
	{
        if (m_isDebugActive)
		{
            // for direction movement
            if (Input.touchCount == 1)
			{
                // saves first touch
                if (Input.GetTouch(0).phase == TouchPhase.Began)
				{
                    m_startPosScreen = Input.GetTouch(0).position;
                    m_startPosWorld = transform.position;
				}
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					float scaleX = 20f / Screen.width;
					float scaleY = 20f / Screen.height;

					Vector2 newPos = Input.GetTouch(0).position;
                    Vector3 translateDirection = new Vector3((
                        m_startPosScreen.x - newPos.x) * scaleX,
					    0,
                        (m_startPosScreen.y - newPos.y) * scaleY);
                    transform.position = m_startPosWorld + translateDirection; 
				}
			}
			if (Input.touchCount == 2) 
			{
                // saves first touch
                if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began) 
				{
                    m_startDis = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    m_startZoomValue = transform.position.y;
				}
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
				{
					float curDis = 0f;
					curDis = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    float change = (m_startDis - curDis) * m_zoomSensitivity;
                    transform.position = new Vector3(transform.position.x,
                        m_startZoomValue + change,
					    transform.position.z);
				}
			}
		}
	}
}

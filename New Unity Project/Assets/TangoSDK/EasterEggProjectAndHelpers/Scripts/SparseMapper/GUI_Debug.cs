//-----------------------------------------------------------------------
// <copyright file="GUI_Debug.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// Does the same thing as DebugCamera, but with custom raycast button instead of GUI.
/// Shows top down camera.
/// </summary>
public class GUI_Debug : Button
{
    // this should be your prefab camera (ARCamera/VRCamera)
    public GameObject m_camObj; 

    // top down camera from your scene
    public GameObject m_topDownCamera;

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
    /// Switches between top down and normal camera
    /// Sets the top down camera position above the player when activated.
    /// </summary>
	protected override void TouchUp()
	{
		base.TouchUp();
        if (!m_isDebugActive)
		{
            m_camObj.camera.enabled = false;
            m_topDownCamera.transform.position = new Vector3(m_camObj.transform.position.x,
                m_topDownCamera.transform.position.y,
                m_camObj.transform.position.z);
            m_topDownCamera.camera.enabled = true;
            m_isDebugActive = true;
            m_debugString = "debug camera on";
		}
		else
		{
            m_camObj.camera.enabled = true;
            m_topDownCamera.camera.enabled = false;
            m_isDebugActive = false;
            m_debugString = "debug camera off";
		}
        m_raycastCamera.SendMessage("_UpdateStatus", m_debugString);
	}

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
	{
		base.Update();
        if (m_isDebugActive)
		{
            // for direction movement
            if (Input.touchCount == 1)
			{
                // saves first touch
                if (Input.GetTouch(0).phase == TouchPhase.Began)
				{
                    m_startPosScreen = Input.GetTouch(0).position;
                    m_startPosWorld = m_topDownCamera.transform.position;
				}
				
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					float scaleX = 20f / Screen.width;
					float scaleY = 20f / Screen.height;
					
					Vector2 newPos = Input.GetTouch(0).position;
					Vector3 translateDirection = new Vector3(
                        (m_startPosScreen.x - newPos.x) * scaleX,
                        0,
                        (m_startPosScreen.y - newPos.y) * scaleY);
                    m_topDownCamera.transform.position = m_startPosWorld + translateDirection; 
				}
			}
			if (Input.touchCount == 2)
			{
                // saves first touch
                if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
				{
                    m_startDis = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    m_startZoomValue = m_topDownCamera.transform.position.y;
				}
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                { 
					float curDis = 0f;
					curDis = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    float change = (m_startDis - curDis) * m_zoomSensitivity;
                    m_topDownCamera.transform.position = new Vector3(
                        m_topDownCamera.transform.position.x,
                        m_startZoomValue + change,
                        m_topDownCamera.transform.position.z);
				}
			}
		}
	}
}

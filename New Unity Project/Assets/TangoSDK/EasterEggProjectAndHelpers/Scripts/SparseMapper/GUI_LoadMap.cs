//-----------------------------------------------------------------------
// <copyright file="GUI_LoadMap.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace Tango
{
    /// <summary>
    /// Loads the sparse map from file directory.
    /// </summary>
	public class GUI_LoadMap : Button
    {
        // AR/VR camera you use
        public GameObject m_cameraObj;

        // Trail loader object in your scene
        public GameObject m_trailLoaderObj;

        // GUI skin for the load interface
        public GUISkin m_customGuiSkin;

        // File Browser object
        protected FileListViewer m_fileViewer;

        /// <summary>
        /// Custom button press function
        /// Calls the fileSelectedCallBack to show file browser.
        /// </summary>
		protected override void TouchUp()
        {
            m_raycastCamera.SendMessage("_TurnOffTutorial");
			base.TouchUp();
            m_debugString = "choose file, trying to load";
            m_raycastCamera.SendMessage("_UpdateStatus", m_debugString);
			{
                m_fileViewer.ReInitialize();
                if (m_cameraObj.GetComponent<Navigator>() != null)
                {
                    Destroy(m_cameraObj.GetComponent<Navigator>());
                }
			}
		}

        /// <summary>
        /// File List viewer call back to load a file
        /// Loads sparse map and trail.txt file.
        /// </summary>
        /// <param name="path">Path of file to be loaded.</param>
        protected void CallBack(string path)
        {
            if (m_fileViewer.m_path != string.Empty)
            {
                m_cameraObj.AddComponent<Navigator>();
                m_cameraObj.GetComponent<Navigator>().SetPath(Application.persistentDataPath + "/" + m_fileViewer.m_path);
                TrailManager trailManagerObj = m_trailLoaderObj.GetComponent<TrailManager>();
                if (trailManagerObj.LoadTrailFromFile(Application.persistentDataPath + "/" + m_fileViewer.m_path + ".txt"))
                {   
                    trailManagerObj.CreateTrailFromList();
                    m_debugString = "loaded successfully ";
                }
                else
                {
                    m_debugString = "Trail file not found";
                }
            }
            else
            {
                m_debugString = "path is null";
            }
            m_raycastCamera.SendMessage("_UpdateStatus", m_debugString);
        }

        /// <summary>
        /// Use this for GUI related calls.
        /// </summary>
        private void OnGUI()
        {
            GUI.skin = m_customGuiSkin;
            if (m_fileViewer != null)
            {
                m_fileViewer.OnGUI();
            }
        }

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        private void Start()
        {
            m_fileViewer = new FileListViewer(CallBack);
        }
	}
}
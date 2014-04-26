//-----------------------------------------------------------------------
// <copyright file="GUI_RecordMap.cs" company="Google">
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
    /// Example script to show how to record sparse map. 
    /// Also shows how to save sparse map.
    /// </summary>
	public class GUI_RecordMap : Button
    {
        // AR/VR Camera object from your scene
		public GameObject cameraObj;

        // Trail loader object in your scene
        public GameObject trailManagerGameObj;

        // recording RED texture
        public Texture2D m_onRecording;

        // bool to show recording status.
        private bool m_isRecording;

        /// <summary>
        /// Changes texture when not recording.
        /// </summary>
        public void RecordingOff()
        {
            m_isRecording = false;
            renderer.material.mainTexture = m_outTouchTexture;
        }

        /// <summary>
        /// Custom button press function
        /// Calls the trail manager to start saving trails.
        /// </summary>
        protected override void TouchUp()
		{
            m_raycastCamera.SendMessage("_TurnOffTutorial");

            if (!m_isRecording)
			{ // start recording sparsemap
                renderer.material.mainTexture = m_onRecording;
                trailManagerGameObj.GetComponent<TrailManager>().StartTrailBuilding();

                // cameraObj.SendMessage("ReInitialize",Navigator.Mode.VisualIntertialNavigaitionAndMapping);
                if (cameraObj.GetComponent<Navigator>() == null)
                {
                    cameraObj.AddComponent<Navigator>();
                }
                m_debugString = "New Sparse Map Recording...";
                m_raycastCamera.SendMessage("_UpdateStatus", m_debugString);
                m_isRecording = true;
			}
			else
			{ 
                // save sparsemap
                renderer.material.mainTexture = m_outTouchTexture;
				if (cameraObj.GetComponent<Navigator>() != null)
				{
                    string fileName = Application.persistentDataPath + "/sparse_map" + _GetCurrentTimeStamp();
                    if (cameraObj.GetComponent<Navigator>().SaveSparseMap(fileName))
					{
                        m_debugString = "Done Save sparse_map" + _GetCurrentTimeStamp();
                        trailManagerGameObj.GetComponent<TrailManager>().StopTrailBuilding(fileName + ".txt");
                        m_isRecording = false;
					}
					else
                    {
                        m_debugString = "Save Error!";
                    }
				}
				else
                {
                    m_debugString = "Problem in navigator script";
                }
                m_raycastCamera.SendMessage("_UpdateStatus", m_debugString);
			}
		}

        /// <summary>
        /// Changes texture when not on button.
        /// </summary>
		protected override void OutTouch()
        {
            if (!m_isRecording)
            {
                renderer.material.mainTexture = m_outTouchTexture;
            }
		}

        /// <summary>
        /// Returns timestamp in a particular format.
        /// </summary>
        /// <returns> Returns system time stamp.</returns>
        private string _GetCurrentTimeStamp()
        {
            return string.Format("{0:yyyyMMdd_HHmmss}", System.DateTime.Now);
        }
	}
}

//-----------------------------------------------------------------------
// <copyright file="TrailManager.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the trail data
/// Contains functions for saving, loading trail file.
/// </summary>
public class TrailManager : MonoBehaviour
{
    // Distance between each trail point saved, higher distance = lower accuracy
    public float m_distanceFactor = 0.5f;

    // This is your AR/VR Camera
    public GameObject m_cameraObj;

    // material of the line which was recorded
    public Material m_RecordedDataMaterial;
    
    // material of the line which is being drawn
    public Material m_LiveDataMaterial;

    // List of positions you save during trail formation
    // this is public so that you can access in other scripts
    public List<Vector3> m_savePositionData;

    // flag for checking if recording is started
    private bool m_startRecording;

    // material of the line
    private Material m_lineMaterial;

    // List of game objects generated when a trail is loaded from a file
    private List<GameObject> m_trailGameObjects;

    /// <summary>
    /// Starts recording trails by setting m_startRecording to true.
    /// </summary>
    public void StartTrailBuilding()
    {
        Vector3 startPos = new Vector3(m_cameraObj.transform.position.x,
            m_cameraObj.transform.position.y,
            m_cameraObj.transform.position.z);
        m_savePositionData.Add(startPos);
        m_startRecording = true;
    }

    /// <summary>
    /// Stops recording trails by setting m_startRecording to false.
    /// Calls the function to write to a file.
    /// </summary>
    /// <param name="fileName"> Name of file you want to save trail vector3 data in.</param>
    public void StopTrailBuilding(string fileName)
    {
        m_startRecording = false;
        _WriteToFile(fileName);
    }

    /// <summary>
    /// Read the file and creates reinitialize positions array.
    /// </summary>
    /// <param name="fileName"> Name of file you want to save trail vector3 data in.</param>
    /// <returns> Returns true if load successful.</returns>
    public bool LoadTrailFromFile(string fileName)
    {
        char[] delimeters = 
        {
            ',',
            ')',
            '('
        };
        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        if (fs == null)
        {
            return false;
        }
        _ReInitializeTrail();
        StreamReader fileToRead = new StreamReader(fs);
        while (!fileToRead.EndOfStream)
        {
            Vector3 position;
            string fileData = fileToRead.ReadLine();
            string[] xyzCordinate = fileData.Split(delimeters);

            // xyzCordinate[0] has ' ' , reason = '(' is a delimiter too
            // so we ignore element at index 0
            position.x = float.Parse(xyzCordinate[1]); 
            position.y = float.Parse(xyzCordinate[2]);
            position.z = float.Parse(xyzCordinate[3]);
            m_savePositionData.Add(position);
        }
        return true;
    }

    /// <summary>
    /// Creates a trail using line renderer.
    /// </summary>
    public void CreateTrailFromList()
    {
        for (int i = 0; i < m_savePositionData.Count - 1; i++)
        {
            _CreateLine(m_savePositionData[i], m_savePositionData[i + 1], m_RecordedDataMaterial);
        }
    }
        
    /// <summary>
    /// Use this for initialization.
    /// </summary>
    private void Start()
    {
        m_savePositionData = new List<Vector3>();
        m_trailGameObjects = new List<GameObject>();
	}
	
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (m_startRecording)
		{
            // check if distance has been changed more than the distanceFactor
            if (Vector3.Distance(m_savePositionData[m_savePositionData.Count - 1], m_cameraObj.transform.position) > m_distanceFactor)
			{
                Vector3 currentPosition = new Vector3(m_cameraObj.transform.position.x,
                    m_cameraObj.transform.position.y,
                    m_cameraObj.transform.position.z);
                _CreateLine(m_savePositionData[m_savePositionData.Count - 1], currentPosition, m_LiveDataMaterial);
                m_savePositionData.Add(currentPosition);
			}
		}
	}
        
    /// <summary>
    /// Writes vector3 trail to a file.
    /// </summary>
    /// <param name="fileName"> Name of file you want to save trail vector3 data in.</param>
    private void _WriteToFile(string fileName)
    {
        FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

        // if there is an error creating a file
        if (fs == null)
        {
            return;
        }
        StreamWriter textRecorder = new StreamWriter(fs);
        for (int i = 0; i < m_savePositionData.Count; i++)
        {
            textRecorder.WriteLine(m_savePositionData[i].ToString());
        }
        textRecorder.Close();
        fs.Close();
    }

    /// <summary>
    /// Clean up / reinitialize function.
    /// </summary>
    private void _ReInitializeTrail()
	{
        for (int i = m_trailGameObjects.Count - 1; i >= 0; i--)
		{
            Destroy(m_trailGameObjects[i]);
            m_trailGameObjects.RemoveAt(i);
		}
        m_savePositionData.Clear();
        m_savePositionData = new List<Vector3>();
        m_trailGameObjects = new List<GameObject>();
	}

    /// <summary>
    /// Draws line between 2 points and assigns line renderer properties.
    /// </summary>
    /// <param name="start"> Starting position of trail.</param>
    /// <param name="end"> Ending position of trail.</param>
    /// <param name="colorOfLine"> Color to be associated with line, red for live,green for recorded.</param>
    private void _CreateLine(Vector3 start, Vector3 end, Material colorOfLine)
    {
		float lineSize = 0.1f;
		GameObject drawPoint = new GameObject("drawPoint");
        m_trailGameObjects.Add(drawPoint);
		drawPoint.transform.parent = transform;
		drawPoint.transform.rotation = transform.rotation;
		LineRenderer lines = (LineRenderer)drawPoint.AddComponent<LineRenderer>();
        lines.sharedMaterial = colorOfLine;
		lines.useWorldSpace = false;
		lines.SetWidth(lineSize, lineSize);
		lines.SetVertexCount(2);
		lines.SetPosition(0, start);
		lines.SetPosition(1, end);
	}
}

//-----------------------------------------------------------------------
// <copyright file="RunTimePath.cs" company="Google">
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
public class RunTimePath : MonoBehaviour
{
    // Distance between each trail point saved, higher distance = lower accuracy
    public float m_distanceFactor = 0.5f;

    // This is your AR/VR Camera
    public GameObject m_cameraObj;

    // Cube Prefab
    public GameObject m_cubePrefab;

    // List of game objects generated when a trail is loaded from a file
    private List<GameObject> m_trailGameObjects;

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    private void Awake()
    {
        m_trailGameObjects = new List<GameObject>();
        _GenerateGameObject();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // check if distance has been changed more than the distanceFactor
        if (Vector3.Distance(m_trailGameObjects[m_trailGameObjects.Count - 1].transform.position, m_cameraObj.transform.position) > m_distanceFactor)
        {
            _GenerateGameObject();
        }
    }

    /// <summary>
    /// Generates a game object and adds it to the list.
    /// </summary>
    private void _GenerateGameObject()
    {
        GameObject pathPoint = GameObject.Instantiate(m_cubePrefab, m_cameraObj.transform.position, Quaternion.identity) as GameObject;
        m_trailGameObjects.Add(pathPoint);
    }
}

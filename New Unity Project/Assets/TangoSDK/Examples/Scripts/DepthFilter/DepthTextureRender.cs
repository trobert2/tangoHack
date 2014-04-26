//-----------------------------------------------------------------------
// <copyright file="DepthTextureRender.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngine;
using Tango;

/// <summary>
/// DepthTextureRender has the functionality to render the depth image
/// as well as turn on/off filter and change 
/// </summary>
public class DepthTextureRender : MonoBehaviour
{
    public GameObject m_camObj;
    public GameObject m_screenObj;
    private const int m_DepthBufferWidth = 320;
    private const int m_DepthBufferHeight = 180;	
    private Tango.Synchronizer m_tangoCam;
    private Texture2D m_tex;
    private Color[] m_depthColorArr;
    private int[] m_depthArr_int;
    private float[] m_zmeter;
	private float[] filteredZMeter;

	// farest value from depth
	private float m_top = 8.0f;
	private bool m_isUsingFilter = false;
	private float m_blendValue;
	private double m_timeStamp = 0.0;
	private double m_preTimeStamp = 0.0;

    /// <summary>
    /// Use this for initialization.
    /// </summary>
	private void Start() 
	{
        m_tex = new Texture2D(m_DepthBufferWidth, m_DepthBufferHeight);
        m_depthColorArr = new Color[m_DepthBufferWidth * m_DepthBufferHeight];
        m_zmeter = new float[m_DepthBufferWidth * m_DepthBufferHeight];
        filteredZMeter = new float[m_DepthBufferWidth * m_DepthBufferHeight];
        m_depthArr_int = new int[m_DepthBufferWidth * m_DepthBufferHeight];
        m_tangoCam = m_camObj.GetComponent<Tango.Synchronizer>() as Tango.Synchronizer;
        for (int i = 0; i < m_DepthBufferWidth * m_DepthBufferHeight; i++) 
        {
            m_depthColorArr[i] = Color.white;
        }
	}

    /// <summary>
    /// Update is called once per frame.
	/// Feed both color texture and depth texture into shader
    /// </summary>
	private void Update() 
	{	
        m_tangoCam.getDepthBuffer(ref m_depthArr_int, ref m_timeStamp);
//		if (m_timeStamp == m_preTimeStamp)
//        {
//            return;
//        }
//		m_preTimeStamp = m_timeStamp;
        _GetZFromRaw(ref m_zmeter, m_depthArr_int);
        float[] zarray;
        if (m_isUsingFilter)
        {
            Helper.Filter(m_zmeter, filteredZMeter, 3);
            zarray = filteredZMeter;
        }
        else
        {
            zarray = m_zmeter;
        }
        for (int i = 0; i < m_DepthBufferWidth * m_DepthBufferHeight; i++) 
        {
            m_depthColorArr[i].r = (float)((float)zarray[i] / (float)m_top);
            m_depthColorArr[i].g = (float)((float)zarray[i] / (float)m_top);
            m_depthColorArr[i].b = (float)((float)zarray[i] / (float)m_top);
        }
        m_tex.SetPixels(m_depthColorArr);
        m_tex.Apply();
        renderer.material.SetTexture("_ColorTex", m_screenObj.renderer.material.mainTexture);
        renderer.material.SetTexture("_DepthTex", m_tex);
        renderer.material.SetFloat("_BlendValue", m_blendValue);
	}

    /// <summary>
	/// get z buffer from raw data, put pixels upside down
    /// </summary>
    /// <param name="z"> z buffer in meters </param>
    /// <param name="intArr"> raw int array data </param>
    private void _GetZFromRaw(ref float[] z, int[] intArr)
    {
        for (int i = 0; i < m_DepthBufferHeight; i++)
        {
            for (int j = 0; j < m_DepthBufferWidth; j++)
            {
                int index = ((m_DepthBufferHeight - i - 1) * m_DepthBufferWidth) + j;
                int z_raw = intArr[index];
                z[(i * m_DepthBufferWidth) + j] = (float)z_raw * 0.001f;
            }
        }
    }

    /// <summary>
    /// Updates GUI.
    /// </summary>
	private void OnGUI()
	{
        if (GUI.Button(new Rect(1000, 70, 150, 70), "+"))
        {
            m_blendValue += 0.1f;
        }
        if (GUI.Button(new Rect(1000, 140, 150, 70), "-"))
        {
            m_blendValue -= 0.1f;
        }
        if (GUI.Button(new Rect(1000, 210, 150, 70), m_isUsingFilter ? "Filter On" : "Filter Off"))
        {
            m_isUsingFilter = !m_isUsingFilter;
        }
		m_blendValue = Mathf.Clamp(m_blendValue, 0.0f, 1.0f);
	}	
}
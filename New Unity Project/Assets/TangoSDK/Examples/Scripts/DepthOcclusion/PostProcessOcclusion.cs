//-----------------------------------------------------------------------
// <copyright file="PostProcessOcclusion.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// This class will pass real depth buffer, real rgb buffer, virtual depth buffer and virtual 
/// rgb buffer into the post process shader
/// </summary>
public class PostProcessOcclusion : MonoBehaviour
{
    public Material m_depthOcclusionMaterial;

    // AR/VR camera object from your scene
    public GameObject m_camObj;
    public GameObject m_screenObj;
    public Tango.VideoOverlayRenderer m_videoOverlayRenderer;
	private const int m_DepthBufferWidth = 320;
    private const int m_DepthBufferHeight = 180;
    private Tango.Synchronizer m_tangoCam;
    private Texture2D m_tex;
    private Color[] m_colorArr;
    private int[] m_depthArr_int;
    private float[] m_zmeter;

	// this value is the far clip plane of cam
    private float m_top = 10.0f;
    private double m_timeStamp = 0.0;
    private double m_preTimeStamp = 0.0;

	private float[] m_filterArr = new float[m_DepthBufferWidth * m_DepthBufferHeight];
    private bool m_isShowingDepthImage = false;
    private bool m_isUsingFilter = false;

    /// <summary>
	/// add an UFO object into the scene
    /// </summary>
    public void AddUFO()
    {
        Vector3 objectPosition = new Vector3(
            m_camObj.transform.position.x,
            m_camObj.transform.position.y,
            m_camObj.transform.position.z);        
        GameObject newUFOObject = (GameObject)Instantiate(
            Resources.Load("Prefabs/UFO_Grunt"),
            objectPosition,
            Quaternion.identity); 
        newUFOObject.transform.parent = m_camObj.gameObject.transform;
        newUFOObject.transform.localPosition = new Vector3(0f, -0.1f, 0.5f);
        newUFOObject.transform.parent = null;
    }

    /// <summary>
    /// Use this for initialization.
	/// Init all the data for depth texture construct
    /// </summary>
	private void Start() 
	{
        camera.depthTextureMode = DepthTextureMode.Depth;
        m_tex = new Texture2D(m_DepthBufferWidth, m_DepthBufferHeight);
        m_colorArr = new Color[m_DepthBufferWidth * m_DepthBufferHeight];
        m_zmeter = new float[m_DepthBufferWidth * m_DepthBufferHeight];
        m_depthArr_int = new int[m_DepthBufferWidth * m_DepthBufferHeight];
        m_tangoCam = m_camObj.GetComponent<Tango.Synchronizer>() as Tango.Synchronizer;
        for (int i = 0; i < m_DepthBufferWidth * m_DepthBufferHeight; i++) 
        {
            m_colorArr[i] = Color.blue;
        }
	}

    /// <summary>
    /// Update is called once per frame.
	/// Running filter, filling out the buffer and pass to the shader each frame.
    /// </summary>
	private void Update() 
	{
        m_tangoCam.getDepthBuffer(ref m_depthArr_int, ref m_timeStamp);
//        if (m_preTimeStamp == m_timeStamp)
//        {
//            return;
//        }
//        m_preTimeStamp = m_timeStamp;
        _GetZFromRaw(ref m_zmeter, m_depthArr_int);
        if (m_isUsingFilter)
        {
            Helper.Filter(m_zmeter, m_filterArr, 2);
        }
        for (int i = 0; i < m_DepthBufferWidth * m_DepthBufferHeight; i++) 
        {
            if (m_isUsingFilter)
            {
                m_colorArr[i].r = (float)((float)m_filterArr[i] / (float)m_top);
                m_colorArr[i].g = (float)((float)m_filterArr[i] / (float)m_top);
                m_colorArr[i].b = (float)((float)m_filterArr[i] / (float)m_top);
                m_colorArr[i].a = 1.0f;
            }
            else
            {
                m_colorArr[i].r = (float)((float)m_zmeter[i] / (float)m_top);
                m_colorArr[i].g = (float)((float)m_zmeter[i] / (float)m_top);
                m_colorArr[i].b = (float)((float)m_zmeter[i] / (float)m_top);
                m_colorArr[i].a = 1.0f;
            }
        }
        m_tex.SetPixels(m_colorArr);
        m_tex.Apply();
        m_depthOcclusionMaterial.SetTexture("_ColorTex", m_videoOverlayRenderer.frameTexture);
        m_depthOcclusionMaterial.SetTexture("_DepthTex", m_tex);
	}
	
    /// <summary>
	/// get the z buffer from raw data. arrange the pixel upside down
    /// </summary>
    /// <param name="z"> Your Comment2.</param>
    /// <param name="intArr"> Your Comment3.</param>
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
	/// Post process from source texture to destination
    /// </summary>
	/// <param name="z"> z buffer in meters </param>
	/// <param name="intArr"> raw int array data </param>
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, m_depthOcclusionMaterial);
    }

    /// <summary>
    /// Updates GUI.
    /// </summary>
	private void OnGUI()
	{
        if (GUI.Button(new Rect(10, 10, 250, 70), m_isShowingDepthImage ? "Depth View On" : "Depth View Off")) 
        {
            m_isShowingDepthImage = !m_isShowingDepthImage;
            if (m_isShowingDepthImage)
            {
                m_depthOcclusionMaterial.SetInt("_IsShowingDepth", 1);
            }
            else
            {
                m_depthOcclusionMaterial.SetInt("_IsShowingDepth", 0);
            }
        }
        if (GUI.Button(new Rect(10, 90, 250, 70), m_isUsingFilter ? "Filter On" : "Filter Off")) 
        {
            m_isUsingFilter = !m_isUsingFilter;
        }
        if (GUI.Button(new Rect(10, 170, 250, 150), "Add UFO")) 
        {
            AddUFO();
        }
    }
}

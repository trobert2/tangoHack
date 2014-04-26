//-----------------------------------------------------------------------
// <copyright file="MeshGenerate.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;
using Tango;

/// <summary>
/// Mesh construct class.
/// </summary>
public class MeshGenerate : MonoBehaviour 
{
	// tango ARCamera object
	public GameObject camObj;

	// render mode, point cloud by default
	public MeshTopology renderMode = MeshTopology.Points;
	
	// step size, every n pixel take 1 value
	public int stepSize = 1;
	
	// since the depth image has some non-valid data on the left and bottom
	// edge, so we set a edge size to skip the non-valid data
	public int edgeSize = 0;

	// debug texture
	public Texture2D debugTexture;

	// some const value
	private const int DepthBufferWidth = 320;
	private const int DepthBufferHeight = 180;
	private const float MillimeterConversion = 0.001f;

	// tango camera instance
	private Tango.Synchronizer tangoCam;

	// screen obj, used to get the rgb texture
	private VideoOverlayRenderer videoOverlayRender;

	// camera's property
	// used for put mesh in the center of screen
	// and make it in the right scale
	private int screenWidth;
	private int screenHeight;

	// debug use: texture's pixels arr
	private Color[] pixelArr;

	// raw depth data
	private int[] depthArr_int;

	// real z value in meters
	private float[] depthMeter;

	private int texWidth, texHeight;
	private int traverseWidth, traverseHeight;
	private double timeStamp;
	private double prevTimestamp;

	// vertices will be assigned to this mesh
	private Mesh mesh;
	private MeshCollider meshCollider;

	// mesh data
	private Vector3[] vertices;
	private Vector2[] uv;
	private int[] triangles;

	private int frameCounter = 0;

	// vertex cut out threshold from depth z distance
	private float depthDistThreshold = 0.3f;

	// vertex cut out threshold from neighbor vetex distnce
	private float neighborDistThreshold = 0.5f;

	/// <summary>
	/// Use this for initialization.
	/// </summary>
	public void Start() 
	{
		#if (UNITY_EDITOR)
		texWidth = debugTexture.width;
		texHeight = debugTexture.height;
		traverseWidth = texWidth / stepSize;
		traverseHeight = texHeight / stepSize;
		
		#elif (UNITY_ANDROID)
		texWidth = DepthBufferWidth;
		texHeight = DepthBufferHeight;
		traverseWidth = texWidth / stepSize;
		traverseHeight = texHeight / stepSize;
		tangoCam = camObj.GetComponent<Tango.Synchronizer>();
		videoOverlayRender = camObj.GetComponent<Tango.VideoOverlayRenderer>();
		videoOverlayRender.is_runningBackgroundRenderLoop = false;
		depthMeter =  new float[DepthBufferWidth * DepthBufferHeight];
		depthArr_int = new int[DepthBufferWidth * DepthBufferHeight];
		timeStamp = prevTimestamp = 0.0;
		#else
		#error platform is not supported
		#endif

		// re-calculate edge size based on step size
		edgeSize = edgeSize / stepSize;

		// note: this is the real screen size, not camera size
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		
		// get the reference of mesh
		MeshFilter mf = gameObject.GetComponent<MeshFilter>();
		if (mf == null) 
		{
			MeshFilter meshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
			meshFilter.mesh = mesh = new Mesh();
			MeshRenderer renderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
			renderer.material.shader = Shader.Find("Mobile/Unlit (Supports Lightmap)");
		} 
		else 
		{
			mesh = mf.mesh;
		}
		CreateMesh();
	}
	
	/// <summary>
	/// Update is called once per frame.
	/// </summary>
	private void Update() 
	{
		UpdateMesh();
	}
	
	/// <summary>
	/// Update mesh texture.
	/// </summary>
	private void UpdateTexture()
	{
		#if (UNITY_EDITOR)
		#elif (UNITY_ANDROID)
		videoOverlayRender.RenderBackgoundTextureOnDemand();
		this.renderer.material.mainTexture = videoOverlayRender.frameTexture;
		#else
		#error platform is not supported
		#endif
	}

	/// <summary>
	/// Create the mesh using two passes.
	/// first pass get all the vertices and texture coordinates
	/// second pass to get all the triangles.
	/// </summary>
	private void CreateMesh()
	{
		// alloc vertices array, uv array and triangle array.
		int arrSize = traverseWidth * traverseHeight;
		vertices = new Vector3[arrSize];
		uv = new Vector2[arrSize];
		int counter = 0;

		// traverse the screen to fill in the vertice array
		// and uv array
		counter = 0;
		for (int i = 0; i < traverseHeight; i++) 
		{
			for (int j = 0; j < traverseWidth; j++)
			{
				// UV
				uv[counter].x = 
					(float)((float)j / (float)traverseWidth);
				uv[counter].y = 
					(float)(((float)traverseHeight - (float)i) / (float)traverseHeight);
				counter++;
			}
		}

		// second pass to assign the triangles	
		int w = traverseWidth - 1;
		int h = traverseHeight - 1;
		int triCounter = 0;
		triangles = new int[6 * w * h];
		for (int i = 0; i < h; i++)
		{
			for (int j = 0; j < w; j++)
			{
				// w + 1 is the real width of vertices
				// triangle 1
				triangles[triCounter] = (i * (w + 1)) + j;
				triangles[triCounter + 1] = ((i + 1) * (w + 1)) + j;
				triangles[triCounter + 2] = (i * (w + 1)) + (j + 1);

				// triangle 2
				triangles[triCounter + 3] = ((i + 1) * (w + 1)) + j;
				triangles[triCounter + 4] = ((i + 1) * (w + 1)) + (j + 1);
				triangles[triCounter + 5] = (i * (w + 1)) + (j + 1);
				triCounter += 6;
			}
		}
		
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	/// <summary>
	/// Update the mesh vertices and triangles
	/// separated computation into different frame.
	/// </summary>
	private void UpdateMesh()
	{
		if (frameCounter == 0)
		{
			tangoCam.getDepthBuffer(ref depthArr_int, ref timeStamp);
			if (prevTimestamp == timeStamp) 
			{
				return;
			}
			prevTimestamp = timeStamp;

			// convert the compressed int array to z meters array
			GetZFromRaw(ref depthMeter, depthArr_int);
			frameCounter++;
			return;
		}

		if (frameCounter == 1)
		{
			int vertexCounter = 0;
			float xd = screenWidth / traverseWidth;
			float yd = screenHeight / traverseHeight;
			for (int i = 0; i < traverseHeight; i++) 
			{
				for (int j = 0; j < traverseWidth; j++)
				{
					vertices[vertexCounter] = 
						camObj.camera.ScreenToWorldPoint(new Vector3(j * (float)xd, 
							i * (float)yd,
							depthMeter[(i * stepSize * texWidth) + (j * stepSize)]));
					vertexCounter++;
				}
			}
			frameCounter++;
			return;
		}

		if (frameCounter == 2)
		{
			int w = traverseWidth - 1;
			int h = traverseHeight - 1;

			int triCounter = 0;
			int len = 6 * w * h;
			for (int i = 0; i < len; i++) 
			{
				triangles[i] = 0;
			}
			for (int i = 0; i < h; i++)
			{
				for (int j = 0; j < w; j++)
				{
					// w + 1 is the real width of vertices
					// vertex index map
					// 1 3
					// 0 2
					int index0 = (i * stepSize * texWidth) + (j * stepSize);
					int index1 = ((i + 1) * stepSize * texWidth) + (j * stepSize);
					int index2 = (i * stepSize * texWidth) + ((j + 1) * stepSize);
					int index3 = ((i + 1) * stepSize * texWidth) + ((j + 1) * stepSize);
					
					// triangle 1
					float distance = Mathf.Abs(depthMeter[index0] - depthMeter[index1]);
					distance += Mathf.Abs(depthMeter[index0] - depthMeter[index2]);
					distance += Mathf.Abs(depthMeter[index1] - depthMeter[index2]);
					if (depthMeter[index0] > depthDistThreshold &&
					    depthMeter[index1] > depthDistThreshold &&
					    depthMeter[index2] > depthDistThreshold && 
					   distance < neighborDistThreshold)
					{
						triangles[triCounter] = (i * (w + 1)) + j;
						triangles[triCounter + 1] = ((i + 1) * (w + 1)) + j;
						triangles[triCounter + 2] = (i * (w + 1)) + (j + 1);
					}
					
					// triangle 2
					distance = 0.0f;
					distance += Mathf.Abs(depthMeter[index1] - depthMeter[index2]);
					distance += Mathf.Abs(depthMeter[index2] - depthMeter[index3]);
					distance += Mathf.Abs(depthMeter[index1] - depthMeter[index3]);
					if (depthMeter[index1] > depthDistThreshold &&
					    depthMeter[index3] > depthDistThreshold &&
					    depthMeter[index2] > depthDistThreshold && 
					   distance < neighborDistThreshold)
					{
						triangles[triCounter + 3] = ((i + 1) * (w + 1)) + j;
						triangles[triCounter + 4] = ((i + 1) * (w + 1)) + (j + 1);
						triangles[triCounter + 5] = (i * (w + 1)) + (j + 1);
					}
					triCounter += 6;
				}
			}
			frameCounter++;
			return;
		}
		if (frameCounter == 3)
		{
			// update the vertices
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			mesh.SetIndices(triangles, renderMode, 0);
			UpdateTexture();
			frameCounter = 0;
		}
	}

	/// <summary>
	/// Get float depth data from raw int data
	/// also flip around x axis.
	/// </summary>
	/// <param name="z">Float array of depth.</param>
	/// <param name="intArr">Raw int data.</param>
	private void GetZFromRaw(ref float[] z, int[] intArr)
	{
		for (int i = 0; i < DepthBufferHeight; ++i)
		{
			int invRowIndex = (DepthBufferHeight - i - 1) * DepthBufferWidth;
			int rowIndex = i * DepthBufferWidth;
			for (int j = 0; j < DepthBufferWidth; ++j)
			{
				int index = invRowIndex + j;
				int z_raw = intArr[index];

				// depth data comes in with unit of millimeter, 
				// we convert it into the meter unit we are using in unity.
				z[rowIndex + j] = (float)z_raw * MillimeterConversion;
			}
		}
	}
}

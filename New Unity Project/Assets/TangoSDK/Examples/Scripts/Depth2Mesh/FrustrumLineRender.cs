﻿//-----------------------------------------------------------------------
// <copyright file="FrustrumLineRender.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//---------------------------------------------------------------
using System.Collections;
using UnityEngine;

/// <summary>
/// Frustrum render.
/// </summary>
public class FrustrumLineRender : MonoBehaviour 
{
	// ar camera
	public Camera cam;

	// far end plane distance.
	public float distance;

	public Material lineMat;

	// screen corner unprojected
	private Vector3 lb, lt, rb, rt;

	/// <summary>
	/// Use this for initialization.
	/// </summary>
	private void Start()
	{
		lb = new Vector3(0.0f, 0.0f, distance);
		lt = new Vector3(0.0f, cam.pixelHeight, distance);
		rb = new Vector3(cam.pixelWidth, 0.0f, distance);
		rt = new Vector3(cam.pixelWidth, cam.pixelHeight, distance);
	}

	/// <summary>
	/// Unity post render call back.
	/// </summary>
	private void OnPostRender() 
	{	
		Vector3 pos0 = cam.transform.position;
		Vector3 pos1 = cam.ScreenToWorldPoint(lb);
		Vector3 pos2 = cam.ScreenToWorldPoint(lt);
		Vector3 pos3 = cam.ScreenToWorldPoint(rt);
		Vector3 pos4 = cam.ScreenToWorldPoint(rb);
	
		GL.PushMatrix();
		lineMat.SetPass(0);
		GL.Begin(GL.LINES);

		GL.Color(Color.white);
		GL.Vertex(pos0);
		GL.Vertex(pos1);
		GL.Vertex(pos0);
		GL.Vertex(pos2);
		GL.Vertex(pos0);
		GL.Vertex(pos3);
		GL.Vertex(pos0);
		GL.Vertex(pos4);

		GL.Vertex(pos1);
		GL.Vertex(pos2);
		GL.Vertex(pos2);
		GL.Vertex(pos3);
		GL.Vertex(pos3);
		GL.Vertex(pos4);
		GL.Vertex(pos4);
		GL.Vertex(pos1);
		GL.End();
		GL.PopMatrix();
	}
}

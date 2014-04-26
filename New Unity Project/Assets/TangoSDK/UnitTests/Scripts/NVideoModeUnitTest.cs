// Copyright 2013 Motorola Mobility LLC. Part of the Trailmix project.
// CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.

using UnityEngine;
using System.Collections;

using Tango;

public class VideoModeUnitTest : MonoBehaviour {

	// Test 
	void Start () 
	{
		testNVideoMode_visibleRect();	
	}
	
	private void testNVideoMode_visibleRect()
	{
		//720p
		VideoMode.VideoModeAttributes [] videoModes = new VideoMode.VideoModeAttributes[3];
		videoModes[0].width = 1280; videoModes[0].height = 720;
		videoModes[1].width = 640;  videoModes[1].height = 480;
		videoModes[2].width = 480;  videoModes[2].height = 360;
		
		//iphone
		Vector2 [] screenResoltions = new Vector2[5];
		//iphone
		screenResoltions[0].x = 480; screenResoltions[0].y = 320; 
		screenResoltions[1].x = 960; screenResoltions[1].y = 640; 
		screenResoltions[2].x = 1136; screenResoltions[2].y = 640;
		//ipad
		screenResoltions[3].x = 1204; screenResoltions[3].y = 768; 
		screenResoltions[4].x = 2408; screenResoltions[4].y = 1536;

		
		foreach(VideoMode.VideoModeAttributes videoMode in videoModes)
		{
			foreach(Vector2 r in screenResoltions)
			{
				Vector2 visibleRect = VideoMode.visibleRect(videoMode, r);
				//aspect have to identical to screen resoltion aspect
				float aspect = (float)visibleRect.x/(float)visibleRect.y;
				float windowAspect = r.x/r.y;
				if(Mathf.Abs( aspect - windowAspect) > 0.01)
				{
					Debug.Log("In:("+videoMode+r+")");
					Debug.Log("Out:("+visibleRect+")");
					Debug.LogError("Wrong aspect ratio:[" + aspect+"],["+windowAspect+"]");
				}
		
				//width or height have to be equal to video mode width and height
				if((int)visibleRect.x != videoMode.width && (int)visibleRect.x != videoMode.height)
				{
					Debug.Log("In:("+videoMode+r+")");
					Debug.Log("Out:("+visibleRect+")");
					Debug.LogError("Wrong resoltion");
				}
				
			}
		}
	}	
}

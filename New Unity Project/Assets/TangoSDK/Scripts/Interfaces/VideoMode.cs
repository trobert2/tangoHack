// Copyright 2013 Motorola Mobility LLC. Part of the Trailmix project.
// CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Tango
{
	public class VideoMode
	{
		#region NESTED
		[StructLayout(LayoutKind.Sequential, Pack = 1)]                                                            
		public struct VideoModeAttributes
		{
			public int width;
			public int height;
			public float frameRate;
			
			public VideoModeAttributes(int w, int h, float fr)
			{
				width = w;
				height = h;
				frameRate = fr;
			}
			
			public override string ToString()
			{
				return "NVideoModeAttributes:[" + width + "," + height + "," + frameRate + "]";
			}
			
		}
		#endregion
		
		#region PUBLIC_METHODS
		static public int lookupVideoMode (ref VideoModeAttributes videoMode)
		{ 
			int videoModeID = -1;
			IntPtr videoModePtr = Marshal.AllocHGlobal (Marshal.SizeOf (typeof(VideoModeAttributes)));
			
			int numModes = TangoVideoModeAPI.VideoModeNumberVideoModes ();
			if( numModes>0 )
			{
				//choose the very first video mode
				Common.RetCodes res = (Common.RetCodes)TangoVideoModeAPI.VideoModeGetVideoMode (0, videoModePtr);
				if (res == Common.RetCodes.kCAPISuccess)
				{
					VideoModeAttributes supportedVideoMode = (VideoModeAttributes)Marshal.PtrToStructure (videoModePtr, typeof(VideoModeAttributes));
					videoMode.frameRate = supportedVideoMode.frameRate;
					videoMode.width = supportedVideoMode.width;
					videoMode.height = supportedVideoMode.height;
					videoModeID = 0;
				}else{
					DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR, "Failed to get video mode with error:"+res);
				}
			}else{
				DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR, "Number of video modes:"+numModes);
			}
			Marshal.FreeHGlobal (videoModePtr);
			return videoModeID;
		}
		
		public static Vector2 visibleRect(VideoMode.VideoModeAttributes userVideoMode, Vector2 screenResolution)
		{
			float ha = (float)userVideoMode.width/screenResolution.x;
			float va = (float)userVideoMode.height/screenResolution.y;
			float windowAspect =  screenResolution.x/screenResolution.y;
			
			Vector2 resolution = new Vector2();
			if(ha>va)
			{
				resolution.x = ((float)userVideoMode.height*windowAspect);
				resolution.y = (float)userVideoMode.height;
			}
			else
			{
				resolution.x = (float)userVideoMode.width;
				resolution.y = ((float)userVideoMode.width/windowAspect);
			}
			return resolution;
		}	
		#endregion
		
		#region NATIVE_FUNCTIONS
		struct TangoVideoModeAPI
		{
			#if (UNITY_EDITOR)
			public static int VideoModeNumberVideoModes(){
				return 0;
			}
			
			public static int VideoModeGetVideoMode(int videoModeID, IntPtr videoMode){
				return (int)Common.RetCodes.kCAPISuccess;
			}
			#elif (UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_ANDROID)
			[DllImport(Common.TangoUnityDLL)]
			public static extern int VideoModeNumberVideoModes();
			
			[DllImport(Common.TangoUnityDLL)]
			public static extern int VideoModeGetVideoMode(int videoModeID, IntPtr videoMode);
			#else
			#error platform is not supported
			#endif
		}
		#endregion // NATIVE_FUNCTIONS
	}
}

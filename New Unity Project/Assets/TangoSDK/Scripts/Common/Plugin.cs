// Copyright 2013 Motorola Mobility LLC. Part of the Trailmix project.
// CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
using UnityEngine;
using System.Collections;

namespace Tango
{
	
	public class Plugin
	{
		public const int eventsStartPoint = 0xfe;
		public enum PluginEvents
		{
			InitBackgroundEvent = eventsStartPoint,
			RenderFrameEvent,
			ReleaseBackgroundEvent
		}
		
		public static void IssuePluginEvent (PluginEvents eventid)
		{
			#if (UNITY_EDITOR || UNITY_STANDALONE_OSX)
			GL.IssuePluginEvent((int) eventid);
			#endif		
		}

		public static void LoadSharedLibrary(string libName)
		{
			AndroidJavaClass systemClass = new AndroidJavaClass("java.lang.System"); 
			systemClass.CallStatic("loadLibrary", libName);
		}

	}
}



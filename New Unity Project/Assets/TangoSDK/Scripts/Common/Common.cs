// Copyright 2013 Motorola Mobility LLC. Part of the Tango project.
// CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tango
{
	public struct Common
	{	
#region NESTED
		public enum RetCodes
		{
			kCAPISuccess = 0,
			kCAPIFail = -1,
			kCAPIAlreadyInstasiated = 100,
			kCAPINotInitialized = 101,
			kCAPIDataNotAvailable = 102,
			kCAPIWrongInputData = 103,
			kCAPIOperationFailed = 104
		}
#endregion // NESTED


#region PUBLIC_MEMBER_VARIABLES

#if (UNITY_EDITOR || UNITY_STANDALONE_OSX)
public static bool Mirroring = true; 
#elif (UNITY_IPHONE || UNITY_ANDROID) 
public static bool Mirroring = false; 
#else 
		public static bool Mirroring = false;
#endif

		public const string TangoUnityDLL = "tango_api";

#endregion // PUBLIC_MEMBER_VARIABLES


#region PUBLIC_FUNCTIONS
		public static Quaternion worldRotation {
			get {
				return OrientationManager.worldRotation;
			}
		} 

		// current window resoltion where width everytime bigger than height
		public static Vector2 windowResoltion {
			get {
				Vector2 screenSize;
				if (Screen.width > Screen.height)
					screenSize = new Vector2 (Screen.width, Screen.height);
				else
					screenSize = new Vector2 (Screen.height, Screen.width);
				return screenSize;
			}
		}

		public static float windowResoltionAspect {
			get {
				Vector2 resolution = windowResoltion;
				return resolution.x / resolution.y;
			}
		}

		public static void quit ()
		{	
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit ();
#endif
		}

#endregion // PUBLIC_FUNCTIONS
	}
}
// Copyright 2013 Motorola Mobility LLC. Part of the Trailmix project.
// CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
using UnityEngine;
using System.Collections;

namespace Tango
{
	public class OrientationManager
	{
		#region PUBLIC_METHODS
		public static ScreenOrientation screenOrientation {
			get {
				#if (UNITY_EDITOR || UNITY_STANDALONE_OSX)
				if(Screen.width > Screen.height)
					return ScreenOrientation.LandscapeLeft;
				else
					return ScreenOrientation.Portrait;
				#elif (UNITY_IPHONE || UNITY_ANDROID)
				return Screen.orientation; 
				#else 
				#error not supported platform
				#endif
			}		
		}
		
		public static Quaternion worldRotation {
			get {
				ScreenOrientation orientation = screenOrientation;
				Quaternion transformation = Quaternion.identity;
				if (orientation == ScreenOrientation.LandscapeLeft) {
					transformation = Quaternion.identity;
				} else if (orientation == ScreenOrientation.LandscapeRight) {
					transformation = Quaternion.AngleAxis (180f, Vector3.forward);
				} else if (orientation == ScreenOrientation.PortraitUpsideDown) {
					transformation = Quaternion.AngleAxis (90f, Vector3.forward);
				} else if (orientation == ScreenOrientation.Portrait) {
					transformation = Quaternion.AngleAxis (-90f, Vector3.forward);
				}
				return transformation;
			}
		}
		#endregion // PUBLIC_METHODS
		
	}
}
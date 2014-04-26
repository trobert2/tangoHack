using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Tango
{
	public class HardwareParameters
	{
		static public float verticalFieldOfView(int visibleHeight) {
			//our calibration file has calibration for resolution 640x480 but not 1280x720
			//so lets hardcode fov for now
			return 39;
			//return HardwareParametersAPI.HardwareParametersCameraVerticalFieldOfView(visibleHeight);
		}
		
		static public float horizontalFieldOfView(int visibleWidth) {
			return HardwareParametersAPI.HardwareParametersCameraHorizontalFieldOfView(visibleWidth);
		}
		
		#region NATIVE_FUNCTIONS
		private struct HardwareParametersAPI
		{
			#if (UNITY_EDITOR)
			public static float HardwareParametersCameraHorizontalFieldOfView(int visibleWidth){
				return 0.0f;
			}
			
			public static float HardwareParametersCameraVerticalFieldOfView(int visibleHeight){
				return 0.0f;
			}
			#elif (UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_ANDROID)
			[DllImport(Common.TangoUnityDLL)]
			public static extern float HardwareParametersCameraHorizontalFieldOfView(int visibleWidth);
			
			[DllImport(Common.TangoUnityDLL)]
			public static extern float HardwareParametersCameraVerticalFieldOfView(int visibleHeight);
			#else
			#error platform is not supported
			#endif
		}
		#endregion // NATIVE_FUNCTIONS
	}
	
}
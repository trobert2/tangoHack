using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Tango
{
	public class Synchronizer : MonoBehaviour {
		#region PRIVATE_MEMBERS	
		private bool mValid = false;
		private Resolution mDepthBufferResolution;
		private System.IntPtr mApplicationHandler = System.IntPtr.Zero;
		#endregion // PRIVATE_MEMBERS
		
		#region MONOBEHAVIOR
		void Awake ()
		{
			TangoUtilAPI.UtilSetDepthNoiseLevel (2);
			TangoUtilAPI.UtilSetDepthConfidenceLevel (2);
			mApplicationHandler = TangoApplicationAPI.ApplicationInitialize("[Superframes Small-Peanut]");
			if (mApplicationHandler == System.IntPtr.Zero) {
				ErrorHandler.instance.presentErrorMessage("Application initialization failed");	
			}
			
			//let's hardcode depth image reoslution
			//but eventially we can get resolution using VideoMode interface 
			mDepthBufferResolution = new Resolution();
			mDepthBufferResolution.width = 320;
			mDepthBufferResolution.height = 180;
			mDepthBufferResolution.refreshRate = 5;
		}
		
		void LateUpdate ()
		{
			//load next frame
			if (mApplicationHandler != System.IntPtr.Zero) {
				Common.RetCodes retCode = (Common.RetCodes)TangoApplicationAPI.ApplicationDoStep(mApplicationHandler);
				mValid = (retCode == Common.RetCodes.kCAPISuccess);
//				if(retCode != Common.RetCodes.kCAPISuccess)
//					ErrorHandler.instance.presentErrorMessage("Application step failed with error code:" + retCode);
//				else
//					Debug.Log("Do step succeed");
			}
		}
		
		void OnDestroy ()
		{
//			if (mApplicationHandler != System.IntPtr.Zero)
//				TangoApplicationAPI.ApplicationShutdown (mApplicationHandler);
		}
		#endregion // MONOBEHAVIOR

		#region PUBLIC_METHODS
		public bool getDepthBuffer(ref int [] buffer, ref double timestamp)
		{
			if(buffer.Length < getDepthBufferSize()){
				DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_WARN, "Wrong depth buffer size");
				return false;
			}
			
			GCHandle bufferHandler = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			Common.RetCodes retCode = (Common.RetCodes)TangoUtilAPI.UtilGetDepthFrame(mApplicationHandler, bufferHandler.AddrOfPinnedObject(),
			                                                                          mDepthBufferResolution.width*mDepthBufferResolution.height,
			                                                                          ref timestamp);
			if(retCode != Common.RetCodes.kCAPISuccess)
				DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_WARN, "Failed to get depth buffer with error:"+retCode);
			bufferHandler.Free();
			return retCode == Common.RetCodes.kCAPISuccess;
		}
		
		public int getDepthBufferSize()
		{
			//let's hardcode depth image reoslution
			//but eventially we can up
			return depthBufferResolution.width*depthBufferResolution.height;
			
		}

		public int getDepthNoiseLevel()
		{
			return TangoUtilAPI.UtilGetDepthNoiseLevel ();
		}

		public int getDepthConfidenceLevel()
		{
			return TangoUtilAPI.UtilGetDepthConfidenceLevel ();
		}
		#endregion // PUBLIC_METHODS
		
		#region PUBLIC_PROPS
		public System.IntPtr handler
		{
			get{
				return mApplicationHandler;
			}
		}

		public bool initialized {
			get {
				return mApplicationHandler != System.IntPtr.Zero;
			}
		}
		
		public bool valid{
			get{
				return mValid;
			}
		} 
		
		Resolution depthBufferResolution
		{
			get{
				return mDepthBufferResolution;
			}
		}
		#endregion // PUBLIC_PROPS
		
		
		#region NATIVE_FUNCTIONS
		struct TangoApplicationAPI
		{
			#if (UNITY_EDITOR)
			public static System.IntPtr ApplicationInitialize([MarshalAs(UnmanagedType.LPStr)] string dataSource){
				return System.IntPtr.Zero;
			}
			
			public static int ApplicationDoStep(System.IntPtr applicationHandler){
				return (int) Tango.Common.RetCodes.kCAPISuccess;
			}
			
			public static int ApplicationShutdown(System.IntPtr applicationHandler){
				return (int) Tango.Common.RetCodes.kCAPISuccess;
			}
			#elif (UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_ANDROID)
			[DllImport(Common.TangoUnityDLL)]
			public static extern System.IntPtr ApplicationInitialize([MarshalAs(UnmanagedType.LPStr)] string dataSource);	
			
			[DllImport(Common.TangoUnityDLL)]
			public static extern int ApplicationDoStep(System.IntPtr applicationHandler);	
			
			[DllImport(Common.TangoUnityDLL)]
			public static extern int ApplicationShutdown(System.IntPtr applicationHandler);	
			#else
			#error platform is not supported
			#endif
		}

		struct TangoUtilAPI
		{
			#if (UNITY_EDITOR)
			public static int UtilGetDepthFrame(System.IntPtr applicationHandler, System.IntPtr depthBuffer, int bufferSize, [In, Out] ref double timestamp){
				return (int) Tango.Common.RetCodes.kCAPISuccess;
			}

			public static void UtilSetDepthConfidenceLevel(int confidence_level)
			{
				Debug.Log ("unable to set depth confidence level in Editor mode");
			}

			public static void UtilSetDepthNoiseLevel(int noise_level)
			{
				Debug.Log ("unable to set depth noise level in Editor mode");
			}

			public static int UtilGetDepthConfidenceLevel()
			{
				Debug.Log ("unable to get depth confidence level in Editor mode");
				return -1;
			}

			public static int UtilGetDepthNoiseLevel()
			{
				Debug.Log ("unable to get depth noise level in Editor mode");
				return -1;
			}

			#elif (UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_ANDROID)
			[DllImport(Common.TangoUnityDLL)]
			public static extern int UtilGetDepthFrame(System.IntPtr applicationHandler, System.IntPtr depthBuffer, int bufferSize, [In, Out] ref double timestamp);

			[DllImport(Common.TangoUnityDLL)]
			public static extern void UtilSetDepthConfidenceLevel(int confidence_level);
			
			[DllImport(Common.TangoUnityDLL)]
			public static extern void UtilSetDepthNoiseLevel(int noise_level);
			
			[DllImport(Common.TangoUnityDLL)]
			public static extern int UtilGetDepthConfidenceLevel();
			
			[DllImport(Common.TangoUnityDLL)]
			public static extern int UtilGetDepthNoiseLevel();

			#else
			#error platform is not supported
			#endif
		}
		#endregion // NATIVE_FUNCTIONS
	}
}

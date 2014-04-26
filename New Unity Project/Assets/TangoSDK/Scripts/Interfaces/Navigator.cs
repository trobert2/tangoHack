// Copyright 2013 Motorola Mobility LLC. Part of the Trailmix project.
// CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Tango
{
	[RequireComponent(typeof(Synchronizer))]
	public class Navigator : MonoBehaviour
	{
		#region NESTED
		public enum Mode
		{
			VisualIntertialNavigaition = 0,
			VisualIntertialNavigaitionAndMapping
		}
		#endregion

		#region PRIVATE_MEMBERS	
		//private VideoOverlay mVideoOverlay;

		//Keep it private but serializable to allow changes in Editor 
		//but not runtime
		[SerializeField]
				private Mode operationMode = Mode.VisualIntertialNavigaitionAndMapping;
		[SerializeField]
		//"/data/data/com.motorola.atap.tangomapper/files/sparse_map";
		private string sparseMapPath = "";
		private System.IntPtr mVIOHandler = System.IntPtr.Zero;
		#endregion // PRIVATE_MEMBERS    
		
		#region PUBLIC_PROPS
		public bool initialized {
			get {
				return mVIOHandler != System.IntPtr.Zero;
			}
		}
        public void SetPath(string path){
			sparseMapPath = path;
		}
		#endregion // PUBLIC_PROPS
		
		#region PRIVATE_METHODS
		#endregion
		
		#region PUBLIC_METHODS
        public bool SaveSparseMap(string sparseMapPath)
		{
            return (Common.RetCodes)TangoVIOAPI.VIOSaveSparseMap(mVIOHandler, sparseMapPath) == Common.RetCodes.kCAPISuccess;
		}
            
		#endregion

		#region MONOBEHAVIOR
		void Start ()
		{
			#if (UNITY_EDITOR)
			DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO, typeof(Camera).Name + " : Adding input controls to run in editor.");
			gameObject.AddComponent<TangoFlyCam>();
			#elif (UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_ANDROID)
			#else
			#error platform is not supported
			#endif

			Synchronizer sync = gameObject.GetComponent<Synchronizer>();
			int  withSparseMapping = (operationMode == Mode.VisualIntertialNavigaitionAndMapping)?1:0;
			mVIOHandler = TangoVIOAPI.VIOInitialize(sync.handler, withSparseMapping, sparseMapPath);
			if (mVIOHandler == System.IntPtr.Zero) {
				ErrorHandler.instance.presentErrorMessage("VIO initialization failed");	
			}
		}
		
		void Update ()
		{
			Quaternion rotation = Quaternion.identity;
			Vector3 position = Vector3.zero;

            #if (UNITY_EDITOR)
                DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR, "Navigator.Update() : Updating TangoFlyCam");
                gameObject.GetComponent<TangoFlyCam>().GetRawTransformData(ref position, ref rotation);
			#elif (UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_ANDROID)
                if (mVIOHandler == System.IntPtr.Zero)
                {
                    DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR, "Navigator.Update() : VIO Handler is not initialized");
                    return;
                }

			    float [] unityFormatRotation = new float [4];
			    float [] unityFormatPosition = new float [3];
			    if((Common.RetCodes)TangoVIOAPI.VIOGetCameraPoseUnity (mVIOHandler, unityFormatRotation, unityFormatPosition) == Common.RetCodes.kCAPISuccess)
			    {
				    //we don't have IMU to narrow field of view camera calibration, so lets + 10cm to camera position to compencate that
				    //until we have calibrarion.
				    position = new Vector3 ((float)unityFormatPosition [0], (float)unityFormatPosition [1], (float)unityFormatPosition [2]);
				    //position = new Vector3(0f, 0f, 0f) ;
				    rotation = new Quaternion ((float)unityFormatRotation [0], (float)unityFormatRotation [1], (float)unityFormatRotation [2], (float)unityFormatRotation [3]);
			    }else{
            //DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_INFO, typeof(Camera).Name + " :Pose is not available");
			    }	
            #else
            #error platform is not supported
            #endif


                gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
		}

		void OnDestroy ()
		{
//			if (mVIOHandler != System.IntPtr.Zero)
//				TangoVIOAPI.VIOShutdown(mVIOHandler);
		}
		
		#endregion // MONOBEHAVIOR
		
		
		#region NATIVE_FUNCTIONS
		struct TangoVIOAPI
		{
			#if (UNITY_EDITOR)
            public static System.IntPtr  VIOInitialize(System.IntPtr applicationHandler, int withParseMappping, [MarshalAs(UnmanagedType.LPStr)] string mapPath)
			{return System.IntPtr.Zero;}
			
			public static int VIOShutdown(System.IntPtr vioHandler)
			{return (int) Tango.Common.RetCodes.kCAPISuccess;}

			public static int VIOGetCameraPose(System.IntPtr vioHandler, 
			                                   [MarshalAs(UnmanagedType.LPArray)] float[] estimatorFormatRotation, 
			                                   [MarshalAs(UnmanagedType.LPArray)] float[] estimatorFormatTranslation)
			{return (int) Tango.Common.RetCodes.kCAPISuccess;}


			public static int VIOGetCameraPoseUnity(System.IntPtr vioHandler, 
			                                   [MarshalAs(UnmanagedType.LPArray)] float[] unityFormatRotation, 
			                                   [MarshalAs(UnmanagedType.LPArray)] float[] unityFormatTranslation)
			{return (int) Tango.Common.RetCodes.kCAPISuccess;}

			public static int VIOConvertPoseToUnityFormat([MarshalAs(UnmanagedType.LPArray)] float[] estimatorFormatRotation, 
			                                               [MarshalAs(UnmanagedType.LPArray)] float[] estimatorFormatTranslation,
			                                                      [MarshalAs(UnmanagedType.LPArray)] float[] unityFormatRotation,
			                                                      [MarshalAs(UnmanagedType.LPArray)] float[] unityFormatTranslation)
			{return (int) Tango.Common.RetCodes.kCAPISuccess;}

            public static int VIOSaveSparseMap(System.IntPtr application_handler, [MarshalAs(UnmanagedType.LPStr)] string mapPath)
			{return (int) Tango.Common.RetCodes.kCAPISuccess;}

			#elif (UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_ANDROID)
			[DllImport(Common.TangoUnityDLL)]
            public static extern System.IntPtr VIOInitialize(System.IntPtr application_handler, int withParseMappping, [MarshalAs(UnmanagedType.LPStr)] string mapPath);	

			[DllImport(Common.TangoUnityDLL)]
			public static extern int VIOShutdown(System.IntPtr vioHandler);	

			[DllImport(Common.TangoUnityDLL)]
			public static extern int VIOGetCameraPose(System.IntPtr vioHandler,
			                                          [MarshalAs(UnmanagedType.LPArray)] float[] estimatorFormatRotation, 
			                                          [MarshalAs(UnmanagedType.LPArray)] float[] estimatorFormatTranslation);

			[DllImport(Common.TangoUnityDLL)]
			public static extern int VIOGetCameraPoseUnity(System.IntPtr vioHandler,
			                                          [MarshalAs(UnmanagedType.LPArray)] float[] unityFormatRotation, 
			                                          [MarshalAs(UnmanagedType.LPArray)] float[] untiyFormatTranslation);

			[DllImport(Common.TangoUnityDLL)]
			public static extern int VIOConvertPoseToUnityFormat([MarshalAs(UnmanagedType.LPArray)] float[] estimatorFormatRotation, 
			                                                      [MarshalAs(UnmanagedType.LPArray)] float[] estimatorFormatTranslation,
			                                                      [MarshalAs(UnmanagedType.LPArray)] float[] unityFormatRotation,
			                                                      [MarshalAs(UnmanagedType.LPArray)] float[] unityFormatTranslation);

            [DllImport(Common.TangoUnityDLL)]
            public static extern int VIOSaveSparseMap(System.IntPtr application_handler, [MarshalAs(UnmanagedType.LPStr)] string mapPath);	

			#else
			#error platform is not supported
			#endif
		}
		#endregion // NATIVE_FUNCTIONS
	}
}
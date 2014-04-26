//-----------------------------------------------------------------------
// <copyright file="VideoOverlayRenderer.cs" company="Google">
//
// Copyright 2014 Google. Part of the Tango project. CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
//
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Tango
{
	/// <summary>
	/// Render video overlay.
	/// </summary>
	[RequireComponent(typeof(Synchronizer))]
	public class VideoOverlayRenderer : MonoBehaviour
	{	
		#region NESTED
		/// <summary>
		/// Rendering quality mode
		/// Defined background screen texture resolution.
		/// </summary>
		public enum Mode
		{
			QualityOptimized,
			PerformanceAndQuality,
			PerformanceOptimized,
		}
		#endregion
		
		#region PUBLIC_MEMBERS
		// Background screen object.
		// Has to be initialized statically in Unity Inspector.
		public GameObject screen;
		
		// Rendering quality mode.
		public Mode mode = Mode.PerformanceAndQuality;

		public bool is_runningBackgroundRenderLoop = true;
		#endregion
		
		#region PRIVATE_MEMBERS

		private System.IntPtr m_videoOverlayRendererHandler = System.IntPtr.Zero;
		
		// Instance of the texture that will used for a hw camera frames presentation.
		private Texture2D m_backgroundTexture;

		private int m_videoModeID;
		private VideoMode.VideoModeAttributes m_videoMode = new VideoMode.VideoModeAttributes();
		#endregion
		
		#region PUBLIC_PROPS
		/// <summary>
		/// Background texture accessor
		/// can be used for cool visual effects.
		/// </summary>
		/// <value>Texture.</value>
		public Texture2D frameTexture 
		{
			get 
			{
				return m_backgroundTexture;
			}
		}

		/// <summary>
		/// Video mode aspect ratio.
		/// </summary>
		/// <value>float.</value>
		public float videoModeAspect 
		{
			get 
			{
				return (float)m_videoMode.width / (float)m_videoMode.height;
			}
		}

		/// <summary>
		/// Current Video Mode.
		/// </summary>
		/// <value>Video mode.</value>
		public VideoMode.VideoModeAttributes videoMode 
		{
			get 
			{
				return m_videoMode;
			}
		}
		#endregion

		#region PUBLIC_METHODS
		/// <summary>
		/// Object major initializtion step.
		/// Build screen texture and initialize screen object.
		/// </summary>
		/// <param name="videoMode">Video mode.</param>
		/// <param name="verticalFieldOfView">Vertical FOV.</param>
		public void Init(VideoMode.VideoModeAttributes videoMode, float verticalFieldOfView)
		{
			InitTexture(mode, videoMode, ref m_backgroundTexture);
			InitScreen(videoMode, m_backgroundTexture);
			UpdateScreenTransformation(videoMode, verticalFieldOfView);
		}

		/// <summary>
		/// Render the background texture when called.
		/// </summary>
		/// <returns>Is background updated.</returns>
		public bool RenderBackgoundTextureOnDemand()
		{
			// if Background is not initialized we are not rendering
			if (m_videoOverlayRendererHandler == System.IntPtr.Zero) 
			{
				return false;
			}
			
			// render next frame to texture
			Tango.Common.RetCodes ret = 
				(Tango.Common.RetCodes)TangoVideoOverlayAPI.VideoOverlayRenderFrame(m_videoOverlayRendererHandler, 
				                                                                    m_backgroundTexture.GetNativeTextureID(), 
				                                                                    m_backgroundTexture.width, 
				                                                                    m_backgroundTexture.height);
			
			if (ret != Tango.Common.RetCodes.kCAPISuccess) 
			{
				DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR, 
				                       "Failed to render video overlay texture with error code:" + ret);
				return false;
			}
			#if (UNITY_EDITOR || UNITY_STANDALONE_OSX)
			//issue plugin event to render right after rendering thread finished rendering
			Plugin.IssuePluginEvent(Plugin.PluginEvents.RenderFrameEvent);
			#elif (UNITY_IPHONE || UNITY_ANDROID)
			GL.InvalidateState();
			#else
			#error platform is not supported
			#endif
			return true;
		}
		#endregion
		
		#region PRIVATE_METHODS
		/// <summary>
		/// Update camera FOV.
		/// </summary>
		/// <param name="deviceHorizontalFieldOfView">Device Horizontal FOV.</param>
		/// <param name="deviceVerticalFieldOfView">Device Vertical FOV.</param>
		private void UpdateCameraFieldOfView(ref float deviceHorizontalFieldOfView, ref float deviceVerticalFieldOfView)
		{
			Vector2 visibleRect = VideoMode.visibleRect(videoMode, new Vector2(Common.windowResoltion.x, Common.windowResoltion.y));
			deviceHorizontalFieldOfView = HardwareParameters.horizontalFieldOfView((int)visibleRect.x);
			deviceVerticalFieldOfView = HardwareParameters.verticalFieldOfView((int)visibleRect.y);

			if (deviceHorizontalFieldOfView < 0.0f 
			    || deviceVerticalFieldOfView < 0.0f) 
			{
				ErrorHandler.instance.presentErrorMessage("Camera intrinsic parameters are not available");	
			}		
		}

		/// <summary>
		/// Set camera FOV.
		/// </summary>
		private void SetCameraFieldOfView()
		{
			float deviceHorizontalFieldOfView = 0;
			float deviceVerticalFieldOfView = 0;
			
			UpdateCameraFieldOfView(ref deviceHorizontalFieldOfView, ref deviceVerticalFieldOfView);
			ScreenOrientation orientation = OrientationManager.screenOrientation;
			
			float cameraFOV = 1.0f;
			if (orientation == ScreenOrientation.LandscapeLeft
			    || orientation == ScreenOrientation.LandscapeRight) 
			{
				cameraFOV = deviceVerticalFieldOfView;
			} 
			else if (orientation == ScreenOrientation.PortraitUpsideDown
			           || orientation == ScreenOrientation.Portrait) 
			{
				cameraFOV = deviceHorizontalFieldOfView;
			}
			camera.fieldOfView = cameraFOV;
		}

		/// <summary>
		/// Init Texture2D.
		/// </summary>
		/// <param name="qualityMode">Video quality.</param>
		/// <param name="videoMode">Video mode.</param>
		/// <param name="backgroundTexture">Reference to Texture2D.</param>
		private void InitTexture(Mode qualityMode, VideoMode.VideoModeAttributes videoMode, ref Texture2D backgroundTexture)
		{
			int potWidth, potHeight;
			TextureFormat textureFormat;
			if (qualityMode == Mode.QualityOptimized) 
			{
				potWidth = Mathf.NextPowerOfTwo(videoMode.width);
				potHeight = Mathf.NextPowerOfTwo(videoMode.height);
				textureFormat = TextureFormat.ARGB32;
			} 
			else if (qualityMode == Mode.PerformanceAndQuality) 
			{
				potWidth = Mathf.ClosestPowerOfTwo(videoMode.width);
				potHeight = Mathf.ClosestPowerOfTwo(videoMode.height);
				textureFormat = TextureFormat.RGB565;
			} 
			else 
			{
				potWidth = 512;
				potHeight = 512;
				textureFormat = TextureFormat.RGB565;
			}
			
			backgroundTexture = new Texture2D(potWidth, potHeight, textureFormat, false);
			backgroundTexture.filterMode = FilterMode.Bilinear;
			backgroundTexture.wrapMode = TextureWrapMode.Clamp;
		}

		/// <summary>
		/// Init screen, set screen texture.
		/// </summary>
		/// <param name="videoMode">Video mode.</param>
		/// <param name="backgroundTexture">Screen background texture.</param>
		private void InitScreen(VideoMode.VideoModeAttributes videoMode, Texture2D backgroundTexture)
		{
			if (screen == null) 
			{
				ErrorHandler.instance.presentErrorMessage("Camera component does not contain NCamera object of Screen is not assigned");	
			}
			screen.renderer.material.mainTexture = backgroundTexture;
			StartCoroutine("InitBackground");		
		}

		/// <summary>
		/// Update the screen Transformation.
		/// </summary>
		/// <param name="videoMode">Video mode.</param>
		/// <param name="verticalFieldOfView">Vertical FOV.</param>
		private void UpdateScreenTransformation(VideoMode.VideoModeAttributes videoMode, float verticalFieldOfView)
		{
			float fov = verticalFieldOfView;
			float distanceToScreen = gameObject.camera.farClipPlane;
			float frameAspect = (float)videoMode.width / (float)videoMode.height;
			
			float scaleFactor = 1.0f;
			
			// Unity's Plane mesh default scale is 5
			float screenSize = 0.2f 
				* distanceToScreen
					* Mathf.Tan(0.5f * fov * Mathf.Deg2Rad) 
					* scaleFactor;
			
			float screenWidth = screenSize * frameAspect;
			float screenHeight = screenSize;
			
			Vector3 screenScale = new Vector3((Common.Mirroring ? 1 : -1) * screenWidth, 1f, screenHeight);
			screen.transform.localScale = screenScale;
			
			// We need this transformation dew to the way unity handles phone orientation
			screen.transform.localRotation = Common.worldRotation * Quaternion.AngleAxis(90f, Vector3.left);

			// distanceToScreen * 0.99f to make sure we are closer than camera far clipframe
			screen.transform.localPosition = new Vector3(0f, 0f, distanceToScreen * 0.99f);
		}
		#endregion

		#region MONOBEHAVIOR
		/// <summary>
		/// Unity Awake function.
		/// </summary>
		private void Awake()
		{
			m_videoModeID = VideoMode.lookupVideoMode(ref m_videoMode);			
			if (m_videoModeID == -1) 
			{
				ErrorHandler.instance.presentErrorMessage("Video mode is not available");	
			}			
			SetCameraFieldOfView();
		}

		/// <summary>
		/// Unity Start function.
		/// </summary>
		private void Start()
		{
			Init(videoMode, camera.fieldOfView);
		}

		/// <summary>
		/// Unity on object enable function.
		/// </summary>
		private void OnEnable()
		{
			// show screen.
			screen.SetActive(true);
			if (is_runningBackgroundRenderLoop)
			{
				// start rendering in Coroutine.
				StartCoroutine("RenderBackgoundTexture");
			}
		}

		/// <summary>
		/// Unity on object disable function.
		/// </summary>
		private void OnDisable()
		{
			if (is_runningBackgroundRenderLoop) 
			{
				// stop rendering Coroutine.
				StopCoroutine("RenderBackgoundTexture");
			}

			// hide screen
			if (screen != null) 
			{
				screen.SetActive(false);
			}
		}

		/// <summary>
		/// Init the background texture at end of the frame.
		/// </summary>
		/// <returns>Yield return.</returns>
		private IEnumerator InitBackground()
		{
			// wait for end of frame to not interfere with rendering thread on mac
			yield return new WaitForEndOfFrame();
			Synchronizer sync = gameObject.GetComponent<Synchronizer>();
			m_videoOverlayRendererHandler = TangoVideoOverlayAPI.VideoOverlayInitialize(sync.handler);
			if (m_videoOverlayRendererHandler == System.IntPtr.Zero) 
			{
				DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_WARN, "Failed to initialize VideoOverlay");
			}
			#if (UNITY_EDITOR || UNITY_STANDALONE_OSX)
			//issue plugin event to render right after rendering thread finished rendering
			Plugin.IssuePluginEvent(Plugin.PluginEvents.InitBackgroundEvent);
			#elif (UNITY_IPHONE || UNITY_ANDROID)
			GL.InvalidateState();
			#else
			#error platform is not supported
			#endif
		}

		/// <summary>
		/// Render the background texture at the end of frame.
		/// </summary>
		/// <returns>Yield return.</returns>
		private IEnumerator RenderBackgoundTexture()
		{
			while (is_runningBackgroundRenderLoop) 
			{
				// if Background is not initialized we are not rendering
				if (m_videoOverlayRendererHandler == System.IntPtr.Zero)
				{
					yield return null;
				}

				// wait for end of frame to not interfere with rendering thread on mac
				yield return new WaitForEndOfFrame();
				
				// render next frame to texture
				Tango.Common.RetCodes ret = (Tango.Common.RetCodes)TangoVideoOverlayAPI.VideoOverlayRenderFrame(m_videoOverlayRendererHandler, 
				                                                                         m_backgroundTexture.GetNativeTextureID(), 
				                                                                         m_backgroundTexture.width, 
				                                                                         m_backgroundTexture.height);

				if (ret != Tango.Common.RetCodes.kCAPISuccess)
				{
					DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR, 
					                       "Failed to render video overlay texture with error code:" + ret);
				}

				#if (UNITY_EDITOR || UNITY_STANDALONE_OSX)
				// issue plugin event to render right after rendering thread finished rendering
				Plugin.IssuePluginEvent(Plugin.PluginEvents.RenderFrameEvent);
				#elif (UNITY_IPHONE || UNITY_ANDROID)
				GL.InvalidateState();
				#else
				#error platform is not supported
				#endif
				// yield now to let class stop this coroutine if needed
				yield return null;	
			}
		}
		
		#endregion
		
		#region NATIVE_FUNCTIONS
		/// <summary>
		/// Video overlay native function import.
		/// </summary>
		public struct TangoVideoOverlayAPI
		{
			#if (UNITY_EDITOR)
			public static System.IntPtr VideoOverlayInitialize(System.IntPtr applicationHandler){
				return System.IntPtr.Zero;
			}
			
			public static int VideoOverlayRenderFrame(System.IntPtr videoOverlayHandler, int textureID, int width, int height){
				return (int) Tango.Common.RetCodes.kCAPISuccess;
			}
			
			public static int VideoOverlayShutdown(System.IntPtr videoOverlayHandler){
				return (int) Tango.Common.RetCodes.kCAPISuccess;
			}
			#elif (UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_ANDROID)
			[DllImport(Common.TangoUnityDLL)]
			public static extern System.IntPtr VideoOverlayInitialize(System.IntPtr applicationHandler);
			
			[DllImport(Common.TangoUnityDLL)]
			public static extern int VideoOverlayRenderFrame(System.IntPtr videoOverlayHandler, int textureID, int width, int height);
			
			[DllImport(Common.TangoUnityDLL)]
			public static extern int VideoOverlayShutdown(System.IntPtr videoOverlayHandler);
			#else

			#error platform is not supported
			#endif
			#endregion
		}
	}
}

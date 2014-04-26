using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Tango{
	public class Version {
		
		private Version(){}
		~Version(){}
		
		public static string versionString{
			get{
				IntPtr buf = IntPtr.Zero;
				string versionString = "";
				try {
					const int bufferSize = 1024;
					buf = Marshal.AllocHGlobal (bufferSize);
					VersionAPI.VersionGetAPIVersionString (buf, bufferSize);
					versionString = Marshal.PtrToStringAnsi (buf);
				} catch (Exception ex) {
					Console.WriteLine (ex);
				} finally {
					if (buf != IntPtr.Zero)
						Marshal.FreeHGlobal (buf);
				}
				return versionString;
			}
		}
		
		public static void printVersionString(){
			Debug.Log(Version.versionString);
		}

		struct VersionAPI
		{
			#region NATIVE_FUNCTIONS
			#if (UNITY_EDITOR)
			public static void VersionGetAPIVersionString(IntPtr versionBuffer, int bufferSize){}
			#elif (UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_ANDROID)
			[DllImport(Common.TangoUnityDLL)]
			public static extern void VersionGetAPIVersionString(IntPtr versionBuffer, int bufferSize);
			#else
			#error platform is not supported
			#endif			
			#endregion // NATIVE_FUNCTIONS

		}
	}
}
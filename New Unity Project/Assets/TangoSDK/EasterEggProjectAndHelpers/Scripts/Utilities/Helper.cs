using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class Helper {

	static public bool usingCLevelCode = false;

	public static void Filter(float[] sourceArr, float[] destArr, int level)
	{
		GCHandle sourceHandler = GCHandle.Alloc(sourceArr, GCHandleType.Pinned);
		GCHandle destHandler  = GCHandle.Alloc(destArr, GCHandleType.Pinned);
		DepthNoiseFilter(sourceHandler.AddrOfPinnedObject(), destHandler.AddrOfPinnedObject(), level);
		destHandler.Free ();
		sourceHandler.Free ();
	} 

	[DllImport("TangoHelpers")]
	public static extern void DepthNoiseFilter(System.IntPtr srouce, System.IntPtr dest, int level);
}

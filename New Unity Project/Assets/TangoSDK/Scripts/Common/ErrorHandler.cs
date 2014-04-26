// Copyright 2013 Motorola Mobility LLC. Part of the Trailmix project.
// CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
using UnityEngine;
using System.Collections;

namespace Tango
{
	public class ErrorHandler : MonoBehaviour
	{
		private static ErrorHandler mErrorHandler;

		public static ErrorHandler instance {
			get {
				if (mErrorHandler == null) {
					GameObject errorHandlerObject = GameObject.Find ("ErrorHandler");
					if(errorHandlerObject == null)
						mErrorHandler = errorHandlerObject.GetComponent<ErrorHandler> ();
				}

				if (mErrorHandler == null) {
					mErrorHandler = new ErrorHandler ();
				}
				return mErrorHandler;
			}
		}

		public virtual void presentErrorMessage (string message)
		{
			DebugLogger.WriteToLog(DebugLogger.EDebugLevel.DEBUG_ERROR, " :Tango error:" + message);
		}
	}
}
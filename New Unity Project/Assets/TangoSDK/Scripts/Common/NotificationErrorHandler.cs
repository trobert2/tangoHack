// Copyright 2013 Motorola Mobility LLC. Part of the Trailmix project.
// CONFIDENTIAL. AUTHORIZED USE ONLY. DO NOT REDISTRIBUTE.
using UnityEngine;
using System.Collections;

namespace Tango
{
	public class NotificationErrorHandler : ErrorHandler
	{
		#region PRIVATE_MEMBER_VARIABLES
		private string mErrorMessage = "";
		private bool mErrorOccurred = false;
		private string mTitle = "Error";
		#endregion // PRIVATE_MEMBER_VARIABLES
		
		
		#region PUBLIC_METHODS
		public override void presentErrorMessage (string message)
		{
			mErrorOccurred = true;
			mErrorMessage = message;
		}
		
		#endregion // PUBLIC_METHODS
		
		
		#region UNTIY_MONOBEHAVIOUR_METHODS
		
		void OnGUI ()
		{
			if (mErrorOccurred)
				GUI.Window (0, new Rect (0, 0, Screen.width, Screen.height), drawError, mTitle);
		}
		
		#endregion // UNTIY_MONOBEHAVIOUR_METHODS
		
		#region PRIVATE_METHODS
		private void drawError (int id)
		{
			GUI.Label (new Rect (10, 25, Screen.width - 20, Screen.height - 95), mErrorMessage);
			
			if (GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height - 60, 150, 50), "Close"))
				Common.quit ();
		}
		
		#endregion // PRIVATE_METHODS
	}
}
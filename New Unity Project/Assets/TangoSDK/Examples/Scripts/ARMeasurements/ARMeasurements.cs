using UnityEngine;
using System.Collections;

public class ARMeasurements : MonoBehaviour
{
	
		public Transform arcamera;
		public Transform lineObject;
		public GUIText cameraPositionText;
		public GUIText distancePosition;
		private GameObject newObject;
		private Vector3 mStartPosition;
		private Vector3 mEndPosition;

		void Start ()
		{
				mStartPosition = new Vector3 (0f, 0f, 0f);
		}

		void Update ()
		{


				Vector3 offset = mEndPosition - mStartPosition;
				Vector3 scale = new Vector3 (0.01f, offset.magnitude / 2.2f, 0.01f);
				Vector3 position = mStartPosition + (offset / 2.0f);

				lineObject.transform.position = position;
				lineObject.transform.up = offset;
				lineObject.transform.localScale = scale;

				float distance = Vector3.Magnitude (mEndPosition - mStartPosition); 
				distancePosition.text = "Distance:" + distance.ToString ("######0.00") + "  meters";

				cameraPositionText.text = "Camera position: [" + arcamera.transform.position.x.ToString ("######0.00") + ","
						+ arcamera.transform.position.y.ToString ("######0.00") + ","
						+ arcamera.transform.position.z.ToString ("######0.00") + "]";
		}
	
		void FixedUpdate ()
		{
				//if start measurement 
				if (Input.touchCount > 0 && 
						Input.GetTouch (0).phase == TouchPhase.Began) {
						mStartPosition = arcamera.position;
						mEndPosition = mStartPosition;
				}

				if (Input.touchCount > 0 && 
						(Input.GetTouch (0).phase == TouchPhase.Stationary
						|| Input.GetTouch (0).phase == TouchPhase.Moved)) {
						mEndPosition = arcamera.position;
				}
		}
}
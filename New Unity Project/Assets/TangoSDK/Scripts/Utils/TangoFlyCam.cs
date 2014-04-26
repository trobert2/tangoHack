using UnityEngine;
using System.Collections;

public class TangoFlyCam : MonoBehaviour
{
	
	/*
	Tango FLYCAM
		Johnny Lee (motorola.com), 15 Dec 2013.
		Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
 
	LICENSE
		Free as in speech, and free as in beer.
 
	FEATURES
		WASD/Arrows:    Movement
		          R:    Climb
		          F:    Drop
                      Shift:    Move faster
                    Control:    Move slower
                        End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
	*/

	public Vector3 position;
	public Quaternion rotation;
	public float mouseLookSensitivity = 120;
	public float normalMoveSpeed = 1;
	public float slowMoveFactor = 0.25f;
	public float fastMoveFactor = 3;
	
	private float rotationX = 0.0f;
	private float rotationY = 0.0f;

	public void Start ()
	{
		Screen.lockCursor = true;
		position.Set (0, 0, 0);
		rotation = Quaternion.identity;
	}
	
	/// <summary>
	/// Get the current position/rotation of the device
	/// </summary>
	/// <param name="position">Position of the device</param>
	/// <param name="rotation">Rotation of the device</param>
	/// <returns></returns>
	public bool GetRawTransformData(ref Vector3 pos, ref Quaternion rot)
	{
		const float CameraHeightInMeters = 1.2192f; //4 feet, a typical starting height for adults
		pos = position + new Vector3(0,CameraHeightInMeters,0);
		rot = rotation;
		return true;
	}

	void Update ()
	{
		rotationX += Input.GetAxis("Mouse X") * mouseLookSensitivity * Time.deltaTime;
		rotationY += Input.GetAxis("Mouse Y") * mouseLookSensitivity * Time.deltaTime;
		rotationY = Mathf.Clamp (rotationY, -90, 90);

		rotation = Quaternion.AngleAxis(rotationX, Vector3.up);
		rotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
		
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
		{

			position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
			position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
		}
		else if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl))
		{
			position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
			position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
		}
		else
		{
			position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
			position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
		}


		if (Input.GetKey (KeyCode.R)) {position += transform.up * normalMoveSpeed * Time.deltaTime;}
		if (Input.GetKey (KeyCode.F)) {position -= transform.up * normalMoveSpeed * Time.deltaTime;}

		if (Input.GetKeyDown (KeyCode.End))
		{
			Screen.lockCursor = (Screen.lockCursor == false) ? true : false;
		}
	}
}
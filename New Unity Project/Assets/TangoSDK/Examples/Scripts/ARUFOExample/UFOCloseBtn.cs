using UnityEngine;
using System.Collections;

public class UFOCloseBtn : TouchableObject {

	public Texture2D touchUpTexture;
	public Texture2D touchDownTexture;
	
	protected override void TouchUp ()
	{
		base.TouchUp ();
		Application.Quit ();
	}
	
	protected override void OnTouch ()
	{
		base.OnTouch ();
		renderer.material.mainTexture = touchDownTexture;
	}
	
	protected override void OutTouch ()
	{
		base.OutTouch ();
		renderer.material.mainTexture = touchUpTexture;
	}
}

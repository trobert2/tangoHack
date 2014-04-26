using UnityEngine;
using System.Collections;

public class AddKnobButton : TouchableObject {
	
	public ARController arExampleController;
	public Texture2D touchUpTexture;
	public Texture2D touchDownTexture;
	
	protected override void TouchUp ()
	{
		base.TouchUp ();
		arExampleController.AddKnob ();
		renderer.material.mainTexture = touchUpTexture;
		if (!Globals.isShowedTouchToAddText)
			Globals.isShowedTouchToAddText = true;
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

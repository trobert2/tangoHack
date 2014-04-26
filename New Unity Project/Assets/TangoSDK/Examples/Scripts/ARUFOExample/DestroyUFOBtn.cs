using UnityEngine;
using System.Collections;

public class PlaceUFOBtn : TouchableObject {
	
	public ARUFOExampleController arUfoExampleController;
	public Texture2D touchUpTexture;
	public Texture2D touchDownTexture;
	
	protected override void TouchUp ()
	{
		base.TouchUp ();
		arUfoExampleController.AddKnob ();
		renderer.material.mainTexture = touchUpTexture;
		if (!UFOGlobals.isShowedTouchToAddText)
			UFOGlobals.isShowedTouchToAddText = true;
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

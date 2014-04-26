using UnityEngine;
using System.Collections;

public class UFOParticleDestory : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 2.5f);
	}
}

using UnityEngine;
using System.Collections;

public class UFOController : MonoBehaviour {

	public float rotateSpeed;

	private float startY;
	private float time;
	// Use this for initialization
	void Start () {
		startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		transform.Rotate (transform.up, rotateSpeed * Time.deltaTime);
		transform.position = new Vector3(transform.position.x, 
		                                 startY + Mathf.PingPong(time/5.0f, 0.3f), 
		                                 transform.position.z);
	}
}

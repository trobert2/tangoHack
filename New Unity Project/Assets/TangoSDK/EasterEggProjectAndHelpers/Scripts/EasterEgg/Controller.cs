using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public float rotateSpeed;
	public int points;
	public ARController arController;

	private float startY;
	private float time;
	private float distance;
	// Use this for initialization
	void Start () {
		startY = transform.position.y;
		points = 0;
		arController.AddEgg ();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		transform.Rotate (transform.up, rotateSpeed * Time.deltaTime);
		transform.position = new Vector3(transform.position.x, 
		                                 startY + Mathf.PingPong(time/5.0f, 0.3f), 
		                                 transform.position.z);
		distance = Vector3.Distance(transform.position, arController.newObject.transform.position);
	}
}

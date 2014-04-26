using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.rigidbody2D.velocity = new Vector2(-5.0f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(this.transform.position.x < -10){
			this.transform.position = new Vector2(10, 0);
		}
	}
}

using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	const float X_MAX = 10.0f * (4.0f/3.0f);

	// Use this for initialization
	void Start()
	{
		this.rigidbody2D.velocity = Game.Instance.ScrollSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if(this.transform.position.x < -X_MAX)
		{
			this.transform.position = new Vector2(X_MAX, 0);
		}
	}
}

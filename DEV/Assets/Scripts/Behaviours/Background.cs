using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	const float X_MAX = 10.0f * (4.0f/3.0f);
	private bool isReadyToUpdate = false;

	// Use this for initialization
	void Start()
	{
		Game.Instance.Background = this;
		this.rigidbody2D.velocity = Game.Instance.ScrollSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if(this.transform.position.x < -X_MAX)
		{

			this.transform.position = new Vector2(X_MAX, 0);

			Game.Instance.TickBackgroundInt(this);

			if (isReadyToUpdate)
			{
				SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
				renderer.sprite = Game.Instance.DeparmentMap[Game.Instance.CurrentDepartment];
				
				isReadyToUpdate = false;
			}
		}
	}

	public void UpdateBackground()
	{
		isReadyToUpdate = true;
	}
}

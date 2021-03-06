﻿using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	//float X_MAX = 25.8f * (4.0f/3.0f);
	const float X_MAX = 25.5f;
	private bool isReadyToUpdate = true;

	// Use this for initialization
	void Start()
	{
		Game.Instance.Background.Add(this);

		this.rigidbody2D.velocity = Game.Instance.ScrollSpeed;
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = Game.Instance.DeparmentMap[Game.Instance.CurrentDepartment];
	}

	// Update is called once per frame
	void Update () {
		if(this.transform.position.x < -X_MAX)
		{
			//X_MAX = gameObject.GetComponent<SpriteRenderer>().sprite.rect.width*gameObject.transform.localScale.x/100 * (4.0f/3.0f);
			this.transform.position = new Vector2(X_MAX, 0);

			Game.Instance.TickBackgroundInt();

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

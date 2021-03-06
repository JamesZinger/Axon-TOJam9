﻿using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ObjectGenerator : MonoBehaviour
{
	//float startY = 0;
	public Rigidbody2D background;
	float maxHeight;
	//float _defaultBuffer = 50.0f;
	public float jumpForce;
	float _ratio = 4.0f / 3.0f;
	float _size;
	public float groundLevel = 1.0f;
	public GameObject go;
	float initialVelocity;
	//int _nextSpawnType = 1;
	float _elapsedTime;
	public float topValue;
	public float middleValue;
	public GameObject cashPrefab;
	public GameObject giftPrefab;
	public GameObject meatPrefab;
	public GameObject a,b,c;
	public Vector2 spawnPoint;

	// Use this for initialization
	void Start()
	{
		spawnPoint = new Vector2( 15, groundLevel );
		_size = Camera.main.orthographicSize;
		SpawnObject( new Vector3( spawnPoint.x, 1.0f, 0 ) );
		Game.Instance.Player.Jump += OnJump;
		maxHeight = 3.0f;
	}
	void SpawnObject( Vector2 pos )
	{
		go = new GameObject();

		Furniture f = go.AddComponent<Furniture>();
		SpriteRenderer rend = go.AddComponent<SpriteRenderer>();

		List<FurnitureTemplate> RandomList = Game.Instance.FurnitureManager.furnitureMap[ Game.Instance.CurrentDepartment ];

		FurnitureTemplate template = RandomList[ Random.Range( 0, RandomList.Count - 1 ) ];
		rend.sprite = template.Sprite;
		f.Template = template;
		go.name = template.Name;
		go.layer = LayerMask.NameToLayer( "Furniture" );
		float yValue = 0;
		//int powerUpLevel;
		switch ( template.Zone )
		{
			case Furniture.SpawnZone.High:
				yValue = topValue;
				break;
			case Furniture.SpawnZone.Medium:
				yValue = middleValue;
				break;
			case Furniture.SpawnZone.Low:
				yValue = groundLevel;
				break;
		}
		go.transform.position = new Vector2( pos.x, yValue );

		PolygonCollider2D bc = go.AddComponent<PolygonCollider2D>();
		Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
		rb.gravityScale = 0f;
		bc.isTrigger = true;
	}
	void SpawnEither( Vector2 pos, int id )
	{
		int spawn = Random.Range( 0, 100 );
		if ( spawn % 2 == 0 )
		{
			SpawnDistraction( pos, id );
		}
		else
		{
			SpawnPowerup( pos, id );
		}
	}
	void SpawnPowerup( Vector2 pos, int id )
	{
		GameObject powerup;
		id = Random.Range( 1, 4 );
		id = ( id > 3 ? 1 : id );
		switch ( id )
		{
			case 1:
				powerup = ( GameObject )Instantiate( cashPrefab );
				powerup.transform.position = pos;
				break;
			case 2:
				powerup = ( GameObject )Instantiate( giftPrefab );
				powerup.transform.position = pos;
				break;
			case 3:
				powerup = ( GameObject )Instantiate( meatPrefab );
				powerup.transform.position = pos;
				break;
		}
	}
	void SpawnDistraction( Vector2 pos, int id )
	{
		GameObject powerup;
		pos = new Vector2( pos.x, groundLevel );
		switch ( id )
		{
			case 1:
				powerup = ( GameObject )Instantiate( a );
				powerup.transform.position = pos;
				break;
			case 2:
				powerup = ( GameObject )Instantiate( b );
				powerup.transform.position = pos;
				break;
			case 3:
				powerup = ( GameObject )Instantiate( c );
				powerup.transform.position = pos;
				break;
		}
	}
	// Update is called once per frame
	void Update()
	{
		_elapsedTime += Time.deltaTime;
		if ( _elapsedTime > 3 )
		{
			CheckSpawn();
			_elapsedTime = 0;
		}
	}
	void DrawDebugRect( Rect r )
	{
		Vector3 TopLeft, TopRight, BottomLeft, BottomRight;
		TopLeft = new Vector3( r.xMin, r.yMax );
		TopRight = new Vector3( r.xMax, r.yMax );
		BottomLeft = new Vector3( r.xMin, r.yMin );
		BottomRight = new Vector3( r.xMax, r.yMin );

		Debug.DrawLine( TopLeft, TopRight, Color.green, 10000, false );
		Debug.DrawLine( TopLeft, BottomLeft, Color.green, 10000, false );
		Debug.DrawLine( TopLeft, BottomRight, Color.white, 10000, false );
		Debug.DrawLine( BottomRight, TopRight, Color.green, 10000, false );
		Debug.DrawLine( BottomRight, BottomLeft, Color.green, 10000, false );
		Debug.DrawLine( BottomLeft, TopRight, Color.white, 10000, false );
	}
	void CheckSpawn()
	{
		if ( go != null )
		{
			SpawnObject( new Vector3( spawnPoint.x, go.transform.position.y, 0 ) );
			SpawnPowerup( new Vector2( spawnPoint.x + Random.Range( 1, 5 ), 7 ), Random.Range( 1, 4 ) );
		}
		else
		{
			SpawnObject( new Vector3( spawnPoint.x, groundLevel, 0 ) );
		}
		Rect checkRect = new Rect( go.transform.position.x, go.transform.position.y, go.GetComponent<SpriteRenderer>().sprite.rect.width / 200, go.GetComponent<SpriteRenderer>().sprite.rect.height / 200 );
		float dist = GetMinDist( checkRect );
		SpawnObject( new Vector3( dist + go.transform.position.x, go.transform.position.y, 0 ) );
		SpawnEither( new Vector2( dist + go.transform.position.x + 4, 7 ), Random.Range( 1, 4 ) );
		go.rigidbody2D.velocity = Game.Instance.ScrollSpeed;
	}

	void OnJump()
	{
		StartCoroutine( calcMaxHeight() );
		Game.Instance.Player.Jump -= OnJump;
	}

	IEnumerator calcMaxHeight()
	{
		//This will trigger on the first pass
		yield return new WaitForFixedUpdate();
		//This will trigger after a frame, at the end of a fixed update.
		//return new WaitForEndOfFrame();
		initialVelocity = Game.Instance.Player.rigidbody2D.velocity.y;
		maxHeight = -( initialVelocity * initialVelocity ) / ( 2.0f * Physics2D.gravity.y ) + groundLevel;
		//Debug.Log(maxHeight);
	}
	float GetMinDist( Rect obstacle )
	{
		//For now, return farDist so successfully jumping or sliding an object always results in safety
		Vector2 closeDist;
		//Check if we can jump the oncoming obstacle
		if ( obstacle.y >= maxHeight )
		{
			//Object is too tall to jump, therefore slide
			closeDist = new Vector2( obstacle.xMax, groundLevel );
			//Debug.Log("High");
		}
		else
		{
			//Object is short enough to jump
			//closeDist = CalcEarliestJump(obstacle);
			closeDist = CalcEarliestJump( obstacle );
			//closeDist = new Vector2(obstacle.height, groundLevel);
		}
		if ( closeDist.x < 0 )
		{
			closeDist = new Vector2( ( _size * _ratio ) * 2, groundLevel );
		}
		float dist = GetMaxDist();
		return dist;
	}
	float GetMaxDist()
	{
		float a = Physics2D.gravity.y;
		
		return 2 * maxHeight / a * background.velocity.x;
	}
	//Calculate the earliest take-off point and return the landing. Use the bottom-left corner of the player hitbox
	Vector2 CalcEarliestJump( Rect obstacle )
	{
		Vector2 start = new Vector2();//Where the jump has to start
		Vector2 peak;//The peak of the first jump
		Vector2 collision;//Where the player hits the obstacle
		float timeToCollide;//Time between jump peak and collision
		float timeToLand;//Time from peak to ground (this shouldn't ever change

		collision = new Vector2( obstacle.x, obstacle.y + obstacle.width );
		//DrawDebugRect(obstacle);
		timeToCollide = Mathf.Sqrt( 2.0f * ( collision.y - maxHeight ) / -Physics2D.gravity.y );
		peak.y = maxHeight;
		peak.x = collision.x - background.velocity.x * timeToCollide;
		timeToLand = Mathf.Sqrt( ( 2.0f * maxHeight ) / ( -Physics2D.gravity.y ) );
		start.x = peak.x + background.velocity.x * timeToLand;
		start.y = groundLevel;

		//Should be renamed end
		return start;
	}
}

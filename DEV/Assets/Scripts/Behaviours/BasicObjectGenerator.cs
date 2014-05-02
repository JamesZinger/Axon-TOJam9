using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BasicObjectGenerator : MonoBehaviour 
{

	public float SpawnsInterval = 10.0f;

	private Rect ScreenRect;
	private Rect PlayableRect;
	private float SpawnYDiff;


	// Use this for initialization
	void Start () 
	{
		ScreenRect = new Rect(0, 0, 10.0f * (4f/3f), 10);
		PlayableRect = new Rect(0, (4f/3f),10.0f * (4f/3f),7.5f );
		SpawnYDiff = PlayableRect.height / 8;
		DrawDebugRect(PlayableRect);
		StartCoroutine(SpawnCycle());
	}
	
	void OnDestory()
	{
		StopAllCoroutines();
	}

	void DrawDebugRect(Rect r)
	{
		Vector3 TopLeft, TopRight, BottomLeft, BottomRight;
		TopLeft = new Vector3(r.xMin, r.yMax);
		TopRight = new Vector3(r.xMax, r.yMax);
		BottomLeft = new Vector3(r.xMin, r.yMin);
		BottomRight = new Vector3(r.xMax, r.yMin);

		Debug.DrawLine(TopLeft, TopRight, Color.green, 10000, false);
		Debug.DrawLine(TopLeft, BottomLeft, Color.green, 10000, false);
		Debug.DrawLine(TopLeft, BottomRight, Color.white, 10000, false);
		Debug.DrawLine(BottomRight, TopRight, Color.green, 10000, false);
		Debug.DrawLine(BottomRight, BottomLeft, Color.green, 10000, false);
		Debug.DrawLine(BottomLeft, TopRight, Color.white, 10000, false);
	}

	IEnumerator SpawnCycle()
	{
		while ( true )
		{

			Vector2 pos = new Vector2( PlayableRect.xMax, PlayableRect.yMin + SpawnYDiff * Random.Range( 1, 8 ) );
			GameObject go = new GameObject();
			go.transform.position = pos;
			Furniture f = go.AddComponent<Furniture>();
			SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
			
			List<FurnitureManager.TemplateFurniture> RandomList = Game.Instance.FurnitureManager.furnitureMap[Game.Instance.CurrentDepartment];

			FurnitureManager.TemplateFurniture template = RandomList[Random.Range(0, RandomList.Count - 1)];
			rend.sprite = template.Sprite;
			f.allanKeyValue = template.AllanKeys;
			f.price = template.Price;
			f.department = template.Department;
			f.desc = template.Description;
			go.name = template.Name;
            go.gameObject.layer = 10;
            
			Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            PolygonCollider2D bc = go.AddComponent<PolygonCollider2D>();
            bc.isTrigger = true;

			yield return new WaitForSeconds( SpawnsInterval );
		}
	}

}

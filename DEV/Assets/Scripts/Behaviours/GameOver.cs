using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameOver : MonoBehaviour 
{
	struct FurnitureListElement
	{
		public Rect NameRect;
		public Rect PriceRect;
		public FurnitureTemplate Template;
	}

	public Data dataobject;
	private float SpentMoney;
	private int AllenKeyTotal;
	public GUISkin skin;

	private float alpha;
	private bool alphaTweenStarted;
	private int numCompletedTweens;
	private int numItems;
	private List<FurnitureListElement> itemRects;
	public float itemSpacing = 20f;
	public float itemSpeed = 250f;

	private Dictionary<Element, GUIStyle>	styleMap;
	private Dictionary<Element, Rect>		rectMap;
	private Dictionary<Element, float>		alphaMap;

	private const int SHOWN_ITEMS = 16;


	
	void Awake()
	{
		styleMap = new Dictionary<Element, GUIStyle>();
		rectMap = new Dictionary<Element, Rect>();

		styleMap.Add( Element.AllenKey  		, skin.GetStyle( "AllenKey" 				) );
		styleMap.Add( Element.Background		, skin.GetStyle( "GameOverBackground"		) );
		styleMap.Add( Element.Quit	   			, skin.GetStyle( "Quit" 					) );
		styleMap.Add( Element.Total	   			, skin.GetStyle( "TotalStyle" 				) );
		styleMap.Add( Element.FurnitureName		, skin.GetStyle( "End-Item-Name" 			) );
		styleMap.Add( Element.FurniturePrice	, skin.GetStyle( "End-Item-Price" 			) );


		rectMap.Add( Element.Background	, new Rect(	  0,   0, 1024, 768 ) );
		rectMap.Add( Element.AllenKey	, new Rect( 352, 450,  320, 190 ) );
		rectMap.Add( Element.Quit		, new Rect( 341, 670,  341, 64  ) );
		rectMap.Add( Element.Total		, new Rect( 102, 230,  870, 75  ) );


		SpentMoney 	  = 0;
		AllenKeyTotal = 0;
	}

	void Start()
	{
		dataobject 		= GameObject.FindObjectOfType<Data>();
		if ( dataobject == null )
		{
			GameObject go = new GameObject();
			Data d = go.AddComponent<Data>();
			d.CollectedFurniture = new List<FurnitureTemplate>();
			int NumFurniture = 100;//Random.Range(10, 21);
			for ( int i = 0; i < NumFurniture; i++ )
			{
				FurnitureTemplate temp = new FurnitureTemplate();
				temp.AllanKeys 		   = Random.Range( 1, 4 );
				int temp2 			   = System.Enum.GetNames(typeof(DepartmentType)).Length;
				temp.Department 	   = (DepartmentType)Random.Range( 0, temp2 - 1);
				temp.Description 	   = "Description: " + temp.Department.ToString();
				temp.Name 			   = "Name: " + temp.Department.ToString();
				temp.Price 			   = Random.Range( 0.0f, 1000.0f );
				temp.Sprite 		   = null;
				d.CollectedFurniture.Add( temp );
			}

			d.didWin = false;
			d.ElapsedTime = new System.TimeSpan( 0, Random.Range( 0, 60 ), Random.Range( 0, 60 ) );
			d.RemainingMoney = 0;
			dataobject = d;
		}

		alphaTweenStarted = false;
		numItems = 0;
		numCompletedTweens = 0;
		itemRects = new List<FurnitureListElement>();
		foreach( FurnitureTemplate template in dataobject.CollectedFurniture )
		{
			numItems++;

			// Total the number of Allen keys and amount spent
			SpentMoney += template.Price;
			AllenKeyTotal += template.AllanKeys;

			Rect priceRect, nameRect;
			FurnitureListElement pair = new FurnitureListElement();

			// Shopping cart item name field
			nameRect  = new Rect( 520f, 0f - 2f * numItems * itemSpacing, 280f, 30f );
			priceRect = new Rect( 800f, 0f - 2f * numItems * itemSpacing, 100f, 30f );
			
			pair.NameRect  = nameRect;
			pair.PriceRect = priceRect;
			pair.Template  = template;
			
			float itemTargetHeight = 670f - itemSpacing * numItems;
			itemRects.Add(pair);
			if (numItems <= SHOWN_ITEMS)
				StartCoroutine( TweenTextFields( itemRects.Count - 1, itemTargetHeight ) );
		}
	}

	IEnumerator TweenTextFields( int itemIndex, float targetHeight )
	{
		Debug.Log( "Index: " + itemIndex + ", Target: " + targetHeight );
		while ( itemRects[ itemIndex ].NameRect.yMin < targetHeight )
		{
			yield return null;
			FurnitureListElement pair = itemRects[itemIndex];
			pair.NameRect = new Rect(
				pair.NameRect.xMin,
				Mathf.Clamp( pair.NameRect.yMin + itemSpeed * Time.deltaTime, -1000000f, targetHeight ),
				pair.NameRect.width,
				pair.NameRect.height
			);

			pair.PriceRect = new Rect(
				pair.PriceRect.xMin,
				Mathf.Clamp( pair.PriceRect.yMin + itemSpeed * Time.deltaTime, -1000000f, targetHeight ),
				pair.PriceRect.width,
				pair.PriceRect.height
			);

			itemRects[itemIndex] = pair;
		}

		// Increment the number of tweens and if all of them are complete, start the fade of the total and allen keys
		numCompletedTweens++;
		if ( !alphaTweenStarted && (numCompletedTweens >= numItems || numCompletedTweens >= SHOWN_ITEMS ))
		{
			Debug.Log( "All tweens complete!" );
			StartCoroutine( TweenAlpha( 0f, 1f, 1f ) );
		}
	}

	IEnumerator TweenAlpha( float start, float end, float overSeconds )
	{
		alphaTweenStarted = true;
		alpha = start;
		var startTime = Time.time;
		while ( alpha < end )
		{
			yield return null;
			float fraction = Mathf.Clamp( ( Time.time - startTime ) / overSeconds, 0f, 1f );
			alpha = Mathf.Lerp( start, end, fraction );
		}
	}

	void Update()
	{
		Xbox360GamepadState.Instance.UpdateState();

		if ( Xbox360GamepadState.Instance.IsButtonDown( Xbox.Button.B ) )
		{
			Destroy( dataobject.gameObject );
			Application.LoadLevel( 0 );
		}


	}

	void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( 1.0f * Screen.width / 1024, 1.0f * Screen.height / 768, 1.0f ) );
		GUI.color = new Color( 1f, 1f, 1f, 1f );
		GUI.Box(   rectMap[ Element.Background ]  , "",  styleMap[ Element.Background ] );

		foreach ( FurnitureListElement r in itemRects )
		{
			GUI.Box( r.NameRect	, r.Template.Name,							styleMap[ Element.FurnitureName ] );
			GUI.Box( r.PriceRect, "$"+r.Template.Price.ToString("0.00"),	styleMap[ Element.FurniturePrice ] );
		}
		GUI.color = new Color( 1f, 1f, 1f, alpha );
		GUI.Label( rectMap[ Element.AllenKey ]  , "x " + AllenKeyTotal.ToString(), 				  styleMap[ Element.AllenKey ]	);
		GUI.Label( rectMap[ Element.Total ]	   , "TOTAL $" + SpentMoney.ToString( "0.00" ) + "!", styleMap[ Element.Total ]		);

		if ( GUI.Button( rectMap[ Element.Quit ], "", styleMap[ Element.Quit ] ) )
		{
			Destroy( dataobject.gameObject );
			Application.LoadLevel( 0 );
		}

	}

	private enum Element
	{
		Background,
		AllenKey,
		Total,
		Quit,
		FurnitureName,
		FurniturePrice
	}
}

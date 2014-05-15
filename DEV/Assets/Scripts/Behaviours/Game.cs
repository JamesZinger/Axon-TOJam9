using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 	A game singleton responsible for being a container for essential game objects and various
/// 	game functions.
/// </summary>
/// <remarks>	James, 2014-04-19. </remarks>
public class Game : MonoBehaviour
{
	public enum Gamestate { Win, Lose, NONE };
	public Gamestate State = Gamestate.NONE;

	#region Private Fields

	private GameControls		controls			= null;
	private Player				player				= null;
	private DepartmentType		currentDepartment	= DepartmentType.NONE;
	private List<Background>	background			= null;
	private int					backgroundTick		= 0;
	private FurnitureManager	furnitureManager;
	private GameGUI				gameGUI;
	private TimeSpan			elapsedGameTime;
	
	private Dictionary<DepartmentType, Sprite>	departmentMap;
	private Queue<PointBurst>					pointBursts;
	
	#endregion

	// Inspector Properties
	public Data				dataObject;
	public GUISkin			Skin;
	public Audiopocalypse	ap;
	public GUIText			list;
	public GameObject		PointBurst;
	public Vector2			ScrollSpeed;
	public PauseMenu		PauseMenu;

	#region Events

	public delegate void GameOverHandler();
	public event GameOverHandler GameOver;

	#endregion

	// Properties
	public GameControls		Controls
	{
		get { return controls; }
		set { controls = value; }
	}
	public Player			Player
	{
		get { return player; }
		set { player = value; }
	}
	public DepartmentType	CurrentDepartment
	{
		get { return currentDepartment; }
		private set { currentDepartment = value; }
	}
	public bool				IsPaused
	{
		get { return PauseMenu.IsPaused; }
	}
	public FurnitureManager FurnitureManager
	{
		get { return furnitureManager; }
		set { furnitureManager = value; }
	}
	public TimeSpan ElapsedGameTime
	{
		get { return elapsedGameTime; }
		private set { elapsedGameTime = value; }
	}

	public List<Background>						Background
	{
		get { return background; }
		set { background = value; }
	}
	public Dictionary<DepartmentType, Sprite>	DeparmentMap
	{
		get { return departmentMap; }
		private set { departmentMap = value; }
	}
	public Queue<PointBurst> PointBursts
	{
		get { return pointBursts; }
		private set { pointBursts = value; }
	}
	

	#region Unity Events

	void Awake()
	{
		if ( instance != null )
		{
			Debug.LogError( "[ERROR - Singleton] Tried to instantiate a second instance of the Game singleton" );
			this.GameOver += OnGameOver;
			Destroy( this );
			return;
		}

	
		Instance = this;

		departmentMap = new Dictionary<DepartmentType, Sprite>();

		string[] names = System.Enum.GetNames( typeof( DepartmentType ) );
		
		int firstdepartment = UnityEngine.Random.Range(0, names.Length - 2);
		
		CurrentDepartment = (DepartmentType)firstdepartment;

		background = new List<Background>(2);

		MapDepartmentTextures();

		gameGUI = gameObject.GetComponent<GameGUI>();

		PointBursts = new Queue<PointBurst>();

		ElapsedGameTime = new TimeSpan();
	}

	void Start()
	{
		if (Controls == null)
		{
			Controls = gameObject.AddComponent<GameControls>();
		}

		Controls.PauseButton += OnPause;
	}

	void OnDestory()
	{
		Instance = null;
	}
   
	public void OutOfCoins()
	{
		OnGameOver();
	}

	void OnGameOver()
	{
		ap.PlayClip(Audiopocalypse.Sounds.Death);
		if (GameOver != null){
			
			GameOver();
		}
	}

	void Update()
	{
		Xbox360GamepadState.Instance.UpdateState();
	}

	void FixedUpdate()
	{
		ElapsedGameTime = ElapsedGameTime.Add( TimeSpan.FromSeconds(Time.fixedDeltaTime) );
	}
	
	#endregion

	/// <summary>	Map department textures to a dictionary of types to textures. </summary>
	/// <remarks>	James, 2014-04-26. </remarks>
	void MapDepartmentTextures()
	{
		string[] names = System.Enum.GetNames( typeof( DepartmentType ) );
		foreach ( string s in names )
		{

			DepartmentType deptType = (DepartmentType)System.Enum.Parse(typeof(DepartmentType), s);

			if (deptType == DepartmentType.NONE)
				continue;

			Sprite tex = null;

			tex = Resources.Load<Sprite>("Sprites/BackGround/" + s + "");

			//Debug.Log("Sprites/BackGround/" + s + "");

			departmentMap.Add(deptType, tex);
		}
	}

	public void TickBackgroundInt()
	{
		backgroundTick++;
		if ( backgroundTick >= 5 )
		{
			currentDepartment = (DepartmentType)UnityEngine.Random.Range( 0, Enum.GetNames( typeof( DepartmentType ) ).Length - 2 );

			foreach ( Background BG in Background )
				BG.UpdateBackground();

			backgroundTick = 0;
		}
	}

	void OnPause()
	{
		if (State != Gamestate.NONE || IsPaused) return;

		ap.PlayClip(Audiopocalypse.Sounds.Menu_Click);
	}

	public void Win()
	{
		if (State != Gamestate.NONE) return;

		State = Gamestate.Win;

		dataObject.RemainingMoney = Player.Cash;
		dataObject.didWin = true;
		dataObject.ElapsedTime = ElapsedGameTime;
		DontDestroyOnLoad( dataObject );
		Application.LoadLevel(2);
	}

	public void Lose()
	{
		
		if (State != Gamestate.NONE) return;

		State = Gamestate.Lose;

		dataObject.RemainingMoney = Player.Cash;
		dataObject.didWin = false;
		dataObject.ElapsedTime = ElapsedGameTime;
		DontDestroyOnLoad( dataObject );
		Application.LoadLevel(2);
	}
	
	public void OnFurnitureCollected( FurnitureTemplate Template )
	{
		PointBurst burst = (Instantiate( PointBurst, Vector3.zero, Quaternion.identity ) as GameObject).GetComponent<PointBurst>();

		PointBursts.Enqueue( burst );

		burst.SetUpForFurniture( new Vector2( 0, 0 ), Template.Price, Template.AllanKeys );

		dataObject.CollectedFurniture.Add( Template );

		if ( Player.HasDiscount )
		{
			switch ( Player.DiscountType )
			{
				case GiftCard.Discount.DIS_25:
					burst.SetUpForFurniture( new Vector2( 0, 0 ), Template.Price * 0.75f, Template.AllanKeys );
					break;
				case GiftCard.Discount.DIS_50:
					burst.SetUpForFurniture( new Vector2( 0, 0 ), Template.Price * 0.50f, Template.AllanKeys );
					break;
				case GiftCard.Discount.DIS_75:
					burst.SetUpForFurniture( new Vector2( 0, 0 ), Template.Price * 0.25f, Template.AllanKeys );
					break;

			}

		}

		ap.PlayClip( Audiopocalypse.Sounds.Pickup_Furniture );



		if ( Player.Cash < 0 && State == Gamestate.NONE )
			Lose();
		
	}

	public void OnMoneyCollected(Cash c)
	{
		PointBurst burst = (Instantiate(Game.Instance.PointBurst, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<PointBurst>();
		burst.SetUpForCash( Vector2.zero, (float)c.amount );
		PointBursts.Enqueue( burst );

	}

	public void ToggleGameGUI()
	{
		gameGUI.enabled = !gameGUI.enabled;

		foreach ( PointBurst pb in PointBursts )
		{
			pb.enabled = !pb.enabled;
		}
	}

	#region Singleton Method and Instance

	private static Game instance = null;

	/// <summary>	Gets or sets the singleton instance. </summary>
	/// <value>	The instance. </value>
	public static Game Instance
	{
		get
		{
			if ( instance == null )
			{
				GameObject go = new GameObject();
				go.name = "GameManager";
				instance = go.AddComponent<Game>();
			}

			return instance;
		}

		private set { instance = value; }
	}

	#endregion

}

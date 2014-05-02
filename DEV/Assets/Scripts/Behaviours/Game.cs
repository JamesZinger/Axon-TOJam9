using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 	A game singleton responsible for being a container for essential game objects and various
/// 	game functions.
/// </summary>
/// <remarks>	James, 2014-04-19. </remarks>
public class Game : MonoBehaviour
{
	public enum Gamestate { Win, Lose, NONE };
	public Gamestate state = Gamestate.NONE;

	private GameControls controls = null;
	private Player player = null;
	private DepartmentType currentDepartment = DepartmentType.NONE;
	private Dictionary<DepartmentType, Sprite> departmentMap;
	private List<Background> background = null;
	private List<FurnitureManager.TemplateFurniture> shoppingList;
	private List<FurnitureManager.TemplateFurniture> originalShoppingList;
	private int backgroundTick = 0;
	private bool isPaused = false;
	private float fixedTimeStep = 0.0f;

	[HideInInspector] public List<FurnitureManager.TemplateFurniture> CollectedFuriture = new List<FurnitureManager.TemplateFurniture>();
	public Data dataObject;
	private FurnitureManager furnitureManager;

    public GUISkin Skin;
	public Audiopocalypse ap;
	public GUIText list;
	
    public GameObject PointBurst;

    #region Events

    public delegate void GameOverHandler();
    public event GameOverHandler GameOver;

    #endregion

	#region Rects
	
	#endregion

	public Vector2 ScrollSpeed;

	public GameControls Controls
	{
		get { return controls; }
		set { controls = value; }
	}

	public Player Player
	{
        get { return player; }
        set { player = value; }
	}

	public DepartmentType CurrentDepartment
	{
		get { return currentDepartment; }
		private set { currentDepartment = value; }
	}

	public bool IsPaused
	{
		get { return isPaused; }
		private set { isPaused = value; }
	}

	public List<Background> Background
	{
		get { return background; }
		set { background = value; }
	}

	public Dictionary<DepartmentType, Sprite> DeparmentMap
	{
		get { return departmentMap; }
		private set {  departmentMap = value; }
	}

	public FurnitureManager FurnitureManager
	{
		get { return furnitureManager; }
		set { furnitureManager = value; }
	}

	public List<FurnitureManager.TemplateFurniture> ShoppingList
	{
		get { return shoppingList; }
		private set { shoppingList = value; }
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

		fixedTimeStep = Time.timeScale;
	}

    void Start()
    {
        if (Controls == null)
        {
            Controls = gameObject.AddComponent<GameControls>();
        }

        Controls.PauseButton += OnPause;
		int DeptCount = 7;//System.Enum.GetNames(typeof(DepartmentType)).Length;
		shoppingList = new List<FurnitureManager.TemplateFurniture>();
		for(int ii = 0; ii < 3; ii++){
			int rand = Random.Range(0,DeptCount);
			//int itemCount = Game.Instance.FurnitureManager.furnitureMap[(DepartmentType)rand].Count;
			
			List<FurnitureManager.TemplateFurniture> RandomList = Game.Instance.FurnitureManager.furnitureMap[(DepartmentType)rand];	
			FurnitureManager.TemplateFurniture template = RandomList[Random.Range(0, RandomList.Count - 1)];
			shoppingList.Add(template);
			list.guiText.text += template.Name + "\n";
		}

		originalShoppingList = shoppingList;
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
		if(backgroundTick >= 5)
		{ 
			int loadNeeded;
			loadNeeded = Random.Range(0,100);
			if(loadNeeded%2 == 1){
				currentDepartment = (DepartmentType) Random.Range(0, System.Enum.GetNames(typeof(DepartmentType)).Length - 2);
			}else{
				int levelsNeeded;
				levelsNeeded = Random.Range(0, shoppingList.Count);
				currentDepartment = shoppingList[levelsNeeded].Department;
			}
		    //Debug.Log("Current Department: " + currentDepartment);
		
			foreach (Background BG in Background)
				BG.UpdateBackground();

			backgroundTick = 0;
		}
	}

	void OnPause()
	{
		if (state != Gamestate.NONE) return;

		if (IsPaused)
			Time.timeScale = fixedTimeStep;	

		else
			Time.timeScale = 0;

		IsPaused = !IsPaused;
		ap.PlayClip(Audiopocalypse.Sounds.Menu_Click);
	}

	public void Win()
	{
		Debug.Log("WIN!");

		if (state != Gamestate.NONE) return;

		state = Gamestate.Win;

		dataObject.Allenkeys = Player.AllanKeys;
		dataObject.RemainingMoney = Player.Cash;
		dataObject.CollectedFurniture = CollectedFuriture;
		dataObject.didWin = true;

		Application.LoadLevel(2);
	}

	public void Lose()
	{
		Debug.Log("Lose :(");

		if (state != Gamestate.NONE) return;

		state = Gamestate.Lose;

		dataObject.Allenkeys = Player.AllanKeys;
		dataObject.RemainingMoney = Player.Cash;
		dataObject.CollectedFurniture = CollectedFuriture;
		dataObject.didWin = false;

		Application.LoadLevel(2);
	}

}

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

	private GameControls controls = null;
	private Player player = null;
	private DepartmentType currentDepartment = DepartmentType.NONE;
	private Dictionary<DepartmentType, Sprite> departmentMap;
	private List<Background> background = null;
	private int backgroundTick = 0;
   
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
				GameObject go = Instantiate( new GameObject(), Vector3.zero, Quaternion.identity ) as GameObject;
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
	}

	void Start()
	{
		DontDestroyOnLoad( gameObject );
		
		if (Controls == null)
		{
			Controls = gameObject.AddComponent<GameControls>();
		}


	}

	void OnDestory()
	{
		Instance = null;
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

			tex = Resources.Load<Sprite>("Sprites/" + s + "");
			departmentMap.Add(deptType, tex);
		}
	}

	public void TickBackgroundInt()
	{
		backgroundTick++;
		if(backgroundTick >= 5)
		{ 
			currentDepartment = (DepartmentType) Random.Range(0, System.Enum.GetNames(typeof(DepartmentType)).Length - 2);
			Debug.Log("Current Department: " + currentDepartment);
		
			foreach (Background BG in Background)
				BG.UpdateBackground();

			backgroundTick = 0;
		}
	}
}

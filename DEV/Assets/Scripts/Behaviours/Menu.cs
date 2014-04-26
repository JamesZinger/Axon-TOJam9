using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{

	private MenuControls controls;
	public MenuControls Controls
	{
		get { return controls; }
		set { controls = value; }
	}

	#region Singleton Instance and Property

	private static Menu instance;
	public static Menu Instance
	{
		get 
		{
			if ( instance == null )
			{
				GameObject go = new GameObject();
				go.name = "MenuManager";
				instance = go.AddComponent<Menu>();

			}

			return instance;
		}
		private set { instance = value; }
	}

	#endregion

	void Awake()
	{
		if ( instance != null )
		{
			Debug.LogError( "[ERROR - Singleton] Tried to instantiate a second instance of the Menu singleton" );
			Destroy( this );
			return;
		}

		Instance = this;

	}

	// Use this for initialization
	void Start()
	{
		DontDestroyOnLoad( gameObject );
	}

	void OnDestory()
	{
		Instance = null;
	}
}

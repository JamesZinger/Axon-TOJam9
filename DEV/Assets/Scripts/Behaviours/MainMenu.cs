using UnityEngine;
using System.Collections.Generic;


public class MainMenu : MonoBehaviour 
{
	
	public GUISkin skin;

	private ScreenState currentScreen;

	public enum MenuState : byte { Main = 0, PlayMode, Instructions, Credits, NONE = System.Byte.MaxValue }

	private Dictionary<MenuState, ScreenState>	screenMap;
    private SpriteRenderer						spRender;
	private Xbox360GamepadState					controller;

	#region Screen States

	/// <summary>	An abstract screen state for the main menu. </summary>
	/// <remarks>	James, 2014-05-02. </remarks>
	private abstract class ScreenState
	{

		#region Member Variables and Properties

		// Member variables
		private				Sprite background;
		private readonly	GUISkin skin;
		private	readonly	SpriteRenderer backgroundRenderer;
		private readonly	MainMenu mainMenu;

		//Properties
		public Sprite Background
		{
			get { return background; }
			set { background = value; }
		}

		public GUISkin Skin
		{
			get { return skin; }
		}

		protected SpriteRenderer BackgroundRenderer
		{
			get { return backgroundRenderer; }
		}

		protected MainMenu MainMenu
		{
			get { return mainMenu; }
		}

		#endregion

		// Constructor
		public ScreenState( GUISkin Skin, SpriteRenderer BackgroundRenderer, MainMenu MainMenu)
		{
			this.background 			= null;
			this.skin 					= Skin;
			this.backgroundRenderer 	= BackgroundRenderer;
			this.mainMenu 				= MainMenu;
		}

		// Member Functions
		public abstract void HandleControls();
		
		public abstract void DrawGUI();

		public virtual void CleanUp() { }
	};

	private class MenuScreen : ScreenState
	{
		private enum MenuButton : byte { Play = 0, Instructions, Credits, Quit, NONE = System.Byte.MaxValue };
		
		private MenuButton						activeButton;
		private Dictionary<MenuButton, string>	styleMap;
		private Rect							playRct;
		private Rect							instRct;
		private Rect							creditRct;
		private Rect							quitRct;
		
		private Dictionary<MenuButton, Texture2D>	unselectedButtonMap;
		private Dictionary<MenuButton, Texture2D>	selectedButtonMap;

		public MenuScreen( GUISkin Skin, SpriteRenderer BackgroundRenderer, MainMenu MainMenu )
			: base( Skin, BackgroundRenderer, MainMenu )
		{
			Background 			= Resources.Load<Sprite>( "Sprites/GUI/Menu/mainMenu-home");

			unselectedButtonMap = new Dictionary<MenuButton, Texture2D>();
			selectedButtonMap 	= new Dictionary<MenuButton, Texture2D>();
			styleMap			= new Dictionary<MenuButton, string>();

			unselectedButtonMap.Add( MenuButton.Play		, Resources.Load<Texture2D>( "Sprites/GUI/Menu/playUnselected" ) );
			unselectedButtonMap.Add( MenuButton.Instructions, Resources.Load<Texture2D>( "Sprites/GUI/Menu/instructionsUnselected" ) );
			unselectedButtonMap.Add( MenuButton.Credits		, Resources.Load<Texture2D>( "Sprites/GUI/Menu/creditsUnselected" ) );
			unselectedButtonMap.Add( MenuButton.Quit		, Resources.Load<Texture2D>( "Sprites/GUI/Menu/quitUnselected" ) );

			selectedButtonMap  .Add( MenuButton.Play		, Resources.Load<Texture2D>( "Sprites/GUI/Menu/playSelected" ) );
			selectedButtonMap  .Add( MenuButton.Instructions, Resources.Load<Texture2D>( "Sprites/GUI/Menu/instructionsSelected" ) );
			selectedButtonMap  .Add( MenuButton.Credits		, Resources.Load<Texture2D>( "Sprites/GUI/Menu/creditsSelected" ) );
			selectedButtonMap  .Add( MenuButton.Quit		, Resources.Load<Texture2D>( "Sprites/GUI/Menu/quitSelected" ) );

			playRct   = new Rect( 328, 207, 200, 75 );
			instRct   = new Rect( 213, 300, 431, 75 );
			creditRct = new Rect( 287, 400, 282, 75 );
			quitRct   = new Rect( 328, 500, 203, 75 );

			activeButton = MenuButton.Play;

			styleMap.Add( MenuButton.Play		 , "Play Button" );
			styleMap.Add( MenuButton.Instructions, "Instruction Button" );
			styleMap.Add( MenuButton.Credits	 , "Credit Button" );
			styleMap.Add( MenuButton.Quit		 , "Quit Button" );

			Skin.GetStyle( styleMap[ activeButton ] ).normal.background = selectedButtonMap[ activeButton ];
		}

		public override void DrawGUI()
		{
			if ( GUI.Button( playRct  , "", Skin.GetStyle( styleMap[ MenuButton.Play		] ) ) ) { MainMenu.ChangeScreen( MenuState.PlayMode ); 	   }
			if ( GUI.Button( instRct  , "", Skin.GetStyle( styleMap[ MenuButton.Instructions] ) ) ) { MainMenu.ChangeScreen( MenuState.Instructions ); }
			if ( GUI.Button( creditRct, "", Skin.GetStyle( styleMap[ MenuButton.Credits 	] ) ) )	{ MainMenu.ChangeScreen( MenuState.Credits ); 	   }
			if ( GUI.Button( quitRct  , "", Skin.GetStyle( styleMap[ MenuButton.Quit 		] ) ) ) 
			{ 
#if UNITY_EDITOR
						UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
						//Application.OpenURL("");
						Application.Quit();
#else
						Application.Quit();
#endif			
			
			}
		}

		public override void HandleControls()
		{
			if ( Xbox360GamepadState.Instance.AxisJustPastThreshold( Xbox.Axis.LAnalogY, -0.8f ) )
			{
				if ( activeButton == MenuButton.Quit )
				{
					SwtichActiveButton( MenuButton.Play );
				}
				else
				{
					SwtichActiveButton( activeButton + 1 );
				}
				
			}

			else if ( Xbox360GamepadState.Instance.AxisJustPastThreshold( Xbox.Axis.LAnalogY, 0.8f ) )
			{
				if ( activeButton == MenuButton.Play )
				{
					SwtichActiveButton( MenuButton.Quit );
				}
				else
				{
					SwtichActiveButton( activeButton - 1 );
				}

			}

			if ( Xbox360GamepadState.Instance.IsButtonDown( Xbox.Button.A ) )
			{
				switch ( activeButton )
				{
					case MenuButton.Play:			MainMenu.ChangeScreen( MenuState.PlayMode ); 		break;
					case MenuButton.Instructions:	MainMenu.ChangeScreen( MenuState.Instructions ); 	break;
					case MenuButton.Credits:		MainMenu.ChangeScreen( MenuState.Credits ); 		break;
					case MenuButton.Quit:
#if UNITY_EDITOR
						UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
						//Application.OpenURL("");
						Application.Quit();
#else
						Application.Quit();
#endif			
						break;
				}
			}
		}

		private void SwtichActiveButton( MenuButton b )
		{
			Skin.GetStyle( styleMap[ activeButton ] ).normal.background = unselectedButtonMap[ activeButton ];
			activeButton = b;
			Skin.GetStyle( styleMap[ activeButton ] ).normal.background = selectedButtonMap[ activeButton ];
		}

		public override void CleanUp()
		{
			Skin.GetStyle( styleMap[ activeButton ] ).normal.background = unselectedButtonMap[ activeButton ];
		}
	};

	private class InstructionScreen : ScreenState
	{
		private Rect instructionBackRect;

		public InstructionScreen( GUISkin Skin, SpriteRenderer BackgroundRenderer, MainMenu MainMenu )
			: base( Skin, BackgroundRenderer, MainMenu )
		{
			Background = Resources.Load<Sprite>( "Sprites/GUI/Menu/Instructions_page" );

			instructionBackRect = new Rect	 ( 656, 562, 150, 44 );
		}

		public override void DrawGUI()
		{
			if ( GUI.Button( instructionBackRect, "", Skin.GetStyle( "Back Button" ) ) )
			{
				MainMenu.ChangeScreen( MenuState.Main );
			}
		}

		public override void HandleControls()
		{
			if ( Xbox360GamepadState.Instance.IsButtonDown( Xbox.Button.B ) )
			{
				MainMenu.ChangeScreen( MenuState.Main );
			}
		}
	};

	private class PlayModeScreen : ScreenState
	{

		private enum Option : byte { Coworker = 0, Student, NONE = byte.MaxValue }

		private Option	currentSelection;
		private Rect	backRct;
		private Rect	coworkRct;
		private Rect	studentRct;

		private Dictionary<Option, string> styleMap;
		private Dictionary<Option, Texture2D> unselectedOptions;
		private Dictionary<Option, Texture2D> selectedOptions;

		public PlayModeScreen( GUISkin Skin, SpriteRenderer BackgroundRenderer, MainMenu MainMenu )
			: base( Skin, BackgroundRenderer, MainMenu )
		{
			Background = Resources.Load<Sprite>( "Sprites/GUI/Menu/mainMenu-playmode" );

			backRct    = new Rect( 353, 562, 150, 44 );
			coworkRct  = new Rect(  70, 200, 355, 175 );
			studentRct = new Rect( 430, 200, 355, 175 );

			styleMap 		  = new Dictionary<Option, string>	 ();
			unselectedOptions = new Dictionary<Option, Texture2D>();
			selectedOptions   = new Dictionary<Option, Texture2D>();

			styleMap.Add( Option.Coworker, "Coworker Opt" );
			styleMap.Add( Option.Student , "Student Opt"  );

			unselectedOptions.Add( Option.Coworker, Resources.Load<Texture2D>( "Sprites/GUI/Menu/coworkerUnselected" ) );
			unselectedOptions.Add( Option.Student , Resources.Load<Texture2D>( "Sprites/GUI/Menu/studentUnselected"  ) );

			selectedOptions.Add( Option.Coworker, Resources.Load<Texture2D>( "Sprites/GUI/Menu/coworkerSelected" ) );
			selectedOptions.Add( Option.Student	, Resources.Load<Texture2D>( "Sprites/GUI/Menu/studentSelected"   ) );

			currentSelection = Option.Coworker;
			Skin.GetStyle( styleMap[ currentSelection ] ).normal.background = selectedOptions[ currentSelection ];
		}

		public override void DrawGUI()
		{
			if ( GUI.Button( backRct, "", Skin.GetStyle( "Back Button" ) ) )
				MainMenu.ChangeScreen( MenuState.Main );

			if ( GUI.Button( coworkRct, "", Skin.GetStyle( "Coworker Opt" ) ) )
			{
				Application.LoadLevel( 1 );
			}
			if ( GUI.Button( studentRct, "", Skin.GetStyle( "Student Opt" ) ) )
			{
				Application.LoadLevel( 1 );
			}
		}

		public override void HandleControls()
		{
			// Down
			if ( Xbox360GamepadState.Instance.AxisJustPastThreshold( Xbox.Axis.LAnalogY, -0.8f ) )
			{

			}
			// Up
			else if ( Xbox360GamepadState.Instance.AxisJustPastThreshold( Xbox.Axis.LAnalogY, 0.8f ) )
			{

			}
			// Right
			else if ( Xbox360GamepadState.Instance.AxisJustPastThreshold( Xbox.Axis.LAnalogX, 0.8f ) )
			{
				if ( currentSelection == Option.Coworker )
				{
					ChangeSelection( Option.Student );
				}

			}
			// Left
			else if ( Xbox360GamepadState.Instance.AxisJustPastThreshold( Xbox.Axis.LAnalogX, -0.8f ) )
			{
				if ( currentSelection == Option.Student )
				{
					ChangeSelection( Option.Coworker );
				}
			}

			if ( Xbox360GamepadState.Instance.IsButtonDown( Xbox.Button.A ) )
			{
				switch ( currentSelection )
				{
					case Option.Coworker: Application.LoadLevel( 1 ); break;
					case Option.Student:  Application.LoadLevel( 1 ); break;
				}
			}

			if ( Xbox360GamepadState.Instance.IsButtonDown( Xbox.Button.B ) )
			{
				MainMenu.ChangeScreen( MenuState.Main );
			}
		}

		private void ChangeSelection( Option o )
		{
			Skin.GetStyle( styleMap[ currentSelection ] ).normal.background = unselectedOptions[ currentSelection ];
			currentSelection = o;
			Skin.GetStyle( styleMap[ currentSelection ] ).normal.background = selectedOptions[ currentSelection ];
		}

		public override void CleanUp()
		{
			Skin.GetStyle( styleMap[ currentSelection ] ).normal.background = unselectedOptions[ currentSelection ];
		}
	};

	private class CreditScreen : ScreenState
	{
		private Rect backRct;

		public CreditScreen( GUISkin Skin, SpriteRenderer BackgroundRenderer, MainMenu MainMenu )
			: base( Skin, BackgroundRenderer, MainMenu )
		{
			Background = Resources.Load<Sprite>( "Sprites/GUI/Menu/mainMenu-credits" );

			backRct = new Rect( 353, 562, 150, 44 );
		}

		public override void DrawGUI()
		{
			if ( GUI.Button( backRct, "", Skin.GetStyle( "Back Button" ) ) )
			{
				MainMenu.ChangeScreen( MenuState.Main );
			}
		}

		public override void HandleControls()
		{
			if ( Xbox360GamepadState.Instance.IsButtonDown( Xbox.Button.B ) )
			{
				MainMenu.ChangeScreen( MenuState.Main );
			}
		}
	}

	#endregion

	#region Unity Events

	void Start()
	{
		spRender = gameObject.GetComponent<SpriteRenderer>();
		screenMap = new Dictionary<MenuState, ScreenState>();

		screenMap.Add( MenuState.Main		 , new MenuScreen( skin, spRender, this ) );
		screenMap.Add( MenuState.PlayMode	 , new PlayModeScreen( skin, spRender, this ) );
		screenMap.Add( MenuState.Credits	 , new CreditScreen( skin, spRender, this ) );
		screenMap.Add( MenuState.Instructions, new InstructionScreen( skin, spRender, this ) );

		currentScreen = screenMap[ MenuState.Main ];

		spRender.sprite = currentScreen.Background;
	}

	void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / 856, 1.0f * Screen.height / 642, 1.0f));

		currentScreen.DrawGUI();
    }

	void Update()
	{
		Xbox360GamepadState.Instance.UpdateState();
		currentScreen.HandleControls();
	}

	void OnDestroy()
	{
		foreach ( MenuState state in screenMap.Keys )
		{
			screenMap[ state ].CleanUp();
		}
	}

	#endregion

	#region Private Member Methods

	/// <summary>	Change screen. </summary>
	/// <remarks>	James, 2014-05-02. </remarks>
	/// <exception cref="NotSupportedException">	Thrown when the state of the menu is set to NONE. </exception>
	/// <param name="state">	The state of the menu to be changed to. </param>
	public void ChangeScreen( MenuState state )
	{
		if ( state == MenuState.NONE )
			throw new System.NotSupportedException( "The state of the menu cannot be NONE" );

		currentScreen = screenMap[ state ];
		spRender.sprite = currentScreen.Background;
	}

	#endregion
}


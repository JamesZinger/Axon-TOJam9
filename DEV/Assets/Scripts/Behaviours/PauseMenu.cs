using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(SpriteRenderer))]
public class PauseMenu : MonoBehaviour
{
	public enum MenuState : byte { Pause = 0, Instructions, NONE = byte.MaxValue }
	public GUISkin Skin;


	private MenuState currentScreen;
	private Dictionary<MenuState, ScreenState> screenMap;
	private SpriteRenderer BackgroundRenderer;
	private Game game;
	private float defaultTimeScale;

	public bool IsPaused
	{
		get { return BackgroundRenderer.enabled; }
	}


	void Awake()
	{
		screenMap 		   = new Dictionary<MenuState, ScreenState>();
		currentScreen 	   = MenuState.Pause;
		BackgroundRenderer = gameObject.GetComponent<SpriteRenderer>();
		screenMap.Add( MenuState.Pause, new PauseScreen( Skin, BackgroundRenderer, this ) );
		screenMap.Add( MenuState.Instructions, new InstructionScreen( Skin, BackgroundRenderer, this ) );
	}

	void Start()
	{
		game = Game.Instance;
		BackgroundRenderer.sprite = screenMap[ currentScreen ].Background;
		defaultTimeScale = Time.timeScale;
		game.Controls.PauseButton += OnPauseButton;
	}

	void Update()
	{
		if ( !IsPaused ) return;
		//Xbox360GamepadState.Instance.UpdateState();
		screenMap[ currentScreen ].HandleControls();
	}

	void OnGUI()
	{
		if ( !IsPaused ) return;
		GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( 1.0f * Screen.width / 856, 1.0f * Screen.height / 642, 1.0f ) );

		screenMap[ currentScreen ].DrawGUI();
	}

	void OnDestroy()
	{
		foreach ( MenuState state in screenMap.Keys )
		{
			screenMap[ state ].CleanUp();
		}
	}

	public void ChangeScreen( MenuState newState )
	{
		if ( currentScreen == MenuState.NONE || newState == MenuState.NONE )
			throw new System.NotSupportedException( "The state of the pause menu cannot be NONE" );

		currentScreen = newState;
		BackgroundRenderer.sprite = screenMap[currentScreen].Background;
	}

	public void OnPauseButton()
	{
		if ( game.State != Game.Gamestate.NONE ) return;

		if ( IsPaused && currentScreen == MenuState.Pause )
		{
			UnPause();
		}
		else if (!IsPaused)
		{
			Pause();
		}
	}

	public void Pause()
	{
		if ( IsPaused ) return;

		Time.timeScale = 0;
		BackgroundRenderer.enabled = true;
		game.ToggleGameGUI();
	}

	public void UnPause()
	{
		if ( !IsPaused ) return;

		Time.timeScale = defaultTimeScale;
		BackgroundRenderer.enabled = false;
		game.ToggleGameGUI();
	}


	#region Screen State Classes

	private abstract class ScreenState
	{
		#region Fields and Properties

		private				Sprite			background;
		private readonly	GUISkin			skin;
		private readonly	SpriteRenderer	backgroundRenderer;
		private readonly	PauseMenu		pauseMenu;
		
		public PauseMenu PauseMenu
		{
			get { return pauseMenu; }
		}
		public Sprite Background
		{
			get { return background; }
			protected set { background = value; }
		}
		protected GUISkin Skin
		{
			get { return skin; }
		}
		protected SpriteRenderer BackgroundRenderer
		{
			get { return backgroundRenderer; }
		}

		#endregion

		public ScreenState( GUISkin Skin, SpriteRenderer BackgroundRenderer, PauseMenu PauseMenu )
		{
			this.skin = Skin;
			this.backgroundRenderer = BackgroundRenderer;
			this.pauseMenu = PauseMenu;
			this.background = null;
		}

		public abstract void DrawGUI();
		public abstract void HandleControls();
		public virtual	void CleanUp() { }
	}

	private class PauseScreen : ScreenState
	{
		private enum MenuButton : byte { Resume, Restart, Instructions, QuitToMainMenu, NONE = byte.MaxValue }
		private MenuButton activeButton;

		private Dictionary<MenuButton, Texture2D>	unselectedButtonMap;
		private Dictionary<MenuButton, Texture2D>	selectedButtonMap;
		private Dictionary<MenuButton, GUIStyle>		styleMap;
		private Dictionary<MenuButton, Rect>		rectMap;

		public PauseScreen( GUISkin Skin, SpriteRenderer BackgroundRenderer, PauseMenu PauseMenu)
			: base( Skin, BackgroundRenderer, PauseMenu )
		{
			activeButton 		= MenuButton.Resume;
		
			unselectedButtonMap = new Dictionary<MenuButton, Texture2D>();
			selectedButtonMap 	= new Dictionary<MenuButton, Texture2D>();
			styleMap 			= new Dictionary<MenuButton, GUIStyle> ();
			rectMap 			= new Dictionary<MenuButton, Rect>	   ();

			unselectedButtonMap.Add( MenuButton.Resume		  , Resources.Load<Texture2D>( "Sprites/GUI/PauseScreen/pauseResumeUnselected" 		 ) );
			unselectedButtonMap.Add( MenuButton.Restart		  , Resources.Load<Texture2D>( "Sprites/GUI/PauseScreen/pauseRestartUnselected" 	 ) );
			unselectedButtonMap.Add( MenuButton.Instructions  , Resources.Load<Texture2D>( "Sprites/GUI/PauseScreen/pauseinstructionsUnselected" ) );
			unselectedButtonMap.Add( MenuButton.QuitToMainMenu, Resources.Load<Texture2D>( "Sprites/GUI/PauseScreen/pauseQuitUnselected" 		 ) );

			selectedButtonMap.Add( MenuButton.Resume		, Resources.Load<Texture2D>( "Sprites/GUI/PauseScreen/pauseResumeSelected" 		 ) );
			selectedButtonMap.Add( MenuButton.Restart		, Resources.Load<Texture2D>( "Sprites/GUI/PauseScreen/pauseRestartSelected" 	 ) );
			selectedButtonMap.Add( MenuButton.Instructions	, Resources.Load<Texture2D>( "Sprites/GUI/PauseScreen/pauseinstructionsSelected" ) );
			selectedButtonMap.Add( MenuButton.QuitToMainMenu, Resources.Load<Texture2D>( "Sprites/GUI/PauseScreen/pauseQuitSelected" 		 ) );

			styleMap.Add( MenuButton.Resume		   , Skin.GetStyle("Pause-Resume" 	   ) );
			styleMap.Add( MenuButton.Restart	   , Skin.GetStyle("Pause-Restart"	   ) );
			styleMap.Add( MenuButton.Instructions  , Skin.GetStyle("Pause-Instructions") );
			styleMap.Add( MenuButton.QuitToMainMenu, Skin.GetStyle("Pause-QuitToMenu"  ) );

			rectMap.Add( MenuButton.Resume		  , new Rect( 303, 207, 250, 75 ) );
			rectMap.Add( MenuButton.Restart		  , new Rect( 303, 300, 250, 75 ) );
			rectMap.Add( MenuButton.Instructions  , new Rect( 275, 400, 325, 75 ) );
			rectMap.Add( MenuButton.QuitToMainMenu, new Rect( 240, 500, 400, 75 ) );

			Background = Resources.Load<Sprite>( "Sprites/GUI/Menu/mainMenu-home" );

			styleMap[ activeButton ].normal.background = selectedButtonMap[ activeButton ];

		}

		public override void DrawGUI()
		{

			if ( GUI.Button( rectMap[ MenuButton.Resume ]			, "", styleMap[ MenuButton.Resume ] 		 ) ) { Resume(); }
			if ( GUI.Button( rectMap[ MenuButton.Restart ]			, "", styleMap[ MenuButton.Restart ] 	   	 ) ) { Restart(); }
			if ( GUI.Button( rectMap[ MenuButton.Instructions ]		, "", styleMap[ MenuButton.Instructions ]    ) ) { Instructions(); }
			if ( GUI.Button( rectMap[ MenuButton.QuitToMainMenu ]	, "", styleMap[ MenuButton.QuitToMainMenu ]  ) ) { Quit(); }

		}

		public override void HandleControls()
		{
			if ( Xbox360GamepadState.Instance.AxisJustPastThreshold( Xbox.Axis.LAnalogY, -0.8f ) )
			{
				if ( activeButton == MenuButton.QuitToMainMenu )
				{
					SwitchActiveButton( MenuButton.Resume );
				}
				else
				{
					SwitchActiveButton( activeButton + 1 );
				}

			}

			else if ( Xbox360GamepadState.Instance.AxisJustPastThreshold( Xbox.Axis.LAnalogY, 0.8f ) )
			{
				if ( activeButton == MenuButton.Resume )
				{
					SwitchActiveButton( MenuButton.QuitToMainMenu );
				}
				else
				{
					SwitchActiveButton( activeButton - 1 );
				}

			}

			if ( Xbox360GamepadState.Instance.IsButtonDown( Xbox.Button.A ) )
			{
				switch ( activeButton )
				{
					case MenuButton.Resume		  : Resume()	  ; break;
					case MenuButton.Restart		  : Restart()	  ; break;
					case MenuButton.Instructions  : Instructions(); break;
					case MenuButton.QuitToMainMenu: Quit()		  ; break;
				}
			}
		}

		public override void CleanUp()
		{
			styleMap[ activeButton ].normal.background = unselectedButtonMap[ activeButton ];
		}

		private void SwitchActiveButton(MenuButton button)
		{
			styleMap[ activeButton ].normal.background = unselectedButtonMap[ activeButton ];
			activeButton = button;
			styleMap[ activeButton ].normal.background = selectedButtonMap[ activeButton ];
		}

		private void Resume()
		{
			PauseMenu.UnPause();
		}

		private void Restart()
		{
			PauseMenu.UnPause();
			Application.LoadLevel(Application.loadedLevel);
		}

		private void Instructions()
		{
			PauseMenu.ChangeScreen( MenuState.Instructions );
		}

		private void Quit()
		{
			PauseMenu.UnPause();
			Application.LoadLevel( 0 );
		}
	}

	private class InstructionScreen : ScreenState
	{

		private Rect backRect;
		GUIStyle backStyle;

		public InstructionScreen(GUISkin Skin, SpriteRenderer BackgroundRenderer, PauseMenu PauseMenu) 
			: base (Skin, BackgroundRenderer, PauseMenu)
		{

			Background = Resources.Load<Sprite>( "Sprites/GUI/Menu/Instructions_page" );
			backStyle = Skin.GetStyle( "Pause-InsBack" );
			backRect = new Rect( 656, 562, 150, 44 );

		}

		public override void DrawGUI()
		{
			if ( GUI.Button( backRect, "", backStyle ) )
			{
				Back();
			}
		}

		public override void HandleControls()
		{
			if ( Xbox360GamepadState.Instance.IsButtonDown( Xbox.Button.B ) )
			{
				Back();
			}
		}

		private void Back()
		{
			PauseMenu.ChangeScreen( MenuState.Pause );
		}
	}

	#endregion
}

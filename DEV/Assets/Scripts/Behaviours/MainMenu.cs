using UnityEngine;
using System.Collections.Generic;


public class MainMenu : MonoBehaviour 
{
   public enum MenuButton : byte { Play = 0, Instructions, Credits, Quit, NONE = System.Byte.MaxValue };

	public GUISkin skin;

	private UIScreen screen;
	private MenuButton ActiveButton;

	private Dictionary<MenuButton, Sprite> unselectedButtonMap;
	private Dictionary<MenuButton, Sprite> selectedButtonMap;
	private Dictionary<UIScreen, Sprite> screenMap;
    private SpriteRenderer spRender;
	private Xbox360GamepadState controller = new Xbox360GamepadState();

	#region Screen States

	private class ScreenState 
	{
		// Member variables
		private Sprite background;
		private readonly Xbox360GamepadState controller;
		private readonly GUISkin skin;


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

		protected Xbox360GamepadState Controller
		{
			get	{ return controller; }
		}


		// Constructor
		public ScreenState( Xbox360GamepadState controller, GUISkin skin)
		{
			Background = null;
			this.controller = controller;
			this.skin = skin;
		}


		public virtual void HandleControls()
		{
			Controller.UpdateState();
		}
		
		public abstract void DrawGUI();
	};
	
	private class MenuScreen : ScreenState
	{
		Rect playRct;


		public MenuScreen( Xbox360GamepadState controller, GUISkin skin )
			: base( controller, skin )
		{
			Background = Resources.Load<Sprite>( "Sprites/GUI/Menu/mainMenu-home");
		}

		public override void DrawGUI()
		{
			if ( GUI.Button( playRct, "", Skin.GetStyle( "Play Button" ) ) )
			{
				ChangeScreen( UIScreen.PlayMode );
			}
			if ( GUI.Button( instRct, "", Skin.GetStyle( "Instruction Button" ) ) )
			{
				ChangeScreen( UIScreen.Instructions );
			}
			if ( GUI.Button( creditRct, "", Skin.GetStyle( "Credit Button" ) ) )
			{
				ChangeScreen( UIScreen.Credits );
			}
			if ( GUI.Button( quitRct, "", Skin.GetStyle( "Quit Button" ) ) )
			{
				Application.Quit();
			}
		}
	};

	private class InstructionScreen : ScreenState
	{

	};

	private class PlayModeScreen : ScreenState
	{

	};

	private class CreditScreen : ScreenState
	{

	}
	#endregion

	#region GUI Rects

	private Rect playRct;
    private Rect instRct;
    private Rect creditRct;
	private Rect quitRct;
    private Rect backRct;
    private Rect coworkRct;
    private Rect studentRct;
	private Rect instructionBackRect;
	private Rect screenRect;

    #endregion 


	#region Initialization

	void Start() 
    {
		unselectedButtonMap = new Dictionary<MenuButton, Sprite>();
		selectedButtonMap 	= new Dictionary<MenuButton, Sprite>();

		screenMap		   .Add( UIScreen.PlayMode		, Resources.Load<Sprite>( "Sprites/GUI/Menu/mainMenu-playmode" ) );
		screenMap		   .Add( UIScreen.Credits		, Resources.Load<Sprite>( "Sprites/GUI/Menu/mainMenu-credits" ) );
		screenMap		   .Add( UIScreen.Instructions	, Resources.Load<Sprite>( "Sprites/GUI/Menu/Instructions_page" ) );

		unselectedButtonMap.Add( MenuButton.Play		, Resources.Load<Sprite>( "Sprites/GUI/Menu/playUnselected" ) );
		unselectedButtonMap.Add( MenuButton.Instructions, Resources.Load<Sprite>( "Sprites/GUI/Menu/instructionsUnselected" ) );
		unselectedButtonMap.Add( MenuButton.Credits		, Resources.Load<Sprite>( "Sprites/GUI/Menu/creditsUnselected" ) );
		unselectedButtonMap.Add( MenuButton.Quit		, Resources.Load<Sprite>( "Sprites/GUI/Menu/quitUnselected" ) );

		selectedButtonMap  .Add( MenuButton.Play		, Resources.Load<Sprite>( "Sprites/GUI/Menu/playSelected" ) );
		selectedButtonMap  .Add( MenuButton.Instructions, Resources.Load<Sprite>( "Sprites/GUI/Menu/instructionsSelected" ) );
		selectedButtonMap  .Add( MenuButton.Credits		, Resources.Load<Sprite>( "Sprites/GUI/Menu/creditsSelected" ) );
		selectedButtonMap  .Add( MenuButton.Quit		, Resources.Load<Sprite>( "Sprites/GUI/Menu/quitSelected" ) );

		spRender = this.gameObject.GetComponent<SpriteRenderer>();
		screen = UIScreen.Main;
		ActiveButton = MenuButton.Play;

        ChangeScreen(screen);
        SetupRects();
	}

    private void SetupRects()
	{
		screenRect = new Rect(  0 ,  0 , 856, 642 );
		instructionBackRect = new Rect	 ( 656, 562, 150, 44 );

		playRct = new Rect	 ( 328, 207, 200, 75 );
		instRct = new Rect	 ( 213, 300, 431, 75 );
		creditRct = new Rect ( 287, 400, 282, 75 );
		quitRct = new Rect	 ( 328, 500, 203, 75 );

		backRct = new Rect	 ( 353, 562, 150, 44 );

		coworkRct = new Rect ( 70 , 200, 355, 175 );
		studentRct = new Rect( 430, 200, 355, 175 );
	}

	#endregion


	#region Screen Functions

	void MainScreen()
    {
        if (screen != UIScreen.Main) return;
		//if (controller.Axies[XboxAxis.LAnalog].y == -1 && controller.prevAxies[XboxAxis.LAnalog].y == 0)
		//{ 

		//	ActiveButton--;
		//	if (ActiveButton == MenuButton.Quit)
		//		ActiveButton = MenuButton.Play;
		//}

        
    }

	void MainScreenControls()
	{
		if ( screen != UIScreen.Main ) return;

		if (controller.Axes[Xbox.Axis.LAnalog].y == -1 && controller.PrevAxes[Xbox.Axis.LAnalog].y == 0)
		{ 

			ActiveButton--;
			if (ActiveButton == MenuButton.Quit)
				ActiveButton = MenuButton.Play;
		}
	}

    void Credits()
    {
        if (screen != UIScreen.Credits) return;

        if (GUI.Button(backRct, "", skin.GetStyle("Back Button")))
        {
            ChangeScreen(UIScreen.Main);
        }

    }

    void PlayMode()
    {
        if (screen != UIScreen.PlayMode) return; 

        if (GUI.Button(backRct, "", skin.GetStyle("Back Button")))
            ChangeScreen(UIScreen.Main);

        if (GUI.Button(coworkRct, "", skin.GetStyle("Coworker Opt")))
        {
            Application.LoadLevel(1);
        }
		if ( GUI.Button( studentRct, "", skin.GetStyle( "Student Opt" ) ) )
		{
			Application.LoadLevel( 1 );
		}

    }

	void Instructions()
	{
		if ( screen != UIScreen.Instructions ) return;

		GUI.Box( screenRect, "", skin.GetStyle( "InstructionBackground" ) );
		if ( GUI.Button( instructionBackRect, "", skin.GetStyle( "Back Button" ) ) )
		{
			ChangeScreen( UIScreen.Main );
		}
	}

	#endregion


	#region Unity Events (Start() is in Initalization)

	void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / 856, 1.0f * Screen.height / 642, 1.0f));
        MainScreen();
        Credits();
        PlayMode();
		Instructions();
    }

	void Update()
	{
		controller.UpdateState();

		switch ( screen )
		{
			case UIScreen.Main:
				MainScreenControls();
				break;
		}
	}

	#endregion


	#region Private Member Methods
	
	void ChangeScreen( UIScreen screen )
	{
		if ( screen == UIScreen.NONE )
			spRender.sprite = screenMap[ screen ];
		this.screen = screen;
	}

	#region Changing Active Buttons

	void MainChangeActiveButton( MenuButton newButton )
	{
		if ( screen != UIScreen.Main ) return;

		
	}

	#endregion

	#endregion
}


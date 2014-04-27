using UnityEngine;
using System.Collections.Generic;

public class MenuBackGround : MonoBehaviour 
{
    public enum UIScreen { None, Main, Instructions, PlayMode, Credits };
    public UIScreen screen;

	public enum MenuButton { Play = 0, Instructions, Credits, Quit, NONE = int.MaxValue }
	private MenuButton ActiveButton = MenuButton.NONE;
    public GUISkin skin; 

    List<Sprite> imgList = new List<Sprite>();
    SpriteRenderer spRender;
	//Xbox360GamepadState controller = new Xbox360GamepadState();

    #region GUI Rects

    private Rect playRct;
    private Rect instRct;
    private Rect creditRct;
    private Rect quitRct;

    private Rect backRct;

    public Rect coworkRct;
    private Rect studentRct;

	Rect backRect;
	Rect screenRect;

    #endregion 

    void Start () 
    {
		screenRect = new Rect( 0, 0, Screen.width, Screen.height );
		

        imgList.Add((Sprite)Resources.Load("Sprites/GUI/Menu/mainMenu-home", typeof(Sprite)));
        imgList.Add((Sprite)Resources.Load("Sprites/GUI/Menu/mainMenu-playmode", typeof(Sprite)));
        imgList.Add((Sprite)Resources.Load("Sprites/GUI/Menu/mainMenu-credits", typeof(Sprite)));
		imgList.Add((Sprite)Resources.Load("Sprites/GUI/Menu/Instructions_page", typeof(Sprite)));

        Debug.Log("Width: " +Screen.width + " Height: " + Screen.height);

        spRender = this.gameObject.GetComponent<SpriteRenderer>();
        screen = UIScreen.Main;

        ChangeScreen(screen);
        Init();
	}

    void Init()
    {
        
		backRect = new Rect(856 - 200, 642 - 80, 150, 44); 

        playRct = new Rect(328, 207, 200, 75);
        instRct = new Rect(213, 300, 431, 75);
        creditRct = new Rect(287, 400, 282, 75);
        quitRct = new Rect(328, 500, 203, 75);

        backRct = new Rect((856/2 - 150/2), 642 - 80, 150, 44);

        coworkRct = new Rect(70, 200, 355, 175);
        studentRct = new Rect(430, 200, 355, 175);
    }

    void MainScreen()
    {
        if (screen != UIScreen.Main) return;
		//if (controller.Axies[XboxAxis.LAnalog].y == -1 && controller.prevAxies[XboxAxis.LAnalog].y == 0)
		//{ 

		//	ActiveButton--;
		//	if (ActiveButton == MenuButton.Quit)
		//		ActiveButton = MenuButton.Play;
		//}

        if (GUI.Button(playRct, "", skin.GetStyle("Play Button")))
        {
            ChangeScreen(UIScreen.PlayMode);
        }
        if (GUI.Button(instRct, "", skin.GetStyle("Instruction Button")))
        {
            ChangeScreen(UIScreen.Instructions);
        }
        if (GUI.Button(creditRct, "", skin.GetStyle("Credit Button")))
        {
            ChangeScreen(UIScreen.Credits);
        }
        if (GUI.Button(quitRct, "", skin.GetStyle("Quit Button")))
        {
            Application.Quit();
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
        if (GUI.Button(studentRct, "", skin.GetStyle("Student Opt")))
        {
            Application.LoadLevel(1);
        }

    }
    
	
    void ChangeScreen(UIScreen screen)
    {
        switch (screen)
        {
            case UIScreen.None:       spRender.sprite = null; break;
            case UIScreen.Main:       spRender.sprite = imgList[0]; break;
            case UIScreen.PlayMode:   spRender.sprite = imgList[1]; break;
            case UIScreen.Credits:    spRender.sprite = imgList[2]; break;
			case UIScreen.Instructions: spRender.sprite = imgList[3]; break;
        }
        this.screen = screen;
    }

	void Instructions()
	{
		if(screen != UIScreen.Instructions) return;

		GUI.Box( screenRect, "", skin.GetStyle( "InstructionBackground" ) );
		if (GUI.Button(backRect, "", skin.GetStyle("Back Button")))
		{
			ChangeScreen(UIScreen.Main);
		}
	}

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / 856, 1.0f * Screen.height / 642, 1.0f));
        MainScreen();
        Credits();
        PlayMode();
		Instructions();
    }
}

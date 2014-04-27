using UnityEngine;
using System.Collections.Generic;

public class MenuBackGround : MonoBehaviour 
{
    public enum UIScreen { None, Main, PlayMode, Credits };
    public UIScreen screen;

    public GUISkin skin; 

    List<Sprite> imgList = new List<Sprite>();
    SpriteRenderer spRender;

    #region GUI Rects

    private Rect playRct;
    private Rect instRct;
    private Rect creditRct;
    private Rect quitRct;

    private Rect backRct;

    public Rect coworkRct;
    private Rect studentRct;

    #endregion 

    void Start () 
    {
        imgList.Add((Sprite)Resources.Load("Sprites/GUI/Menu/mainMenu-home", typeof(Sprite)));
        imgList.Add((Sprite)Resources.Load("Sprites/GUI/Menu/mainMenu-playmode", typeof(Sprite)));
        imgList.Add((Sprite)Resources.Load("Sprites/GUI/Menu/mainMenu-credits", typeof(Sprite)));

        Debug.Log("Width: " +Screen.width + " Height: " + Screen.height);

        spRender = this.gameObject.GetComponent<SpriteRenderer>();
        screen = UIScreen.Main;

        ChangeScreen(screen);
        Init();
	}

    void Init()
    {
        
        playRct = new Rect(328, 207, 200, 75);
        instRct = new Rect(213, 300, 431, 75);
        creditRct = new Rect(287, 400, 282, 75);
        quitRct = new Rect(328, 500, 203, 75);

        backRct = new Rect(((Screen.width /2) - 150/2), Screen.height - 80, 150, 44);

        coworkRct = new Rect(70, 200, 355, 175);
        studentRct = new Rect(430, 200, 355, 175);
    }

    void MainScreen()
    {
        if (screen != UIScreen.Main) return;

        if (GUI.Button(playRct, "", skin.GetStyle("Play Button")))
        {
            ChangeScreen(UIScreen.PlayMode);
        }
        if (GUI.Button(instRct, "", skin.GetStyle("Instruction Button")))
        {
            //ChangeScreen(UIScreen.Credits);
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
        }
        this.screen = screen;
    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / 856, 1.0f * Screen.height / 642, 1.0f));
        MainScreen();
        Credits();
        PlayMode();
    }
}

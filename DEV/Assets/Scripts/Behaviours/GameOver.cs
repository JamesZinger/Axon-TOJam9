using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour 
{
	public Data dataobject;
	private Rect ScreenRect;
	private Rect AllenKeyRect;
	private Rect QuitRect;
	private Rect TotalRect;
	private float SpentMoney;
	public GUISkin skin;
	

	void Start()
	{
		dataobject = GameObject.FindObjectOfType<Data>();

		ScreenRect = new Rect(0,0, Screen.width, Screen.height);
		AllenKeyRect = new Rect(Screen.width/2 - 50, Screen.height/2 + Screen.height / 25, Screen.width/3, Screen.height/3);
		QuitRect = new Rect(Screen.width/2 - Screen.width/6 , Screen.height * 0.9f, Screen.width/3, Screen.height/12);
		TotalRect = new Rect(Screen.width * 0.1f, Screen.height *0.3f, Screen.width * .85f, Screen.height * .1f);
		SpentMoney = 0;
		foreach( FurnitureManager.TemplateFurniture template in dataobject.CollectedFurniture )
		{
			SpentMoney += template.Price;
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
		if (dataobject.didWin)
			GUIWin();
		else
			GUILose();

		GUICommon();

	}

	void GUIWin()
	{
		GUI.Box( ScreenRect, "", skin.GetStyle( "WinBackground" ) );

		
	}

	void GUILose()
	{
		GUI.Box( ScreenRect, "", skin.GetStyle( "LoseBackground" ) );
	}

	void GUICommon()
	{
		GUI.Label(AllenKeyRect, "X " + dataobject.Allenkeys.ToString(), skin.GetStyle("AllenKey"));
		GUI.Label(TotalRect, "TOTAL $" + SpentMoney.ToString("0.00") + "!", skin.GetStyle("TotalStyle") );
		if (GUI.Button(QuitRect, "", skin.GetStyle("Quit")))
		{
			Destroy(dataobject.gameObject);
			Application.LoadLevel(0);
		}
	}

}

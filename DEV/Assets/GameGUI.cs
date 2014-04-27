using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour 
{
    GUISkin skin;

    // Allen Key
    private Rect keysGroupRect;
    private Rect alKeyRect;
    private Rect alTextRect;

    private Texture2D allenKeyImg ;
    private int keys;

    // Money
    private Rect cashRect;

    //MeatBalls
    private Rect meatGroupRct;
    private Texture2D emptyMeatBallImg;
    private Texture2D fullMeatBallImg;
    private Rect mb1, mb2, mb0;

	void Start () 
    {
        skin = Game.Instance.Skin;

        allenKeyImg = (Texture2D)Resources.Load("Sprites/GUI/UI/allanKeyUI", typeof(Texture2D));
        emptyMeatBallImg = (Texture2D)Resources.Load("Sprites/GUI/UI/fullMeatballUI", typeof(Texture2D));
        fullMeatBallImg = (Texture2D)Resources.Load("Sprites/GUI/UI/emptyMeatballUI", typeof(Texture2D));




        Init();
	}

    void Init()
    {
        // Allen key
        keysGroupRect = new Rect(0, 0, 100, 75);
        alKeyRect = new Rect(0, 0, 25, 40);
        alTextRect = new Rect(30, 5, 70, 75);

        // Cash
        cashRect = new Rect(856 - 150, 0, 150, 75);

        // meatballs
        meatGroupRct = new Rect(856/2, 0, 150, 75);

        mb0 = new Rect(0, 0, 50, 50);
        mb1 = new Rect(50, 0, 50, 50);
        mb2 = new Rect(100, 0, 50, 50);
        
    }
	
	void Update () 
    {
	
	}

    void OnGUI()
    {
        GUI.skin = skin;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / 856, 1.0f * Screen.height / 642, 1.0f));

        GUI.BeginGroup(keysGroupRect);
        {
            GUI.DrawTexture(alKeyRect, allenKeyImg);
            GUI.Label(alTextRect, "x " + Game.Instance.Player.AllanKeys);
        }
        GUI.EndGroup();

        GUI.Label(cashRect, "$" + Game.Instance.Player.Cash);

        GUI.BeginGroup(meatGroupRct);
        {
            // Lazy Code... I know  Ricardo
            if (Game.Instance.Player.MeatBallCount <= 0)
                GUI.DrawTexture(mb0, fullMeatBallImg);
            else GUI.DrawTexture(mb0, emptyMeatBallImg);

            if (Game.Instance.Player.MeatBallCount < 2)
                GUI.DrawTexture(mb1, fullMeatBallImg);
            else GUI.DrawTexture(mb1, emptyMeatBallImg);

            if (Game.Instance.Player.MeatBallCount < 3)
                GUI.DrawTexture(mb2, fullMeatBallImg);
            else GUI.DrawTexture(mb2, emptyMeatBallImg);
        }
        GUI.EndGroup();
    }
}

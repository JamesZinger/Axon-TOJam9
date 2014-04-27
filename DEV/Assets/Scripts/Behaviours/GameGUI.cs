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

    // Boosters
    Texture2D sale25;
    Texture2D sale50;
    Texture2D sale75;

    Texture2D meatBall;
    Texture2D distracted;

    private Rect boostRect;
    private Rect tipUseMB;
    Texture2D useMeatBallTipImg;

	void Start () 
    {
        skin = Game.Instance.Skin;

        allenKeyImg = (Texture2D)Resources.Load("Sprites/GUI/UI/allanKeyUI", typeof(Texture2D));
        emptyMeatBallImg = (Texture2D)Resources.Load("Sprites/GUI/UI/fullMeatballUI", typeof(Texture2D));
        fullMeatBallImg = (Texture2D)Resources.Load("Sprites/GUI/UI/emptyMeatballUI", typeof(Texture2D));

        sale25 = (Texture2D)Resources.Load("Sprites/GUI/UI/sale25", typeof(Texture2D));
        sale50 = (Texture2D)Resources.Load("Sprites/GUI/UI/sale50", typeof(Texture2D));
        sale75 = (Texture2D)Resources.Load("Sprites/GUI/UI/sale75", typeof(Texture2D));
        distracted = (Texture2D)Resources.Load("Sprites/GUI/UI/distracted", typeof(Texture2D));
        useMeatBallTipImg = (Texture2D)Resources.Load("Sprites/GUI/UI/useMeatballUI", typeof(Texture2D));




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

        // Boost Rect
        boostRect = new Rect((856/2) - 200, 500, 1024, 65);
        
        // Tip
        tipUseMB = new Rect((856/2) - 150, 0, 150, 50);
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
        GUI.DrawTexture(tipUseMB, useMeatBallTipImg);

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


        if (Game.Instance.Player.HasDiscount)
        {
            if (Game.Instance.Player.DiscountType == GiftCard.Discount.DIS_25)
                GUI.Label(boostRect, sale75);
            else if (Game.Instance.Player.DiscountType == GiftCard.Discount.DIS_50)
                GUI.Label(boostRect, sale75);
            else if (Game.Instance.Player.DiscountType == GiftCard.Discount.DIS_75)
                GUI.Label(boostRect, sale75);
        }
        if (Game.Instance.Player.Distracted)
        {
            GUI.Label(boostRect, distracted);
        }
    }
}

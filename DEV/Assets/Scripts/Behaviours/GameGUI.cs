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
	Texture2D meatballImg;

	void Start () 
	{
		skin = Game.Instance.Skin;

		allenKeyImg 	  = Resources.Load<Texture2D>("Sprites/GUI/UI/allanKeyUI"	  );
		emptyMeatBallImg  = Resources.Load<Texture2D>("Sprites/GUI/UI/fullMeatballUI" );
		fullMeatBallImg   = Resources.Load<Texture2D>("Sprites/GUI/UI/emptyMeatballUI");

		sale25 			  = Resources.Load<Texture2D>("Sprites/GUI/UI/sale25"		  );
		sale50 			  = Resources.Load<Texture2D>("Sprites/GUI/UI/sale50"		  );
		sale75 			  = Resources.Load<Texture2D>("Sprites/GUI/UI/sale75"		  );
		distracted 		  = Resources.Load<Texture2D>("Sprites/GUI/UI/distracted"	  );
		useMeatBallTipImg = Resources.Load<Texture2D>("Sprites/GUI/UI/useMeatballUI"  );
		meatballImg 	  = Resources.Load<Texture2D>("Sprites/GUI/UI/meatballBoost" );

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

		GUI.Label(cashRect, "$" + Game.Instance.Player.Cash.ToString("0.00"));
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
				GUI.Label(boostRect, sale25);
			else if (Game.Instance.Player.DiscountType == GiftCard.Discount.DIS_50)
				GUI.Label(boostRect, sale50);
			else if (Game.Instance.Player.DiscountType == GiftCard.Discount.DIS_75)
				GUI.Label(boostRect, sale75);
		}
		if (Game.Instance.Player.Distracted)
		{
			GUI.Label(boostRect, distracted);
		}
		if (Game.Instance.Player.Meatballed)
		{
			GUI.Label(boostRect, meatballImg);
		}
	}
}

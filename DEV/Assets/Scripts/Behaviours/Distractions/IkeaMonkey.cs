﻿using UnityEngine;
using System.Collections;

public class IkeaMonkey : PickUp
{
    const int TIME_ADDED = 10;

	protected override void Start () 
    {
        base.Start();
	}

    public override void AddPickUp()
    {
        Game.Instance.Player.Distracted = true;
        Game.Instance.Player.AddInvincibilityTime(10f);
        base.AddPickUp();
    }
	
}

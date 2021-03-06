﻿using UnityEngine;
using System.Collections;

public class Distraction : PickUp 
{
    protected float remainingTime;
    protected bool activated;

    //public enum Type { None, Cat, GoatOnPole, IkeaMoneky }
    //public Type type;

	protected override void Start () 
    {
        base.Start();
		pickupSound = Audiopocalypse.Sounds.Pickup_Distractor;
	}
	
	void Update () 
    {
        activated = false;
        if (remainingTime <= 0.0f) return;

        remainingTime -= Time.fixedDeltaTime;
        activated = true;
	}

    public virtual void AddTime()
    {
    }

    public float RemainingTime
    {
        get { return remainingTime; }
        set { remainingTime = value; }
    }
    public bool Activated
    {
        get { return activated; }
        set { activated = value; }
    }
}

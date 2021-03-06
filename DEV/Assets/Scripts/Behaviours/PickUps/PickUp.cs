﻿using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour 
{
    public enum PickUpType { MeatBalls, GiftCard, Cash }
	public Audiopocalypse.Sounds pickupSound;

    private PickUpType pickUp;

	protected virtual void Start () 
    {
        this.rigidbody2D.velocity = Game.Instance.ScrollSpeed;
	}

    public virtual void AddPickUp() 
    {
        if (this.gameObject == Game.Instance.Player.gameObject) return;
		Game.Instance.ap.PlayClip(pickupSound);
        Destroy(this.gameObject);
    }

    public PickUpType Pickup
    {
        get { return pickUp; }
        protected set { pickUp = value; }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
            AddPickUp();
    }

    void Update()
    {
        //CheckIfOnScreen();
    }

    void CheckIfOnScreen()
    {
        if (transform.position.x < -5)
            Destroy(this.gameObject);
    }
}

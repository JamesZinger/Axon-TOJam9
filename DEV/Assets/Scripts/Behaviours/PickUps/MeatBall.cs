using UnityEngine;
using System.Collections;

public class MeatBall : PickUp
{

	void Start () 
    {
        base.Start();
        this.Pickup = PickUpType.MeatBalls;
		pickupSound = Audiopocalypse.Sounds.Pickup_Meatball;
	}

    public override void AddPickUp()
    {
        Game.Instance.Player.MeatBallCount++;
        Game.Instance.Player.Meatballed = true;
        base.AddPickUp();
    }
}

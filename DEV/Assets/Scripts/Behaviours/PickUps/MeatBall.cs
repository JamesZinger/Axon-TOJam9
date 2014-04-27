using UnityEngine;
using System.Collections;

public class MeatBall : PickUp
{

	void Start () 
    {
        base.Start();
        this.Pickup = PickUpType.MeatBalls;
		pickupSound = Audiopocalypse.Sounds.Menu_Click;
	}

    public override void AddPickUp()
    {
        Game.Instance.Player.MeatBallCount++;
        Game.Instance.Player.Meatballed = true;
        base.AddPickUp();
    }
}

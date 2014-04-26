using UnityEngine;
using System.Collections;

public class MeatBall : PickUp
{

	void Start () 
    {
        base.Start();
        this.Pickup = PickUpType.MeatBalls;
	}

    public override void AddPickUp()
    {
        Game.Instance.Player.MeatBallCount++;
        base.AddPickUp();
    }
}

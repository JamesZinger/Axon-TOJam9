using UnityEngine;
using System.Collections;

public class MeatBall : PickUp
{

	void Start () 
    {
        this.Pickup = PickUpType.MeatBalls;
	}

    public override void AddPickUp()
    {
        Game.Instance.Player.PickUplist.Add(this);
    }

    public override void DoAction()
    {
 	    base.DoAction();
    }
}

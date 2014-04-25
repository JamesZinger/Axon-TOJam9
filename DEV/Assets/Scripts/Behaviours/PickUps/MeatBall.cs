using UnityEngine;
using System.Collections;

public class MeatBall : PickUp
{

	void Start () 
    {
        this.pickUp = PickUpType.MeatBalls;
	}

    public override PickUp.PickUpType AddPickUp()
    {
        return base.AddPickUp();
    }

    public override void DoAction()
    {
 	    base.DoAction();
    }
}

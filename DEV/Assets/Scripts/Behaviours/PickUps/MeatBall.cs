using UnityEngine;
using System.Collections;

public class MeatBall : PickUp
{

	void Start () 
    {
        this.pickUp = PickUpType.MeatBalls;
	}

    public override void DoAction()
    {
 	    base.DoAction();
        // Add To Player PicUp List
    }
}

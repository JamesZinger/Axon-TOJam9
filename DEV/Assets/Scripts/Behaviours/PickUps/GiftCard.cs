using UnityEngine;
using System.Collections;

public class GiftCard : PickUp 
{
    void Start()
    {
        base.Start();
        this.Pickup = PickUpType.GiftCard;
    }

    public override void  AddPickUp()
    {
        Game.Instance.Player.AddDiscountTime();
 	    base.AddPickUp();
    }

}

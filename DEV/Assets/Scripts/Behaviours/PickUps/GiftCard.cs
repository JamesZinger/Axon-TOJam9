using UnityEngine;
using System.Collections;

public class GiftCard : PickUp 
{
    void Start()
    {
        base.Start();
        this.Pickup = PickUpType.GitCard;
    }

    public override void  AddPickUp()
    {
        Game.Instance.Player.AddDiscountTime();
 	    base.AddPickUp();
    }

}

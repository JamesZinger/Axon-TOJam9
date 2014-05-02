using UnityEngine;
using System.Collections;

public class GiftCard : PickUp 
{
    public enum Discount { None, DIS_25, DIS_50, DIS_75 }
    public Discount discount;

    protected override void Start()
    {
        base.Start();
        this.Pickup = PickUpType.GiftCard;
		pickupSound = Audiopocalypse.Sounds.Pickup_Card;

        switch(Random.Range(0, 3))
        {
            case 0: discount = Discount.DIS_25; break;
            case 1: discount = Discount.DIS_50; break;
            case 2: discount = Discount.DIS_75; break;
        }
    }

    public override void  AddPickUp()
    {
        Game.Instance.Player.AddDiscount(discount);

 	    base.AddPickUp();
    }

}

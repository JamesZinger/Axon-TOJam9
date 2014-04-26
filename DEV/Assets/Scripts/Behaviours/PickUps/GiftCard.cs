using UnityEngine;
using System.Collections;

public class GiftCard : PickUp 
{
    void Start()
    {
        base.Start();
        this.Pickup = PickUpType.GitCard;
    }

}

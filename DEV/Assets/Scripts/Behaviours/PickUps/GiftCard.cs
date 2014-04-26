using UnityEngine;
using System.Collections;

public class GiftCard : PickUp 
{
    void Start()
    {
        this.Pickup = PickUpType.GitCard;
    }

}

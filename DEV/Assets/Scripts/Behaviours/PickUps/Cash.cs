using UnityEngine;
using System.Collections;

public class Cash : PickUp 
{
    public Sprite s1;
    public Sprite s2;
    public Sprite s3;

    SpriteRenderer sprite;

    public enum Amount { None, Fifty, One_Hundred, One_Hundred_Fifty}
    public Amount amount;

	void Start () 
    {
        base.Start();
        this.Pickup = PickUpType.Cash;
		pickupSound = Audiopocalypse.Sounds.Pickup_Money;
        int rand = Random.Range(0, 3);

        sprite = this.gameObject.GetComponent<SpriteRenderer>();

        switch (rand)
        {
            case 0: amount = Amount.Fifty; sprite.sprite = s1; break;
            case 1: amount = Amount.One_Hundred; sprite.sprite = s2; break;
            case 2: amount = Amount.One_Hundred_Fifty; sprite.sprite = s3; break;
        }
	}

    public override void AddPickUp()
    {
        switch (amount)
        {
            case Amount.Fifty: Game.Instance.Player.Cash += 50;                 break;
            case Amount.One_Hundred: Game.Instance.Player.Cash += 100;          break;
            case Amount.One_Hundred_Fifty: Game.Instance.Player.Cash += 150;    break;
        }
         
        base.AddPickUp();
    }
}

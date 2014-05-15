using UnityEngine;
using System.Collections;

public class Cash : PickUp 
{
    public Sprite s1;
    public Sprite s2;
    public Sprite s3;

    SpriteRenderer sprite;

    public enum Amount { None, Fifty = 50, One_Hundred = 100, One_Hundred_Fifty = 150}
    public Amount amount;

	public delegate void CoinCollectedHandeler( Cash c );
	public event CoinCollectedHandeler CoinCollected;

	protected override void Start() 
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
		CoinCollected += Game.Instance.Player.OnMoneyCollected;
		CoinCollected += Game.Instance.OnMoneyCollected;
	}

    public override void AddPickUp()
    {
		CoinCollected(this);
                        
        base.AddPickUp();
    }
}

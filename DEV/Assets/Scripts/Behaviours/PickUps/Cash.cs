using UnityEngine;
using System.Collections;

public class Cash : PickUp 
{
    public enum Amount { Fifty, One_Hundred, One_Hundred_Fifty}
    public Amount amount;

	void Start () 
    {
        base.Start();
        this.Pickup = PickUpType.Cash;

        int rand = Random.Range(0, 3);

        switch (rand)
        {
            case 0: amount = Amount.Fifty;              break;
            case 1: amount = Amount.One_Hundred;        break;
            case 2: amount = Amount.One_Hundred_Fifty;  break;
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

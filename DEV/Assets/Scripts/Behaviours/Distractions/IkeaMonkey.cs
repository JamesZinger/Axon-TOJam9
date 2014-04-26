using UnityEngine;
using System.Collections;

public class IkeaMonkey : PickUp
{
    const int TIME_ADDED = 10;

	void Start () 
    {
        base.Start();
	}

    public override void AddPickUp()
    {
        Game.Instance.Player.Invincible = true;
        Game.Instance.Player.AddInvincibilityTime(15f);
        base.AddPickUp();
    }
	
}

using UnityEngine;
using System.Collections;

public class Cats : PickUp
{
    const int TIME_ADDED = 15;

    protected override void Start()
    {
        base.Start();
    }

    public override void AddPickUp()
    {
        Game.Instance.Player.Distracted = true;
        Game.Instance.Player.AddInvincibilityTime(10f);
        base.AddPickUp();
    }
}

using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour 
{
    public enum PickUpType { MeatBalls, Money, GitCard }

    private PickUpType pickUp;

	void Start () 
    {
	}

    public virtual void AddPickUp()
    {
    }

    public virtual void DoAction()
    {
        
    }

    public PickUpType Pickup
    {
        get { return pickUp; }
        set { pickUp = value; }
    }
}

using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour 
{
    public enum PickUpType { MeatBalls, Money, GitCard }

    protected PickUpType pickUp;

	void Start () 
    {
	}

    public virtual PickUpType AddPickUp()
    {
        return this.pickUp;
    }

    public virtual void DoAction()
    {
        
    }
}

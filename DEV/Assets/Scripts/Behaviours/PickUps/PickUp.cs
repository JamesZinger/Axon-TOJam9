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
        Destroy(this.gameObject);
    }

    public PickUpType Pickup
    {
        get { return pickUp; }
        set { pickUp = value; }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered");

        AddPickUp();
    }
}

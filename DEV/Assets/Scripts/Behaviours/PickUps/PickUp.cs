using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour 
{
    public enum PickUpType { MeatBalls, Money, GitCard, Cash }

    private PickUpType pickUp;

	protected void Start () 
    {
        this.rigidbody2D.velocity = Game.Instance.ScrollSpeed;
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

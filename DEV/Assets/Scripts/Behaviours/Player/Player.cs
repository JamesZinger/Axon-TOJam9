using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
    // Jump Force
    public Vector2 jumpForce;

    // Event Handlers
	public delegate void JumpHandeler();
	public event JumpHandeler Jump;

    // Class Variables
    private int cash;
    private bool hasDiscount;
    List<PickUp> pickUplist = new List<PickUp>();

    private float discountRemainingTime;

	void Start () 
    {
        Game.Instance.Player = this;
		Game.Instance.Controls.JumpButton += OnJump;
	}

	void Update () 
    {
        Debug.Log("Discount: " + hasDiscount);
        Discount();
	}
    
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 50), "Cash: $" + this.cash + " MeatBalls: " + pickUplist.Count + " Discount Time: " + discountRemainingTime + "Has Discount: " + hasDiscount);
    }

    void OnJump()
    {
        gameObject.rigidbody2D.AddForce(jumpForce);

		if (Jump != null)
			Jump();
    }

    void Discount()
    {
        if (discountRemainingTime <= 0.0f) { hasDiscount = false; return; } 

        hasDiscount = true;
        discountRemainingTime -= Time.fixedDeltaTime;
    }

    

    public List<PickUp> PickUplist
    {
        get { return pickUplist; }
        set { pickUplist = value; }
    }
    public int Cash
    {
        get { return cash; }
        set { cash = value; }
    }
    public float DiscountRemainingTime
    {
        get { return discountRemainingTime; }
        set { discountRemainingTime = value; }
    }
    public void AddDiscountTime()
    {
        discountRemainingTime += 5.0f;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
    List<PickUp> pickUplist = new List<PickUp>();
    
    public Vector2 jumpForce;

	public delegate void JumpHandeler();
	public event JumpHandeler Jump;

	void Start () 
    {
        Game.Instance.Player = this;
		Game.Instance.Controls.JumpButton += OnJump;
	}

	void Update () 
    {
        Debug.Log("Count: " + pickUplist.Count);
	}

    void OnJump()
    {
        gameObject.rigidbody2D.AddForce(jumpForce);

		if (Jump != null)
			Jump();
    }

    public List<PickUp> PickUplist
    {
        get { return pickUplist; }
        set { pickUplist = value; }
    }
}

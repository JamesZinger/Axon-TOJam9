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
        Debug.Log(Game.Instance.Controls);
		Game.Instance.Controls.JumpButton += OnJump;
	}

	void Update () 
    {
	
	}

    void OnJump()
    {
        gameObject.rigidbody2D.AddForce(jumpForce);
        Debug.Log("Helo");

		if (Jump != null)
			Jump();
    }

    public List<PickUp> PickUplist
    {
        get { return pickUplist; }
        set { pickUplist = value; }
    }
}

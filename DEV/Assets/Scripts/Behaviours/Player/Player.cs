using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
    List<PickUp> pickUplist = new List<PickUp>();
    public Vector2 jumpForce;

	void Start () 
    {
		Game.Instance.Controls.JumpButton += OnJump;
	}

	void Update () 
    {
	
	}

    void UserInput()
    {
        Game.Instance.Controls.JumpButton += OnJump;
    }

    void OnJump()
    {
        gameObject.rigidbody2D.AddForce(jumpForce);
    }
}

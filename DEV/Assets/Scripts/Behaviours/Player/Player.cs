using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
    List<PickUp> pickUplist = new List<PickUp>();
    public Vector2 jumpForce;

	//public delegate void 

	void Start () 
    {
		Game.Instance.Controls.JumpButton += OnJump;
	}

	void Update () 
    {
	
	}

    void UserInput()
    {
    }

    void OnJump()
    {
        gameObject.rigidbody2D.AddForce(jumpForce);
    }
}

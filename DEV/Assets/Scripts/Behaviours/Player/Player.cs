using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
    List<PickUp> pickUplist = new List<PickUp>();
	public List<PickUp> PickUplist
	{
		get { return pickUplist; }
		set { pickUplist = value; }
	}

    public Vector2 jumpForce;

	private bool hasDoubleJumped = false;
	public bool HasDoubleJumped
	{
		get { return hasDoubleJumped; }
		private set { hasDoubleJumped = value; }
	}

	private bool isGrounded = true;
	public bool IsGrounded
	{
		get { return isGrounded; }
		private set { isGrounded = value; }
	}


	#region Events

	public delegate void JumpHandeler();
	public event JumpHandeler Jump;

	#endregion

	void Awake () 
    {	
		Game.Instance.Player = this;
		Game.Instance.Controls.JumpButton += OnJump;
	}

	void Update () 
    {

	}

    void OnJump()
    {
		if (IsGrounded  == true)
		{ 
			HasDoubleJumped = false;
			gameObject.rigidbody2D.AddForce(jumpForce);

			if ( Jump != null )
				Jump();
		}
		
		else if (HasDoubleJumped == false)
		{
			HasDoubleJumped = true;
			
			gameObject.rigidbody2D.AddForce(jumpForce);
		}
		

		IsGrounded = false;

		StartCoroutine(CheckIfGrounded());
    }

	IEnumerator CheckIfGrounded()
	{

		yield return new WaitForEndOfFrame();

	}


}

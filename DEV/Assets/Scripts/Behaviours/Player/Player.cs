using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{

	public Vector2 jumpForce;

	public bool HasDoubleJumped
	{
		get { return hasDoubleJumped; }
		private set { hasDoubleJumped = value; }
	}

	public bool IsGrounded
	{
		get { return isGrounded; }
		private set { isGrounded = value; }
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

	public int MeatBallCount
	{
		get { return meatBallCount; }
		set { meatBallCount = value; }
	}

	#region Events

	public delegate void JumpHandeler();
	public event JumpHandeler Jump;

	#endregion

	#region Fields

	private int meatBallCount;
	private int cash;
    private bool hasDiscount;
	private SpriteRenderer sprite;
	private int rayFilter;
	private float discountRemainingTime;
	private bool isGrounded = true;
	private bool hasDoubleJumped = false;
	private bool isSliding = false;
	#endregion

	#region Unity Events

	void Awake () 
    {	
		Game.Instance.Player = this;
		sprite = gameObject.GetComponent<SpriteRenderer>();
		

		if (sprite == null || sprite.sprite == null)
		{ 
			Debug.LogError("Player sprite renderer variable is null");
			return;
		}

		int layerMask = LayerMask.NameToLayer("Ground");

		rayFilter = 1 << layerMask;

		meatBallCount = 0;
	}

	void Start()
	{
		GameControls controls = Game.Instance.Controls;
		controls.JumpButton			+= OnJump;
		controls.UseItemButton		+= OnUseItem;
		controls.SlideButton		+= OnSlide;
		controls.StopSlideButton	+= OnStopSlide;
		controls.UseShortcutButton	+= OnUseShortcut;
	}

	void Update () 
    {
        Discount();
	}
    
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 50), "Cash: $" + this.cash + " MeatBalls: " + MeatBallCount + " Discount Time: " + discountRemainingTime + "Has Discount: " + hasDiscount);
    }

	#endregion

	#region Event Handelers

	void OnJump()
	{
		if (isSliding == true)
			return;

		if ( IsGrounded == true )
		{
			IsGrounded = false;
			HasDoubleJumped = false;

			gameObject.rigidbody2D.velocity = jumpForce;

			if ( Jump != null )
				Jump();

			StartCoroutine( CheckIfGrounded() );
		}

		else if ( HasDoubleJumped == false )
		{
			HasDoubleJumped = true;
			gameObject.rigidbody2D.velocity = jumpForce;
		}

	}

	void OnUseItem()
	{
		if (MeatBallCount > 0)
		{

			MeatBallCount --;
		}
	}

	void OnSlide()
	{
		if (IsGrounded != true)
			return;



	}

	void OnStopSlide()
	{

	}

	void OnUseShortcut()
	{

	}

	#endregion

	void Discount()
    {
        if (discountRemainingTime <= 0.0f) { hasDiscount = false; return; } 

        hasDiscount = true;
        discountRemainingTime -= Time.fixedDeltaTime;
    }

	IEnumerator CheckIfGrounded()
	{
		yield return new WaitForSeconds(0.1f);
		while ( true )
		{
			Vector2 origin = new Vector2( transform.position.x, transform.position.y );

			RaycastHit2D hit;
			hit = Physics2D.Raycast( origin, Vector2.up * -1, 1000, rayFilter );

			if ( hit != null )
			{
				Vector2 hitVector = hit.point - origin;
				//Debug.Log(hitVector.magnitude);
				if ( hitVector.magnitude < 0.703f )
				{
					IsGrounded = true;
					HasDoubleJumped = false;
					break;
				}
			}

			yield return new WaitForEndOfFrame();
		}
	}

    public void AddDiscountTime()
    {
        discountRemainingTime += 5.0f;
	}


}

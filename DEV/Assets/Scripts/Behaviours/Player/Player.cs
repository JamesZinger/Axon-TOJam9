using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{

	private enum PlayerState { Walking = 0, Jumping, Sliding }
	private PlayerState state;


	public Vector2 jumpForce;
    public float InitialCash;
	public bool Meatballed;
    private int allanKeys;

    public int AllanKeys
    {
        get { return allanKeys; }
        set { allanKeys = value; }
    }

    public bool Distracted
    {
        get { return distracted; }
        set 
        { 
            distracted = value;
            invincibillityRemainingTime += 10.0f;
        }
    }

	public bool HasDoubleJumped
	{
		get { return hasDoubleJumped; }
		private set { hasDoubleJumped = value; }
	}

	public float Cash
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
    public GiftCard.Discount DiscountType
    {
        get { return discountType; }
        set { discountType = value; }
    }

	#region Events

	public delegate void JumpHandeler();
	public event JumpHandeler Jump;

	#endregion

	#region Fields

    private bool distracted;
	private bool isGrounded;
    private float invincibillityRemainingTime;
	private int meatBallCount;
	private float cash;
    public bool HasDiscount;
    private float discountRemainingTime;
	private SpriteRenderer spriteRenderer;
	private int rayFilter;
	private bool hasDoubleJumped = false;
	private GiftCard.Discount discountType;
    private float meatBalledRemainingTime;
	private Dictionary<PlayerState, PolygonCollider2D> colliderMap;
	private Dictionary<PlayerState, List<Sprite>> animationMap;
	private int walkAnimCounter;

	#endregion

	#region Unity Events

	void Awake () 
    {	
		Game.Instance.Player = this;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		
		PolygonCollider2D[] Colliders = gameObject.GetComponents<PolygonCollider2D>();

		animationMap = new Dictionary<PlayerState, List<Sprite>>();
		colliderMap  = new Dictionary<PlayerState, PolygonCollider2D>();
		
		colliderMap.Add( PlayerState.Walking, Colliders[ 0 ] );
		colliderMap.Add( PlayerState.Jumping, Colliders[ 1 ] );
		colliderMap.Add( PlayerState.Sliding, Colliders[ 2 ] );

		animationMap.Add( PlayerState.Walking, new List<Sprite>() );
		animationMap.Add( PlayerState.Jumping, new List<Sprite>() );
		animationMap.Add( PlayerState.Sliding, new List<Sprite>() );

		animationMap[ PlayerState.Walking ].Add( Resources.Load<Sprite>( "Sprites/Player/walking_animation_01" ) );
		animationMap[ PlayerState.Walking ].Add( Resources.Load<Sprite>( "Sprites/Player/walking_animation_02" ) );
		animationMap[ PlayerState.Walking ].Add( Resources.Load<Sprite>( "Sprites/Player/walking_animation_03" ) );
		animationMap[ PlayerState.Walking ].Add( Resources.Load<Sprite>( "Sprites/Player/walking_animation_04" ) );

		animationMap[ PlayerState.Jumping ].Add( Resources.Load<Sprite>( "Sprites/Player/jumping" ) );

		animationMap[ PlayerState.Sliding ].Add( Resources.Load<Sprite>( "Sprites/Player/sliding" ) );

		walkAnimCounter = 0;

		if (spriteRenderer == null || spriteRenderer.sprite == null)
		{ 
			Debug.LogError("Player sprite renderer variable is null");
			return;
		}

		int layerMask = LayerMask.NameToLayer("Ground");

		rayFilter = 1 << layerMask;

		meatBallCount = 0;
        cash = InitialCash;

		state = PlayerState.Walking;
	}

	void Start()
	{
		GameControls controls = Game.Instance.Controls;
		
		controls.JumpButton			+= OnJump;
		controls.UseItemButton		+= OnUseItem;
		controls.SlideButton		+= OnSlideButton;
		controls.StopSlideButton	+= OnStopSlide;
		controls.UseShortcutButton	+= OnUseShortcut;

		ChangeState( PlayerState.Walking );
	}

	void Update()
	{

		Discount();
		Distract();
		Meatball();

	}
    
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 50), "Cash: $" + this.cash + " MeatBalls: " + MeatBallCount + " Discount Time: " + discountRemainingTime + "Has Discount: " + HasDiscount + " Inv: " + distracted);
    }

	#endregion

	#region Event Handelers
	
	void OnJump()
	{
		if (Game.Instance.IsPaused)
			return;

		//if ( state == PlayerState.Sliding && isGrounded == false && HasDoubleJumped == false)
		//{
		//	HasDoubleJumped = true;
		//	gameObject.rigidbody2D.velocity = jumpForce;
		//	Game.Instance.ap.PlayClip( Audiopocalypse.Sounds.Jump );
		//}

		if ( state == PlayerState.Walking )
		{
			
			if (state != PlayerState.Sliding)
				ChangeState( PlayerState.Jumping );

			gameObject.rigidbody2D.velocity = jumpForce;

			isGrounded = false;

			Game.Instance.ap.PlayClip( Audiopocalypse.Sounds.Jump );
			if ( Jump != null )
				Jump();
	
		}

		else if ( state == PlayerState.Jumping && HasDoubleJumped == false )
		{
			HasDoubleJumped = true;
			gameObject.rigidbody2D.velocity = jumpForce;

			Game.Instance.ap.PlayClip( Audiopocalypse.Sounds.Jump );
		}
		
	}

	void OnUseItem()
	{
		if ( Game.Instance.IsPaused )
			return;

		if (MeatBallCount > 0)
		{
            Meatballed = true;
            meatBalledRemainingTime = 10;
            Debug.Log("Meatballs Activated");
			Game.Instance.ap.PlayClip(Audiopocalypse.Sounds.Menu_Click);
			MeatBallCount --;
		}
	}

	void OnSlideButton()
	{
		if ( Game.Instance.IsPaused )
			return;

		if ( state == PlayerState.Sliding ) return;

		Game.Instance.ap.PlayClip(Audiopocalypse.Sounds.Slide);
		ChangeState( PlayerState.Sliding );
	}

	void OnStopSlide()
	{
		if ( Game.Instance.IsPaused )
			return;

		if ( state != PlayerState.Sliding )
			return;

		if ( !isGrounded )
			ChangeState( PlayerState.Jumping ); 
		else
			ChangeState( PlayerState.Walking );
	}

	void OnUseShortcut()
	{
		if ( Game.Instance.IsPaused )
			return;
	}

	#endregion

    void Meatball()
    {
        if (!Meatballed) return;

        HasDiscount = false; discountRemainingTime = 0.0f;
        distracted = false; invincibillityRemainingTime = 0.0f;

        if (meatBalledRemainingTime <= 0.0f) { Meatballed = false; return; }

        meatBalledRemainingTime -= Time.fixedDeltaTime;
    }

	void Discount()
    {
        if (discountRemainingTime <= 0.0f) { HasDiscount = false; return; } 

        HasDiscount = true;
        Distracted = false; invincibillityRemainingTime = 0;
        Meatballed = false; meatBalledRemainingTime = 0.0f;

        discountRemainingTime -= Time.fixedDeltaTime;
    }

    void Distract()
    {
        if (!distracted) return;

        HasDiscount = false; discountRemainingTime = 0;
        Meatballed = false; meatBalledRemainingTime = 0.0f;

        if (invincibillityRemainingTime <= 0.0f) { distracted = false; return; }

        invincibillityRemainingTime -= Time.fixedDeltaTime;
        //Debug.Log("Dis time: "+ invincibillityRemainingTime);
    }
	
	IEnumerator WalkingAnimation()
	{
		while(true)
		{
			if ( state == PlayerState.Walking )
			{
				walkAnimCounter++;
				if ( walkAnimCounter >= 4 )
					walkAnimCounter = 0;

				spriteRenderer.sprite = animationMap[ PlayerState.Walking ][ walkAnimCounter ];
			}
			else
			{
				break;
			}

			yield return new WaitForSeconds( 0.10f );
		}
	}
  
	public void AddDiscount(GiftCard.Discount type)
    {
        discountRemainingTime = 5;

        this.DiscountType = type;
	}

    public void AddInvincibilityTime(float time)
    {
        invincibillityRemainingTime = 5;
    }

    public void DeductCash(float price)
    {
        
        if (HasDiscount)
        {
            switch (DiscountType)
            {
                case GiftCard.Discount.DIS_25: this.cash -= price * 0.75f; break;
                case GiftCard.Discount.DIS_50: this.cash -= price * 0.5f; break;
                case GiftCard.Discount.DIS_75: this.cash -= price * 0.25f; break;
            }
        }
        else this.cash -= price;

        if (cash < 0)
            Game.Instance.OutOfCoins();
    }

	private void ChangeState( PlayerState newState )
	{
		if ( newState == PlayerState.Walking )
		{
			colliderMap[ state ].enabled = false;
			colliderMap[ PlayerState.Walking ].enabled = true;
		}

		else if ( newState == PlayerState.Jumping )
		{
			StopCoroutine( "WalkingAnimation" );
			spriteRenderer.sprite = animationMap[ PlayerState.Jumping ][ 0 ];
			colliderMap[ state ].enabled = false;
			colliderMap[ PlayerState.Jumping ].enabled = true;
		}

		else if ( newState == PlayerState.Sliding )
		{
			StopCoroutine( "WalkingAnimation" );
			spriteRenderer.sprite = animationMap[ PlayerState.Sliding ][ 0 ];
			colliderMap[ state ].enabled = false;
			colliderMap[ PlayerState.Sliding ].enabled = true;
		}

		state = newState;

		if ( state == PlayerState.Walking )
			StartCoroutine( "WalkingAnimation" );
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if ( collision.gameObject.layer == LayerMask.NameToLayer( "Ground" ) )
		{
			//Debug.Log( rigidbody2D.velocity );
			if ( rigidbody2D.velocity.y <= 0f )
			{
				if ( state == PlayerState.Jumping )
					ChangeState( PlayerState.Walking );

//				if ( state == PlayerState.Sliding && isGrounded == false )

				HasDoubleJumped = false;
				isGrounded = true;
			}
		}
	}

}

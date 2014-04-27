using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{

	public Vector2 jumpForce;
    public float InitialCash;
	public Sprite jumpTexture, slideTexture;
	Sprite activeWalk;
	public Sprite[] walkAnims;
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

	public bool IsGrounded
	{
		get { return isGrounded; }
		private set { isGrounded = value; }
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
    private float invincibillityRemainingTime;
	private int meatBallCount;
	private float cash;
    public bool HasDiscount;
    private float discountRemainingTime;
	private SpriteRenderer sprite;
	private int rayFilter;
	private bool isGrounded = true;
	private bool hasDoubleJumped = false;
	private bool isSliding = false;
    private GiftCard.Discount discountType;
    private float meatBalledRemainingTime;

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
        cash = InitialCash;
	}

	void Start()
	{
		GameControls controls = Game.Instance.Controls;
		
		controls.JumpButton			+= OnJump;
		controls.UseItemButton		+= OnUseItem;
		controls.SlideButton		+= OnSlide;
		controls.StopSlideButton	+= OnStopSlide;
		controls.UseShortcutButton	+= OnUseShortcut;

		//walkAnims = new Sprite[4];
		activeWalk = walkAnims[0];
		StartCoroutine(walk());
	}

	void Update () 
    {

        Discount();
        Distract();
        Meatball();

        //Debug.Log(meatBalledRemainingTime);

		if(IsGrounded == true){
			SetSprite(activeWalk);
			Vector2 box = new Vector2(sprite.sprite.rect.width / 150,sprite.sprite.rect.height / 150);
			gameObject.GetComponent<BoxCollider2D>().size = box;
			gameObject.GetComponent<BoxCollider2D>().center = box/2;
		}
		if(isSliding){
			SetSprite(slideTexture);
			Vector2 box = new Vector2(sprite.sprite.rect.width / 150,sprite.sprite.rect.height / 150);
			gameObject.GetComponent<BoxCollider2D>().size = box;
			gameObject.GetComponent<BoxCollider2D>().center = box/2;
		}
	}
    
    void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 300, 50), "Cash: $" + this.cash + " MeatBalls: " + MeatBallCount + " Discount Time: " + discountRemainingTime + "Has Discount: " + HasDiscount + " Inv: " + distracted);
    }

	#endregion

	#region Event Handelers
	void SetSprite(Sprite sp){
		sprite.sprite = sp;
		Vector2 box = new Vector2(sprite.sprite.rect.width / 150,sprite.sprite.rect.height / 150);
		gameObject.GetComponent<BoxCollider2D>().size = box;
		gameObject.GetComponent<BoxCollider2D>().center = box/2;

	}
	
	void OnJump()
	{
		if (Game.Instance.IsPaused)
			return;

		if ( IsGrounded == true )
		{
			SetSprite(jumpTexture);		
			Vector2 box = new Vector2(sprite.sprite.rect.width / 150,sprite.sprite.rect.height / 150);
			gameObject.GetComponent<BoxCollider2D>().size = box;
			gameObject.GetComponent<BoxCollider2D>().center = box/2;
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
		Game.Instance.ap.PlayClip(Audiopocalypse.Sounds.Jump);
	}

	void OnUseItem()
	{
		if ( Game.Instance.IsPaused )
			return;

		if (MeatBallCount > 0)
		{
            Meatballed = true;
            //Debug.Log("Meatballs Activated");
			Game.Instance.ap.PlayClip(Audiopocalypse.Sounds.Menu_Click);
			MeatBallCount --;
		}
	}

	void OnSlide()
	{
		if ( Game.Instance.IsPaused )
			return;

		if(isSliding){
		}else{
			Game.Instance.ap.PlayClip(Audiopocalypse.Sounds.Slide);
			isSliding = true;
		}
	}

	void OnStopSlide()
	{
		if ( Game.Instance.IsPaused )
			return;

		isSliding = false;
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
				//Half height
				if ( hitVector.magnitude <= 1.0f )
				{
					SetSprite(activeWalk);
					Vector2 box = new Vector2(sprite.sprite.rect.width / 150,sprite.sprite.rect.height / 150);
					gameObject.GetComponent<BoxCollider2D>().size = box;
					IsGrounded = true;
					HasDoubleJumped = false;
					break;
				}
			}
			yield return new WaitForEndOfFrame();
		}
	}
	IEnumerator walk(){
		int count = 0;
		while(true){
			count++;
			activeWalk = walkAnims[count%4];
			yield return new WaitForSeconds(0.10f);
		}
	}
    public void AddDiscount(GiftCard.Discount type)
    {
        discountRemainingTime = 15;

        this.DiscountType = type;
	}

    public void AddInvincibilityTime(float time)
    {
        invincibillityRemainingTime = 15;
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

}

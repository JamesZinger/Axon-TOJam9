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

    private int allanKeys;

    public int AllanKeys
    {
        get { return allanKeys; }
        set { allanKeys = value; }
    }

    public bool Invincible
    {
        get { return invincible; }
        set 
        { 
            invincible = value;
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

	#region Events

	public delegate void JumpHandeler();
	public event JumpHandeler Jump;

	#endregion

	#region Fields

    private bool invincible;
    private float invincibillityRemainingTime;
	private int meatBallCount;
	private float cash;
    private bool hasDiscount;
    private float discountRemainingTime;
	private SpriteRenderer sprite;
	private int rayFilter;
	private bool isGrounded = true;
	private bool hasDoubleJumped = false;
	private bool isSliding = false;
    private GiftCard.Discount discountType;

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
        Invincibillity();
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
        GUI.Label(new Rect(0, 0, 300, 50), "Cash: $" + this.cash + " MeatBalls: " + MeatBallCount + " Discount Time: " + discountRemainingTime + "Has Discount: " + hasDiscount + " Inv: " + invincible);
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

	}

	void OnUseItem()
	{
		if ( Game.Instance.IsPaused )
			return;

		if (MeatBallCount > 0)
		{
            Debug.Log("Meatballs Activated");
			MeatBallCount --;
		}
	}

	void OnSlide()
	{
		if ( Game.Instance.IsPaused )
			return;

		if(isSliding){
		}else{
			//Game.Instance.Player.transform.position = Game.Instance.Player.transform.position - new Vector3(0, 1.5f, 0);
			isSliding = true;
		}
	}

	void OnStopSlide()
	{
		if ( Game.Instance.IsPaused )
			return;

		//Game.Instance.Player.transform.position = Game.Instance.Player.transform.position + new Vector3(0, 1.5f, 0);
		isSliding = false;
	}

	void OnUseShortcut()
	{
		if ( Game.Instance.IsPaused )
			return;
	}

	#endregion

	void Discount()
    {
        if (discountRemainingTime <= 0.0f) { hasDiscount = false; return; } 

        hasDiscount = true;
        discountRemainingTime -= Time.fixedDeltaTime;
    }

    void Invincibillity()
    {
        if (!invincible) return;

        if (invincibillityRemainingTime <= 0.0f) { invincible = false; return; }

        invincibillityRemainingTime -= Time.fixedDeltaTime;
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
        discountRemainingTime += 10.0f;

        this.discountType = type;
	}

    public void AddInvincibilityTime(float time)
    {
        invincibillityRemainingTime += time;
    }

    public void DeductCash(float price)
    {
        
        if (hasDiscount)
        {
            switch (discountType)
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

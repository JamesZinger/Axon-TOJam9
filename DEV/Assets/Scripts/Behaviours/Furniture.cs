using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour
{
	public int allanKeyValue = 0;
	public float price = 0;
	public DepartmentType department = DepartmentType.NONE;
	public string desc;
	public SpriteRenderer SpriteRenderer;
    public enum Zone { None, High, Medium, Low }
    public Zone zone;
	public FurnitureManager.TemplateFurniture templateSource;

    PolygonCollider2D c;

    private GUISkin skin;
    Rect labelRct;

	void Start () 
	{
		rigidbody2D.velocity = Game.Instance.ScrollSpeed;
        skin = Game.Instance.Skin;
        labelRct = new Rect(0, 0, 100, 25);
        c = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        //Vector2 tmp = Camera.main.WorldToScreenPoint(new Vector3(gameObject.renderer.bounds.center.x, gameObject.renderer.bounds.max.y, 1));
        Vector2 tmp = Camera.main.WorldToScreenPoint(new Vector3(gameObject.renderer.bounds.center.x, gameObject.renderer.bounds.max.y, 0));
        labelRct.Set(tmp.x - 50, (Screen.height - tmp.y) - (Screen.height/10), 100, 25);
        //Debug.Log("Size: " + gameObject.renderer.bounds.center);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

			if (Game.Instance.Player.Distracted)
			{
 				//Destroy(this.gameObject);
				return;
			}

            GameObject obj = Instantiate(Game.Instance.PointBurst, Vector3.zero, Quaternion.identity) as GameObject;
            PointBurst burst = obj.GetComponent<PointBurst>();

            burst.SetUpForFurniture(new Vector2(0, 0), this.price, this.allanKeyValue);

			Game.Instance.CollectedFuriture.Add(this.templateSource);

            if (Game.Instance.Player.HasDiscount)
            {
                switch (Game.Instance.Player.DiscountType)
                {
                    case GiftCard.Discount.DIS_25:
                        burst.SetUpForFurniture(new Vector2(0, 0), this.price * 0.75f, this.allanKeyValue);
                        break;
                    case GiftCard.Discount.DIS_50:
                        burst.SetUpForFurniture(new Vector2(0, 0), this.price * 0.50f, this.allanKeyValue);
                        break;
                    case GiftCard.Discount.DIS_75: 
                        burst.SetUpForFurniture(new Vector2(0, 0), this.price * 0.25f, this.allanKeyValue);
                        break;

                }
                Game.Instance.Player.DeductCash(this.price);
            }
            else
                Game.Instance.Player.DeductCash(this.price);


			Game.Instance.ap.PlayClip(Audiopocalypse.Sounds.Pickup_Furniture);
            Game.Instance.Player.AllanKeys += this.allanKeyValue;

			if (Game.Instance.ShoppingList.Contains(templateSource))
			{ 
				Game.Instance.ShoppingList.Remove(templateSource);
				if (Game.Instance.ShoppingList.Count == 0)
				{
					Game.Instance.Win();
				}
			}

			if (Game.Instance.Player.Cash < 0 && Game.Instance.state == Game.Gamestate.NONE)
			{
				Game.Instance.Lose();
			}
			

            Destroy(this.gameObject);
        }
    }


    void OnGUI()
    {
        GUI.skin = skin;
       //GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / 856, 1.0f * Screen.height / 642, 1.0f));

        GUI.Label(labelRct, this.name.ToUpper());


    }


}

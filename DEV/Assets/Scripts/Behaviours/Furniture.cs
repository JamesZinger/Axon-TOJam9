using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour
{
	public SpriteRenderer SpriteRenderer;
	public enum SpawnZone { None, High, Medium, Low }
	public SpawnZone Zone;
	public FurnitureTemplate Template;

	public delegate void	FuritureCollectedHandeler( FurnitureTemplate furniture );
	public event			FuritureCollectedHandeler FurnitureCollected;

	//private PolygonCollider2D c;
	private GUISkin skin;
	private Rect labelRct;

	#region Unity Events

	void Start() 
	{
		rigidbody2D.velocity = Game.Instance.ScrollSpeed;
		skin = Game.Instance.Skin;
		labelRct = new Rect(0, 0, 100, 25);
		//c = GetComponent<PolygonCollider2D>();

		FurnitureCollected += Game.Instance.Player.OnFurnitureCollected;
		FurnitureCollected += Game.Instance.OnFurnitureCollected;
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

			
			if (FurnitureCollected != null)
				FurnitureCollected( Template );

			Destroy(this.gameObject);
		}
	}

	void OnGUI()
	{

		if ( Game.Instance.IsPaused ) return;

		GUI.skin = skin;
	   //GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / 856, 1.0f * Screen.height / 642, 1.0f));

		GUI.Label(labelRct, this.name.ToUpper());


	}

	#endregion
}

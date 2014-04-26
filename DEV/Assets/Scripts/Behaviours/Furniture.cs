using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour
{
	public int allanKeyValue = 0;
	public float price = 0;
	public string name = "";
	public DepartmentType department = DepartmentType.NONE;
	public string desc;
	public SpriteRenderer SpriteRenderer;
    public enum Zone { None, High, Medium, Low }
    public Zone zone;

	void Start () 
	{
		rigidbody2D.velocity = Game.Instance.ScrollSpeed;
    }

    void Update()
    {
    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Game.Instance.Player.DeductCash(this.price);
            Destroy(this.gameObject);
        }
    }



}

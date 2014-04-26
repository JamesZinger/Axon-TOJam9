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

	void Start () 
	{
		rigidbody2D.velocity = Game.Instance.ScrollSpeed;
    }

    void Update()
    {

    }



}

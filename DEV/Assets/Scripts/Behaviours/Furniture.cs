using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour
{
    private int allanKeyValue = 0;
    private float price = 0;
    private string name = "";
    private DepartmentType department = DepartmentType.NONE;
    private Texture2D texture;
    private string desc;

	void Start () 
	{
		
    }

    void Update()
    {

    }

    public void SetPrice(string s)
    {
        this.price = float.Parse(s);
    }
    public void SetAllenKeyVal(string s)
    {
        this.allanKeyValue = int.Parse(s);
    }
    public void SetTexture(string s)
    {
        //Debug.Log(s);
        this.texture = (Texture2D)Resources.Load("FurnitureTextures/" + s, typeof(Texture2D));
        //Debug.Log(texture);
    }



    public float Price
    {
        get { return price; }
        set { price = value; }
    }
    public int AllanKeyValue
    {
        get { return allanKeyValue; }
        set { allanKeyValue = value; }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string Desc
    {
        get { return desc; }
        set { desc = value; }
    }
    public Texture2D Texture
    {
        get { return texture; }
        set { texture = value; }
    }
    public DepartmentType Department
    {
        get { return department; }
        set { department = value; }
    }
}

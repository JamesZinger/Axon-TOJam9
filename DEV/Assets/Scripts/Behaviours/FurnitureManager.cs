using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShoppingListPair
{
	private FurnitureTemplate	template;
	private bool				hasBeenCollected;
	
	public FurnitureTemplate Template
	{
		get { return template; }
		set { template = value; }
	}
	public bool HasBeenCollected
	{
		get { return hasBeenCollected; }
		set { hasBeenCollected = value; }
	}

	public ShoppingListPair( FurnitureTemplate Template )
	{
		this.Template = Template;
		HasBeenCollected = false;
	}
}

public class FurnitureManager : MonoBehaviour
{


	#region Private Fields

	private TextAsset txt;

	#endregion

	// Public Fields
	public Dictionary<DepartmentType, List<FurnitureTemplate>> furnitureMap;
	
	#region Unity Events 

	void Awake () 
	{
		furnitureMap = new Dictionary<DepartmentType, List<FurnitureTemplate>>();

		furnitureMap.Add( DepartmentType.BathRoom	, new List<FurnitureTemplate>() );
		furnitureMap.Add( DepartmentType.BedRoom	, new List<FurnitureTemplate>() );
		furnitureMap.Add( DepartmentType.Childrens	, new List<FurnitureTemplate>() );
		furnitureMap.Add( DepartmentType.DiningRoom	, new List<FurnitureTemplate>() );
		furnitureMap.Add( DepartmentType.Kitchen	, new List<FurnitureTemplate>() );
		furnitureMap.Add( DepartmentType.LivingRoom	, new List<FurnitureTemplate>() );
		furnitureMap.Add( DepartmentType.Workspaces	, new List<FurnitureTemplate>() );

		ReadCSV();


	}

	void Start()
	{
		Game.Instance.FurnitureManager = this;
	}

	#endregion

	void ReadCSV()
	{
		txt = (TextAsset)Resources.Load("ikea2", typeof(TextAsset));

		string[] line = txt.text.Split('\n');

		for (int i = 1; i < line.Length -1; ++i)
		{
			FurnitureTemplate furniture = new FurnitureTemplate();
			string[] values = line[i].Split(',');

			for ( int j = 0; j < values.Length; ++j )
			{
				if ( j == 0 ) furniture.SetTexture( values[ j ] );
				if ( j == 1 ) furniture.Description = values[ j ];
				if ( j == 2 ) furniture.Name = ( values[ j ] );
				if ( j == 3 ) furniture.SetDepartment( values[ j ] );
				if ( j == 4 ) furniture.Price = float.Parse( values[ j ] );
				if ( j == 5 ) furniture.AllanKeys = int.Parse( values[ j ] );
				if ( j == 6 ) furniture.SetZone( values[ j ] );
			}
			furnitureMap[ furniture.Department ].Add( furniture );
		}
	}


}

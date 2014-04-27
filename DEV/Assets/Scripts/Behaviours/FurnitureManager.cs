using UnityEngine;
using System.Collections.Generic;

public class FurnitureManager : MonoBehaviour 
{
	public struct TemplateFurniture
	{
		public Sprite Sprite;
		public string Description;
		public string Name;
		public DepartmentType Department;
		public float Price;
		public int AllanKeys;
        public Furniture.Zone Zone;

		public void SetDepartment( string s )
		{
			if ( s.Contains( "Bathroom" ) ) this.Department = DepartmentType.BathRoom;
			if ( s.Contains( "Bedroom" ) ) this.Department = DepartmentType.BedRoom;
			if ( s.Contains( "Children" ) ) this.Department = DepartmentType.Childrens;
			if ( s.Contains( "Dining" ) ) this.Department = DepartmentType.DiningRoom;
			if ( s.Contains( "Kitchen" ) ) this.Department = DepartmentType.Kitchen;
			if ( s.Contains( "Living Room" ) ) this.Department = DepartmentType.LivingRoom;
			if ( s.Contains( "Workspace" ) ) this.Department = DepartmentType.Workspaces;

			//Debug.Log(s);
			//Debug.Log(this.Department);
		}

		public void SetTexture( string s )
		{
			//Debug.Log(s);
			this.Sprite = (Sprite)Resources.Load( "FurnitureTextures/" + s, typeof( Sprite ) );

			//Debug.Log(texture);
		}

        public void SetZone(string s)
        {
            //Debug.Log(s);
            if (s.Contains("0")) { this.Zone = Furniture.Zone.Low; return; }
            if (s.Contains("1")) { this.Zone = Furniture.Zone.Medium; return; }
            if (s.Contains("2")) { this.Zone = Furniture.Zone.High; return; }
        }
    }

   public  Dictionary<DepartmentType, List<TemplateFurniture>> furnitureMap;
    TextAsset txt;

	void Awake () 
    {
        furnitureMap = new Dictionary<DepartmentType, List<TemplateFurniture>>();

        furnitureMap.Add(DepartmentType.BathRoom, new List<TemplateFurniture>());
        furnitureMap.Add(DepartmentType.BedRoom, new List<TemplateFurniture>());
        furnitureMap.Add(DepartmentType.Childrens, new List<TemplateFurniture>());
        furnitureMap.Add(DepartmentType.DiningRoom, new List<TemplateFurniture>());
        furnitureMap.Add(DepartmentType.Kitchen, new List<TemplateFurniture>()); 
        furnitureMap.Add(DepartmentType.LivingRoom, new List<TemplateFurniture>());
        furnitureMap.Add(DepartmentType.Workspaces, new List<TemplateFurniture>());

	

        ReadCSV();
	}

	void Start()
	{
		Game.Instance.FurnitureManager = this;
	}

    void ReadCSV()
    {
        txt = (TextAsset)Resources.Load("ikea2", typeof(TextAsset));

        string[] line = txt.text.Split('\n');

		for (int i = 1; i < line.Length -1; ++i)
		{
			TemplateFurniture furniture = new TemplateFurniture();
			string[] values = line[i].Split(',');

			for (int j = 0; j < values.Length; ++j)
			{
				if (j == 0) furniture.SetTexture(values[j]);
				if (j == 1) furniture.Description = values[j];
				if (j == 2) furniture.Name = (values[j]);
				if (j == 3) furniture.SetDepartment(values[j]);
				if (j == 4) furniture.Price = float.Parse(values[j]);
				if (j == 5) furniture.AllanKeys = int.Parse(values[j]);
                if (j == 6) furniture.SetZone(values[j]);
			 }
			//Debug.Log(furniture.Zone);
			furnitureMap[furniture.Department].Add(furniture);
		}
    }
	
	void Update () 
    {
	
	}
}

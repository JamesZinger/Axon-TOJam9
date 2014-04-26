using UnityEngine;
using System.Collections.Generic;

public class FurnitureManager : MonoBehaviour 
{
    Dictionary<DepartmentType, List<Furniture>> furnitureMap;    
    TextAsset txt;

	void Awake () 
    {
        FurnitureMap = new Dictionary<DepartmentType, List<Furniture>>();

        FurnitureMap.Add(DepartmentType.BathRoom, new List<Furniture>());
        FurnitureMap.Add(DepartmentType.BedRoom, new List<Furniture>());
        FurnitureMap.Add(DepartmentType.Chlidrens, new List<Furniture>());
        FurnitureMap.Add(DepartmentType.DiningRoom, new List<Furniture>());
        FurnitureMap.Add(DepartmentType.Kitchen, new List<Furniture>()); 
        FurnitureMap.Add(DepartmentType.LivingRoom, new List<Furniture>());
        FurnitureMap.Add(DepartmentType.Workspaces, new List<Furniture>());

        ReadCSV();
	}

    void ReadCSV()
    {
        txt = (TextAsset)Resources.Load("ikea", typeof(TextAsset));

        string[] line = txt.text.Split('\n');

        for (int i = 1; i < line.Length - 1; ++i)
        {
            Furniture furniture = new Furniture();
            string[] values = line[i].Split(',');

            for (int j = 0; j < values.Length; ++j)
            {
                if (j == 0) furniture.SetTexture(values[j]);
                if (j == 1) furniture.Desc = (values[j]);
                if (j == 2) furniture.Name = (values[j]);
                if (j == 3) furniture.SetDepartment(values[j]);
                if (j == 4) furniture.SetPrice(values[j]);
                if (j == 5) furniture.SetAllenKeyVal(values[j]);
             }
            //Debug.Log(furniture.Department);
            FurnitureMap[furniture.Department].Add(furniture);
        }
    }
	
	void Update () 
    {
	
	}
    
    public Dictionary<DepartmentType, List<Furniture>> FurnitureMap
    {
        get { return furnitureMap; }
        set { furnitureMap = value; }
    }
}

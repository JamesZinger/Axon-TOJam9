using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class FurnitureTemplate
{
	public Sprite			Sprite;
	public string			Description;
	public string			Name;
	public DepartmentType	Department;
	public float			Price;
	public int				AllanKeys;
	public Furniture.SpawnZone	Zone;

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

	public void SetZone( string s )
	{
		//Debug.Log(s);
		if ( s.Contains( "0" ) ) { this.Zone = Furniture.SpawnZone.Low; return; }
		if ( s.Contains( "1" ) ) { this.Zone = Furniture.SpawnZone.Medium; return; }
		if ( s.Contains( "2" ) ) { this.Zone = Furniture.SpawnZone.High; return; }
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();

		sb.AppendLine( "Name: " 	   + Name);
		sb.AppendLine( "Description: " + Description);
		sb.AppendLine( "Department: "  + Department.ToString());
		sb.AppendLine( "Price: " 	   + Price);
		sb.AppendLine( "Allen Keys: "  + AllanKeys);
		sb.AppendLine( "Zone: " 	   + Zone.ToString());
		sb.AppendLine( "Sprite: " 	   + Sprite.name);

		return sb.ToString();
	}
}
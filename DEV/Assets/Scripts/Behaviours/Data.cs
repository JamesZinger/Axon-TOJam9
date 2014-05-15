using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour 
{

	public List<FurnitureTemplate> CollectedFurniture;
//	public int Allenkeys;
	public float RemainingMoney;
	public bool didWin;
	public TimeSpan ElapsedTime;

	// Use this for initialization
	void Start () 
	{
		CollectedFurniture = new List<FurnitureTemplate>();
	}
	
}

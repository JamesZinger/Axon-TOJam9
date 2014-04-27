using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour 
{

	public List<FurnitureManager.TemplateFurniture> CollectedFurniture;
	public int Allenkeys;
	public float RemainingMoney;
	public bool didWin;

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(this);
	}
	
}

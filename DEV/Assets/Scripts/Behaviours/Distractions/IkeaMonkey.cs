using UnityEngine;
using System.Collections;

public class IkeaMonkey : Distraction
{
    const int TIME_ADDED = 10;

	void Start () 
    {
        base.Start();
	}

    public override void AddTime()
    {
        remainingTime += TIME_ADDED;
    }
	
}

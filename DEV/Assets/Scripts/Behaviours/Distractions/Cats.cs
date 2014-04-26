using UnityEngine;
using System.Collections;

public class Cats : Distraction
{
    const int TIME_ADDED = 15;

    void Start()
    {
        base.Start();
    }

    public override void AddTime()
    {
        remainingTime += TIME_ADDED;
    }
}

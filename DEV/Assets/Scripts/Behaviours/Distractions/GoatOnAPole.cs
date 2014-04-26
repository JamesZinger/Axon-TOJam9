using UnityEngine;
using System.Collections;

public class GoatOnAPole : Distraction {

    const int TIME_ADDED = 20;

    void Start()
    {
        base.Start();
    }

    public override void AddTime()
    {
        remainingTime += TIME_ADDED;
    }
}

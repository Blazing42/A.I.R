using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewAI : GOAPAgent
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("doorClosed", 1, true);
        goals.Add(s1, 5);
    }
}

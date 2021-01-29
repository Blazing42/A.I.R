using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestState : BaseState
{
    Actor creature;
    Vector3 initialTarget;
    bool initialPosition = true;

    public HarvestState(Actor creature) : base(creature.gameObject)
    {
        this.creature = creature;
    }
    public override Type Tick()
    {
        if(initialPosition == true)
        {
            initialTarget = creature.TargetPosition;
            initialPosition = false;
        }
        if(initialTarget != creature.TargetPosition)
        {
            initialPosition = true;
            return typeof(MoveState);
        }
        return typeof(HarvestState);
        
    }
}

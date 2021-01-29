using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    Actor creature;

    public IdleState(Actor creature) : base(creature.gameObject)
    {
        this.creature = creature;
    }

    public override Type Tick()
    {
        if(creature.TargetPosition != Vector3.zero)
        {
            return typeof(MoveState);
        }
        return typeof(IdleState);
    }
}

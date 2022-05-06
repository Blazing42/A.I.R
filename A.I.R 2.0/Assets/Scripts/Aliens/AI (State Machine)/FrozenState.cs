using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenState : BaseState
{
    Actor creature;
    bool slowing = false;
    bool frozen = false;
    ParticleEffectHolder effectHolder;

    public FrozenState(Actor creature) : base(creature.gameObject)
    {
        this.creature = creature;
    }

    public override Type Tick()
    {
        if (RoomChangeCheck() == true || environmentChanged == true)
        {
            var tempValue = base.RoomTemp();
            if (tempValue != AtmosphereUtilities.TempValue.FREEZING)
            {
                return typeof(MoveState);
            }
        }

        return typeof(FrozenState);
    }
}

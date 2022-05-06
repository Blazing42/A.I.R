using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FrozenDyingState : BaseState
{
    Actor creature;
    ParticleEffectHolder effectHolder;
    Human human;

    public FrozenDyingState(Actor creature) : base(creature.gameObject)
    {
        this.creature = creature;
        human = creature.gameObject.GetComponent<Human>();
    }

    public override Type Tick()
    {
        if (RoomChangeCheck() == true || environmentChanged == true)
        { 
            var tempValue = base.RoomTemp();
            if (tempValue != AtmosphereUtilities.TempValue.FREEZING)
            {
                environmentChanged = false;
                return typeof(PatrolState);
            }
        }

        return typeof(FrozenDyingState);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayState : BaseState
{
    private Actor creature;
    ParticleEffectHolder effectHolder;

    public RunAwayState(Actor creature) : base(creature.gameObject)
    {
        this.creature = creature;
        effectHolder = ParticleEffectHolder.Instance;
    }

    public override Type Tick()
    {
        throw new NotImplementedException();
    }
}

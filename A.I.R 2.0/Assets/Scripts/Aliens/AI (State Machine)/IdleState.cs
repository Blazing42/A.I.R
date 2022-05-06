using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    Actor creature;
    Human human;
    ParticleEffectHolder effectHolder;

    public IdleState(Actor creature) : base(creature.gameObject)
    {
        this.creature = creature;
        human = creature.gameObject.GetComponent<Human>();
        effectHolder = ParticleEffectHolder.Instance;
    }

    public override Type Tick()
    { 
        if (human.patrolPoints != null)
        {
            return typeof(PatrolState);
        }

        if (environmentChanged == true)
        {
            var tempValue = base.RoomTemp();
            if (tempValue == AtmosphereUtilities.TempValue.COLD)
            {
                effectHolder.EndEffect(creature.gameObject);
                effectHolder.StartEffect(effectHolder.coldParticleEffect, creature.gameObject);
                environmentChanged = false;
            }
            else if (tempValue == AtmosphereUtilities.TempValue.HOT)
            {
                effectHolder.EndEffect(creature.transform.parent.gameObject);
                effectHolder.StartEffect(effectHolder.hotParticleEffect, creature.gameObject);
                environmentChanged = false;
            }
            else if (tempValue == AtmosphereUtilities.TempValue.BURNING)
            {
                return typeof(BurningState);
                //set the creature into its too hot dying state
            }
            else if (tempValue == AtmosphereUtilities.TempValue.FREEZING)
            {
                AnimationSystem.StopMovement(animator);
                //set the creature into its too cold frozen state
                return typeof(FrozenState);
            }
            else
            {
                effectHolder.EndAllEffects(creature.gameObject);
                environmentChanged = false;
            }
        }
        return typeof(IdleState);
    }
}

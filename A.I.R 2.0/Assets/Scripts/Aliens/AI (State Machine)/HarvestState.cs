using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestState : BaseState
{
    Actor creature;
    Vector3 initialTarget;
    bool initialPosition = true;
    Core targetCore;
    ParticleEffectHolder effectHolder;

    public HarvestState(Actor creature) : base(creature.gameObject)
    {
        this.creature = creature;
        effectHolder = ParticleEffectHolder.Instance;
    }
    public override Type Tick()
    {
        if(targetCore == null)
        {
            targetCore = GameObject.FindObjectOfType<Core>();
            targetCore.creaturesAttacking++;
            targetCore.TakeDamage(1);
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
                targetCore.creaturesAttacking--;
                return typeof(BurningState);
                //set the creature into its too hot dying state
            }
            else if (tempValue == AtmosphereUtilities.TempValue.FREEZING)
            {
                AnimationSystem.StopMovement(animator);
                //set the creature into its too cold frozen state
                targetCore.creaturesAttacking--;
                return typeof(FrozenState);
            }
            else
            {
                effectHolder.EndAllEffects(creature.gameObject);
                environmentChanged = false;
            }
        }

        if (initialPosition == true)
        {
            initialTarget = creature.TargetPosition;
            initialPosition = false;
        }
        if(initialTarget != creature.TargetPosition)
        {
            initialPosition = true;
            targetCore.creaturesAttacking--;
            return typeof(MoveState);
        }
        return typeof(HarvestState);
        
    }
}

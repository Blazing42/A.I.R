using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    private Actor creature;
    ParticleEffectHolder effectHolder;
    private Human human;

    private int currentPathIndex;
    private List<Vector3> pathfindingVectorList;
    private Vector3 targetPosition;
    private int patrolIndex;

    public PatrolState(Actor creature) : base(creature.gameObject)
    {
        this.creature = creature;
        human = creature.gameObject.GetComponent<Human>();
        patrolIndex = human.patrolNo;

    }

    public override Type Tick()
    {
        if(effectHolder == null)
        {
            effectHolder = ParticleEffectHolder.Instance;
        }

        if(human.patrolPoints == null)
        {
            return typeof(IdleState);
        }

        if (pathfindingVectorList == null)
        {
            //set the initial path index to 0
            currentPathIndex = 0;
            //set the movement target position to the creatures target
            targetPosition = human.patrolPoints[patrolIndex].transform.position;
            //calculate the path that the creature will take
            pathfindingVectorList = Pathfinding.Instance?.FindPath(transform.position, targetPosition);
        }

        if (RoomChangeCheck() == true || environmentChanged == true)
        {
            var tempValue = base.RoomTemp();
            if (tempValue == AtmosphereUtilities.TempValue.COLD)
            {
                effectHolder.EndAllEffects(creature.gameObject);
                effectHolder.StartEffect(effectHolder.coldParticleEffect, creature.gameObject);
                environmentChanged = false;
                creature.speed = 2.5f;
            }
            else if (tempValue == AtmosphereUtilities.TempValue.HOT)
            {
                effectHolder.EndAllEffects(creature.gameObject);
                effectHolder.StartEffect(effectHolder.hotParticleEffect, creature.gameObject);
                environmentChanged = false;
                creature.speed = 2.5f;
            }
            else if (tempValue == AtmosphereUtilities.TempValue.BURNING)
            {
                environmentChanged = false;
                return typeof(BurningState);
                //set the creature into its too hot dying state
            }
            else if (tempValue == AtmosphereUtilities.TempValue.FREEZING)
            {
                environmentChanged = false;
                //set the creature into its too cold frozen state
                return typeof(FrozenDyingState);
            }
            else
            {
                effectHolder.EndAllEffects(creature.gameObject);
                environmentChanged = false;
                creature.speed = 5f;
            }
        }

        Vector3 nextPosition = pathfindingVectorList[currentPathIndex];
        Vector3 moveDir = (nextPosition - transform.position).normalized;
        transform.position = transform.position + moveDir * creature.speed * Time.deltaTime;
        AnimationSystem.MoveAnimation(base.animator, moveDir);
        if (Vector3.Distance(nextPosition, base.transform.position) <= 0.5f)
        {
            currentPathIndex++;
        }

        if (currentPathIndex >= pathfindingVectorList.Count - 1)
        {
            patrolIndex++;
            human.patrolNo++;
            if (patrolIndex >= human.patrolPoints.Count)
            {
                patrolIndex = 0;
                human.patrolNo++;
            }
            pathfindingVectorList = null;
        }
        return typeof(PatrolState);
    }
}

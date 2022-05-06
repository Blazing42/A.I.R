using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    private int currentPathIndex;
    private List<Vector3> pathfindingVectorList;
    private Actor creature;
    private Vector3 targetPosition;
    //SpriteRenderer sprite;
    ParticleEffectHolder effectHolder;
    //GameObject parent;

    public MoveState(Actor creature) : base(creature.gameObject)
    {
        this.creature = creature;
        effectHolder = ParticleEffectHolder.Instance;
        //parent = creature.gameObject.transform.parent.gameObject;
    }
    public override Type Tick()
    {
        //if the pathfinding path has not been set up or is the creatures target has been changed during movement
        if(pathfindingVectorList == null || creature.TargetPosition != null && creature.TargetPosition != targetPosition)
        {
            //set the initial path index to 0
            currentPathIndex = 1;
            //set the movement target position to the creatures target
            targetPosition = creature.TargetPosition;
            //calculate the path that the creature will take
            pathfindingVectorList = creature.startingPath; //Pathfinding.Instance?.FindPath(base.transform.position, targetPosition);
        }

        //if the creture changes to a different room do this check
        if(RoomChangeCheck() == true || environmentChanged == true)
        {
            var tempValue = base.RoomTemp();
            if( tempValue == AtmosphereUtilities.TempValue.COLD)
            {
                //half the creatures movement speed
                //sprite = actorScript.gameObject.GetComponent<SpriteRenderer>();
                //sprite.color = Color.blue;
                effectHolder.EndEffect(creature.gameObject);
                effectHolder.StartEffect(effectHolder.coldParticleEffect, creature.gameObject);
                environmentChanged = false;
                creature.speed = 2.5f;
            }
            else if(tempValue == AtmosphereUtilities.TempValue.HOT)
            {
                //sprite = actorScript.gameObject.GetComponent<SpriteRenderer>();
                //sprite.color = Color.red;
                effectHolder.EndEffect(creature.gameObject);
                effectHolder.StartEffect(effectHolder.hotParticleEffect, creature.gameObject);
                environmentChanged = false;
                creature.speed = 2.5f;
            }
            else if(tempValue == AtmosphereUtilities.TempValue.BURNING)
            {
                environmentChanged = false;
                return typeof(EvaporationState);
                //set the creature into its too hot dying state
            }
            else if (tempValue == AtmosphereUtilities.TempValue.FREEZING)
            {
                environmentChanged = false;
                //set the creature into its too cold frozen state
                return typeof(FrozenState);
            }
            else
            {
                //sprite = actorScript.gameObject.GetComponent<SpriteRenderer>();
                //sprite.color = Color.white;
                //if comfortable set the creature back to its normal movement speed
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

        
        if(currentPathIndex >= pathfindingVectorList.Count -1)
        {
            AnimationSystem.StopMovement(base.animator);
            pathfindingVectorList = null;
            return typeof(HarvestState);
        }
        return typeof(MoveState);
    }
}

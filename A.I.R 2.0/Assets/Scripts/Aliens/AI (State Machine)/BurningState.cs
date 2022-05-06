using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BurningState : BaseState
{
    Actor creature;
    ParticleEffectHolder effectHolder;
    private Human human;

    private int currentPathIndex;
    private List<Vector3> pathfindingVectorList;
    private Vector3 targetPosition;
    private int patrolIndex;

    public BurningState(Actor creature) : base(creature.gameObject)
    {
        this.creature = creature;
    }

    public override Type Tick()
    {
        //have the alien pick a random direction to run in for a few second then pick a different direction etc*/
        if (RoomChangeCheck() == true || environmentChanged == true)
        {
            var tempValue = base.RoomTemp();
            if (tempValue != AtmosphereUtilities.TempValue.BURNING)
            {
                environmentChanged = false;
                return typeof(PatrolState);
            }
        }
        return typeof(BurningState);
    }
}

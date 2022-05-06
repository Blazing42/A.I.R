using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EvaporationState : BaseState
{
    Actor creature;

    public EvaporationState(Actor creature) : base(creature.gameObject)
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
                return typeof(MoveState);
            }
        }
        return typeof(EvaporationState);
    }
}

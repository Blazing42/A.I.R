using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    private float speed = 20f;
    private int currentPathIndex;
    private List<Vector3> pathfindingVectorList;
    private Actor creature;
    private Vector3 targetPosition;

    public MoveState(Actor creature) : base(creature.gameObject)
    {
        this.creature = creature;
    }
    public override Type Tick()
    {
        if(pathfindingVectorList == null)
        {
            currentPathIndex = 0;
            targetPosition = creature.TargetPosition;
            pathfindingVectorList = Pathfinding.Instance?.FindPath(base.transform.position, targetPosition);
        }

        Vector3 nextPosition = pathfindingVectorList[currentPathIndex];
        //if(currentPathIndex <= 0)
        //{
            Vector3 moveDir = (nextPosition - base.transform.position).normalized;
            transform.position = transform.position + moveDir * speed * Time.deltaTime;
            if(Vector3.Distance(nextPosition,base.transform.position) <=  0.5f)
            {
                currentPathIndex++;
            }
        //}
        /*else
        {
            Vector3 previousPosition = pathfindingVectorList[currentPathIndex - 1];
            Vector3 moveDir = (nextPosition - previousPosition).normalized;
            transform.position = transform.position + moveDir * speed * Time.deltaTime;
            if (Vector3.Distance(nextPosition, base.transform.position) <= 0.5f)
            {
                currentPathIndex++;
            }
        }*/
        
        if(currentPathIndex >= pathfindingVectorList.Count)
        {
            pathfindingVectorList = null;
            return typeof(HarvestState);
        }
        return typeof(MoveState);
    }
}

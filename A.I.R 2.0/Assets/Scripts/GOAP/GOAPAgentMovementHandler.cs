using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPAgentMovementHandler : MonoBehaviour
{
    private const float speed = 20f;
    private int currentPathIndex;
    public List<Vector3> pathVectorList;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if(animator == null)
        {
            animator = this.GetComponent<Animator>();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }

    public void SetTargetPosition(Vector3 targetPos)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(ReturnPosition(), targetPos);
        if(pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    public void HandleMovement()
    {
        if(pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if(Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;
                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                AnimationSystem.MoveAnimation(animator, moveDir);
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if(currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                    AnimationSystem.StopMovement(animator);
                }
            }
        }
        else
        {
            AnimationSystem.StopMovement(animator);
        }
    }

    public float RemainingDistance(Vector3 finalTarget)
    {
        return Vector3.Distance(transform.position, finalTarget);
    }

    public Vector3 ReturnPosition()
    {
        return transform.position;
    }

    public void StopMoving()
    {
        pathVectorList = null;
    }
}

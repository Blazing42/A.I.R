using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationSystem
{
    //a method to control the animator that is going to set up movement
    public static void MoveAnimation(Animator animator, Vector3 normalisedMoveVector)
    {
        animator?.SetFloat("Speed", 1);
        float horizontal = normalisedMoveVector.x;
        float vertical = normalisedMoveVector.y;
        animator?.SetFloat("Horizontal", horizontal);
        animator?.SetFloat("Vertical", vertical);
    }

    //a method to control the animator and stop the movement setting in back to idle
    public static void StopMovement(Animator animator)
    {
        animator?.SetFloat("Speed", 0f);
    }
        
}

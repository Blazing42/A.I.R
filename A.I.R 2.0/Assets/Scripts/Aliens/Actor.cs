using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    float bodyTemp;
    float suffocationLv;
    float bodyPressure;

    float minComfTemp, maxComfTemp;
    float minComfPressure, maxComfPressure;
    float minComfLight, maxComfLight;
    float minComfNitPer, maxComfNitPer;
    float minComfOxyPer, maxComfOxyPer;
    float minComfCo2Per, maxComfCo2Per;

    Vector3 targetPosition;
    public Vector3 TargetPosition { get { return targetPosition; } }
    StateMechine stateMechine;

    // Start is called before the first frame update
    void Awake()
    {
        stateMechine = GetComponent<StateMechine>();
        //set up state machine when the creature is first created
        InitialiseStateMachine();
        //set it to its first state
    }

    //virtual method used to initialise the different states/behaviours of an alien based on their creature type e.g. humans will follow emegency lights but crawlers will not
    public virtual void InitialiseStateMachine()
    {
        Debug.Log("state machine initialised");
        //have different set of states depending on the alien type, allows for all osrts of different behaviours to be added later on as inheritors of basestate
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            {typeof(IdleState), new IdleState(this) },
            {typeof(MoveState), new MoveState(this) },
            {typeof(HarvestState), new HarvestState(this) }
        };
        stateMechine.SetStates(states);
    }

    public void SetTarget(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    //not required as can be checked in the statemachine to change states
    // Update is called once per frame
    /*void Update()
    {
        CheckVitals();
    }

    public void CheckVitals()
    {
        CheckTemperature();
        CheckPressure();
    }

    void CheckTemperature()
    {
        if(bodyTemp > maxComfTemp && bodyTemp <= maxComfTemp + 15)
        {
            //trigger event/animation when creature is to hot
            Debug.Log("Too Hot");
        }
        else if(bodyTemp > maxComfTemp + 15)
        {
            //trigger dieing of max heat event/ animation
            Debug.Log("Burning");
        }
        else if(bodyTemp < minComfTemp && bodyTemp >= minComfTemp - 15)
        {
            //trigger event/animation when creature is to cold
            Debug.Log("Too Cold");
        }
        else if (bodyTemp < minComfTemp - 15)
        {
            //trigger dieing of min temp event/ animation
            Debug.Log("Freezing");
        }
        else
        {
            Debug.Log("comfortable");
        }
    }

    void CheckPressure()
    {
        if (bodyPressure > maxComfPressure && bodyPressure <= maxComfTemp + 1)
        {
            //trigger event/animation when creature is to hot
            Debug.Log("Too high");
        }
        else if (bodyPressure > maxComfPressure + 1)
        {
            //trigger dieing of max heat event/ animation
            Debug.Log("Implosion");
        }
        else if (bodyPressure < minComfPressure && bodyPressure >= minComfPressure - 1)
        {
            //trigger event/animation when creature is to cold
            Debug.Log("Too Low");
        }
        else if (bodyPressure < minComfPressure - 1)
        {
            //trigger dieing of min temp event/ animation
            Debug.Log("Explosion");
        }
        else
        {
            Debug.Log("Comfortable");
        }
    }*/
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMechine : MonoBehaviour
{
    Dictionary<Type, BaseState> availableStates;
    public BaseState currentState;
    public event Action<BaseState> OnStateChanged;
    GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public void SetStates(Dictionary<Type, BaseState> states)
    {
        availableStates = states;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.pauseState == GameManager.PauseState.RUNNING)
        {
            //if the current state in not assigned make it the first state in the dictionary
            if (currentState == null)
            {
                currentState = availableStates.Values.First();
                return;
            }

            //then if it is assigned do the action that the state is trying to do, while waiting for a new state to be returned by the methods
            Type nextState = currentState.Tick();

            //if a different state is returned by the tick method on the state script
            if (nextState != null && nextState != currentState.GetType())
            {
                //activate the action event that is triggered when the state changes
                SwitchToNewState(nextState);
            }
        }
    }

    void SwitchToNewState(Type newState)
    {
        currentState = availableStates[newState];
        OnStateChanged?.Invoke(currentState);
    }

}

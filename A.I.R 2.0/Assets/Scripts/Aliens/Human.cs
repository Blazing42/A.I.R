using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Human : Actor
{
    float minimumBodyTemp = 21f;
    float maximumBodyTemp = 42f;
    [SerializeField] GameObject humanUIObject;
    Slider healthSlider;
    public List<GameObject> patrolPoints;
    public int patrolNo;
    public bool frozen = false;
    ParticleEffectHolder effectHolder;
    Dictionary<Type, BaseState> states;

    public override void SetUpCreature()
    {
        BodyTemp = 37.0f;
        BodyPressure = 1;
        SuffocationLv = 0f;
        SetUpTempValues(5f, 40f);

        CurrentTileMap = levelSystem.floorTileMap.tileGrid;
        //figures out the rooms in the level
        CurrentRoomDict = levelSystem.roomDict;
        //Debug.Log(currentTileMap.ToString());
        //gets a reference to the state machine
        stateMechine = GetComponent<StateMechine>();
        InitialiseStateMachine();
        stateMechine.OnStateChanged += StateChange;
        //sets 
        base.HP = 100;
        healthBar.SetMaxHealth(HP);
        //Debug.Log("Creature Spawned");

        var UIreference = GameObject.Instantiate(humanUIObject, levelSystem.humanUIPanel.transform);
        healthSlider = UIreference.GetComponent<Slider>();
        healthSlider.maxValue = HP;
        healthSlider.value = HP;
    }

    public override void LoseHp()
    {
        StartCoroutine(SlowlyLoseHealthHuman());
    }

    public void StopLosingHealth()
    {
        StopAllCoroutines();
    }

    IEnumerator SlowlyLoseHealthHuman()
    {
        var startingHp = HP;
        for (int i = 0; i <= startingHp; i++)
        {
            if (HP <= 0)
            {
                levelSystem.UpdateHumansRemaining();
                Destroy(this.gameObject);
            }
            else
            {
                HP -= 1;
                levelSystem.UpdateTotalHRemaining(1);
                healthBar.SetHealthValue(HP);
                healthSlider.value = HP;
                Debug.Log(HP);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public override void InitialiseStateMachine()
    {
        //Debug.Log("state machine initialised");
        //have different set of states depending on the alien type, allows for all osrts of different behaviours to be added later on as inheritors of basestate
        states = new Dictionary<Type, BaseState>()
        {
            {typeof(IdleState), new IdleState(this) },
            {typeof(PatrolState), new PatrolState(this) },
            {typeof(RunAwayState), new RunAwayState(this) },
            {typeof(FrozenDyingState), new FrozenDyingState(this) },
            {typeof(BurningState), new BurningState(this) }
        };
        stateMechine.SetStates(states);
    }

    void StateChange(BaseState state)
    {
        Debug.Log("state changed to " + state.GetType().ToString());
        if(state.GetType() == typeof(FrozenDyingState))
        {
            AnimationSystem.StopMovement(this.GetComponent<Animator>());
            if (effectHolder == null)
            {
                effectHolder = ParticleEffectHolder.Instance;
            }
            effectHolder.EndAllEffects(this.gameObject);
            effectHolder.StartEffect(effectHolder.frozenParticleEffect, this.gameObject);
            LoseHp();
        }

        else if(state.GetType() == typeof(PatrolState))
        {
            speed = 5f;
            if (effectHolder == null)
            {
                effectHolder = ParticleEffectHolder.Instance;
            }
            effectHolder.EndAllEffects(this.gameObject);
            StopLosingHealth();
        }

        else if(state.GetType() == typeof(BurningState))
        {
            speed = 7f;
            if (effectHolder == null)
            {
                effectHolder = ParticleEffectHolder.Instance;
            }
            effectHolder.EndAllEffects(this.gameObject);
            effectHolder.StartEffect(effectHolder.fireParticleEffect, this.gameObject);
            LoseHp();
        }
        
    }
}

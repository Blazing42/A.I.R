using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaterAlien : Actor
{
    float minimumBodyTemp = 100f;
    float maximumBodyTemp = 0f;
    ParticleEffectHolder effectHolder;
    AudioSystem audioSystem;
    [SerializeField] AudioClip freezingSFX;
    [SerializeField] AudioClip evaporatingSFX;

    public override void SetUpCreature()
    {
        audioSystem = AudioSystem.Instance;
        BodyTemp = 50f;
        BodyPressure = 1;
        SuffocationLv = 0f;
        SetUpTempValues(15f, 85f);
        base.SetUpCreature();
        stateMechine = GetComponent<StateMechine>();
        InitialiseStateMachine();
        stateMechine.OnStateChanged += StateChange;
    }

    public override void LoseHp()
    {
        StartCoroutine(SlowlyLoseHealthAlien());
    }

    void StopLosingHp()
    {
        StopAllCoroutines();
    }

    public override void InitialiseStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            {typeof(MoveState), new MoveState(this) },
            {typeof(HarvestState), new HarvestState(this) },
            {typeof(FrozenState), new FrozenState(this) },
            {typeof(EvaporationState), new EvaporationState(this) }
        };
        stateMechine.SetStates(states);
    }
    IEnumerator SlowlyLoseHealthAlien()
    {
        var startingHp = HP;
        for (int i = 0; i <= startingHp; i++)
        {
  
            if (HP <= 0)
            {
                levelSystem.UpdateAliensRemaining();
                Destroy(this.gameObject);
            }
            else
            {
                HP -= 1;
                healthBar.SetHealthValue(HP);
                //Debug.Log(HP);
                yield return new WaitForSeconds(0.1f);
            }
            
        }
    }

    void StateChange(BaseState state)
    {
        Debug.Log("state changed to " + state.GetType().ToString());
        if (state.GetType() == typeof(FrozenState))
        {
            speed = 0f;
            AnimationSystem.StopMovement(this.GetComponentInChildren<Animator>());
            if (effectHolder == null)
            {
                effectHolder = ParticleEffectHolder.Instance;
            }
            effectHolder.EndAllEffects(this.gameObject);
            effectHolder.StartEffect(effectHolder.frozenParticleEffect, this.gameObject);
            audioSystem.PlaySoundEffect(freezingSFX);
        }

        else if (state.GetType() == typeof(MoveState))
        {
            speed = 5f;
            if (effectHolder == null)
            {
                effectHolder = ParticleEffectHolder.Instance;
            }
            effectHolder.EndAllEffects(this.gameObject);
            StopLosingHp();
        }

        else if (state.GetType() == typeof(EvaporationState))
        {
            speed = 7f;
            if (effectHolder == null)
            {
                effectHolder = ParticleEffectHolder.Instance;
            }
            effectHolder.EndAllEffects(this.gameObject);
            effectHolder.StartEffect(effectHolder.evaporationParticleEffect, this.gameObject);
            audioSystem.PlaySoundEffect(evaporatingSFX);
            LoseHp();
        }

    }
}

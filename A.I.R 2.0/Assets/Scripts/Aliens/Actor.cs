using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    //variables used to represent the internal conditions of the creatures body once external conditions are bad enough these will decrease until the creature dies
    //or increase if the creature is outside of a harmful environment
    //these will be initialy be assigned per creature type
    //internal body temperature of the creature
    float bodyTemp;
    public float BodyTemp { get { return bodyTemp; } set { bodyTemp = value; } }
    //internal suffocation level of the creature
    float suffocationLv;
    public float SuffocationLv { get { return suffocationLv; } set { suffocationLv = value; } }
    //internal pressure that the creature is experiencing
    float bodyPressure;
    public float BodyPressure { get { return bodyPressure; } set { bodyPressure = value; } }
    int hp;
    public int HP { get { return hp; } set { hp = value; } }

    //minimum and maximum conditions that the creature can withstand before conditions get bad enough to effect the internal creature variables, or before events are triggered
    //minimum and maximum temp
    float minComfTemp, maxComfTemp;
    public float MinComfTemp { get { return minComfTemp; } private set { minComfTemp = value; } }
    public float MaxComfTemp { get { return maxComfTemp; } private set { maxComfTemp = value; } }
    //minimum and maximum pressure
    float minComfPressure, maxComfPressure;
    public float MinComfPressure { get { return minComfPressure; } }
    public float MaxComfPressure { get { return maxComfPressure; } }
    //minimum and maximum light levels 
    float minComfLight, maxComfLight;
    public float MinComfLight { get { return minComfLight; } }
    public float MaxComfLight { get { return maxComfLight; } }
    //minimum and maximum gas percentages
    /*float minComfNitPer, maxComfNitPer;
    float minComfOxyPer, maxComfOxyPer;
    float minComfCo2Per, maxComfCo2Per;*/

    //variables used to keep track of the creature in the lv and its behaviour
    //the current position of the creatures target in the world, depends on the creature type, engine, human etc
    Vector3 targetPosition;
    public Vector3 TargetPosition { get { return targetPosition; } set { targetPosition = value; } }
    //the current world that the creature is in/what grid it is standing on
    Grid<Tile> currentTileMap;
    public Grid<Tile> CurrentTileMap { get { return currentTileMap; } set { currentTileMap = value; } }
    //variable to hold a reference to the state machine/brain of the creature that controls behaviour
    public StateMechine stateMechine;
    RoomGrid currentRoomDict;
    public RoomGrid CurrentRoomDict { get { return currentRoomDict; } set { currentRoomDict = value; } }

    public LevelSystem levelSystem;

    public List<Vector3> startingPath;

    public HealthBarController healthBar;
    public bool dying = false;
    public float speed = 5f;


    // Start is called before the first frame update
    void Awake()
    {
        levelSystem = LevelSystem.Instance;
        SetUpCreature();
    }

    //virtual method used to initialise the different states/behaviours of an alien based on their creature type e.g. humans will follow emegency lights but crawlers will not
    public virtual void InitialiseStateMachine()
    {
        //Debug.Log("state machine initialised");
        //have different set of states depending on the alien type, allows for all osrts of different behaviours to be added later on as inheritors of basestate
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            {typeof(MoveState), new MoveState(this) },
            {typeof(HarvestState), new HarvestState(this) },
            {typeof(FrozenState), new FrozenState(this) },
            {typeof(BurningState), new BurningState(this) }
        };
        stateMechine.SetStates(states);
    }

    //method that sets the target of the creature, mainly used if the position is variable e.g. human
    //public abstract void SetTarget();

    public virtual void SetUpCreature()
    {
        //figures out which level the creature is on
        CurrentTileMap = levelSystem.floorTileMap.tileGrid;
        //figures out the rooms in the level
        CurrentRoomDict = levelSystem.roomDict;
        //sets 
        hp = 100;
        healthBar.SetMaxHealth(hp);
    }

    public virtual void LoseHp()
    {
        StartCoroutine(SlowlyLoseHealth());
    }

    public void SlowDownSpeed()
    {
        StartCoroutine(SlowDown());
    }

    public void SetUpTempValues(float min, float max)
    {
        MinComfTemp = min;
        MaxComfTemp = max;
    }

    IEnumerator SlowlyLoseHealth()
    {
        var startingHp = hp;
        dying = true;
        //Debug.Log("couroutine started");
        //Debug.Log(dying);
        for (int i = 0; i <= startingHp; i++)
        {
            hp -= 1;
            healthBar.SetHealthValue(hp);
            Debug.Log(hp);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator SlowDown()
    {
        var startingSpeed = speed;
        for (int i = 0; i <= startingSpeed; i++)
        {
            if(speed <= 0)
            {
                yield break;
            }
            else
            {
                speed-= 1;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

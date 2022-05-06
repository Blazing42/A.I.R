using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class SpawnerScript : InteractiveObject
{
    public enum CreatureType { HUMAN, PSYCHIC_GENIE, WATER_ALIEN /* add more creature types when the game is built further*/}
    public CreatureType alienType;

    //eventually have a static class or singleton that contains all of the creature prefabs to work from
    [SerializeField] GameObject alienPrefab;

    public int noOfAliens;
    public float waitTime;
    public float waveDelay;
    public float delayBeforeWave;
    public bool waves = false;
    public int noOfWaves;
    public int wavesPassed = 0;

    List<List<Vector3>> pathOptions;
    Vector3 targetPosition;
    List<PathfindingNode> possibleTargets;
    Text waveCounter;
    public int maxAliens;
    int alienNumber;
    AudioSystem audioSystem;
    [SerializeField] AudioClip waterAlienJingle;
    SpriteRenderer sr;
    Text waveText;



    // Start is called before the first frame update
    void Start()
    {
        audioSystem = AudioSystem.Instance;
        waveCounter = GetComponentInChildren<Text>();
        SetupSpawnerForWaves(alienType, waitTime, noOfAliens, noOfWaves, waveDelay, delayBeforeWave);
        SetTarget();
        pathOptions = Pathfinding.Instance.CalculateNodes(this.transform.position, targetPosition);
        //int pathNo = UnityEngine.Random.Range(0, pathOptions.Count-1);
        SetAlienCounterUI();
        sr = GetComponent<SpriteRenderer>();
        waveText = GetComponentInChildren<Text>();

        //if the spawners has been set up correctly and the bumber of aliens that will be spawned is greater than 0
        if(noOfAliens > 0)
        {
            //if the spawner will only create a single wave of aliens
            if(waves == false)
            {
                //starts single wave couroutine
                StartCoroutine(SpawnSingleWaveOfAliens());
            }
            else
            {
                //start multiple wave couroutine
                StartCoroutine(SpawnMultipleWavesOfAliens());
            }
        }
    }

    void SetTarget()
    {
        if(alienType == CreatureType.WATER_ALIEN)
        {
            InteractiveObject core = GameObject.FindGameObjectWithTag("Core").GetComponent<Core>();
            Debug.Log(core);
            possibleTargets = core.surroundingNodes;
            Debug.Log(core.surroundingNodes);
            int targetNo = UnityEngine.Random.Range(0, possibleTargets.Count);
            PathfindingNode target = possibleTargets[targetNo];
            targetPosition = core.pathfindingGrid.GetWorldPosition(target.X, target.Y);
        } 
    }

    public void SetupSpawnerForWaves(CreatureType alientype, float waitTime, int numberOfAliensPerWave, int numberofWaves, float betweenWaveDelay, float delayBeforeWave)
    {
        this.alienType = alientype;
        this.waitTime = waitTime;
        noOfAliens = numberOfAliensPerWave;
        waves = true;
        noOfWaves = numberofWaves;
        waveDelay = betweenWaveDelay;
        maxAliens = numberOfAliensPerWave * numberofWaves;
        alienNumber = 0;
        this.delayBeforeWave = delayBeforeWave;
    }

    public void SetupSpawnerForSingleWave(CreatureType alientype, float waitTime, int numberOfAliensPerWave, float delayBeforeWave)
    {
        this.alienType = alientype;
        this.waitTime = waitTime;
        noOfAliens = numberOfAliensPerWave;
        this.delayBeforeWave = delayBeforeWave;
        maxAliens = numberOfAliensPerWave;
        alienNumber = 0;
        waves = false;
    }

    public void SetAlienCounterUI()
    {
        waveCounter.text = alienNumber.ToString() + "/" + maxAliens.ToString(); 
    }

    IEnumerator SpawnSingleWaveOfAliens()
    {
        //wait the number of seconds specified as the delay before the wave
        yield return new WaitForSeconds(delayBeforeWave);
        //go through this for loop a number of times equal to how many aliens you want to spawn from this point
        sr.enabled = true;
        audioSystem.PlaySoundEffect(waterAlienJingle);
        for (int i = 0; i < noOfAliens; i++)
        {
            int spawnInt = UnityEngine.Random.Range(0,11);
            Vector3 spawnPosition = levelSystem.pathfindingGrid.PathfindingGrid.GetWorldPosition(surroundingNodes[spawnInt].X, surroundingNodes[spawnInt].Y);
            //instantiate the alien based on the alientype you specified
            GameObject newAlien = Instantiate(alienPrefab, spawnPosition, Quaternion.identity);
            //pick a random path number
            int pathNo = UnityEngine.Random.Range(0, pathOptions.Count);
            //set the aliens random starting path
            newAlien.GetComponent<Actor>().startingPath = pathOptions[pathNo];
            //and its final target position for pathfinding recalculation
            newAlien.GetComponent<Actor>().TargetPosition = targetPosition;
            //setup the alien counter
            alienNumber++;
            SetAlienCounterUI();
            //wait before spawning the next alien
            yield return new WaitForSeconds(waitTime);
        }
        wavesPassed++;
        levelSystem.UpdateWaveCounterUI();
    }

    IEnumerator SpawnMultipleWavesOfAliens()
    {
        //wait the number of seconds specified as the delay before the first wave
        yield return new WaitForSeconds(delayBeforeWave);
        //for each wave up to the number of waves specified
        sr.enabled = true;
        waveText.enabled = true;
        for (int i = 0; i < noOfWaves; i++)
        {
            audioSystem.PlaySoundEffect(waterAlienJingle);
            //spawn the creatures the same as with the single wave
            for (int j = 0; j < noOfAliens; j++)
            {
                int spawnInt = UnityEngine.Random.Range(0, 11);
                Vector3 spawnPosition = levelSystem.pathfindingGrid.PathfindingGrid.GetWorldPosition(surroundingNodes[spawnInt].X, surroundingNodes[spawnInt].Y);
                //instantiate the alien based on the alientype you specified
                GameObject newAlien = Instantiate(alienPrefab, spawnPosition, Quaternion.identity);
                //pick a random path number
                int pathNo = UnityEngine.Random.Range(0, pathOptions.Count);
                //set the aliens random starting path
                newAlien.GetComponent<Actor>().startingPath = pathOptions[pathNo];
                //and its final target position for pathfinding recalculation
                newAlien.GetComponent<Actor>().TargetPosition = targetPosition;
                //setup the alien counter
                alienNumber++;
                SetAlienCounterUI();
                //wait before spawning the next alien
                yield return new WaitForSeconds(waitTime);
            }
            wavesPassed++;
            levelSystem.UpdateWaveCounterUI();
            //wait until the next wave will begin
            yield return new WaitForSeconds(waveDelay);
        }
    }
}

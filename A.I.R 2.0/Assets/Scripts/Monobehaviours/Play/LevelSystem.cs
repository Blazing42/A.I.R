using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    //variables used to keep track of the level progression and score
    int numberOfHumansAlive;
    int maxNumberOfHumans;
    int numberOfAliensAlive;
    int maxNoOfAliens;
    public float timeSinceStartOfFirstWave;
    public int hpTotalofRemainingHumans;
    public int coreHP = 100;

    //variables that control the UI
    //variables that control the room editor panel
    [SerializeField] GameObject roomEditorPanelPrefab;
    List<GameObject> roomEditorPanels;
    //variables to control the game over screen
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text gameOverScore;
    [SerializeField] Text gameOverMessage;
    //variables to control the win screen
    [SerializeField] GameObject winPanel;
    [SerializeField] Text winScore;
    //variables to control the menu panel
    [SerializeField] GameObject menuPanel;
    //variables to control the in game UI panel
    [SerializeField] GameObject inGameUIPanel;
    public GameObject humanUIPanel;
    [SerializeField] Text waveCounterText;
    [SerializeField] Text aliensremainingUIText;
    [SerializeField] Text humansremainingUIText;
    

    //variables used to control the initial set up of the level
    //so that the correct level can be loaded for the start of the game level
    [SerializeField] string loadfilename;
    //TileMap
    public FloorTileMap floorTileMap;
    [SerializeField] private TileMapVisuals visuals;
    //Pathfinding
    public Pathfinding pathfindingGrid;
    //Rooms
    public RoomGrid roomDict;
    [SerializeField] private RoomOverlayVisuals roomOverlay;
    //Audio
    AudioSystem audioSystem;
    [SerializeField] AudioClip backgroundMusic;

    bool setup = false;


    //Singleton setup
    private static LevelSystem _instance;
    public static LevelSystem Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        //set up the Tilemap
        floorTileMap = new FloorTileMap(60, 60, 4, new Vector3(0, 0, 0));
        //load up the tilemap from the savefile
        floorTileMap.LoadTileMap(loadfilename);
        //set the tilemap visuals
        floorTileMap.SetTileMapVisual(visuals);

        //set up the rooms
        roomDict = new RoomGrid();
        //load up the rooms from the saved file
        roomDict.LoadRooms(loadfilename);
        //set the room visuals, to be removed in the final build
        roomOverlay.SetRoomOverlay(roomDict, floorTileMap.tileGrid);
        
        //set up the pathfinder grid
        pathfindingGrid = new Pathfinding(120, 120, 2);
        //initially set up the walkability
        pathfindingGrid.SetWalkability(floorTileMap.tileGrid);

        //set up the background music
        audioSystem = AudioSystem.Instance;
        audioSystem.PlayBackgroundMusic(backgroundMusic);

        roomEditorPanels = new List<GameObject>();
    }

    //methods used to set up the UI
    public void UpdateWaveCounterUI()
    {
        SpawnerScript spawner = FindObjectOfType<SpawnerScript>();
        waveCounterText.text =  "Waves " + spawner.wavesPassed + "/" + spawner.noOfWaves;
    }

    public void SetUpAliensAndHumans()
    {
        Debug.Log("setting up aliens and humans");
        //setting up the alien counters
        SpawnerScript[] spawners = FindObjectsOfType<SpawnerScript>();
        foreach(SpawnerScript spawner in spawners)
        {
            Debug.Log("adding alien spawners");
            maxNoOfAliens += spawner.maxAliens;
        }
        numberOfAliensAlive = maxNoOfAliens;
        UpdateAlienCounterUI();

        //setting up the human counters
        Slider[] sliders = humanUIPanel.GetComponentsInChildren<Slider>();
        foreach (Slider slider in sliders)
        {
            hpTotalofRemainingHumans += (int)slider.value;
            maxNumberOfHumans++;
        }
        numberOfHumansAlive = maxNumberOfHumans;
        UpdateHumanCounterUI();
    }

    void UpdateAlienCounterUI()
    {
        aliensremainingUIText.text = "Aliens Remaining " + numberOfAliensAlive + "/" + maxNoOfAliens;
    }

    void UpdateHumanCounterUI()
    {
        humansremainingUIText.text = "Humans Remaining " + numberOfHumansAlive + "/" + maxNumberOfHumans;
    }

    public void UpdateTotalHRemaining(int damageTaken)
    {
        hpTotalofRemainingHumans -= damageTaken;
    }

    public void UpdateHumansRemaining()
    {
        numberOfHumansAlive -= 1;
        UpdateHumanCounterUI();
        if (numberOfHumansAlive <= 0)
        {
            OpenGameOverScreen();
        }
    }

    public void UpdateAliensRemaining()
    {
        numberOfAliensAlive -= 1;
        UpdateAlienCounterUI();
        if(numberOfAliensAlive <= 0)
        {
            OpenWinScreen();
        }
    }
        

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.pauseState != GameManager.PauseState.PAUSED)
        {
            if (setup == false)
            {
                SetUpAliensAndHumans();
                UpdateWaveCounterUI();
                setup = true;
            }

            //as long as the player isnt clicking on the UI
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //methods that spawn the room editor UI
                if (Input.GetMouseButtonDown(0))
                {
                    var canvas = GameObject.Find("Canvas");
                    //get a position to spawn the panel
                    var panelPosition = Input.mousePosition;
                    //get the tile that was clicked on
                    Tile currentTile = floorTileMap.tileGrid.GetGridObject(InputUtilities.ScreenToWorldPoint(panelPosition, Camera.main));

                    //if the position that was clicked on wasnt a space tile
                    if (currentTile.GetTileType() == Tile.TileType.Floor)
                    {
                        //instantiate the panel in that position if the list is empty
                        if (roomEditorPanels.Count < 1)
                        {
                            GameObject roomEditorPanel = Instantiate(roomEditorPanelPrefab, new Vector3(panelPosition.x + 120f, panelPosition.y + 30f), Quaternion.identity, canvas.transform);
                            roomEditorPanel.GetComponent<RoomControlPanelScript>().SetupPanel(currentTile.GetRoomId());
                            roomEditorPanels.Add(roomEditorPanel);
                        }
                        //if the list isnt empty
                        else
                        {
                            //set all of the currently active panels to be inactive if there are any
                            foreach (GameObject roomEditorPanel in roomEditorPanels)
                            {
                                roomEditorPanel.GetComponent<RoomControlPanelScript>().MakeInvisible();
                            }
                            bool panelExists = false;

                            //go through each of the panels in the list to see if there is one for this room already
                            foreach (GameObject roomEditorPanel in roomEditorPanels)
                            {
                                float roomIDCheck = roomEditorPanel.GetComponent<RoomControlPanelScript>().roomID;
                                if (currentTile.GetRoomId() == roomIDCheck)
                                {
                                    //if there is move its position
                                    roomEditorPanel.transform.position = new Vector3(panelPosition.x + 120f, panelPosition.y + 30f);
                                    //roomEditorPanel.GetComponent<RoomControlPanelScript>().SetupPanel(currentTile.GetRoomId());
                                    //then set it active in the scene
                                    roomEditorPanel.GetComponent<RoomControlPanelScript>().MakeVisable();
                                    panelExists = true;
                                }
                            }
                            //if there isnt a panel associated with that room 
                            if (panelExists == false)
                            {
                                //create a new panel for this room
                                GameObject roomEditorPanel = Instantiate(roomEditorPanelPrefab, new Vector3(panelPosition.x + 120f, panelPosition.y + 30f), Quaternion.identity, canvas.transform);
                                roomEditorPanel.GetComponent<RoomControlPanelScript>().SetupPanel(currentTile.GetRoomId());
                                //add it to the list of existing panels
                                roomEditorPanels.Add(roomEditorPanel);
                            }
                        }
                    }
                    //if the position that was clicked on was a space tile
                    else
                    {
                        //set all of the room panels to be inactive
                        foreach (GameObject roomEditorPanel in roomEditorPanels)
                        {
                            roomEditorPanel.GetComponent<RoomControlPanelScript>().MakeInvisible();
                        }

                    }
                    //if the left mouse button is clicked set all the panels to be inactive
                    if (Input.GetMouseButtonDown(1))
                    {
                        //set all of the room panels to be inactive
                        foreach (GameObject roomEditorPanel in roomEditorPanels)
                        {
                            roomEditorPanel.GetComponent<RoomControlPanelScript>().MakeInvisible();
                        }
                    }
                }
            }
        }
    }

    float CalculatePlayerScore()
    {
        float score = 30000f - timeSinceStartOfFirstWave + coreHP + hpTotalofRemainingHumans;
        return score;
    }

    public void OpenGameOverScreen()
    {
        foreach (GameObject roomEditorPanel in roomEditorPanels)
        {
            Destroy(roomEditorPanel);
        }
        inGameUIPanel.SetActive(false);
        gameOverPanel.SetActive(!gameOverPanel.activeSelf);
        GameManager.Instance.PauseGame();
        if(coreHP <= 0)
        {
            gameOverMessage.text = "Critical System Error!";
        }
        else if(hpTotalofRemainingHumans <= 0)
        {
            gameOverMessage.text = "No Human Lifesigns Detected...... Shutting Down..";
        }
        gameOverScore.text = "Score : " + CalculatePlayerScore().ToString();
    }

    public void OpenWinScreen()
    {
        foreach (GameObject roomEditorPanel in roomEditorPanels)
        {
            Destroy(roomEditorPanel);
        }
        inGameUIPanel.SetActive(false);
        winPanel.SetActive(!winPanel.activeSelf);
        GameManager.Instance.PauseGame();
        winScore.text = "Score : " + CalculatePlayerScore().ToString();
    }



}

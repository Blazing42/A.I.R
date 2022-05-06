using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider progressBar;
    string previousScene;

    public enum PauseState { RUNNING, PAUSED};
    public PauseState pauseState;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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
        SceneManager.LoadSceneAsync("Main Menu" , LoadSceneMode.Additive);
        pauseState = PauseState.RUNNING;
    }

    void Update()
    {
        //method that quits the application if the escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void PauseGame()
    {
        pauseState = PauseState.PAUSED;
    }

    public void UnPauseGame()
    {
        pauseState = PauseState.RUNNING;
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    float totalSceneProgress;
    public void LoadGame()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync("Main Menu"));
        scenesLoading.Add(SceneManager.LoadSceneAsync("LevelOne", LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress("levelOne"));
    }

    public void ReplayLevel()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync("LevelOne"));
        scenesLoading.Add(SceneManager.LoadSceneAsync("LevelOne", LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress("levelOne"));
    }

    public void LoadLevelEditor()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync("Main Menu"));
        scenesLoading.Add(SceneManager.LoadSceneAsync("Level Editor", LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress("Level Editor"));
    }

    public void LoadSettings(string currentSceneName)
    {
        StartCoroutine(OpenSettingsScene(currentSceneName));
    }

    public void ExitSettings()
    {
        if (previousScene == "Main Menu")
        {
            scenesLoading.Add(SceneManager.LoadSceneAsync("Main Menu"));
            pauseState = PauseState.RUNNING;
        }
        else if(previousScene == "LevelOne")
        {
            GameObject.FindObjectOfType<LevelButtonUI>().menu.SetActive(true);
        }
        scenesLoading.Add(SceneManager.UnloadSceneAsync("Settings Menu"));
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(previousScene));
    }

    public void LoadMainMenu(string currentScene)
    {
        scenesLoading.Add(SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Additive));
        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
    }

    IEnumerator GetSceneLoadProgress(string sceneName)
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while(!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0f;
                foreach(AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress; 
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                progressBar.value = Mathf.RoundToInt(totalSceneProgress);
                yield return null;
            }
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        loadingScreen.gameObject.SetActive(false);
    }

    IEnumerator OpenSettingsScene(string currentSceneName)
    {
        if (currentSceneName == "Main Menu")
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync("Main Menu"));
        }

        scenesLoading.Add(SceneManager.LoadSceneAsync("Settings Menu", LoadSceneMode.Additive));

        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0f;
                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }
                yield return null;
            }
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Settings Menu"));
        previousScene = currentSceneName;
        pauseState = PauseState.PAUSED;
    }
}

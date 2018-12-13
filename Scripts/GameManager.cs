#region nameSpaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Soomla;
using Soomla.Store;
#endregion
public class GameManager : MonoBehaviour
{
    #region variables
    [SerializeField]
    private GameObject exitDialog;
    [SerializeField]
    private ShopController shopController;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private InputManager inputManager;
    public static GameState currentGameState;
    public enum GameState
    {
        MAIN,
        GAME,
        PAUSED,
        IN_THE_SHOP,
        SHOWING_HIGHSCORE
    }
    public SoundManager soundManager;
    #endregion
    #region events_and_delegates
    public delegate void EscapeKeyDetectedEventHandler();
    public event EscapeKeyDetectedEventHandler EscapeKeyDetected;

    public delegate void GameSceneUnloadedEventHandler();
    public event GameSceneUnloadedEventHandler GameSceneUnloaded;

    public delegate void GameSceneLoadedEventHandler();
    public event GameSceneLoadedEventHandler GameSceneLoaded;

    public delegate void MainSceneLoadedEventHandler();
    public event MainSceneLoadedEventHandler MainSceneLoaded;

    public delegate void GameRestartedEventHandler();
    public event GameRestartedEventHandler GameRestarted;

    public delegate void GamePausedEventHandler();
    public event GamePausedEventHandler GamePaused;

    public delegate void GameResumedEventHandler();
    public event GamePausedEventHandler GameResumed;
    #endregion
    #region monobehaviour_events
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(transform.root);
    }
    void Start()
    {
        soundManager.Init();
        OnMainSceneLoaded();
    }
    #endregion
    #region methods
    public void ChangeGameState(GameState newState)
    {
        currentGameState = newState;
    }
    public void ShowHighScore(GameState newState)
    {
        currentGameState = newState;
    }
    public void OnMainSceneLoaded()
    {
        currentGameState = GameState.MAIN;
        if (MainSceneLoaded != null)
            MainSceneLoaded();
    }
    //notify listeners about the event
    public void OnGameSceneUnloaded()
    {
        if (GameSceneUnloaded != null)
            GameSceneUnloaded();
    }
    //notify listeners about the event
    public void OnGameSceneLoaded()
    {
        currentGameState = GameState.GAME;
        if (GameSceneLoaded != null)
            GameSceneLoaded();
    }
    public void Quit()
    {
        Application.Quit();
    }
    //notify everyone that game resumed
    public void ResumeGame()
    {
        currentGameState = GameState.GAME;
        if (GameResumed != null)
            GameResumed();
    }
    //game is paused by pause button, notify all listeners
    public void PauseGame()
    {
        currentGameState = GameState.PAUSED;
        if (GamePaused != null)
            GamePaused();
    }
    //reload same scene
    public void RestartGame()
    {
        if (GameRestarted != null)
            GameRestarted();
        // i shouldn't hardcode the name of scene , but 
        // the getactivescene() method wont pass the name
        //of the scene which additiviley been loaded(
        //in this case the game scene).
        SceneManager.UnloadSceneAsync
            (SceneManager.GetSceneByName("DefaultGameScene"));

        LoadScene("DefaultGameScene");
    }
    public void LoadScene(String sceneName)
    {
        if (SceneManager.sceneCount > 1)
        {
            SceneManager.UnloadSceneAsync
                (SceneManager.GetSceneByName
                ("DefaultGameScene"));
            OnGameSceneUnloaded();
        }
        //the main menu scene doesn't need to be loaded
        // because it will always be in the game 
        if (!sceneName.Contains("MainMenu"))
        {
            SceneManager.LoadSceneAsync("DefaultGameScene"
                , LoadSceneMode.Additive);
        }
        else
        {
            OnMainSceneLoaded();
        }
    }
    //don't load mainmenu ,it is a manager scene
    //and will never need to be loaded , since ,
    // it is already loaded from the beginning of game 

    #endregion
    #region custom_events
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("GameScene"))
            OnGameSceneLoaded();
    }
    #endregion
}
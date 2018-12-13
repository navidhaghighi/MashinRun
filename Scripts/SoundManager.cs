#region nameSpaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion
public class SoundManager:MonoBehaviour
{
    #region variables
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private AudioSource mainMusicAudioSource;
    [SerializeField]
    private AudioSource gameMusicAudioSource;
    [SerializeField]
    private Player player;
    private static SoundManager instance;
    public static SoundManager Instance
    {
        set
        {
           instance  = value;

        }
        get { return instance; }
    }
     bool isSoundsOn;
     bool IsSoundsOn
    {
        set
        {
           isSoundsOn  = value;
           int intState;
           if (isSoundsOn)
               intState = 1;
           else intState = 0;
           AudioListener.volume = intState;
           PlayerPrefs.SetInt("sound_state", intState);
           PlayerPrefs.Save();
           OnSoundsStateChanged(isSoundsOn);
        }
        get { return isSoundsOn; }
    }
    #endregion
    #region events_and_delegates
    public delegate void SoundsStateChangedEventHandler
    (bool newState);
    public event SoundsStateChangedEventHandler SoundsStateChanged;
    #endregion
    #region monobehaviour_events
    void Awake()
    {

        if (!Instance)
            Instance = this;
        else Destroy(gameObject);
        if (PlayerPrefs.HasKey("sound_state"))
            IsSoundsOn = (PlayerPrefs.GetInt("sound_state") == 1);
        else
        {
            PlayerPrefs.SetInt("sound_state", 1);
            IsSoundsOn = true;
        }
        gameManager.GameSceneLoaded += OnGameSceneLoaded;
        gameManager.MainSceneLoaded += OnMainSceneLoaded;
        gameManager.GamePaused += OnGamePaused;
        gameManager.GameResumed+= OnGameResumed;
        player.PlayerDied += OnPlayerDied;
        player.PlayerChangedLane += OnPlayerChangedLane;
    }
    #endregion
    #region methods
    void OnSoundsStateChanged(bool newState)
    {
        if (SoundsStateChanged != null) 
            SoundsStateChanged(newState);
    }
    public void Init()
   {
       IsSoundsOn=DataManager.GetSoundState();
   }
   public void SetSoundsState(bool state)
   {
       IsSoundsOn= state;
       if (SoundsStateChanged != null)
           SoundsStateChanged(state);
   }
   public void PlaySound(AudioSource audioSource)
   {
       if((!IsSoundsOn)&&(!audioSource))
          return;
       audioSource.Play();
   }
    #endregion
    #region custom_events
   public void OnGameResumed()
   {
       gameMusicAudioSource.UnPause();
   }
   public void OnGamePaused()
   {
       gameMusicAudioSource.Pause();
   }
   public void OnMainSceneLoaded()
   {
       if (gameMusicAudioSource.isPlaying)
           gameMusicAudioSource.Stop();
       mainMusicAudioSource.Play();
   }
   public void OnPlayerDied(Player player)
   {
       gameMusicAudioSource.Stop();
   }
   public void OnGameSceneLoaded()
   {
       mainMusicAudioSource.Stop();
       gameMusicAudioSource.Play();
   }
    public void OnPlayerChangedLane(Player player)
    {
         if(player.audioSource)
            player.audioSource.Play();
    }
    public void OnCarHitObstacle(Car car, Obstacle obstacle)
    {
         if(obstacle.audioSource)
            obstacle.audioSource.Play();
    }
   public void OnCoinCollected(Coin coin)
   {
       if(coin.audioSource)
           coin.audioSource.Play();
   }
    #endregion
}
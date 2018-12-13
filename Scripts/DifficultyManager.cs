using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    #region variables
    #region singleton
    private static DifficultyManager instance;
    public static DifficultyManager Instance
    {
        set
        {
                instance = value;
        }
        get
        {
                return instance;
             
        }
    }
    #endregion
    #endregion
    #region events_and_delegates
    public delegate void DifficultyIncreasedEventHandler();
    public event DifficultyIncreasedEventHandler DifficultyIncreased;
    #endregion
    #region monobehaviour_events
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else Destroy(gameObject);
    }
    #endregion
    #region custom_events
    public void OnCarExitedTile(Tile tile)
    {
        IncreaseDifficulty();
    }
    #endregion
    #region methods
    void IncreaseDifficulty()
    {
        OnDifficultyIncreased();
    }
    void OnDifficultyIncreased()
    {
        if (DifficultyIncreased != null)
            DifficultyIncreased();
    }

    #endregion
}

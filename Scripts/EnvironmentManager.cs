#region namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
#endregion
public class EnvironmentManager:MonoBehaviour
{
    #region variables
    public GameManager gameManager;
    bool isGameSceneLoaded = false;
    public bool isEnvironmentLoaded = false;
    public int howManyTilesInPool;
    Tile lastTile;
    DifficultyManager difficultyManager;
    List<Pattern> patterns;
    public int howManySafeTiles;
    public int howManyActiveTiles;
    public Tile firstTile;
    List<Tile> tiles;
    public int howManyPrizes=450;
    public int howManyObstacles = 450;

    // i made this variable public so reference manager could 
    //supply reference upon game scene load
    public ObjectPooler objectPooler;
    #endregion
    #region events_and_delegates
    public UnityEvent environmentGotReady;
    #endregion
    #region monobehaviour_events
    void Start()
    {
        gameManager.GameSceneLoaded += OnGameSceneLoaded;
        gameManager.GameSceneUnloaded += OnGameSceneUnloaded;
    }
    #endregion
    #region methods
    List<Pattern> GetPatterns()
    {
        List<Pattern> patterns = DataManager.
            GetAllDataFromResourcesFolder<Pattern>("Patterns");
        return 
            patterns;
    }
    void OnEnvironmentGotReady()
    {
        environmentGotReady.Invoke();
    }
    #region tiles

    void SpawnTiles()
    {
        objectPooler.CreatePoolFromResources("Tiles",    
            howManyTilesInPool);
        foreach (GameObject go in objectPooler.GetPool("Tiles"))
        {
            if (go.GetComponent<Tile>())
            {
                go.GetComponent<Tile>().
                    trigger.environmentManager = this;
                go.GetComponent<Tile>().objectPooler
                    = objectPooler;
            }
        }
        tiles = new List<Tile>();

        Vector3 pos;
        for (int i = 0; i < howManyActiveTiles; i++)
        {
            Tile tile;
            if (i == 0)
                pos = Vector3.zero;
            else pos = tiles[(i - 1)].GetComponent<Tile>()
                .nextTilePosition;
            tile = objectPooler.SpawnFromPool("Tiles", pos)
            .GetComponent<Tile>();
            tile.nextTilePosition = new Vector3
                (tile.transform.position.x,
            tile.transform.position.y, 
            (tile.transform.position.z +
            tile.tileScale.z));
            tile.index = i;
            tiles.Add(tile);
            PatternTile(tile);
            //this line will ensure the objectpooler instances
            //are the same , so you won't get null reference exception
           // tile.objectPooler = objectPooler;
        }
        firstTile = tiles[0];
    }
    public void PatternTile(Tile tile)
    {
        int tileIndex;
        //assign pattern to tile:
        //low index tiles shouldn't have any coins or obstacles on them
        tileIndex = tile.GetComponent<PooledObject>().index;
        if ((tileIndex < howManySafeTiles)||(patterns.Count==0))
            return;
        tile.AssignPattern
            (patterns[UnityEngine.Random.Range(0, patterns.Count )]);
    }
    #endregion
    #region lane_objects
    void SpawnLaneObjects()
    {
        objectPooler.CreatePoolFromResources("Obstacles", howManyObstacles);
        objectPooler.CreatePoolFromResources("Prizes", howManyPrizes);
        foreach(Tile tile in tiles)
        {
            if (!tile.pattern)
                continue;
            tile.SpawnLaneObjects();
        }
    }
    #endregion
    #endregion
    #region custom_events
    public void OnGameSceneUnloaded()
    {
        objectPooler.DestroyAllPools();
    }
    public void OnCarExitedTile(Tile tile)
    {
        lastTile = objectPooler.GetLastItemFromPool("Tiles")
        .GetComponent<Tile>();
        if (!lastTile)
            return;
        tile = objectPooler.SpawnFromPool
            ("Tiles", lastTile.nextTilePosition)
            .GetComponent<Tile>();
        tile. UpdateNextTilePosition();
        PatternTile(tile);
        tile.SpawnLaneObjects();
    }
    public void OnGameSceneLoaded()
    {
        patterns = GetPatterns();
        SpawnTiles();
        SpawnLaneObjects();
        OnEnvironmentGotReady();
       
    }
    #endregion
}
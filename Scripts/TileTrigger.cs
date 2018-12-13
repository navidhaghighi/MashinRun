#region namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
//the trigger attached to each tile
public class TileTrigger : MonoBehaviour
{
    #region variables
    DifficultyManager difficultyManager;
    public EnvironmentManager environmentManager;
    public Tile tile;
    #endregion
    #region events_and_delegates
    public delegate void CarExitedTileEventHandler
       ( Tile tile);
    public event CarExitedTileEventHandler CarExitedTile;
    #endregion
    #region monobehaviour_events
    void Start()
    {
        difficultyManager = DifficultyManager.Instance;
        CarExitedTile += difficultyManager.OnCarExitedTile;
        CarExitedTile += environmentManager.OnCarExitedTile;
    }
    void OnTriggerExit(Collider collider)
    {
        if ((collider.gameObject)&&(collider.gameObject.CompareTag("Car")))
            {
                OnCarExitedTile(tile);
            }
        
    }
    #endregion
    #region custom_events
    void OnCarExitedTile(Tile tile)
    {
        if (CarExitedTile != null)
            CarExitedTile(tile);
    }
    #endregion
}

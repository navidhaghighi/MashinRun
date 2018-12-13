#region nameSpaces
using UnityEngine;
using System;
#endregion
public class Tile : PooledObject
{
    #region variables
    public Vector3 tileScale;
    public ObjectPooler objectPooler;
    public TileTrigger trigger;
    public Pattern pattern;
    public Vector3 nextTilePosition;
    public Container[] leftLaneContainers;
    public Container[] midLaneContainers;
    public Container[] rightLaneContainers;
    #endregion
    #region events_and_delegates
    public delegate void CarExitedTileEventHandler(Tile tile, Player player);
    public event CarExitedTileEventHandler CarExitedTile;
    public delegate void TileCreatedEventHandler(Tile tile);
    public event TileCreatedEventHandler TileCreated;
    #endregion
    #region monobehaviour_events
    void Awake()
    {
        nextTilePosition = new Vector3(transform.position.x,
        transform.position.y, (transform.position.z + transform.localScale.z));
        if(trigger)
        trigger.CarExitedTile += OnCarExitedTile;
    }
    void Start()
    {
        OnTileCreated();
    }
    #endregion
    #region custom_events
    void OnTileCreated()
    {
        if (TileCreated != null)
        {
            TileCreated(this);
        }
    }
    public void AssignPattern(Pattern pattern)
    {
        this.pattern = pattern;
    }
    void OnCarExitedTile(Tile tile)
    {
        ChangeSnappedObjectsStatus(false);
    }
    #endregion
    #region methods
    public void UpdateNextTilePosition()
    {
        nextTilePosition = new Vector3(transform.position.x,
         transform.position.y, (transform.position.z +
       tileScale.z));
    }
    public void SpawnLaneObjects()
    {
        if (!pattern)
            return;
        for (int i = 0; i < leftLaneContainers.GetLength(0);
            i++)
        {

            if (pattern.leftLane[i] == GameResources.CellData.OBSTACLE)
                leftLaneContainers[i].GetComponent<Container>()
                    .snappedObject = objectPooler.SpawnFromPool
                    ("Obstacles", leftLaneContainers[i].
                    transform.position);
            else if (pattern.leftLane[i] == GameResources.CellData.PRIZE)
            
                leftLaneContainers[i].GetComponent<Container>()
                    .snappedObject = objectPooler.SpawnFromPool
                    ("Prizes", leftLaneContainers[i].
                    transform.position);

            
        }

        for (int i = 0; i <
            midLaneContainers.GetLength(0); i++)
        {

            if (pattern.midLane[i] == GameResources.CellData.OBSTACLE)
                midLaneContainers[i]
                    .GetComponent<Container>().snappedObject =
                    objectPooler.SpawnFromPool("Obstacles",
                    midLaneContainers[i].transform.position);
            else if (pattern.midLane[i] == GameResources.CellData.PRIZE)
            
                midLaneContainers[i]
                    .GetComponent<Container>().snappedObject =
                    objectPooler.SpawnFromPool("Prizes",
                    midLaneContainers[i].transform.position);
            
        }
        for (int i = 0; i < rightLaneContainers.GetLength(0); i++)
        {

            if (pattern.rightLane[i] == GameResources.CellData.OBSTACLE)
                rightLaneContainers[i]
                    .GetComponent<Container>().snappedObject =
                    objectPooler.SpawnFromPool("Obstacles",
                    rightLaneContainers[i]
                    .transform.position);
            else if (pattern.rightLane[i] == GameResources.CellData.PRIZE)
            

                rightLaneContainers[i]
                    .GetComponent<Container>().snappedObject =
                    objectPooler.SpawnFromPool
                    ("Prizes", rightLaneContainers[i].transform.position);
        }

        ChangeSnappedObjectsStatus(true);

    }
    public void ChangeSnappedObjectsStatus(Boolean status)
    {
        
        foreach (Container container in leftLaneContainers)
        {
  
            if ((container.snappedObject) && (container
                .snappedObject.transform.childCount>=1))
                container.snappedObject.transform.GetChild(0)
                    .gameObject.SetActive(true);
            else if (container.snappedObject)
                container.snappedObject.SetActive(status);
        }
        foreach (Container container in midLaneContainers)
        {

            if ((container.snappedObject) && (container
                .snappedObject.transform.childCount >= 1))
                container.snappedObject.transform.GetChild(0)
                    .gameObject.SetActive(true);
            else if (container.snappedObject)
                container.snappedObject.SetActive(status);
        }
        foreach (Container container in rightLaneContainers)
        {

            if ((container.snappedObject) && (container
                .snappedObject.transform.childCount >= 1))
                container.snappedObject.transform.GetChild(0)
                    .gameObject.SetActive(true);
            else if (container.snappedObject)
                container.snappedObject.SetActive(status);
        }

    }
    #endregion
}
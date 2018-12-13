#region namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
[ExecuteInEditMode]
public class TileMaker:MonoBehaviour
{
    #region variables
    [SerializeField]
    private GameObject tileGO;
    [SerializeField]
    private int howManyContainersPerLane;
    [SerializeField]
    private int howManyLanes;
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private Collider collider;
    #endregion
    #region monobehaviour_events
    void OnEnable()
    {
        MakeTile();
    }
    #endregion
    #region methods
    public void MakeTile()
    {
        if(!collider)
        {
            Debug.Log("assign a collider first");
            return;
        }
        else if(!container)
        {
            Debug.Log("assign a container first");
            return;
        }
        GameObject lanesParent = new GameObject();
        lanesParent.name = "Lanes";
        lanesParent.transform.SetParent(tileGO.transform);
        //the distance from min z of collider bounding box to it's max z
        float colHeight= ((collider.bounds.extents.z)*2f);
        float colWidth = ((collider.bounds.extents.x)*2f);
        float eachLaneWidth = (colWidth/howManyLanes);
        float containersZDistance = (colHeight/howManyContainersPerLane);
        for(int i = 0;i<howManyLanes;i++)
        {
            GameObject lane = new GameObject();
            lane.name = "Lane";
            lane.transform.SetParent(lanesParent.transform);
            for(int j= 0;j<howManyContainersPerLane ;j++)
            {
                Vector3 position ;
                GameObject instantiatedContainer
                    = Instantiate (container);
                position.x = ((collider.bounds.min.x)+(eachLaneWidth*i));
                position.y = collider.bounds.min.y;
                position.z = ((collider.bounds.min.z)+(containersZDistance*j));
                instantiatedContainer. transform.position = position;
                instantiatedContainer. transform.SetParent(lane
                    .transform);
            }
        }
    }
    #endregion
}
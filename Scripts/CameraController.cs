#region nameSpaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
#endregion
public class CameraController : MonoBehaviour
{
    #region variables
    public GameManager gameManager;
    public Transform showCaseRoomCamTransform;
    GameObject playerCar;
    bool isGameSceneLoaded=false;
    public Transform rotationTransform;
    private static CameraController instance;
    public Player player;
    public Vector3 distanceFromPlayer;
    Vector3 newPos;
    float zDistanceFromPlayer;
    float yDistanceFromPlayer;
    #endregion
    #region monobehaviour_events
    void Start()
    {
        player.PlayerStartedRunning += OnPlayerStartedRunning;
        gameManager.MainSceneLoaded += OnMainSceneLoaded;
        gameManager.GameSceneUnloaded += OnGameSceneUnLoaded;
    }
    #endregion
    #region custom_events
    public void OnGameSceneUnLoaded()
    {
        StopAllCoroutines();
    }
    public void OnMainSceneLoaded()
    {
        transform.position = showCaseRoomCamTransform.position;
        transform.rotation = showCaseRoomCamTransform.rotation;
    }
    public void OnPlayerStartedRunning(Player player)
    {
        playerCar = player.car;
        transform.rotation = rotationTransform.rotation;
        newPos = new Vector3(playerCar.transform.position.x
            - distanceFromPlayer.x,
            playerCar.transform.position.y + distanceFromPlayer.y
            , playerCar.transform.position.z - distanceFromPlayer.z);
        StartCoroutine(FollowCar(playerCar));
       // TurnToPlayer(player);
    }
    #endregion
    #region methods
    void TurnToPlayer(Player player)
    {
    }
    IEnumerator FollowCar(GameObject car)
    {
        while (true)
        {
            transform.position = 
                (car.transform.position 
                - distanceFromPlayer);
            yield return null;
        }
    }
    #endregion
}
#region nameSpaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Soomla;
using Soomla.Store;
#endregion
public class Player : MonoBehaviour
{
    #region variables
    float laneChangeSpeed;
    GameResources.Direction laneChangeDirection;
    //how many steps from a lane traveled 
    float laneStepsTraveled=0f;
    Vector3 laneChangeTarget;
    Lane currentLane;
    Transform leftLane;
    Transform rightLane;
    Transform middleLane;
    float cachedXPos;
    //lane to be changed to
    Lane nextLane;
enum Lane
{
    NONE,
    LEFT,
    MIDDLE,
    RIGHT
}
    Rigidbody rigidBody;
    bool changingLanes;
    //each step amount for lane change's x axis movement.
    float laneChangeStep;
    //how many steps needed for a lane change.
    int laneChangeIterations;
    float forwardMoveAmount;
    float verticalMoveAmount;
    float horizontalMoveAmount;
    //variable to mesasure time on update cycles
    float timePassedSinceLastMovedUpdate = 0f;
    bool allowedToRun = false;
    //how often should we dispatch the player moved forward event
    float movedEventUpdateRate=1f;
    public AudioSource audioSource;
    //if everything is ready for player this bool becomes true
    bool playerIsInitialized =false;
    public GameObject car;
    public Quaternion initialRotation;
    public ScoreManager scoreManager;
    public GameManager gameManager;
    Animator anim;
    bool playerIsIntitialized = false;
    public EnvironmentManager environmentManager;
    public DifficultyManager difficultyManager;
    public InputManager inputManager;
    //to optimize , i'm declaring a raycast for the whole class
    RaycastHit hit;
    bool isGrounded = true;
    private float jumpForce = 0.09f;
    private float gravityForce = 0.18f;
    float difficulty = 0.5f;
    float difficultyFactor = 0.05f;
    float runSpeed;
    float distanceBetweenLanes;
    Tile firstTile;
    float eachLaneDistance;
    #endregion
    #region events_and_delegates
    
    public delegate void PlayerChangedLaneEventHandler(Player player);
    public event PlayerChangedLaneEventHandler PlayerChangedLane;
    public delegate void PlayerExitedTileEventHandler(Player player, Tile tile);
    public event PlayerExitedTileEventHandler PlayerExitedTile;
    public delegate void PlayerStartedRunningEventHandler(Player player);
    public event PlayerStartedRunningEventHandler PlayerStartedRunning;
    public delegate void PlayerMovedForwardEventHandler();
    public event PlayerMovedForwardEventHandler PlayerMovedForward;
    public delegate void PlayerDiedEventHandler(Player player);
    public event PlayerDiedEventHandler PlayerDied;
    #endregion
    #region monobehaviour events
    //because i have multiple scenes and player can restart
    //game scene i am doing these codes in onenable
    void Awake()
    {
        difficultyManager.DifficultyIncreased += OnDifficultyIncreased;
        gameManager.GameSceneUnloaded += OnGameSceneUnloaded;
        inputManager.SwipeDetected += OnSwipeDetected;
    }
    void FixedUpdate()
    {
        if (!allowedToRun)
            return;
        forwardMoveAmount =  ((1f*Time.fixedDeltaTime)*
        (runSpeed*difficulty));
        if (timePassedSinceLastMovedUpdate > movedEventUpdateRate)
        {
            OnPlayerMovedForward();
            timePassedSinceLastMovedUpdate = 0f;
        }
        if (changingLanes)
        {
            if(Math.Abs(car.transform.position.x-laneChangeTarget
                .x)<0.25)
                {
                currentLane = nextLane;
                changingLanes = false;
                }
            else if(laneChangeDirection==GameResources.Direction
                .RIGHT)
                rigidBody.AddForce(Vector3.right*(400f));
            else if(laneChangeDirection==GameResources.Direction
                .LEFT)
                rigidBody.AddForce(Vector3.left*(400f)
                    ,ForceMode.Force);
        }
        rigidBody.velocity = Vector3.forward*forwardMoveAmount;
        timePassedSinceLastMovedUpdate +=Time.fixedDeltaTime;
    }
    #endregion
    #region custom_events
    public void OnGameSceneLoaded()
    {
        gameObject.SetActive(true);
    }
    //upon unloading game scene(e.g: loading main scene or restarting game scene
    //stop listening for inputs and destroy animator
    public void OnGameSceneUnloaded()
    {
        allowedToRun = false;
        Destroy(car);
        StopAllCoroutines();
    }
    public void OnDifficultyIncreased()
    {
        difficulty += difficultyFactor;
    }
    public void OnPlayerMovedForward()
    {
        if (PlayerMovedForward != null)
            PlayerMovedForward();
    }
    public void OnSwipeDetected(GameResources.Direction direction)
    {
        if (!car)
            return;
        if (direction == GameResources.Direction.UP)
            StartCoroutine(Jump());
        else ChangeLane(direction);
    }
    public void OnPlayerDied()
    {
        allowedToRun = false;
        if (PlayerDied != null)
            PlayerDied(this);
    }
    //player (car) can only behave on a ready environment
    //because it needs to be on a collider , and the scene 
    //in assets is empty and needs to be initialized
    public void OnEnvironmentGotReady(EnvironmentManager
        environmentManager)
    {
        
        car = GetCarFromResources();
        car = Instantiate(car);
        firstTile = environmentManager.firstTile;
        car.GetComponentInChildren<Car>().player = this;
        car.transform.root.gameObject.transform.rotation = initialRotation;
        middleLane = firstTile.midLaneContainers[0].transform;
        leftLane = firstTile.leftLaneContainers[0].transform;
        rightLane = firstTile.rightLaneContainers[0].transform;
        car.transform.position = middleLane.transform.position;
        currentLane = Lane.MIDDLE;
        anim = car.GetComponentInChildren<Car>().anim;
        rigidBody = car.GetComponentInChildren<Rigidbody>();
        rigidBody.useGravity = false;
        runSpeed = car.GetComponentInChildren<Car>().carData.runSpeed;
        difficulty = 0.5f;
        /*if (car.GetComponentInChildren<CharacterController>())
            controller = car.GetComponentInChildren
                <CharacterController>();*/
        distanceBetweenLanes = (firstTile.rightLaneContainers[0].transform.position.x
            - firstTile.midLaneContainers[0].transform.position.x);
        allowedToRun = true;
        anim.SetTrigger("Run");
        OnPlayerStartedRunning();
        // StartCoroutine(Run());
    }
    public void OnPlayerStartedRunning()
    {
        if (PlayerStartedRunning != null)
            PlayerStartedRunning(this);
    }
    #endregion
    #region methods
    //get the selected car from resources (the car which player 
    //selected in the main menu)
    public GameObject GetCarFromResources()
    {
         GameObject selectedCar=null;
         List<GameObject> cars= DataManager 
            .GetAllDataFromResourcesFolder<GameObject>("Cars");
         foreach(GameObject car in cars)
         {
             //if car is equipped(selected)
             if ((car.GetComponentInChildren<Car>()) &&
             (car.GetComponentInChildren<Car>().carData)&&
                (StoreInventory.IsVirtualGoodEquipped(
                    car.GetComponentInChildren<Car>().carData.itemId)))
                selectedCar = car;
         }
         return selectedCar.transform.root.gameObject;
    }
    bool IsGrounded(GameObject go)
    {
        return true;
        //if the bottom of character controller is close to ground
        //then it is grounded
        if (Physics.Raycast(go.gameObject.GetComponent<Renderer>()
            .bounds.min, Vector3.down, out hit, 0.1f))
        {
            if ((hit.collider) && (hit.collider.gameObject)
                && (hit.collider.gameObject.tag.Contains("Tile")))
                return true;
            else return false;
        }
        else return false;
    }
    IEnumerator Jump()
    {
        float vSpeed;
        Vector3 moveDirection = Vector3.zero;
        vSpeed = jumpForce;
        if (!IsGrounded(gameObject))
            yield break;
        do
        {
            vSpeed -= (gravityForce * Time.deltaTime);
            moveDirection.y = vSpeed;
            Debug.Log("jumpen ");
            rigidBody.velocity += Vector3.up*200f; 
            rigidBody.useGravity = true;
            yield return null;
            //run the raycasting somewhere other than the 
            //while loop itself, and do it on less frequently
        } while (!(IsGrounded(this.gameObject)));
    }
    private void ChangeLane(GameResources.Direction direction)
    {
        if (PlayerChangedLane != null)
            PlayerChangedLane(this);
            if(nextLane!=Lane.NONE)
                currentLane = nextLane;
        //change lanes independent of frame rate
        //changing lane will take exactly as much as lanechangetime
        //variable , regardless of how many frames per second
        //****Inaccurate calculations: i shouldn't use fixed delta time
        //in this coroutine since fixed delta time here will be 
        //different than next frame's.
        changingLanes = true;
        //the name of the parameter to trigger animation clip
        string animKey = "";
        switch(direction)
    {
        case GameResources.Direction.RIGHT:
        {
            animKey = "RightLaneChange";
            laneChangeDirection = GameResources.Direction.RIGHT;
            if(currentLane==Lane.RIGHT)
                return;
            else if(currentLane ==Lane.MIDDLE)
                nextLane= Lane.RIGHT;
            else //if current lane is left
                nextLane = Lane.MIDDLE;
            break;
        }
        case GameResources.Direction.LEFT:
        {
            animKey = "LeftLaneChange";
            laneChangeDirection = GameResources.Direction.LEFT;
        if(currentLane==Lane.LEFT)
                return;
            else if(currentLane ==Lane.MIDDLE)
                nextLane= Lane.LEFT;
            else //if current lane is left
                nextLane = Lane.MIDDLE;
            break;
        }
        
    }
    //assign lane change target based on where the lane changing direction will be
    switch(nextLane)
    {
        case Lane.RIGHT:
        {
            laneChangeTarget = rightLane.transform.position;
            break;
        }
        case Lane.LEFT:
        {
            laneChangeTarget = leftLane.transform.position;
            break;
        }
        case Lane.MIDDLE:
        {
            laneChangeTarget = middleLane.transform.position;
            break;
        }
    }
    
    cachedXPos = car.transform.position.x;
    
    horizontalMoveAmount = (cachedXPos - laneChangeTarget.x);
    laneChangeStep = (horizontalMoveAmount/15f);
    anim.SetTrigger(animKey);
        
    }
    public void Die()
    {
        StopAllCoroutines();
        //the player will dispatch it's dying event when the 
        //animation is done
        anim.SetTrigger("Die");
    }
    #endregion
}
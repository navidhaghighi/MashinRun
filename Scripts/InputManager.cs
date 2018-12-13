#region nameSpaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
#endregion
public class InputManager:MonoBehaviour
{
    #region variables
    bool allowedToCheckForInputs = true;
    IEnumerator listenForInputs;
    [SerializeField]
    private GameManager gameManager;
    float swipeAmount = (Screen.width/10);
    Boolean isTouching;
    private static InputManager instance;
    public static InputManager Instance
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
    #region events_and_delegates
    public delegate void EscapeKeyDetectedEventHandler();
    public event EscapeKeyDetectedEventHandler EscapeKeyDetected;

    public delegate void SwipeDetectedEventHandler
        (GameResources.Direction direction);
    public event SwipeDetectedEventHandler SwipeDetected;
    #endregion
    #region monobehaviour_events
    void Awake()
    {
        Instance = this;
        listenForInputs = ListenForInputs();
        StartCoroutine(listenForInputs);
    }
    #endregion
    #region custom_events
    public void OnSwipeDetected(GameResources.Direction direction)
    {
        if (SwipeDetected != null)
            SwipeDetected(direction);
    }
    #endregion
    #region methods
    IEnumerator ListenForInputs()
    {
        Vector2 touchStartPosition = Vector2.zero;
        //the distance between the two touches
        Vector2 touchesDistance = Vector2.zero;
        while (allowedToCheckForInputs)
        {
            //even when time scale is 0 , this class can check
            //for escape key press, but no other input
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (EscapeKeyDetected != null)
                    EscapeKeyDetected();
            }
            //if time scale is zero skip detecting input
            if (Time.timeScale == 0)
            {
                //continue after skipping one frame
                yield return null;
                continue;
            }
        #if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnSwipeDetected(GameResources.Direction.LEFT);
            }
            else if (Input.GetKeyDown(KeyCode.D))
                OnSwipeDetected(GameResources.Direction.RIGHT);
            else if (Input.GetKeyDown(KeyCode.Space))
                OnSwipeDetected(GameResources.Direction.UP);
        #endif
        #if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        {
                            touchStartPosition = touch.position;
                            break;
                        }
                    case TouchPhase.Moved:
                        {
                            isTouching = true;
                            break;
                        }
                    case TouchPhase.Ended:
                        {
                            isTouching = false;
                            touchesDistance = (touch.position - touchStartPosition);
                            if (touchesDistance.x > swipeAmount)
                                OnSwipeDetected(GameResources.Direction.RIGHT);
                            else if (touchesDistance.x < -(swipeAmount))
                                OnSwipeDetected(GameResources.Direction.LEFT);
                            break;
                        }

                }
                

            }
        #endif
            yield return null;
        }
    }
    #endregion
}
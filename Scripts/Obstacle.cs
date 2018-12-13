#region nameSpaces
using UnityEngine;
using System;
#endregion
public class Obstacle :  MonoBehaviour,IObstacle
{
    #region variables
    public AudioSource audioSource;
    SoundManager soundManager;
    #endregion
    #region events_and_delegates
    public delegate void CarHitObstacleEventHandler
        (Car car, Obstacle obstacle);
    public event CarHitObstacleEventHandler CarHitObstacle;
    #endregion
    #region monobehaviour_events
    void Awake()
    {
        soundManager = SoundManager.Instance;
        CarHitObstacle += soundManager.OnCarHitObstacle;
    }
    void OnCollisionEnter(Collision collision)
    {
        if ((collision. collider.gameObject) &&
            (collision. collider.gameObject.CompareTag("Car")))
            {
                if(collision.collider.gameObject.transform.childCount>0)
                    Hit(collision. collider.gameObject.transform.
                    GetChild(0).GetComponent<Car>());
                else Hit(collision. collider.gameObject
                    .GetComponent<Car>());
            }
    }
    #endregion
    #region methods
    public void Hit(Car car)
    {
        car.player.Die();
        if (CarHitObstacle != null)
            CarHitObstacle(car, this);
    }
    #endregion
}
#region nameSpaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
public class Coin : MonoBehaviour
{
    #region variables
    //required component reference for sound manager to play sound via this gameobject.
    public AudioSource audioSource;
    SoundManager soundManager;
    ScoreManager scoreManager;
    public int amount;
    #endregion
    #region events_and_delegates
    public delegate void CoinCollectedEventHandler(Coin coin);
    public event CoinCollectedEventHandler CoinCollected;
    #endregion
    #region monobehaviour_events
    void LateUpdate()
    {
        
        if(Time.timeScale!=0)
            transform.Rotate(Vector3.up);
    }
    void Awake()
    {
        scoreManager = ScoreManager.Instance;
        soundManager = SoundManager.Instance;
        CoinCollected += soundManager.OnCoinCollected;
        CoinCollected += scoreManager.OnCoinCollected;
    }
    void OnTriggerEnter(Collider collider)
    {
        if ((collider.gameObject)&&
            (collider.gameObject.CompareTag("Car")))
                OnCoinCollected();
    }
    #endregion
    #region methods
    void OnCoinCollected()
    {
        if (CoinCollected != null)
            CoinCollected(this);
        gameObject.SetActive(false);
    }
    #endregion
}
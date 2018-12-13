#region nameSpaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Soomla;
using Soomla.Store;
#endregion
public class ScoreManager : MonoBehaviour
{
    #region variables
    string mostCoinsInASessionKey = "most_coins_in_a_session";
    //key for player prefs
    string highestTraveledDistanceKey = "highest_traveled_distance";
    //in one session
    int mostCoins;
    int coinsUntilThisSession;
    //coins collected on this session
    int thisSessionCoins;
    [SerializeField]
    private GameManager gameManager;
    ScoreData data;
    public Player player;
    #region singleton
    private static ScoreManager instance;
    public static ScoreManager Instance
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
    float highestTraveledDistance;
    private float HighestTraveledDistance
    {
        set
        {
            highestTraveledDistance = value;
            OnHighestTraveledDistanceChanged(highestTraveledDistance);
        }
        get { return highestTraveledDistance; }

    }
    private float traveledDistance;
    public float TraveledDistance
    {
        set 
        {
        
         traveledDistance = value;
         if (traveledDistance > HighestTraveledDistance)
         {
             HighestTraveledDistance = traveledDistance;
         }
         OnTraveledDistanceChanged(traveledDistance);
        }
        get 
        { 
         return traveledDistance;
        }
    }

    #endregion
    #region events_and_delegates
    public delegate void HighestTraveledDistanceChangedEventHandler(float newAmount);
    public event HighestTraveledDistanceChangedEventHandler HighestTraveledDistanceChanged;
    
    public delegate void TraveledDistanceChangedEventHandler(float newmount);
    public event TraveledDistanceChangedEventHandler TraveledDistanceChanged;
    
    public delegate void MoneyChangedEventHandler(int amount);
    public event MoneyChangedEventHandler MoneyChanged;
    #endregion
    #region monobehaviour_events
    void Start()
    {
        if (!Instance)
            Instance = this;
        else Destroy(gameObject);
        player.PlayerMovedForward += OnPlayerMovedForward;
        player.PlayerDied += OnPlayerDied;
        data = DataManager.GetDataFromResources<ScoreData>
            ("ScoreData");
        if(PlayerPrefs.HasKey(highestTraveledDistanceKey))
            HighestTraveledDistance = PlayerPrefs.GetFloat(highestTraveledDistanceKey);
        else
        {
            PlayerPrefs.SetFloat(highestTraveledDistanceKey,0f);
            HighestTraveledDistance = 0f;
        }
        if(PlayerPrefs.HasKey(mostCoinsInASessionKey))
            mostCoins = PlayerPrefs.GetInt(mostCoinsInASessionKey);
        else{ 
            PlayerPrefs.SetInt(mostCoinsInASessionKey,0);
            mostCoins = 0;
        }
        gameManager.GameSceneLoaded += OnGameSceneLoaded;
    }
    void OnDestroy()
    {
        SaveData();
        Instance = null;
    }
    #endregion
    #region methods
    public int GetSessionCoins()
    {
        if (thisSessionCoins == 0)
        {
            thisSessionCoins = StoreInventory.GetItemBalance
                (MashinRunAssets.MASHIN_RUN_CURRENCY_ITEM_ID) 
                - coinsUntilThisSession;
            if (thisSessionCoins > mostCoins)
                mostCoins = thisSessionCoins;
        }
        return thisSessionCoins;
    }
    public float GetHighestTraveledDistance()
    {
        return highestTraveledDistance;
    }
    public int GetMostCoins()
    {
        return mostCoins;
    }
    public void OnHighestTraveledDistanceChanged(float newAmount)
    {
        if(HighestTraveledDistanceChanged!=null)
           HighestTraveledDistanceChanged(newAmount);
    }
    void SaveData()
    {
        if(PlayerPrefs.HasKey(mostCoinsInASessionKey))
            PlayerPrefs.SetInt(mostCoinsInASessionKey,mostCoins);
        if(PlayerPrefs.HasKey(highestTraveledDistanceKey))
            PlayerPrefs.SetFloat(highestTraveledDistanceKey,HighestTraveledDistance);
    }
    #endregion
    #region custom_events

    public void OnPlayerDied(Player player)
    {
        thisSessionCoins = StoreInventory.GetItemBalance
            (MashinRunAssets.MASHIN_RUN_CURRENCY_ITEM_ID) 
                - coinsUntilThisSession;
        if (thisSessionCoins > mostCoins)
            mostCoins = thisSessionCoins;
    }
    public void OnGameSceneLoaded()
    {
        thisSessionCoins = 0;
        coinsUntilThisSession = StoreInventory.GetItemBalance
            (MashinRunAssets.MASHIN_RUN_CURRENCY_ITEM_ID);
        TraveledDistance = 0f;
    }
    public void OnCoinCollected( Coin coin)
    {
        // Debug.Log("give coin");
        //it has huge overhead to be called every few frame
        StoreInventory.GiveItem(MashinRunAssets
            .MASHIN_RUN_CURRENCY_ITEM_ID, coin.amount);
    }
    public void OnPlayerMovedForward()
    {
        TraveledDistance += 1f;
    }
    public void OnTraveledDistanceChanged(float newAmount)
    {
        if (TraveledDistanceChanged != null)
            TraveledDistanceChanged(newAmount);
    }
    public void OnMoneyChanged(int newAmount)
    {
        if (MoneyChanged != null)
            MoneyChanged(newAmount);
    }
    #endregion
}
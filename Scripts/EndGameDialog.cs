using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameDialog : MonoBehaviour
{
    #region variables
    [SerializeField]
    private Text highestTraveledDistanceText;
    string meterFa;
    [SerializeField]
    private Text traveledDistanceText;
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    private Text sessionCoinsText;
    [SerializeField]
    private Text mostCoinsText;
    #endregion
    void Awake()
    {
        meterFa = Fa.faConvert("متر");
    }
    void OnEnable()
    {
        sessionCoinsText.text = scoreManager
            .GetSessionCoins().ToString();
         mostCoinsText.text = scoreManager.GetMostCoins
             ().ToString();
         traveledDistanceText.text =
          meterFa + scoreManager.TraveledDistance.ToString();
         highestTraveledDistanceText.text = meterFa + 
             scoreManager.GetHighestTraveledDistance().ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreDialog : MonoBehaviour {
    [SerializeField]
    //most coin in one session
    private Text mostCoinText;
    [SerializeField]
    private Text highestTraveledDistanceText;
    [SerializeField]
    private ScoreManager scoreManager;
    void OnEnable()
    {
        highestTraveledDistanceText.text =scoreManager
            .GetHighestTraveledDistance().ToString();
        mostCoinText.text = scoreManager.GetMostCoins().ToString();
    }
}

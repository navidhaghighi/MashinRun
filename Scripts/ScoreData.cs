#region nameSpaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#endregion
[CreateAssetMenu(fileName= "Create", menuName= "ScoreData")]
public class ScoreData:ScriptableObject
{
    #region variables
    public int mostCoinsInASession;
    public float highestTraveledDistance;
    public int money;
    #endregion
}
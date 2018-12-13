#region nameSpaces
using System;
using UnityEngine;
#endregion
[CreateAssetMenu(fileName = "Create" , menuName="CarData")]
public class CarData:ScriptableObject
{
    #region variables
    public string itemId;
    public bool isSelected;
    public float speedRate;
    public float runSpeed;
    public new string name;
    public int price;  
    #endregion 
}
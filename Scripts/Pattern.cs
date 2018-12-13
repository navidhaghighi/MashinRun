#region nameSpaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion
[CreateAssetMenu(fileName="New Pattern",menuName="Pattern")]
public class Pattern:ScriptableObject
{
    public GameResources.CellData[] rightLane;
    public GameResources.CellData[] midLane;
    public GameResources.CellData[] leftLane;
}
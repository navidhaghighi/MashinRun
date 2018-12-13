#region nameSpaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion
public static class DataManager
{
    
    #region methods
    //if returned value is true then the game sounds will be on
    public static Boolean GetSoundState()
    {
        if(PlayerPrefs.GetInt("sound_state")==1) return true;
        else return false;
    }
    public static List<T> GetAllDataFromResourcesFolder<T>(string path)
    {
        object[] data;
        List<T> tList = new List<T>();
        data = Resources.LoadAll(path, typeof(T));
        foreach (object obj in data)
        {
            tList.Add((T)obj);   
        }
        return tList;
    }
    public static T GetDataFromResources<T>(string path)
    {
        object data;
         data = Resources.Load(path,typeof(T));
         return (T)data;
    }
    #endregion

}
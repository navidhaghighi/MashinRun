using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {
    Text text;
	// Use this for initialization
    void Awake()
    {
        text = GetComponent<Text>();
    }
    //called by animation event 
    void OnDisable()
    {
        gameObject.SetActive(false);
    }
}

#region namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Soomla;
using Soomla.Store;
#endregion

public class Car : MonoBehaviour
{
    #region variables
    [SerializeField]
    new private Rigidbody rigidbody;
    public VirtualGood virtualGood;
    public Animator anim;
    public Player player;
    public CarData carData;
    #endregion
    #region monobehaviour_events
   /*  void OnEnable()
    {
        rigidbody.useGravity = true;
    }*/
    #endregion
    #region methods
    public void Die()
    {
        player.OnPlayerDied();
    }
    #endregion
}

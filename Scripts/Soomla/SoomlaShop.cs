#region namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Soomla;
using Soomla.Store;
using UnityEngine.UI;
#endregion
public class SoomlaShop : MonoBehaviour
{
    #region variables
    private string cancelledPurchase = Fa.faConvert(
        "خرید کنسل شد! ");
    private string successfullPurchase = Fa.faConvert(
        "خرید انجام شد! ");
    [SerializeField]
    UIManager uiManager;
    [SerializeField]
    private Text buyingStateText;
    #endregion
    #region monobehaviour_events
    // Use this for initialization
	void Start () {
        SoomlaStore.Initialize(new MashinRunAssets());
        StoreEvents.OnMarketPurchaseCancelled += Failed;
        StoreEvents.OnMarketPurchase += OnMarketPurchase;
        StoreEvents.OnItemPurchased += Purchased;
        StoreEvents.OnMarketPurchaseCancelled += Canceled;
	}
    private void Canceled(PurchasableVirtualItem obj)
    {
        //the one being called by bazaar when purchase is cancelled
        uiManager.ShowMessage(cancelledPurchase);
    }
    #endregion
    #region methods
    public void Buy(string itemId)
    {
       StoreInventory .BuyItem (itemId,itemId+"payload");
    }
    #endregion
    #region custom_events
    private void Purchased(PurchasableVirtualItem arg1,
        string arg2)
    {
        //the one being called by baazar when purchase is done.
        uiManager.ShowMessage(successfullPurchase);
    }
    private void Failed(PurchasableVirtualItem obj)
    {

    }
    
    public void OnMarketPurchase(PurchasableVirtualItem item
        , string purchaseToken, Dictionary<string, string>
        payload)
    {
        
    }
    #endregion
}

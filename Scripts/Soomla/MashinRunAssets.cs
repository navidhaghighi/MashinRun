#region nameSpaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Soomla;
using Soomla.Store;
#endregion
public class MashinRunAssets : IStoreAssets
{
    #region variables
    public static LifetimeVG fiatCar;
    public static LifetimeVG volksWagenCar;
    public static LifetimeVG sportCar;
    public static LifetimeVG beetleCar;
    public int someInt;
    public const string MASHIN_RUN_CURRENCY_ITEM_ID =
        "com.mashinran.currency";
    public const string TEN_THOUSAND_COINS_PACK_ITEM_ID =
        "ten_thousand_coins";
    public const string TWENTY_THOUSAND_COINS_PACK_ITEM_ID =
        "twenty_thousand_coins";
    public const string FIFTY_THOUSAND_COINS_PACK_ITEM_ID =
        "fifty_thousand_coins";
    public static VirtualCurrency mashinRunCurrency;
    public static VirtualCurrencyPack twentyThousandCoinsPack;
    public static VirtualCurrencyPack fiftyThousandCoinsPack;


    public static VirtualCurrencyPack tenThousandCoinsPack;
    public static VirtualCategory moneyCategory;
    #endregion
    #region ctor
    public MashinRunAssets()
    {
        mashinRunCurrency = new VirtualCurrency("Coin", "the currency of the mashin run game"
    , "com.mashinran.currency");
        moneyCategory = new VirtualCategory("money category"
    , new List<string>(new string[] { mashinRunCurrency.ItemId }));
        fiatCar = new EquippableVG
        (EquippableVG.EquippingModel.GLOBAL, "fiat"
        , "Fiat", "com.mashinrun.fiat", new PurchaseWithVirtualItem
        (mashinRunCurrency.ItemId
            , 2000));
        volksWagenCar = new EquippableVG
     (EquippableVG.EquippingModel.GLOBAL, "volksWagen"
     , "Volks wagen", "com.mashinrun.volkswagen", new PurchaseWithVirtualItem
         (mashinRunCurrency.ItemId
             , 0));
        sportCar = new EquippableVG(
    EquippableVG.EquippingModel.GLOBAL, "sportcar"
, "Sport car", "com.mashinrun.sportcar", new PurchaseWithVirtualItem
    (mashinRunCurrency.ItemId, 20000));

        beetleCar = new EquippableVG(
         EquippableVG.EquippingModel.GLOBAL, "beetle"
         , "Beetle car", "com.mashinrun.beetle", new PurchaseWithVirtualItem
         (mashinRunCurrency.ItemId, 8000));

        twentyThousandCoinsPack = new VirtualCurrencyPack
         ("Fifty ThousandCoins", "Fifty thousand coins",
         "twenty_thousand_coins", 20000
          , mashinRunCurrency.ItemId, new PurchaseWithMarket
          (TWENTY_THOUSAND_COINS_PACK_ITEM_ID,
              20000));
        fiftyThousandCoinsPack = new VirtualCurrencyPack
("Fifty ThousandCoins", "Fifty thousand coins",
"fifty_thousand_coins", 50000
 , mashinRunCurrency.ItemId, new PurchaseWithMarket
 (FIFTY_THOUSAND_COINS_PACK_ITEM_ID, 50000));

        tenThousandCoinsPack =
               new VirtualCurrencyPack("Ten ThousandCoins",
                   "Ten thousand coins", "ten_thousand_coins"
                   , 10000
                , mashinRunCurrency.ItemId, new PurchaseWithMarket
                    ("com.mashinran.ten_thousand_coins", 10000));
    }
    #endregion
    #region overriden_methods
    public VirtualGood[] GetGoods()
    {
        return new LifetimeVG[]{fiatCar,beetleCar,sportCar,volksWagenCar
            };
    }
    public VirtualCurrency[] GetCurrencies()
    {
        return new VirtualCurrency[] { mashinRunCurrency };
    }
    public VirtualCurrencyPack[] GetCurrencyPacks()
    {
        return new VirtualCurrencyPack[] { tenThousandCoinsPack
        ,twentyThousandCoinsPack,fiftyThousandCoinsPack};
    }
    public VirtualCategory[] GetCategories()
    {
        return new VirtualCategory[] { moneyCategory };
    }
    public int GetVersion()
    {
        return 0;
    }

    #endregion
}
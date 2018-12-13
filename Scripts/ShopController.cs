#region nameSpaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using Soomla;
using Soomla.Store;
#endregion
namespace Soomla.Store
{
    public class ShopController : MonoBehaviour
    {
        #region variables
        private string unBoughtString = Fa.faConvert("خریداری نشده");
        private string boughtString = Fa.faConvert("خریداری شده");
        [SerializeField]
        private Text carStatus;
        [SerializeField]
        private Text carLaneRateText;
        [SerializeField]
        //text to represent each car speed on car's stats panel
        private Text carSpeedText;
        private string notEnoughMoneyMessage = Fa.faConvert(
           "پولت کافی نیست");
        [SerializeField]
        private GameObject buyDialog;
        [SerializeField]
        private UIManager uiManager;
        [SerializeField]
        private ScoreManager scoreManager;
        [SerializeField]
        private GameObject rightArrowButton;
        [SerializeField]
        private GameObject leftArrowButton;
        [SerializeField]
        private GameObject selectCarButton;
        [SerializeField]
        private GameObject currentShowCasedCar = null;
        GameObject selectedCar;
        bool areCarsSpawned = false;
        [SerializeField]
        private InputManager inputManager;
        [SerializeField]
        private Text carNameText;
        [SerializeField]
        private GameObject carPriceUI;
        [SerializeField]
        private Text carPriceText;
        [SerializeField]
        private GameObject buyCarButton;
        [SerializeField]
        private Player player;
        [SerializeField]
        private GameObject platform;
        [SerializeField]
        private Transform showCaseTransform;
        [SerializeField]
        //the room in which player can browse cars
        private GameObject showCaseRoom;
        Vector3 cameraDistanceFromCars = new Vector3(4f, 0f, 4f);
        new Camera camera;
        Vector3 carsDistance = new Vector3(20f, 0f, 0f);
        List<GameObject> cars;
        [SerializeField]
        private GameManager gameManager;
        #endregion
        #region monobehaviour_events
        void Awake()
        {
            StoreEvents.OnSoomlaStoreInitialized += OnStoreInitialized;
        }
        #endregion
        #region soomla_events
        //will be called by soomla
        public void OnStoreInitialized()
        {
            Debug.Log("store init ");
            int timesPlayed = 0;
            if (PlayerPrefs.HasKey("times_played"))
                timesPlayed = PlayerPrefs.GetInt("times_played");

            Debug.Log(StoreInventory.GetItemBalance(MashinRunAssets.mashinRunCurrency.ItemId));
            if (timesPlayed == 0)
            {
                StoreInventory.BuyItem(MashinRunAssets.volksWagenCar.ItemId);
                StoreInventory.EquipVirtualGood(MashinRunAssets.volksWagenCar.ItemId);
            }
            timesPlayed++;
            PlayerPrefs.SetInt("times_played", timesPlayed);
            cars = DataManager.GetAllDataFromResourcesFolder
                <GameObject>("Cars");
            Debug.Log("cars.Count " + cars.Count);
            Debug.Log("cars");
            Debug.Log("cars.Count" + cars.Count);
            camera = Camera.main;
        }
        #endregion
        #region methods
        public void SelectCar()
        {
            //soomla will automatically unequip all other cars.
            //equip the current showcased car.
            StoreInventory.EquipVirtualGood(currentShowCasedCar.GetComponentInChildren<Car>
                ()
                .virtualGood.ItemId);
            selectedCar = currentShowCasedCar;
            UpdateUI();
        }
        void UpdateUI()
        {
            Debug.Log("currentShowCasedCar" + currentShowCasedCar);

            if (!(currentShowCasedCar.GetComponentInChildren<Car>())
                || !(currentShowCasedCar.GetComponentInChildren<Car>()).carData)
                return;
            Car currentCar = currentShowCasedCar
                .GetComponentInChildren<Car>();
            int currentShowCasedCarIndex = cars.FindIndex
                (x => x == currentShowCasedCar);
            if ((currentCar) && (currentCar.carData))
            {
                carPriceText.text = currentCar.carData.price.ToString();
                carNameText.text = currentCar.carData.name.ToString();
                carSpeedText.text = currentCar.carData.speedRate.ToString();
            }
            if (currentShowCasedCarIndex == 0)
            //if there's no previous car
            {
                rightArrowButton.SetActive(true);
                leftArrowButton.SetActive(false);
            }
            else if (currentShowCasedCarIndex == cars.Count - 1)
            //if there's no next car
            {
                leftArrowButton.SetActive(true);
                rightArrowButton.SetActive(false);
            }
            else // if there's a next car and a previous car
            {
                leftArrowButton.SetActive(true);
                rightArrowButton.SetActive(true);
            }
            //if current car wasn't bought
            if ((currentCar) && (currentCar.carData)
               && (currentCar.virtualGood.GetBalance() == 0))
            {
                carStatus.text = unBoughtString;
                carPriceUI.SetActive(true);
                carPriceText.text = currentCar.carData.price
                    .ToString();
                buyCarButton.SetActive(true);
                selectCarButton.SetActive(false);
            }
            else if ((currentCar) && (currentCar.carData)
              && (currentCar.virtualGood.GetBalance() > 0))
            //if current showcased car was bought
            {
                carStatus.text = boughtString;
                buyCarButton.SetActive(false);
                carPriceUI.SetActive(false);
                if (!StoreInventory.IsVirtualGoodEquipped(
                     currentCar.virtualGood.ItemId))
                {
                    selectCarButton.SetActive(true);
                }
                else // if current show cased car is selected
                {
                    selectCarButton.SetActive(false);
                }

            }
            //if either there is no car or it doesn't have a car data attached.
            else if (!(currentCar) || !(currentCar.carData))

            {
                Debug.Log("couldn't update UI because either there's no car , " +
                    "or it doesn't have a car data attached");
            }
        }
        public void AttemptBuyCar()
        {

            Car showCasedCar = currentShowCasedCar.
                GetComponentInChildren<Car>();
            Debug.Log(((PurchaseWithVirtualItem)showCasedCar.virtualGood
            .PurchaseType).Amount);
            if (StoreInventory.GetItemBalance(MashinRunAssets.
               MASHIN_RUN_CURRENCY_ITEM_ID) >= ((PurchaseWithVirtualItem)
                showCasedCar.virtualGood.PurchaseType).Amount/*price */)
            {
                uiManager.ShowDialog(buyDialog);
            }
            else uiManager.ShowMessage(notEnoughMoneyMessage);
        }
        public void BuyCar()
        {

            Car showCasedCar = currentShowCasedCar.
                GetComponentInChildren<Car>();
            if (StoreInventory.GetItemBalance(MashinRunAssets.
                MASHIN_RUN_CURRENCY_ITEM_ID) >= showCasedCar.carData.price)
            {
                showCasedCar.virtualGood.Buy("somepayload");
                UpdateUI();
            }
        }
        //change current showcased car
        public void ChangeCar(string toWhich)
        {
            currentShowCasedCar.SetActive(false);
            int currentCarIndex;
            //make sure required index won't exceed list indices.
            try
            {
                currentCarIndex = cars.FindIndex(s => s ==
                   currentShowCasedCar);
                if (toWhich == "Previous")
                    currentShowCasedCar = cars[(currentCarIndex) - 1];
                else currentShowCasedCar = cars[(currentCarIndex) + 1];
            }
            catch (Exception e)
            {
                Debug.Log("couldn't fetch car, because " + e);
            }
            currentShowCasedCar.SetActive(true);
            UpdateUI();
        }
        void SpawnCars()
        {
            Debug.Log("spawner car count " + cars.Count);
            try
            {
                Debug.Log("try " + "cars.Count"
                + cars.Count);
                for (int i = 0; i < cars.Count; i++)
                {
                    Debug.Log("cars[i]" + cars[i]);
                    Debug.Log("showCaseTransform" + showCaseTransform);
                    //instantiate cars from assets into scene
                    cars[i] = Instantiate(cars[i]);
                    cars[i].transform.parent =
                    platform.transform;
                    cars[i].transform.position
                       = showCaseTransform.position;
                    cars[i].SetActive(false);
                }
                //put currently selected car on platform
                foreach (GameObject car in cars)
                {
                    Debug.Log(car.GetComponentInChildren<Car>());
                    if ((car) && (car.GetComponentInChildren<Car>())
                       && (StoreInventory.IsVirtualGoodEquipped(car
                       .GetComponentInChildren<Car>().virtualGood.ItemId)))
                    {
                        selectedCar = car;
                        break;
                    }
                }
                if (!selectedCar) // if there's no selected car 
                {
                    Debug.Log("no selected car");
                    //mark a bought car as selected
                    foreach (GameObject car in cars)
                    {
                        // at the beginning there's at least one bought car
                        if ((car) && (car.GetComponentInChildren<Car>()) &&
                      (car.GetComponentInChildren<Car>().virtualGood
                        .GetBalance() > 0))
                        {
                            selectedCar = car;
                            StoreInventory.EquipVirtualGood(car
                                .GetComponentInChildren<Car>().virtualGood.ItemId);
                            break;
                        }
                    }
                }
                //sort by price
                cars = cars.OrderBy(o => o.GetComponentInChildren
                    <Car>().carData
                    .price).ToList<GameObject>();
                areCarsSpawned = true;
            }
            catch (Exception e)
            {
                Debug.Log("i caught this err " + e);
            }
        }
        public void OpenShop()
        {
            gameManager.ChangeGameState(GameManager.GameState.IN_THE_SHOP);
            inputManager.SwipeDetected += OnSwipeDetected;
            if (!showCaseRoom.activeInHierarchy)
                showCaseRoom.SetActive(true);
            if (!areCarsSpawned)
                SpawnCars();
            if ((selectedCar) && !(selectedCar.activeInHierarchy))
                selectedCar.SetActive(true);
            currentShowCasedCar = selectedCar;
            UpdateUI();
        }
        public void CloseShop()
        {
            Debug.Log("closing down shop game man " + gameManager);
            gameManager.ChangeGameState(GameManager.GameState.MAIN);
            currentShowCasedCar.SetActive(false);
            showCaseRoom.SetActive(false);
            inputManager.SwipeDetected -= OnSwipeDetected;
        }
        #endregion
        #region custom_events
        public void OnSwipeDetected(GameResources.Direction direction)
        {
            if (direction == GameResources.Direction.RIGHT)
                ChangeCar("Previous");
            else if (direction == GameResources.Direction.LEFT)
                ChangeCar("Next");
        }
        #endregion
    }
}
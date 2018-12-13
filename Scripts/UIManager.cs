#region nameSpaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Soomla;
using Soomla.Store;
#endregion
public class UIManager : MonoBehaviour
{
    #region variables
    [SerializeField]
    GameObject storeIcon;
    [SerializeField]
    GameObject contactDevIcon;
    [SerializeField]
    GameObject exitDialog;
    [SerializeField]
    InputManager inputManager;
    //current dialog being showed up.
    GameObject currentDialog;
    [SerializeField]
    private Text endGameSessionCoinsText;
    [SerializeField]
    private Text endGameMostCoinsText;
    [SerializeField]
    private GameObject messageBar;
    [SerializeField]
    private Text messageBarText;
    [SerializeField]
    private GameObject backButton;
    [SerializeField]
    private GameObject shopUI;
    [SerializeField]
    private Text endGameUITraveledDistanceText;
    [SerializeField]
    private GameObject pauseIcon;
    [SerializeField]
    private GameObject soundOnIcon;
    [SerializeField]
    private GameObject soundOffIcon;
    [SerializeField]
    private SoundManager soundManager;
    string meterFa;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject mainMenuUI;
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject gameStartUI;
    [SerializeField]
    private ScoreManager scoreManager;
    public Player player;
    private static UIManager instance;
    public static UIManager Instance
    {
        set
        {
            instance = value;
        }
        get
        {
                return instance;
        }
    }
    public GameObject endGameUI;
    public Text highestTraveledDistanceText;
    [SerializeField]
    private Text traveledDistanceText;
    public Text moneyText;
    #endregion
    #region monobehaviour_events
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else Destroy(gameObject);
        meterFa = Fa.faConvert("متر");
        StoreEvents.OnSoomlaStoreInitialized += OnSoomlaStoreInitialized;
        //register for event only if there is ui element to show text in it.
        if(moneyText)
        StoreEvents.OnCurrencyBalanceChanged += OnCurrencyBalanceChanged;
        if(traveledDistanceText)
        scoreManager.TraveledDistanceChanged 
            += OnTraveledDistanceChanged;
        scoreManager.HighestTraveledDistanceChanged 
            += OnHighestTraveledDistanceChanged;
        gameManager. GameSceneLoaded += OnGameSceneLoaded;
        gameManager. GameSceneUnloaded += OnGameSceneUnloaded;
        gameManager. MainSceneLoaded += OnMainSceneLoaded;
        gameManager.GamePaused += OnGamePaused;
        gameManager.GameResumed += OnGameResumed;
        inputManager.EscapeKeyDetected += OnEscapeKeyDetected;
        soundManager.SoundsStateChanged += OnSoundStateChanged;
        player.PlayerDied += OnPlayerDied;
    }
    #endregion
    #region custom_events
    public void OnSoomlaStoreInitialized()
    {
        if(moneyText)
            moneyText.text = StoreInventory.GetItemBalance(MashinRunAssets
            .MASHIN_RUN_CURRENCY_ITEM_ID).ToString();
    }
    public void OnCurrencyBalanceChanged(VirtualCurrency
     currency, int newBalance,int someOtherInt)
    {
        if(currency.ItemId == MashinRunAssets.
            MASHIN_RUN_CURRENCY_ITEM_ID)
        moneyText.text = newBalance.ToString();

    }
    public void OnEscapeKeyDetected()
    {
        if(currentDialog)
        DismissCurrentDialog();
        else if(GameManager.currentGameState == 
        GameManager.GameState.MAIN) 
            ShowDialog(exitDialog);
    }
    public void OnHighestTraveledDistanceChanged(float newAmount)
    {
        highestTraveledDistanceText.text = 
          meterFa + newAmount.ToString() ;
    }
    public void OnSoundStateChanged(bool newState)
    {
        if (newState)
        {
            soundOnIcon.SetActive(true);
            soundOffIcon.SetActive(false);
        }
        else
        {
            soundOnIcon.SetActive(false);
            soundOffIcon.SetActive(true);
        }
    }
    public void OnMainSceneLoaded()
    {
        if (pauseUI)
            pauseUI.SetActive(false);
        if (pauseIcon)
            pauseIcon.SetActive(false);
        if (traveledDistanceText)
            traveledDistanceText.transform.parent.gameObject.
                SetActive(false);
        mainMenuUI.SetActive(true);
        if (storeIcon)
            storeIcon.SetActive(true);
        if (contactDevIcon)
            contactDevIcon.SetActive(true);
        if(highestTraveledDistanceText)
           highestTraveledDistanceText.transform.parent.
              gameObject.SetActive(false);
    }
    public void OnGameResumed()
    {
        if(pauseUI)
        {
           Time.timeScale = 1f;
           pauseUI.SetActive(false);
           pauseIcon.SetActive(true);
        }
    }
    public void OnGamePaused()
    {
        if(pauseUI)
        {
           Time.timeScale = 0f;
           pauseUI.SetActive(true);
           pauseIcon.SetActive(false);
        }
    }
    public void OnGameSceneLoaded()
    {
        if(gameStartUI)
           gameStartUI.SetActive(true);
        if(mainMenuUI)
           mainMenuUI.SetActive(false);
        if (storeIcon)
            storeIcon.SetActive(false);
        if (contactDevIcon)
            contactDevIcon.SetActive(false);
        if (pauseIcon)
            pauseIcon.SetActive(true);
        if (pauseUI)
            pauseUI.SetActive(false);
        if (traveledDistanceText)
            traveledDistanceText.transform.parent.
                gameObject.SetActive(true);
        if (highestTraveledDistanceText)
            highestTraveledDistanceText.transform.parent
                .gameObject.SetActive(true);
    }
    public void OnGameSceneUnloaded()
    {
        Time.timeScale= 1f;
        endGameUI.SetActive(false);
    }
    public void OnPlayerDied(Player player)
    {
        Time.timeScale = 0f;
         endGameUI.SetActive(true);
         pauseIcon.SetActive(false);
        
    }
    public void OnTraveledDistanceChanged(float newAmount)
    {
        traveledDistanceText.text =meterFa + newAmount.ToString();
    }
    public void OnMoneyChanged(int newAmount)
    {
        moneyText.text = newAmount.ToString();
    }
    #endregion
    #region methods
    public void ShowMessage(string message)
    {
        messageBar.SetActive(true);
        messageBarText.text = message;
    }
    public void ShowDialog(GameObject dialog)
    {
        currentDialog = dialog;
        dialog.SetActive(true);
    }
    public void DismissCurrentDialog()
    {
        if(currentDialog)
        {
            currentDialog.SetActive(false);
            currentDialog = null;
        }
    }
    public void DismissDialog(GameObject dialog)
    {
        dialog.SetActive(false);
        if(currentDialog)
        currentDialog = null;
    }
    public void ShowMainMenu()
    {
        if (shopUI.activeInHierarchy)
            shopUI.SetActive(false);
        mainMenuUI.SetActive(true);
        if (backButton.activeInHierarchy)
            backButton.SetActive(false);
    }
    public void ShowShop()
    {
        mainMenuUI.SetActive(false);
        shopUI.SetActive(true);
        backButton.SetActive(true);
    }
    #endregion
}
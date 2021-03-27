using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    public GameObject mainMenu, gameOverMenu, removeAdsBtn, giftPanel, intructionImg;
    public Button settingBtn, backBtn;
    public UIObjects UIO; //ref to the UIObject

    private bool gameStarted = false;
    private int i = 0; //this is to make game over coroutine fun only once

    [SerializeField]
    private Animator anim;

    public bool GameStarted
    {
        get { return gameStarted; }
        set { gameStarted = value; }
    }

    [HideInInspector]
    public managerVars vars;

    void OnEnable()
    {
        vars = Resources.Load<managerVars>("managerVarsContainer");
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

	// Use this for initialization
	void Start ()
    {
//#if AdmobDef
//        if (GameManager.instance.canShowAds) AdsManager.instance.ShowBannerAds();
//        else if (!GameManager.instance.canShowAds) AdsManager.instance.DestroyBannerAds();
//#endif

        //basic setting when the game star
        GameManager.instance.gameOver = false;//game over is false
        GameManager.instance.currentScore = 0;//current score is zero
        GameManager.instance.currentPoints = 0;//current point is zero
        //sound button
        if (GameManager.instance.isMusicOn == true)
        {
            AudioListener.volume = 1; 
            UIO.mainMenuUI.soundImage.sprite = vars.soundOffButton;
        }
        else
        {
            AudioListener.volume = 0;
            UIO.mainMenuUI.soundImage.sprite = vars.soundOnButton;
        }

        if (GameManager.instance.gameRestart)
        {
            GameManager.instance.gameRestart = false;
            PlayBtn();
        }
        //set the text values
        UIO.gameMenuUI.scoreText.text = "" + GameManager.instance.currentScore;
        UIO.gameMenuUI.starText.text = "" + GameManager.instance.points;
    } 

    void Update()
    {   //if game over is false and game started is true
        if (GameManager.instance.gameOver == false && gameStarted == true)
        {   //set the text values
            UIO.gameMenuUI.scoreText.text = "" + GameManager.instance.currentScore;
            UIO.gameMenuUI.starText.text = "" + GameManager.instance.currentPoints;
        }

        //if (GameManager.instance.canShowAds)//if can show ads is true 
        //{   //remove ads button is active
        //    removeAdsBtn.SetActive(true);
        //}
        //else //else deactive
        //{
        //    removeAdsBtn.SetActive(false);
        //}
        ////if reward ad is ready
        //if (UnityAds.instance.RewardAdReady == true)
        //{   //activate the reward ads button
        //    UIO.gameOverMenuUI.adsBtn.gameObject.SetActive(true);
        //}//else deactivate it
        //else if (UnityAds.instance.RewardAdReady == false)
        //{
        //    UIO.gameOverMenuUI.adsBtn.gameObject.SetActive(false);
        //}

    }

    #region MainMenu
    public void PlayBtn()
    {
        UIObjects.instance.ButtonPress();
        UIO.gameMenuUI.scoreText.gameObject.SetActive(true);
        mainMenu.SetActive(false);
        gameStarted = true;
        intructionImg.SetActive(true);
    }

    public void LeaderboardBtn()
    {
        UIObjects.instance.ButtonPress();
#if UNITY_ANDROID
        GooglePlayManager.singleton.OpenLeaderboardsScore();
#elif UNITY_IOS
        LeaderboardiOSManager.instance.ShowLeaderboard();
#endif
    }

    public void RemoveAdsBtn()
    {
        UIObjects.instance.ButtonPress();
        Purchaser.instance.BuyNoAds();
    }

    public void SoundBtn()
    {
        if (GameManager.instance.isMusicOn == true)
        {
            GameManager.instance.isMusicOn = false;
            AudioListener.volume = 0;
            UIO.mainMenuUI.soundImage.sprite = vars.soundOnButton;
            GameManager.instance.Save();
        }
        else
        {
            GameManager.instance.isMusicOn = true;
            AudioListener.volume = 1;
            UIO.mainMenuUI.soundImage.sprite = vars.soundOffButton;
            GameManager.instance.Save();
        }

        UIObjects.instance.ButtonPress();
    }

    public void RateBtn()
    {
        UIObjects.instance.ButtonPress();
        Application.OpenURL(vars.rateButtonUrl);
    }

    public void RestorePurchaseBtn()
    {
        UIObjects.instance.ButtonPress();
        Purchaser.instance.RestorePurchases();
    }

    public void BackBtn()
    {
        anim.Play("MainBtnSlideOut");
        StartCoroutine(StopSettingBtnClick());
    }

    public void SettingsBtn()
    {
        anim.Play("MainBtnSlideIn");
        StartCoroutine(StopSettingBtnClick());
    }

    #endregion

    #region GameOver Menu

    public void ShareBtn()
    {
        UIObjects.instance.ButtonPress();
        ShareScreenShot.instance.ShareMethode();
    }

    public void HomeBtn()
    {
        UIObjects.instance.ButtonPress();
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void RestartBtn()
    {
        UIObjects.instance.ButtonPress();
        GameManager.instance.gameRestart = true;
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void AdsBtn()
    {
        UIObjects.instance.ButtonPress();
        //call the reward ads methode here
        UnityAds.instance.ShowRewardedAd();
    }

    public void GiftBtn()
    {
        UIObjects.instance.ButtonPress();
        GameManager.instance.points += 20;      //increase the coins
        GameManager.instance.Save();           //save it
        UIO.gameMenuUI.starText.text = "" + GameManager.instance.points;
        GiftTimeTracker.instance.TrackTime();  //start the timer
        giftPanel.SetActive(false);
    }

    public void CloseGiftAdsPanel()
    {
        UIObjects.instance.ButtonPress();
        giftPanel.SetActive(false);
    }

    public void FacebookLikeBtn()
    {
        Application.OpenURL(vars.facebookPage);
    }

    #endregion

    public void GameIsOver()
    {
        StartCoroutine(GameOver());//game over coroutine
    }

    private IEnumerator GameOver()
    {
        if (GameManager.instance.currentScore > GameManager.instance.bestScore)
        {
            GameManager.instance.bestScore = GameManager.instance.currentScore;
            GameManager.instance.Save();
        }

//#if AdmobDef
//        AdsManager.instance.ShowInterstitial();
//#endif

        GameManager.instance.lastScore = GameManager.instance.currentScore;
        UIO.gameMenuUI.scoreText.gameObject.SetActive(false);
        UIO.gameOverMenuUI.gameOverScoreText.text = "" + GameManager.instance.currentScore;
        UIO.gameOverMenuUI.gameOverBestScoreText.text = "" + GameManager.instance.bestScore;
        UIO.shopMenuUI.starText.text = "" + GameManager.instance.points;

        yield return new WaitForSeconds(1f);
        gameOverMenu.SetActive(true);
        ActivateGiftPanel();
    }

    private IEnumerator StopSettingBtnClick()
    {
        settingBtn.interactable = false;
        backBtn.interactable = false;
        yield return new WaitForSeconds(0.3f);
        settingBtn.interactable = true;
        backBtn.interactable = true;
    }

    public void ActivateGiftPanel()
    {
        if (GameManager.instance.gameOver)
        {
            if (GiftTimeTracker.instance.giftReady)
            {
                giftPanel.SetActive(true);
            }
        }
    }

}

using UnityEngine;
using System.Collections;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class UnityAds : MonoBehaviour
{

    public static UnityAds instance;

    private int i = 0;
    private bool rewardAdReady = false;

    public bool RewardAdReady
    {
        get { return rewardAdReady; }
        set { rewardAdReady = value; }
    }

    [HideInInspector]
    public managerVars vars;

    void OnEnable()
    {
        vars = Resources.Load("managerVarsContainer") as managerVars;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    void Start()
    {
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Uncomment after activating unity ads
#if UNITY_ADS
        if (Advertisement.IsReady("rewardedVideo"))
        {
            rewardAdReady = true;
        }
        else if (!Advertisement.IsReady("rewardedVideo"))
        {
            rewardAdReady = false;
        }
#endif
        if (GameManager.instance == null)
            return;

        if (GameManager.instance.gameOver == true)
        {
            //we want only one ad to be shown so we put condition that when i is 0 we show ad.
            if (i == 0)
            {
                i++;
                GameManager.instance.gamesPlayed++;

                if (GameManager.instance.gamesPlayed >= vars.showInterstitialAfter)
                {
                    GameManager.instance.gamesPlayed = 0;
#if AdmobDef
                    AdsManager.instance.ShowInterstitial();

#elif UNITY_ADS     //unity ads   
                    if(!vars.admobActive)
                        ShowAd();
#endif
                }
            }
        }
    }

    public void ShowAd()
    {
#if UNITY_ADS
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
#endif
    }

    //use this function for showing reward ads
    public void ShowRewardedAd()
    {
#if UNITY_ADS
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            Debug.Log("Ads not ready");
        }
#endif
    }

#if UNITY_ADS
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                GameManager.instance.points += 20; /*here we give 20 poinst as reward*/

                GameManager.instance.Save();
                ShopManager.instance.totalCoinText.text = "Cost: " + GameManager.instance.coins;
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");

                break;
        }
    }
#endif

}

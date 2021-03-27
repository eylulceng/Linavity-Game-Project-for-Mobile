using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class MainMenuUI
{
    public Image titleImage, playImage, leaderboardBtnImage, noAdsImage, soundImage,
        settingBtnImg, rateBtnImg, restorePurchaseBtnImg, shopBtnImg, backBtnImg;
}

[System.Serializable]
public class ShopMenuUI
{
    public Image starImage, shopCloseImage;
    public Text starText;
}

[System.Serializable]
public class GameMenuUI
{
    public Image starImage;
    public Text starText, scoreText;
}

[System.Serializable]
public class GameOverMenuUI
{
    public Image gameOverImg, shareImage, homeImage, facebookLikeImage, adsBtnImg, giftBtnImg,
        retryBtn;
    public Text gameOverScoreText, gameOverBestScoreText, giftRewardText;
    public Button adsBtn, giftBtn;
}

public class UIObjects : MonoBehaviour
{

    public static UIObjects instance;
    private AudioSource audioS;
    private AudioClip buttonClick;

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
        audioS = GetComponent<AudioSource>();       
    }

    void Start()
    {
        buttonClick = vars.buttonSound;
    }

    public MainMenuUI mainMenuUI;
    public ShopMenuUI shopMenuUI;
    public GameMenuUI gameMenuUI;
    public GameOverMenuUI gameOverMenuUI;
	public Text[] mainFont, secondFont;

    public void ButtonPress()
    {
        audioS.PlayOneShot(buttonClick);
    }

}

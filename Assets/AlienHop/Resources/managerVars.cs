using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class shopItemData{
	public string characterName;
    public Sprite gameCharacterSprite1, gameCharacterSprite2;
    public Sprite shopCharacterSprite;
    public int characterPrice;
}


public class managerVars : ScriptableObject {

    [SerializeField]
	public List<shopItemData> characters = new List<shopItemData>();

    [SerializeField]
    public Sprite soundOnButton, soundOffButton, leaderboardButton, shopButton, playButton,
        homeButton, rateButton, backButton, starImg, titleImage, noAdsImage, shopCloseImage,
        shareImage, facebookLikeImg, gameOverImg, restorePurchaseImg, adsBtnImg, giftBtnImg,
        retryBtnImg;

    [SerializeField]
    public Texture backgroundImg, cloudImg;

    [SerializeField]
    public Color32 gameOverScoreTextColor, gameOverBestScoreTextColor, inGameScoreTextColor,
        gameMenuStarTextColor, shopMenuStarTextColor, giftRewardTextColor;

    [SerializeField]
	public Font mainFont, secondFont;

    [SerializeField]
    public AudioClip buttonSound, starSound, jumpSound, backgroundMusic , deathSound,deepSound;

    // Standart Vars
    [SerializeField]
    public string adMobInterstitialID, adMobBannerID, admobAppID, rateButtonUrl, leaderBoardID,
        facebookPage;
	[SerializeField]
	public int showInterstitialAfter, bannerAdPoisiton;
    [SerializeField]
    public bool admobActive, googlePlayActive, unityIAP;
}

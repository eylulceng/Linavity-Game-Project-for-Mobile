using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

using GL = UnityEngine.GUILayout;
using EGL = UnityEditor.EditorGUILayout;

[System.Serializable]
public class GameDesign : EditorWindow {
	// Editor
	Texture2D book;
	Texture2D GDbanner;
	bool[] toggles;
	string[] buttons;
	private static Texture2D bgColor;
	public GUISkin editorSkin;
	Vector2 scrollPosition = new Vector2(0,0);
	string[] bannerPositionTexts = new string[] {"Bottom", "Bottom Left", "Bottom Right", "Top", "Top Left", "Top Right"};
	public managerVars vars;
	public UIObjects UIO;
	public string listType;
    public GameObject gameUI;
	// Game
	public shopItemData[] characters;
	[SerializeField]
	public string[] deathStrings;

	[MenuItem("Tools/GAME DESIGN")]
	static void Initialize(){
		GameDesign window = EditorWindow.GetWindow<GameDesign>(true, "GAME DESIGN");
		window.maxSize = new Vector2 (500f, 615f);
		window.minSize = window.maxSize;
	}

	void OnEnable(){
		toggles = new bool[]{false, false, false, false, false, false};
		buttons = new string[]{"Open","Open","Open","Open","Open"};
		vars = (managerVars) AssetDatabase.LoadAssetAtPath("Assets/AlienHop/Resources/managerVarsContainer.asset", typeof(managerVars));
		book = Resources.Load("question", typeof(Texture2D)) as Texture2D;
		GDbanner = Resources.Load("GDbanner", typeof(Texture2D)) as Texture2D;
		try{
            gameUI = GameObject.FindGameObjectWithTag("GameUI");
			UIO = gameUI.GetComponent<UIObjects> ();
		}catch(Exception e){}

		try{
			liveUpdate ();
		}catch(Exception e){}

	}

	void OnGUI(){
		// Settings
		bgColor = (Texture2D)Resources.Load ("editorBgColor");
		GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), bgColor, ScaleMode.StretchToFill);
		GUI.skin = editorSkin;
		GL.Label (GDbanner);
		scrollPosition = GL.BeginScrollView(scrollPosition);

        #region Shop Options
        // Start Block
        blockHeader("Shop Options", "Shop items options.", 0);
		if (toggles [0]){
			buttons [0] = "Close";
			BVS ("GroupBox");
			// Content Start
			shopItemCountController ();
			updateShopItems ();
			// Content End
			EditorUtility.SetDirty (vars);
			EV ();
		}else buttons[0] = "Open";
		EV();
        // End Block
        #endregion

        #region UI Options
        // Start Block
        blockHeader("UI Options", "All UI options.", 1);
		if(toggles[1]){
			buttons[1] = "Close";
			BVS("GroupBox");
			// Content Start
			GL.Label ("UI Images", "centerBoldLabel");
			GL.Space (10);
			BV ();
			BH ();
            vars.shopCloseImage = EGL.ObjectField("ShopClose Image", vars.shopCloseImage, typeof(Sprite), false) as Sprite;
            vars.playButton = EGL.ObjectField ("Play Button", vars.playButton, typeof(Sprite), false) as Sprite;
			EH ();
			BH ();
			vars.leaderboardButton = EGL.ObjectField ("Leaderboard Button", vars.leaderboardButton, typeof(Sprite), false) as Sprite;
			vars.shopButton = EGL.ObjectField ("Shop Open Button", vars.shopButton, typeof(Sprite), false) as Sprite;
			EH ();
			BH ();
			vars.homeButton = EGL.ObjectField ("Home Button", vars.homeButton, typeof(Sprite), false) as Sprite;
			vars.rateButton = EGL.ObjectField ("Rate Button", vars.rateButton, typeof(Sprite), false) as Sprite;
			EH ();
			BH ();
			vars.soundOnButton = EGL.ObjectField ("Sound On Button", vars.soundOnButton, typeof(Sprite), false) as Sprite;
			vars.soundOffButton = EGL.ObjectField ("Sound Off Button", vars.soundOffButton, typeof(Sprite), false) as Sprite;
			EH ();

            BH();
            vars.retryBtnImg = EGL.ObjectField("Retry Button", vars.retryBtnImg, typeof(Sprite), false) as Sprite;
            vars.adsBtnImg = EGL.ObjectField("Ads Btn", vars.adsBtnImg, typeof(Sprite), false) as Sprite;
            EH();
            BH();
            vars.titleImage = EGL.ObjectField("Title Image", vars.titleImage, typeof(Sprite), false) as Sprite;
            vars.noAdsImage = EGL.ObjectField("NoAds Image", vars.noAdsImage, typeof(Sprite), false) as Sprite;
            EH();
            BH();
            vars.gameOverImg = EGL.ObjectField("GameOver Title Img", vars.gameOverImg, typeof(Sprite), false) as Sprite;
            vars.shareImage = EGL.ObjectField("Share Image", vars.shareImage, typeof(Sprite), false) as Sprite;
            EH();
            BH();
            vars.giftBtnImg = EGL.ObjectField("Gift Btn", vars.giftBtnImg, typeof(Sprite), false) as Sprite;
            vars.starImg = EGL.ObjectField("Star Image", vars.starImg, typeof(Sprite), false) as Sprite;
            EH();
            BH();
            vars.cloudImg = EGL.ObjectField("Cloud Image", vars.cloudImg, typeof(Texture), false) as Texture;
            vars.backgroundImg = EGL.ObjectField("Background Image", vars.backgroundImg, typeof(Texture), false) as Texture;
            EH();
            EV ();
			separator ();
			GL.Label ("UI Texts", "centerBoldLabel");
			GL.Space (10);
			BVS("GroupBox");
			GL.Label ("Game Over Score Text :");
			vars.gameOverScoreTextColor = EGL.ColorField ("Color", vars.gameOverScoreTextColor);
            GL.Label("Game Over Best Score Text :");
            vars.gameOverBestScoreTextColor = EGL.ColorField("Color", vars.gameOverBestScoreTextColor);
            EV ();
            GL.Space(10);
            BVS("GroupBox");
            GL.Label("GameMenu Star Text :");
            vars.gameMenuStarTextColor = EGL.ColorField("Color", vars.gameMenuStarTextColor);
            GL.Label("ShopMenu Star Text :");
            vars.shopMenuStarTextColor = EGL.ColorField("Color", vars.shopMenuStarTextColor);
            EV();
            GL.Space (5);
			BVS("GroupBox");
			GL.Label ("In Game Score Text :");
			vars.inGameScoreTextColor = EGL.ColorField ("Color", vars.inGameScoreTextColor);
            GL.Label("Gift Reward Text :");
            vars.giftRewardTextColor = EGL.ColorField("Color", vars.giftRewardTextColor);
            EV ();
			separator ();
			GL.Label ("UI Fonts", "centerBoldLabel");
			GL.Space (10);
			vars.mainFont = EGL.ObjectField ("Main Font", vars.mainFont, typeof(Font), false) as Font;
			vars.secondFont = EGL.ObjectField ("Second Font", vars.secondFont, typeof(Font), false) as Font;
            // Content End
            EditorUtility.SetDirty (vars);
			EV();
		}else buttons[1] = "Open";
		EV();
        // End Block
        #endregion

        #region Sound Options
        // Start Block
        blockHeader("Sound Options", "Sound & Music options.", 2);
		if (toggles [2]){
			buttons [2] = "Close";
			BVS ("GroupBox");
			// Content Start
			vars.buttonSound = EGL.ObjectField ("Button Sound",vars.buttonSound,typeof(AudioClip),false) as AudioClip;
			vars.starSound = EGL.ObjectField ("Star Sound",vars.starSound, typeof(AudioClip),false) as AudioClip;
			vars.backgroundMusic = EGL.ObjectField ("Background Music",vars.backgroundMusic, typeof(AudioClip),false) as AudioClip;
			vars.jumpSound = EGL.ObjectField ("Jump Sound",vars.jumpSound, typeof(AudioClip),false) as AudioClip;
            vars.deathSound = EGL.ObjectField("Death Sound", vars.deathSound, typeof(AudioClip), false) as AudioClip;
            // Content End
            EditorUtility.SetDirty (vars);
			EV ();
		}else buttons[2] = "Open";
		EV();
        // End Block
        #endregion

        #region Other Options
        // Start Block
        blockHeader("Other Options", "AdMob, Google Play Services and etc. options.", 4);
		if(toggles [4]){
			buttons[4] = "Close";
			GL.BeginVertical("GroupBox");
            //Admob
            if (GUILayout.Button("Download Admob SDK"))
            {
                Application.OpenURL("https://github.com/googleads/googleads-mobile-unity/releases");
            }
            GL.Label("AdMob Options", EditorStyles.boldLabel);
            GL.BeginHorizontal();
            GL.Label("Show Interstitial After Death Times");
            vars.showInterstitialAfter = EGL.IntSlider(vars.showInterstitialAfter, 1, 25);
            GL.EndHorizontal();
            vars.admobActive = EGL.Toggle("Use Admob Ads", vars.admobActive, "Toggle");
            if (vars.admobActive)
            {
                AssetDefineManager.AddCompileDefine("AdmobDef",
                    new BuildTargetGroup[] { BuildTargetGroup.Android, BuildTargetGroup.iOS });

                //Admob App ID
                vars.admobAppID = EGL.TextField("AdMob App ID", vars.admobAppID);
                separator();
                //Banner
                vars.adMobBannerID = EGL.TextField("AdMob Banner ID", vars.adMobBannerID);
                GL.BeginHorizontal();
                GL.Label("Banner Position");
                vars.bannerAdPoisiton = GL.SelectionGrid(vars.bannerAdPoisiton, bannerPositionTexts, 3, "Radio");
                GL.EndHorizontal();
                separator();

                //Interstitial
                vars.adMobInterstitialID = EGL.TextField("AdMob Interstitial ID", vars.adMobInterstitialID);
            }
            else if (!vars.admobActive)
            {
                AssetDefineManager.RemoveCompileDefine("AdmobDef",
                    new BuildTargetGroup[] { BuildTargetGroup.Android, BuildTargetGroup.iOS });
            }

            separator();

            //Google Play Service
            if (GUILayout.Button("Download Google Play SDK"))
            {
                Application.OpenURL("https://github.com/playgameservices/play-games-plugin-for-unity");
            }
            GL.Label("Google Play Or Game Center", EditorStyles.boldLabel);
            vars.googlePlayActive = EGL.Toggle("Use Leaderboard", vars.googlePlayActive, "Toggle");
            if (vars.googlePlayActive)
            {
#if UNITY_ANDROID
                AssetDefineManager.AddCompileDefine("GooglePlayDef",
                    new BuildTargetGroup[] { BuildTargetGroup.Android });
#endif

                vars.leaderBoardID = EGL.TextField("Leaderboard ID", vars.leaderBoardID);
            }
            else if (!vars.googlePlayActive)
            {
#if UNITY_ANDROID
                AssetDefineManager.RemoveCompileDefine("GooglePlayDef",
                    new BuildTargetGroup[] { BuildTargetGroup.Android });
#endif
            }

            separator();

            //Unity IAP
            GL.Label("Unity IAP", EditorStyles.boldLabel);
            GL.Label("Activate UnityIAP from Services Window", EditorStyles.label);
            vars.unityIAP = EGL.Toggle("Use UnityIAP", vars.unityIAP, "Toggle");
            if (vars.unityIAP)
            {
                AssetDefineManager.AddCompileDefine("UnityIAP",
                    new BuildTargetGroup[] { BuildTargetGroup.Android });
            }
            else if (!vars.unityIAP)
            {
                AssetDefineManager.RemoveCompileDefine("UnityIAP",
                    new BuildTargetGroup[] { BuildTargetGroup.Android });
            }

            separator();

            GL.Label("Other Options", EditorStyles.boldLabel);
            //facebook page
            GL.BeginHorizontal();
            GL.Label("Facebook Page", GL.Width(100f));
            vars.facebookPage = EGL.TextArea(vars.facebookPage, GL.Height(25f));
            GL.EndHorizontal();
            GL.Space(15f);
            //Rate Url
            GL.BeginHorizontal();
            GL.Label("Rate Button Url", GL.Width(100f));
            vars.rateButtonUrl = EGL.TextArea(vars.rateButtonUrl, GL.Height(25f));
            GL.EndHorizontal();
            GL.Space(15f);
            separator();
			//
			EditorUtility.SetDirty (vars);
			GL.EndVertical();
		}else buttons[4] = "Open";
		GL.EndVertical();
        // End Block
        #endregion

        GL.EndScrollView();
		EditorUtility.SetDirty (vars);
		try{
			liveUpdate ();
		}catch(Exception e){}
	}

	void liveUpdate(){
        #region mainMenu
		UIO.mainMenuUI.titleImage.sprite = vars.titleImage;
		UIO.mainMenuUI.playImage.sprite = vars.playButton;
        UIO.mainMenuUI.leaderboardBtnImage.sprite = vars.leaderboardButton;
		UIO.mainMenuUI.noAdsImage.sprite = vars.noAdsImage;
		UIO.mainMenuUI.soundImage.sprite = vars.soundOnButton;
        #endregion

        #region shopMenu
		UIO.shopMenuUI.starImage.sprite = vars.starImg;
        UIO.shopMenuUI.shopCloseImage.sprite = vars.shopCloseImage;
        UIO.shopMenuUI.starText.color = vars.shopMenuStarTextColor;
        #endregion

        #region GameMenu
        UIO.gameMenuUI.starImage.sprite = vars.starImg;
        UIO.gameMenuUI.starText.color = vars.gameMenuStarTextColor;
		UIO.gameMenuUI.scoreText.color = vars.inGameScoreTextColor;
        #endregion

        #region GameOverMenuUI
        UIO.gameOverMenuUI.shareImage.sprite = vars.shareImage;
        UIO.gameOverMenuUI.homeImage.sprite = vars.homeButton;
		UIO.gameOverMenuUI.gameOverBestScoreText.color = vars.gameOverBestScoreTextColor;
		UIO.gameOverMenuUI.gameOverScoreText.color = vars.inGameScoreTextColor;
        UIO.gameOverMenuUI.giftRewardText.color = vars.giftRewardTextColor;
        UIO.gameOverMenuUI.adsBtnImg.sprite = vars.adsBtnImg;
        UIO.gameOverMenuUI.giftBtnImg.sprite = vars.giftBtnImg;
        #endregion

        foreach (Text texts1 in UIO.mainFont){
			texts1.font = vars.mainFont;
		}
		foreach(Text texts2 in UIO.secondFont){
			texts2.font = vars.secondFont;
		}
	}

	void OnDestroy(){
		EditorUtility.SetDirty (vars);
	}

	void shopItemCountController(){
		BH();
		GL.Label ("", GL.Width (250));
		GL.Label ("Characters Count : " + (vars.characters.Count));
		if((vars.characters.Count) != 1){
			if(GL.Button ("-")){
				vars.characters.Remove (vars.characters[vars.characters.Count - 1]);
				EditorUtility.SetDirty (vars);
			}
		}
		if(GL.Button ("+")){
			vars.characters.Add (new shopItemData());
			EditorUtility.SetDirty (vars);
		}
		EH ();
	}

    void updateShopItems(){
		for (int i = 0; i <= (vars.characters.Count-1); i++) {
			GL.Label ("Character " + (i+1)+" options:", EditorStyles.boldLabel);
			BV ();
            GL.Label("Sprite1 for Open Eye / Sprite2 for Close Eye " , EditorStyles.boldLabel);
            BH();
			vars.characters[i].gameCharacterSprite1 = EGL.ObjectField ("Game Character sprite1", vars.characters[i].gameCharacterSprite1, typeof(Sprite), false) as Sprite;
            vars.characters[i].gameCharacterSprite2 = EGL.ObjectField("Game Character sprite2", vars.characters[i].gameCharacterSprite2, typeof(Sprite), false) as Sprite;
            EH ();
            BH();    
            vars.characters[i].shopCharacterSprite = EGL.ObjectField("Shop Character sprite", vars.characters[i].shopCharacterSprite, typeof(Sprite), false) as Sprite;
            EH();
            BH ();
			vars.characters[i].characterName = EGL.TextField ("Character name", vars.characters[i].characterName);
			if(i != 0){
				vars.characters[i].characterPrice = EGL.IntField ("Character price", vars.characters[i].characterPrice);
			}
			EH ();
			EV ();
			separator();
		}
	}

	void drawArray(string arrayName){
		SerializedObject so = new SerializedObject(this);
		SerializedProperty stringsProperty = so.FindProperty (arrayName);
		EGL.PropertyField(stringsProperty, true);
		so.ApplyModifiedProperties();
	}

	void blockHeader(string mainHeader, string secondHeader, int blockIdex){
		BV ();
		GL.Label (mainHeader, "TL Selection H2");
		BH ();
		if (GL.Button (buttons[blockIdex], GL.Height(25f) , GL.Width(50f))) toggles[blockIdex] = !toggles[blockIdex];
		BHS ("HelpBox");
		GL.Label (secondHeader, "infoHelpBoxText");
		GL.Label (book , GL.Height(18f), GL.Width(20f));
		EH ();
		EH ();
		GL.Space (3);
	}

	void separator(){
		GL.Space(10f);
		GL.Label("", "separator", GL.Height(1f));
		GL.Space(10f);
	}

	void BH(){
		GL.BeginHorizontal ();
	}

	void BHS(string style){
		GL.BeginHorizontal (style);
	}

	void EH(){
		GL.EndHorizontal ();
	}

	void BVS(string style){
		GL.BeginVertical (style);
	}

	void BV(){
		GL.BeginVertical ();
	}

	void EV(){
		GL.EndVertical ();
	}
}

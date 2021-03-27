using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using GooglePlayGames;               
//using UnityEngine.SocialPlatforms;
/// <summary>
/// ***********Uncomment lines after importing google play services*******************
/// </summary>

public class GooglePlayManager : MonoBehaviour
{

    public static GooglePlayManager singleton;

    private AudioSource sound;

    [HideInInspector]
    public managerVars vars;

    void OnEnable()
    {
        vars = Resources.Load<managerVars>("managerVarsContainer");
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    // Use this for initialization
    void Start()
    {
        sound = GetComponent<AudioSource>();
        //    PlayGamesPlatform.Activate();
        //    Social.localUser.Authenticate((bool success) =>
        //    {
        //        if (success)
        //        {
        //            InitializeAchievements();
        //        }
        //    });

    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        ReportScore(GameManager.instance.lastScore);
    }

    public void OpenLeaderboardsScore()
    {
        //if (Social.localUser.authenticated)
        //{
        //    PlayGamesPlatform.Instance.ShowLeaderboardUI(vars.leaderBoardID);
        //}
    }

    void ReportScore(int score)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, vars.leaderBoardID, (bool success) => { });
        }
    }

}

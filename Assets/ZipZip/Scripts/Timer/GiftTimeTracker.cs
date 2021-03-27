using UnityEngine;
using System;

public class GiftTimeTracker : MonoBehaviour
{
    public static GiftTimeTracker instance;
    private ulong lastGiftOpen;
    [Tooltip("Its time between two consecutive gifts in minute")]
    public int timeToWait = 15;
    [HideInInspector]
    public bool giftReady;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        lastGiftOpen = GameManager.instance.giftTime;

        if (!IsGiftReady())
        {
            UIObjects.instance.gameOverMenuUI.giftBtn.interactable = false;
            giftReady = false;
        }
        else
        {
            giftReady = true;
        }
    }

    void Update()
    {
        if (!UIObjects.instance.gameOverMenuUI.giftBtn.IsInteractable())
        {
            if (IsGiftReady())
            {
                giftReady = true;
                UIObjects.instance.gameOverMenuUI.giftBtn.interactable = true;
                return;
            }

            //Set timer
            ulong diff = (ulong)DateTime.Now.Ticks - lastGiftOpen;
            ulong milliSec = diff / TimeSpan.TicksPerMillisecond;
            //(1000 millisec = 1 second)
            //1st converted timeGap into seconds then converted milliseconds into seconds and subtracted
            float secondsLeft = (float)(timeToWait * 60) - milliSec / 1000;

            string r = "";
            ////hours (If wanted can add)
            //r += ((int)secondsLeft / 3600).ToString() + ":";
            //secondsLeft -= ((int)secondsLeft / 3600) * 3600;

            //min
            r += ((int)secondsLeft / 60).ToString("00") + ":";
            //sec
            r += (secondsLeft % 60).ToString("00");

            //UIObjects.instance.gameOverMenuUI.giftTimer.text = r;

        }

    }

    public void TrackTime()
    {
        GameManager.instance.giftTime = (ulong)DateTime.Now.Ticks;
        lastGiftOpen = GameManager.instance.giftTime;
        GameManager.instance.Save();
    }

    private bool IsGiftReady()
    {
        ulong diff = (ulong)DateTime.Now.Ticks - lastGiftOpen;
        ulong milliSec = diff / TimeSpan.TicksPerMillisecond;
        //(1000 millisec = 1 second)
        //1st converted timeGap into seconds then converted milliseconds into seconds and subtracted
        float secondsLeft = (float)(timeToWait * 60) - milliSec / 1000;

        if (secondsLeft < 0)
        {
            //UIObjects.instance.gameOverMenuUI.giftTimer.text = "Get Free Diamonds";
            return true;
        }

        return false;

    }



}//class
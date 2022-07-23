using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorMessageLoad : MonoBehaviour
{
    const string TIMESETTING_TXTDATA = "解說時間設定";
    const string NARRATOR_TXTDATA = "解說員文本";
    List<string> storyShowTimeList = new List<string>();

    class storyTime
    {
        public string message;
        public int sec;
        public storyTime(string message, int sec)
        {
            this.message = message;
            this.sec = sec;
        }
    }
    List<storyTime> storyList = new List<storyTime>();
    List<string> storySubStartTime = new List<string>();

    // string feedingTimeDataPath;
    // string narratorTxtDataPath;
    const string TIMECLOCK = "TimeClock";
    void Start()
    {
        // feedingTimeDataPath = Application.persistentDataPath + $"/{TIMESETTING_TXTDATA}.txt";
        // narratorTxtDataPath = Application.persistentDataPath + $"/{NARRATOR_TXTDATA}.txt";
        LoadTxtData();
    }

    void LoadTxtData()
    {
        string feedingTimeSetting = RuntimeText.ReadString(TIMESETTING_TXTDATA);
        if (feedingTimeSetting == null)
        {
            NoticeController.noticeController.AddStoryMessage($"找不到文件:{TIMESETTING_TXTDATA}.txt", 0);
            return;
        }
        string[] feedingStr = feedingTimeSetting.Split('\n');
        if (!feedingStr[0].Contains("[PlaySchedule]"))
        {
            NoticeController.noticeController.AddStoryMessage($"文件:{TIMESETTING_TXTDATA}.txt,格式錯誤,第一行須為[PlaySchedule]", 0);
            return;
        }

        foreach (var item in feedingStr)
        {
            storyShowTimeList.Add(item.Trim());
            //Debug.Log($"讀取到解說播放時間：{item.Trim()}");
        }

        //--解說文本讀取--//
        string narratorData = RuntimeText.ReadString(NARRATOR_TXTDATA);
        if (narratorData == null)
        {
            NoticeController.noticeController.AddStoryMessage($"找不到文件:{NARRATOR_TXTDATA}.txt", 0);
            return;
        }

        string[] narratorStr = narratorData.Split('\n');
        try
        {
            for (int i = 0; i < narratorStr.Length; i++)
            {
                if (narratorStr[i].Contains("[Timeline]"))
                {
                    string[] timeline = narratorStr[i].Split('#');
                    storySubStartTime.Add(timeline[1]);
                    int sec = GetSubSecnds(timeline[1].Trim(), timeline[2].Trim());
                    string message;
                    if (narratorStr[i + 1].Contains("[Timeline]"))
                    {
                        message = "";       //如果下一行又是時間線,此行時間線內容視為空行(間格用)
                    }
                    else
                    {
                        message = narratorStr[i + 1];    //時間訊息的下一行=內容
                    }
                    storyList.Add(new storyTime(message, sec));
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"發生例外{e}");
            NoticeController.noticeController.AddStoryMessage($"{NARRATOR_TXTDATA}文本格式有誤,請檢察", 0);
            return;
        }


        OnLoadComplete();
        //NoticeController.noticeController.AddStoryMessage("LoadFinish,TestRunTxtMsg", 0);
    }


    void OnLoadComplete()
    {
        InvokeRepeating(TIMECLOCK, 0, 1);
    }

    //每秒鐘調用一次
    void TimeClock()
    {
        CheckNarratorShowStartTime();     //每秒鐘秒鐘檢查一次是否為解說時間
    }

    void CheckNarratorShowStartTime()
    {
        //Debug.Log(NowMinute());
        if (!storyShowTimeList.Contains(NowTime())) return;
        ConfigureStory();
    }

    private string NowTime()
    {
        return DateTime.Now.ToString("HH:mm:ss");
    }


    private void ConfigureStory()
    {
        VideoController.videoController.PlayNarratorVideo();

        if (storyList.Count < 1) return;
        foreach (var item in storyList)
        {
            NoticeController.noticeController.AddStoryMessage(item.message, item.sec);
        }
    }

    int GetSubSecnds(string startTimer, string endTimer)
    {
        string[] stime = startTimer.Split(':');
        string[] etime = endTimer.Split(':');
        TimeSpan startSpan = new TimeSpan(int.Parse(stime[0]), int.Parse(stime[1]), int.Parse(stime[2]));
        TimeSpan nowSpan = new TimeSpan(int.Parse(etime[0]), int.Parse(etime[1]), int.Parse(etime[2]));
        TimeSpan subTimer = nowSpan.Subtract(startSpan).Duration();
        return (int)subTimer.TotalSeconds;
    }
}

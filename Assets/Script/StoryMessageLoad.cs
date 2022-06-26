using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryMessageLoad : MonoBehaviour
{
    [SerializeField] string storyTxtDataName;

    string nowTime;
    List<string> storyShowTimeList = new List<string>();
    List<string> storyTextList = new List<string>();

    Dictionary<string, Queue<string>> storyShoweTimeDictionary = new Dictionary<string, Queue<string>>();

    string dataPath;
    const string TIMECLOCK = "TimeClock";
    void Start()
    {
        Application.runInBackground = true;
        dataPath = Application.persistentDataPath + $"/{storyTxtDataName}.txt";
        loadTxtData();
    }

    void loadTxtData()
    {
        string textData = RuntimeText.ReadString(storyTxtDataName);
        if(string.IsNullOrEmpty(textData)) 
        {
            Debug.Log($"找不到文件:{storyTxtDataName}.txt");
            return;
        }
        string[] str = textData.Split('\n');
        for (int i = 0; i < str.Length; i++)
        {
            if (!string.IsNullOrEmpty(str[i]))
            {
                                
            }
        }
        // if (string.IsNullOrEmpty(textData))
        // {
        //     CreateTimeTxtData();
        //     return;
        // }
        // for (int i = 0; i < str.Length; i++)
        // {
        //     if (string.IsNullOrEmpty(str[i]))
        //     {
        //         DebugTextData(i);
        //         return;
        //     }
        //     if (!str[i].Contains("#"))
        //     {
        //         DebugTextData(i, str[i]);
        //         return;
        //     }
        //     string time = str[i].Split('#')[0];
        //     string allmessage = str[i].Split('#')[1];

        //     Queue<string> msgQ = new Queue<string>();
        //     foreach (var item in allmessage.Split('|'))
        //     {
        //         msgQ.Enqueue(item);
        //     }

        //     storyShowTimeList.Add(time);
        //     storyTextList.Add(allmessage);
        //     if (storyShoweTimeDictionary.ContainsKey(time))
        //     {
        //         DebugTextData(time);
        //         return;
        //     }
        //     storyShoweTimeDictionary.Add(time, msgQ);
        // }
        OnLoadCompete();
        NoticeController.noticeController.AddStoryMessage("thisMessage,is One second over", 2);
        NoticeController.noticeController.AddStoryMessage("LoadFinish,TestRunTxtMsg", 0);
    }

    void OnLoadCompete()
    {
        int sec = int.Parse(DateTime.Now.ToString("ss"));
        InvokeRepeating(TIMECLOCK, 0, 1);
    }

    //每秒調用一次
    void TimeClock()
    {
        nowTime = DateTime.Now.ToString("HH:mm:ss");
        Debug.Log(nowTime);
        // if ()
        // {
        // }
        CheckMessageSendTime();     //每分鐘檢查一次是否為發送訊息時間        
    }

    void CheckMessageSendTime()
    {
        if (!storyShoweTimeDictionary.ContainsKey(nowTime)) return;

        int messageLenght = storyShoweTimeDictionary[nowTime].Count;
        for (int i = 0; i < messageLenght; i++)
        {
            NoticeController.noticeController.AddNormalMessage(storyShoweTimeDictionary[nowTime].Dequeue());
        }
    }
}

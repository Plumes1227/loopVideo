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
        Debug.Log(textData);
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
        NoticeController.noticeController.AddStoryMessage("thisMessage,is One second over", 1);
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


    void CreateTimeTxtData()
    {
        string mes = $"{DateTime.Now.ToString("HH:mm")}#我是範例格式|歡迎編輯此跑馬燈資訊|請遵照本格式編輯此文本";
        RuntimeText.WriteString(storyTxtDataName, mes);
        NoticeController.noticeController.AddNormalMessage(
            $"尚未有文本檔案,已生成初始文本檔案,請至{dataPath}查閱"
        );
    }
    void DebugTextData(int rows)
    {
        NoticeController.noticeController.AddNormalMessage($"文本檔案-{storyTxtDataName}第{rows}行為空\n行,不可有空行,請遵循例格式- 24:59#內容A|內容B|...");
        NoticeController.noticeController.AddNormalMessage($"檔案位址:{dataPath}");
    }
    void DebugTextData(int rows, string bugMessage)
    {
        NoticeController.noticeController.AddNormalMessage($"文本檔案-{storyTxtDataName}第{rows}行格式錯誤[{bugMessage}],請遵循例格式- 24:59#內容A|內容B|...");
        NoticeController.noticeController.AddNormalMessage($"檔案位址:{dataPath}");
    }
    void DebugTextData(string timeKey)
    {
        NoticeController.noticeController.AddNormalMessage($"文本檔案-{storyTxtDataName}含有重複的時間點{timeKey},請確認時間點為唯一時間");
        NoticeController.noticeController.AddNormalMessage($"檔案位址:{dataPath}");
    }
}

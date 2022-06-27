using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextMessageLoad : MonoBehaviour
{
    const string TIMEDRUNMESSAGE_TXTDATA = "定時跑馬燈文本";

    string nowTime;
    List<string> messageShowTimeList = new List<string>();
    List<string> messageTextList = new List<string>();

    Dictionary<string, List<string>> showTimeDictionary = new Dictionary<string, List<string>>();

    string dataPath;
    const string TIMECLOCK = "TimeClock";
    void Start()
    {
        Application.runInBackground = true;
        dataPath = Application.persistentDataPath + $"/{TIMEDRUNMESSAGE_TXTDATA}.txt";
        loadTxtData();
    }

    void loadTxtData()
    {
        string textData = RuntimeText.ReadString(TIMEDRUNMESSAGE_TXTDATA);
        if (string.IsNullOrEmpty(textData))
        {
            CreateTimeTxtData();
            return;
        }
        string[] str = textData.Split('\n');
        for (int i = 0; i < str.Length; i++)
        {
            if (!string.IsNullOrEmpty(str[i]))
            {
                if (!str[i].Contains("#"))
                {
                    DebugTextData(i, str[i]);
                    return;
                }
                string time = str[i].Split('#')[0];
                string allmessage = str[i].Split('#')[1];

                List<string> msgL = new List<string>();
                foreach (var item in allmessage.Split('|'))
                {
                    msgL.Add(item);
                }


                messageShowTimeList.Add(time);
                messageTextList.Add(allmessage);
                if (showTimeDictionary.ContainsKey(time))
                {
                    DebugTextData(time);
                    return;
                }
                showTimeDictionary.Add(time, msgL);
            }
        }
        OnLoadComplete();
        NoticeController.noticeController.AddNormalMessage("讀取成功,測試用文字");
    }

    void OnLoadComplete()
    {
        int sec = int.Parse(DateTime.Now.ToString("ss"));
        InvokeRepeating(TIMECLOCK, 60 - sec, 60);
    }

    //每秒調用一次
    void TimeClock()
    {
        nowTime = DateTime.Now.ToString("HH:mm");

        CheckMessageSendTime();     //每分鐘檢查一次是否為發送訊息時間        
    }

    void CheckMessageSendTime()
    {
        if (!showTimeDictionary.ContainsKey(nowTime)) return;

        int messageLenght = showTimeDictionary[nowTime].Count;
        for (int i = 0; i < messageLenght; i++)
        {
            NoticeController.noticeController.AddNormalMessage(showTimeDictionary[nowTime][i]);
        }
    }


    void CreateTimeTxtData()
    {
        string mes = $"{DateTime.Now.ToString("HH:mm")}#我是範例格式|歡迎編輯此跑馬燈資訊|請遵照本格式編輯此文本";
        RuntimeText.WriteString(TIMEDRUNMESSAGE_TXTDATA, mes);
        NoticeController.noticeController.AddNormalMessage(
            $"查無文本檔案,已生成初始文本檔案,請至{dataPath}查閱"
        );
    }
    void DebugTextData(int rows, string bugMessage)
    {
        NoticeController.noticeController.AddNormalMessage($"文本檔案-{TIMEDRUNMESSAGE_TXTDATA}第{rows}行格式錯誤[{bugMessage}],請遵循例格式- 24:59#內容A|內容B|...");
        NoticeController.noticeController.AddNormalMessage($"檔案位址:{dataPath}");
    }
    void DebugTextData(string timeKey)
    {
        NoticeController.noticeController.AddNormalMessage($"文本檔案-{TIMEDRUNMESSAGE_TXTDATA}含有重複的時間點{timeKey},請確認時間點為唯一時間");
        NoticeController.noticeController.AddNormalMessage($"檔案位址:{dataPath}");
    }
}


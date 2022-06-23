using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextMessageLoad : MonoBehaviour
{
    [SerializeField] string txtDataName;

    string nowTime;
    List<string> messageShowTimeList = new List<string>();
    List<string> messageTextList = new List<string>();

    Dictionary<string, Queue<string>> showeTimeDictionary = new Dictionary<string, Queue<string>>();

    [SerializeField] bool isUseResourcs;
    string dataPath;
    const string TIMECLOCK = "TimeClock";
    void Start()
    {
        dataPath = Application.persistentDataPath + $"/{txtDataName}.txt";
        loadTxtData();
    }

    string Check()
    {
        try
        {
            File.ReadAllText(txtDataName);            
        }
        catch
        {
            return "";
        }
        return Resources.Load(txtDataName).ToString();
    }
    void loadTxtData()
    {
        string textData = isUseResourcs ? Check() : RuntimeText.ReadString(txtDataName);
        Debug.Log(textData);
        if (string.IsNullOrEmpty(textData))
        {
            CreateTimeTxtData();
            return;
        }
        string[] str = textData.Split('\n');
        for (int i = 0; i < str.Length; i++)
        {
            if (string.IsNullOrEmpty(str[i]))
            {
                DebugTextData(i);
                return;
            }
            if (!str[i].Contains("#"))
            {
                DebugTextData(i, str[i]);
                return;
            }
            string time = str[i].Split('#')[0];
            string allmessage = str[i].Split('#')[1];

            Queue<string> msgQ = new Queue<string>();
            foreach (var item in allmessage.Split('|'))
            {
                msgQ.Enqueue(item);
            }

            messageShowTimeList.Add(time);
            messageTextList.Add(allmessage);
            if (showeTimeDictionary.ContainsKey(time))
            {
                DebugTextData(time);
                return;
            }
            showeTimeDictionary.Add(time, msgQ);
        }
        OnLoadCompete();
    }

    void OnLoadCompete()
    {
        int sec = int.Parse(DateTime.Now.ToString("ss"));
        InvokeRepeating(TIMECLOCK, 60 - sec, 60);
    }

    //每分鐘調用一次
    void TimeClock()
    {
        nowTime = DateTime.Now.ToString("HH:mm");
        Debug.Log(nowTime);
        CheckMessageSendTime();     //每分鐘檢查一次是否為發送訊息時間        
    }

    void CheckMessageSendTime()
    {
        if (!showeTimeDictionary.ContainsKey(nowTime)) return;

        int messageLenght = showeTimeDictionary[nowTime].Count;
        for (int i = 0; i < messageLenght; i++)
        {
            NoticeController.noticeController.AddMessage(showeTimeDictionary[nowTime].Dequeue());
        }
    }


    void CreateTimeTxtData()
    {
        if (isUseResourcs)
        {
            NoticeController.noticeController.AddMessage($"找不到TimeText.txt檔案,請檢察\\loopVideo_Data\\Resources\\TimeText.txt是否存在");
            return;
        }
        string mes = $"{DateTime.Now.ToString("HH:mm")}#我是範例格式|歡迎編輯此跑馬燈資訊|請遵照本格式編輯此文本";
        RuntimeText.WriteString(txtDataName, mes);
        NoticeController.noticeController.AddMessage(
            $"尚未有文本檔案,已生成初始文本檔案,請至{dataPath}查閱"
        );
    }
    void DebugTextData(int rows)
    {
        NoticeController.noticeController.AddMessage($"文本檔案-{txtDataName}第{rows}行為空\n行,不可有空行,請遵循例格式- 24:59#內容A|內容B|...");
        if (isUseResourcs) return;
        NoticeController.noticeController.AddMessage($"檔案位址:{dataPath}");
    }
    void DebugTextData(int rows, string bugMessage)
    {
        NoticeController.noticeController.AddMessage($"文本檔案-{txtDataName}第{rows}行格式錯誤[{bugMessage}],請遵循例格式- 24:59#內容A|內容B|...");
        if (isUseResourcs) return;
        NoticeController.noticeController.AddMessage($"檔案位址:{dataPath}");
    }
    void DebugTextData(string timeKey)
    {
        NoticeController.noticeController.AddMessage($"文本檔案-{txtDataName}含有重複的時間點{timeKey},請確認時間點為唯一時間");
        if (isUseResourcs) return;
        NoticeController.noticeController.AddMessage($"檔案位址:{dataPath}");
    }
}


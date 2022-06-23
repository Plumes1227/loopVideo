using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextMessageLoad : MonoBehaviour
{
    [SerializeField] string TextDataName;

    [SerializeField] string nowTime;
    List<string> messageShowTimeList = new List<string>();
    List<string> messageTextList = new List<string>();

    Dictionary<string, Queue<string>> showeTimeDictionary = new Dictionary<string, Queue<string>>();

    const string TIMECLOCK = "TimeClock";
    void Start()
    {
        TextAsset txt = Resources.Load(TextDataName) as TextAsset;        

        string[] str = txt.text.Split('\n');
        for (int i = 0; i < str.Length; i++)
        {            
            string time = str[i].Split('#')[0];
            string allmessage = str[i].Split('#')[1];

            Queue<string> msgQ = new Queue<string>();
            foreach (var item in allmessage.Split('|'))
            {                
                msgQ.Enqueue(item);
            }

            messageShowTimeList.Add(time);
            messageTextList.Add(allmessage);
            showeTimeDictionary.Add(time, msgQ);
        }
        int sec = int.Parse(DateTime.Now.ToString("ss"));
        InvokeRepeating(TIMECLOCK, 60 - sec, 60);


        NoticeController.noticeController.AddMessage("---3---");
        NoticeController.noticeController.AddMessage("---2---");
        NoticeController.noticeController.AddMessage("---1---");
    }

    //每分鐘調用一次
    void TimeClock()
    {
        nowTime = DateTime.Now.ToString("HH:mm");
        Debug.Log(nowTime);
        CheckMessageSendTime();     //每分鐘檢查一次是為發送訊息時間        
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
}


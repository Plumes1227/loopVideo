using UnityEngine;
using System.IO;
using UnityEngine.Video;
using System;

public class VideoController : MonoBehaviour
{
    const string VIDEOTPYE_TXTDATA = "影片名稱類型設定";
    string videoDataName;    
    VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        LoadVideoType();
    }

    private void LoadVideoType()
    {
        string path = Application.streamingAssetsPath + $"/{VIDEOTPYE_TXTDATA}.txt";
        try
        {            
            StreamReader reader = new StreamReader(path);
            videoDataName = reader.ReadToEnd();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            NoticeController.noticeController.AddNormalMessage($"影片設定文本讀取失敗,找不到VideoTypeSetting.txt");
            NoticeController.noticeController.AddNormalMessage($"路徑{Application.streamingAssetsPath}");
            return;
        }
        LoadVideoData();
    }
    private void LoadVideoData()
    {
        string path = Application.streamingAssetsPath + $"/{videoDataName}";
        try
        {
            File.Exists(path);
            videoPlayer.url = path;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            NoticeController.noticeController.AddNormalMessage($"影片檔名讀取失敗,請檢查'VideoTypeSetting.txt'文本內容是否與影片[檔名.副檔名] 一致,");
            NoticeController.noticeController.AddNormalMessage($"路徑{Application.streamingAssetsPath}");
            return;
        }
    }
}

using UnityEngine;
using System.IO;
using UnityEngine.Video;
using System;

public class VideoController : MonoBehaviour
{
    [SerializeField] string videoTypeTxtName;
    [SerializeField] string videoDataName;
    string videoType;
    VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        LoadVideoType();
    }

    private void LoadVideoType()
    {
        string path = Application.streamingAssetsPath + $"/{videoTypeTxtName}.txt";
        try
        {
            File.Exists(path);
            StreamReader reader = new StreamReader(path);
            videoType = reader.ReadToEnd();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }
        LoadVideoData();
    }
    private void LoadVideoData()
    {
        string path = Application.streamingAssetsPath + $"/{videoDataName}.{videoType}";
        try
        {
            File.Exists(path);
            videoPlayer.url = path;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }
    }
}

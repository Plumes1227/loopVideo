using UnityEngine;
using System.IO;
using UnityEngine.Video;
using System;
using DG.Tweening;


public class VideoController : MonoBehaviour
{
    public static VideoController videoController;
    [SerializeField] CanvasGroup videoRawImageCanvasGroup;
    [SerializeField] CanvasGroup ExPlanCanvasGroup;
    [SerializeField] CanvasGroup ExRawImageCanvasGroup;
    [SerializeField] RenderTexture render3212;  //32:1200
    [SerializeField] RenderTexture render169;   //16:9
    const string VIDEOTPYE_TXTDATA = "影片名稱類型設定";

    string[] dataStr;
    string loopVideoDataName;
    string narratorVideoDataName;

    string loopVideoPath;
    string narratorVideoPath;
    VideoPlayer videoPlayer;

    [SerializeField] GameObject mp;
    [SerializeField] GameObject umask;
    void Awake()
    {
        videoController = this;
    }
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
            dataStr = reader.ReadToEnd().Split('\n');
        }
        catch (Exception e)
        {
            Debug.Log(e);
            NoticeController.noticeController.AddNormalMessage($"影片設定文本讀取失敗,找不到{VIDEOTPYE_TXTDATA}.txt");
            NoticeController.noticeController.AddNormalMessage($"路徑{Application.streamingAssetsPath}");
            return;
        }

        for (int i = 0; i < dataStr.Length; i++)
        {
            if (dataStr[i].Contains("[LoopVideo]#"))
            {
                loopVideoDataName = dataStr[i].Split('#')[1].Trim();
            }
            if (dataStr[i].Contains("[FeedingVideo]#"))
            {
                narratorVideoDataName = dataStr[i].Split('#')[1].Trim();
            }
        }
        LoadVideoData();
    }
    private void LoadVideoData()
    {
        loopVideoPath = Application.streamingAssetsPath + $"/{loopVideoDataName}";
        narratorVideoPath = Application.streamingAssetsPath + $"/{narratorVideoDataName}";
        try
        {
            File.Exists(loopVideoPath);
            File.Exists(narratorVideoPath);
            PlayLoopVideo();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            NoticeController.noticeController.AddNormalMessage($"影片檔名讀取失敗,請檢查'{VIDEOTPYE_TXTDATA}.txt'文本內容是否與影片[檔名.副檔名] 一致,");
            NoticeController.noticeController.AddNormalMessage($"路徑{Application.streamingAssetsPath}");
            return;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayLoopVideo();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayNarratorVideo();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            NoticeController.noticeController.AddNormalMessage($"循環跑馬燈測試");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            NoticeController.noticeController.AddStoryMessage($"解說字幕測試,即將回到循環影片", 5);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            mp.SetActive(!mp.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.U))
        {
            umask.SetActive(!umask.activeSelf);
        }
    }
    public void PlayLoopVideo()
    {
        FadeIn(videoRawImageCanvasGroup);
        FadeOut(ExPlanCanvasGroup);
        FadeOut(ExRawImageCanvasGroup);
        videoPlayer.targetTexture = render3212;
        videoPlayer.url = loopVideoPath;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }
    public void PlayNarratorVideo()
    {
        FadeOut(videoRawImageCanvasGroup);
        FadeIn(ExPlanCanvasGroup);
        FadeIn(ExRawImageCanvasGroup);
        videoPlayer.targetTexture = render169;
        videoPlayer.url = narratorVideoPath;
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }

    void FadeInAndOut(CanvasGroup canvasGroup)
    {
        canvasGroup.DOFade(0, 0.4f).SetLoops(2, LoopType.Yoyo);
    }
    void FadeIn(CanvasGroup canvasGroup)
    {
        canvasGroup.DOFade(1, 0.5f);
    }
    void FadeOut(CanvasGroup canvasGroup)
    {
        canvasGroup.DOFade(0, 0.3f);
    }


}

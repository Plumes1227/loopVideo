using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NoticeController : MonoBehaviour
{
    public static NoticeController noticeController;
    private void Awake()
    {
        noticeController = this;
    }
    [SerializeField] RectTransform normalTxtMsgRect;
    CanvasGroup normalTxtMsgCvsG;
    [SerializeField] Text normalTxtMsg;//跑馬燈text.
    [SerializeField] RectTransform storyTxtMsgRect;
    CanvasGroup storyTxtMsgCvsG;
    [SerializeField] Text storyTxtMsg;//解說員跑馬燈text.
    [SerializeField] CanvasGroup storyCG;//解說員跑馬燈CG
    Queue<string> m_MsgQueue = new Queue<string>();//文字隊列
    Queue<string> storyMsgQueue = new Queue<string>();//解說員文字隊列
    Queue<float> storySecQueue = new Queue<float>();//解說員播文字放時間對列,
    bool isNormalScrolling = false;//判斷當前text中的跑馬燈是否跑完.
    bool isStoryScrolling = false;
    [SerializeField] float normalMsgSpeed;   //跑馬燈速度
    [SerializeField] float storyMsgSpeed;   //跑馬燈速度
    WaitForSeconds waitForSeconds;
    float storySec;
    private void Start()
    {
        waitForSeconds = new WaitForSeconds(0.5f);
        normalTxtMsgCvsG = normalTxtMsgRect.GetComponent<CanvasGroup>();
        storyTxtMsgCvsG = storyTxtMsgRect.GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 添加跑馬燈信息.
    /// </summary>
    /// <param name="msg"></param>    
    public void AddNormalMessage(string msg)
    {
        if (normalTxtMsgCvsG.alpha == 0)
        {
            ShowOrHideRunText(normalTxtMsgCvsG, true);
        }
        m_MsgQueue.Enqueue(msg);
        if (isNormalScrolling) return;
        StartCoroutine(NormalTxtMsgScrolling());
    }
    private IEnumerator NormalTxtMsgScrolling()
    {
        float rectWidth = normalTxtMsgRect.rect.width;
        while (m_MsgQueue.Count > 0)
        {
            string msg = m_MsgQueue.Dequeue();
            normalTxtMsg.text = msg;
            float txtWidth = normalTxtMsg.preferredWidth;//文本自身的長度.
            Vector3 pos = normalTxtMsg.rectTransform.localPosition;
            float startPos = rectWidth / 2 + txtWidth * 0.7f;
            float duration = startPos / storyMsgSpeed;
            isNormalScrolling = true;
            normalTxtMsg.rectTransform.localPosition = new Vector3(startPos, pos.y, pos.z);
            normalTxtMsg.rectTransform.DOLocalMoveX(-startPos, duration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(duration);
        }
        isNormalScrolling = false;
        ShowOrHideRunText(normalTxtMsgCvsG, false);
        yield break;
    }

    /// <summary>
    /// 添加解說員跑馬燈信息,限制秒數內播放完畢,0=不限制,由原速度控制
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="sec"></param>
    public void AddStoryMessage(string msg, float sec)
    {
        if (storyTxtMsgCvsG.alpha == 0)
        {
            ShowOrHideRunText(storyTxtMsgCvsG, true);
        }
        storyMsgQueue.Enqueue(msg);
        storySecQueue.Enqueue(sec);
        if (isStoryScrolling) return;
        StartCoroutine(StoryTxtMsgScrolling());
    }

    private IEnumerator StoryTxtMsgScrolling()
    {
        float rectWidth = storyTxtMsgRect.rect.width;
        while (storyMsgQueue.Count > 0)
        {
            string msg = storyMsgQueue.Dequeue();
            storyTxtMsg.text = msg;
            float txtWidth = storyTxtMsg.preferredWidth;//文本自身的長度.
            Vector3 pos = storyTxtMsg.rectTransform.localPosition;
            float startPos = rectWidth / 2 + txtWidth * 0.7f;
            float duration = startPos / storyMsgSpeed;
            isStoryScrolling = true;

            storySec = storySecQueue.Dequeue();
            //storyTxtMsg.rectTransform.localPosition = new Vector3(startPos, pos.y, pos.z);
            if (storySec > 0)
            {
                //Debug.Log($"秒數{storySec}");
                //storyTxtMsg.rectTransform.DOLocalMoveX(-startPos, storySec).SetEase(Ease.Linear);
                ShowOrHideRunText(storyCG,true);
                yield return new WaitForSeconds(storySec);
                ShowOrHideRunText(storyCG,false);
            }
            else
            {
                //storyTxtMsg.rectTransform.DOLocalMoveX(-startPos, duration).SetEase(Ease.Linear);
                ShowOrHideRunText(storyCG,true);
                yield return new WaitForSeconds(duration);
                ShowOrHideRunText(storyCG,false);
            }
            yield return waitForSeconds;
        }
        isStoryScrolling = false;
        ShowOrHideRunText(storyTxtMsgCvsG, false);
        VideoController.videoController.PlayLoopVideo();
        yield break;
    }

    void ShowOrHideRunText(CanvasGroup canvasGroup, bool isShow)
    {
        canvasGroup.DOFade(isShow ? 1 : 0, 0.4f).SetEase(Ease.OutBack);
    }
}
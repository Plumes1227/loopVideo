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
    [SerializeField]
    Text m_TxtMsg;//跑馬燈text.
    [SerializeField] Queue<string> m_MsgQueue = new Queue<string>();//燈隊列.
    //Font m_Font;
    bool isScrolling = false;//判斷當前text中的跑馬燈是否跑完.
    [SerializeField] float moveSpeed;
    public void Init()
    {
        m_MsgQueue = new Queue<string>();
    }


    /// <summary>
    /// 添加跑馬燈信息.
    /// </summary>
    /// <param name="msg"></param>    
    public void AddMessage(string msg)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            Init();
        }
        m_MsgQueue.Enqueue(msg);
        if (isScrolling) return;
        StartCoroutine(Scrolling());
    }
    public IEnumerator Scrolling()
    {
        float beginX = 1650;
        float leftX = -1650;
        while (m_MsgQueue.Count > 0)
        {
            float duration = 10f;
            string msg = m_MsgQueue.Dequeue();
            m_TxtMsg.text = msg;
            float txtWidth = m_TxtMsg.preferredWidth;//文本自身的長度.
            Vector3 pos = m_TxtMsg.rectTransform.localPosition;
            float distance = beginX - leftX + txtWidth;
            duration = distance / moveSpeed;
            isScrolling = true;

            m_TxtMsg.rectTransform.localPosition = new Vector3(beginX + txtWidth / 2, pos.y, pos.z);
            m_TxtMsg.rectTransform.DOLocalMoveX(-distance / 2, duration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(duration);
        }
        isScrolling = false;
        //gameObject.SetActive(false);
        yield break;
    }
}
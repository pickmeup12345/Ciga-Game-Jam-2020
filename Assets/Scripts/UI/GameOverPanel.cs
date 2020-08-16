using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public Text TitleText;
    public Text DesText;
    public GameObject FaildObj;
    public GameObject SuccessObj;
    public AudioClip PassClip;
    public AudioClip FailedClip;
    public Color FailedColor;
    public Color PassColor;
    
    public void Show(bool isFailed, string des)
    {
        if (!isFailed)
        {
            GameMgr.Instance.PlayBgm(PassClip);
        }
        else
        {
            AudioSource.PlayClipAtPoint(FailedClip, Vector3.zero);
        }
        TitleText.text = isFailed ? "失败！" : "胜利！";
        TitleText.color = isFailed ? FailedColor : PassColor;
        DesText.color = isFailed ? FailedColor : PassColor;
        FaildObj.SetActive(isFailed);
        SuccessObj.SetActive(!isFailed);
        DesText.text = des;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

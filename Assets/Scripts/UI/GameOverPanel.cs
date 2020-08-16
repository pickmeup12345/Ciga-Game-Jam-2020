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

    public void Show(bool isFailed, string des)
    {
        TitleText.text = isFailed ? "游戏失败！" : "游戏胜利！";
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

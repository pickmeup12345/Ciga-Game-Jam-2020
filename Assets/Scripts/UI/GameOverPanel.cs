using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public Text TitleText;
    public Text DesText;

    public void Show(string title, string des)
    {
        TitleText.text = title;
        DesText.text = des;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

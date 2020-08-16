using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TipsContainer : MonoBehaviour
{
    public GameObject TipsPrefab;
    public float TipsTime;
    public float TipsMoveDist;

    public void ShowTips(string tips)
    {
        var obj = Instantiate(TipsPrefab, transform);
        obj.transform.localPosition = Vector3.zero;
        var text = obj.GetComponentInChildren<Text>();
        text.text = tips;
        var t = TipsTime / 3f;
        DOTween.Sequence().Join(obj.transform.DOLocalMoveY(TipsMoveDist, TipsTime))
            .Join(obj.GetComponent<CanvasGroup>().DOFade(0.1f, t).SetDelay(2 * t))
            .Join(obj.transform.DOScale(0.1f, t).SetDelay(2 * t))
            .SetEase(Ease.InOutSine)
            .OnComplete(() => Destroy(obj));
    }
}

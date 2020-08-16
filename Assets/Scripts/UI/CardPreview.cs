using System;
using System.Collections;
using System.Collections.Generic;
using CjGameDevFrame.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardPreview : MonoBehaviour, IEventListener<CardEvent>
{
    public CanvasGroup YesGroup;
    public CanvasGroup NoGroup;
    public CanvasGroup DesGroup;

    private Text _yesText;
    private Text _noText;
    private Text _desText;

    private float _yesPosY;
    private float _noPosY;
    private bool _isPrepare;
    private Tween _yesTween;
    private Tween _noTween;

    private CardResult? _previewResult;
    
    void Awake()
    {
        _yesText = YesGroup.GetComponentInChildren<Text>();
        _noText = NoGroup.GetComponentInChildren<Text>();
        _desText = DesGroup.GetComponentInChildren<Text>();

        _yesPosY = YesGroup.transform.localPosition.y;
        _noPosY = NoGroup.transform.localPosition.y;
    }

    private void OnEnable()
    {
        this.EventStartListening();
    }

    public void HidePreview()
    {
        if (_previewResult == null) return;
        _previewResult = null;
        PlayPreviewAnim(CardResult.Yes, 0, _yesPosY, 6);
        PlayPreviewAnim(CardResult.No, 0, _noPosY, 6);
    }

    public void ShowPreview(CardConfigData configData, CardResult result)
    {
        if (_previewResult == result) return; 
        _previewResult = result;
        
        if (result == CardResult.Yes)
        {        
            PlayPreviewAnim(CardResult.Yes, 1, _yesPosY + 30, 3);
            PlayPreviewAnim(CardResult.No, 0, _noPosY, 6);
        }
        else
        {
            PlayPreviewAnim(CardResult.No, 1, _noPosY - 30, 3);
            PlayPreviewAnim(CardResult.Yes, 0, _yesPosY, 6);
        }
    }
    
    private void PlayPreviewAnim(CardResult result, float a, float my, float speed)
    {
        var g = result == CardResult.Yes ? YesGroup : NoGroup;
        var duration = Mathf.Abs(g.alpha - a) / speed;
        if (result == CardResult.Yes)
            _yesTween?.Kill();
        else if (result == CardResult.No)
            _noTween?.Kill();
        var tween = DOTween.Sequence()
            .Join(g.transform.DOLocalMoveY(my, duration))
            .Join(g.DOFade(a, duration))
            .SetEase(Ease.OutSine);
        if (result == CardResult.Yes)
            _yesTween = tween;
        else if (result == CardResult.No)
            _noTween = tween;
    }

    public void OnEvent(CardEvent e)
    {
        if (e.ResolvePhase == CardResolvePhase.Preview)
        {
            if (e.ConfigData == null)
            {
                HidePreview();
            }
            else if (_isPrepare)
            {
                ShowPreview(e.ConfigData, e.Result);
            }
        }

        if (e.ResolvePhase == CardResolvePhase.Prepare)
        {
            _desText.text = e.ConfigData.Destext;
            _yesText.text = e.ConfigData.Yestext;
            _noText.text = e.ConfigData.Notext;
            if (!string.IsNullOrEmpty(_desText.text)) PlayDesAnim(1, 1);
            _isPrepare = true;
        }

        if (e.ResolvePhase == CardResolvePhase.Resolving)
        {
            PlayDesAnim(0, 0.8f);
            HidePreview();
            _isPrepare = false;
        }
    }

    private void PlayDesAnim(float a, float s)
    {
        var duration = Mathf.Abs(DesGroup.alpha - a) / 1f;
        DOTween.Sequence()
            .Join(DesGroup.DOFade(a, duration))
            .Join(DesGroup.transform.DOScale(s, duration))
            .SetEase(Ease.OutBounce);
    }
}

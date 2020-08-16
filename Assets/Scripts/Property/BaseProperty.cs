using System;
using System.Collections;
using System.Collections.Generic;
using CjGameDevFrame.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BaseProperty : MonoBehaviour, IEventListener<CardEvent>
{
    public int MaxValue;
    public int InitValue;
    public TweenSlider[] TweenSliders;
    public Text ValueText;
    public Image PreviewImg;
    public int MinBigPreview;

    private int _val;
    public int Value
    {
        get { return _val; }
        set
        {            
            if (_val == value) return;
            _val = Mathf.Clamp(value, 0, MaxValue);
            foreach (var slider in TweenSliders)
            {
                slider.TweenTo(_val);
            }
            if (ValueText != null) ValueText.text = _val.ToString();
        }
    }

    private Sequence _tween;
    
    protected virtual void OnEnable()
    {
        this.EventStartListening();
    }

    private void Start()
    {
        ResetValue();
    }

    protected virtual void OnDisable()
    {
        this.EventStopListening();
    }

    public virtual void ResetValue()
    {
        foreach (var slider in TweenSliders)
        {
            slider.Init(0, MaxValue, InitValue);
        }

        _val = InitValue;
        if (ValueText != null) ValueText.text = _val.ToString();
        
        HidePreview();
    }

    public virtual void Preview(CardConfigData configData, CardResult result)
    {
        var val = GetPropertyVal(configData, result);
        val = Mathf.Abs(val);
        if (val >= MinBigPreview)
        {
            PlayPreviewAnim(PreviewImg, 1.25f, 1);
        }
        else if (val > 0)
        {
            PlayPreviewAnim(PreviewImg, 0.9f, 1);
        }
    }

    public void HidePreview()
    {
        PlayPreviewAnim(PreviewImg, 0, 0);
    }

    public virtual void ResolvingCard(CardConfigData configData, CardResult result)
    {
        var val = GetPropertyVal(configData, result);
        if (val != 0) Value += val;
    }

    public virtual void ResolvedCard(CardConfigData configData, CardResult result)
    {
        
    }

    public virtual void ResolveEndDay()
    {
        
    }

    public virtual void ResolveNewDayStart()
    {
        
    }
    
    public void OnEvent(CardEvent e)
    {
        if (e.ResolvePhase == CardResolvePhase.Preview)
        {
            if (e.ConfigData == null) HidePreview();
            else Preview(e.ConfigData, e.Result);
        }

        if (e.ResolvePhase == CardResolvePhase.Resolving)
        {
            ResolvingCard(e.ConfigData, e.Result);
        }

        if (e.ResolvePhase == CardResolvePhase.Resolved)
        {
            ResolvedCard(e.ConfigData, e.Result);
        }
    }
    
    protected virtual int GetPropertyVal(CardConfigData configData, CardResult result)
    {
        return 0;
    }

    private void PlayPreviewAnim(Image image, float scale, float a)
    {
        _tween?.Kill();
        var duration = Mathf.Abs(image.transform.localScale.x - scale) / 3f;
        _tween = DOTween.Sequence()
            .Join(image.transform.DOScale(scale, duration))
            .Join(image.DOFade(a, duration))
            .SetEase(Ease.OutBounce);
    }
}

using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSlider : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Vector2 SlideDiret;
    public Vector2 OtherDiret;
    public float ConfirmDist;
    public float ResolvedDist;
    public float BackTime;
    public bool YesIsNegativeDir;
    
    public Image IconImg;
    public Text MsgText;

    private CardConfigData _configData;
    public CardConfigData ConfigData
    {
        get { return _configData; }
        set
        {
            _configData = value;
            if (!string.IsNullOrEmpty(_configData.Iconname))
            {
                IconImg.sprite = Resources.Load<Sprite>(_configData.Iconname);
            }
        }
    }

    public bool IsConfirmed { get; private set; }

    private CanvasGroup _cardGroup;

    private Vector2 _lastDragPos;
    private float _dragSpeed;
    private CardResult? _previewResult;

    private void Awake()
    {
        _cardGroup = GetComponent<CanvasGroup>();
    }

    public void ShowMsg(string msg)
    {
        if (MsgText != null) MsgText.text = msg;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastDragPos = eventData.position;
        _dragSpeed = 0;
        DOTween.Kill(transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsConfirmed) return;
        
        var moveStep = Vector2.Dot(SlideDiret, eventData.position - _lastDragPos);
        moveStep = Mathf.SmoothDamp(0, moveStep, ref _dragSpeed, Time.deltaTime, 100000);
        transform.localPosition += moveStep * (Vector3)SlideDiret.normalized;
        var otherStep = Vector2.Dot(OtherDiret, eventData.position - _lastDragPos);
        transform.localPosition += otherStep * (Vector3) OtherDiret.normalized;
        _lastDragPos = eventData.position;

        var posDir = Vector2.Dot(transform.localPosition, SlideDiret);
        var previewResult = DirToResult(posDir);

        if (transform.localPosition.magnitude < ConfirmDist)
        {
            CardEvent.Send(CardResolvePhase.Preview, previewResult, null);
            _previewResult = null;
        }
        else
        {
            CardEvent.Send(CardResolvePhase.Preview, previewResult, ConfigData);
            _previewResult = previewResult;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_previewResult != null)
        {
            Confirm(_previewResult.Value);
            _previewResult = null;
            return;
        }
        
        CardEvent.Send(CardResolvePhase.Preview, CardResult.Yes, null);
        transform.DOLocalMove(Vector3.zero, BackTime)
            .SetEase(Ease.OutSine);
    }
    
    public void Confirm(CardResult result)
    {
        if (IsConfirmed) return;
        CardEvent.Send(CardResolvePhase.Preview, result, null);
        Debug.Log($"Confirmed {result} {ConfigData.Id} {ConfigData.Destext}");
        IsConfirmed = true;
        CardEvent.Send(CardResolvePhase.Resolving, result, ConfigData);
        DOTween.Kill(transform);
        var dir = ResultToDir(result);
        var endPos = dir * ResolvedDist * SlideDiret;
        var time = Vector2.Distance(transform.localPosition, endPos) / 500;
        transform.DOLocalMove(endPos, time)
            .OnComplete(() => { CardEvent.Send(CardResolvePhase.Resolved, result, ConfigData); })
            .SetEase(Ease.OutQuad);
        _cardGroup.DOFade(0, time)
            .SetEase(Ease.OutQuad);
    }
    
    private CardResult DirToResult(float posDir)
    {
        return posDir < 0 && YesIsNegativeDir || posDir > 0 && !YesIsNegativeDir ? CardResult.Yes : CardResult.No;
    }

    private float ResultToDir(CardResult result)
    {
        return result == CardResult.Yes && !YesIsNegativeDir || result == CardResult.No && YesIsNegativeDir ? 1 : -1;
    }
}

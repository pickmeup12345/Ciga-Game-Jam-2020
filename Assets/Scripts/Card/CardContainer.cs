using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CjGameDevFrame.Common;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CardResolvePhase
{
    Prepare, Preview, Resolving, Resolved,
}

public enum CardResult
{
    Yes, No
}

public struct CardEvent
{
    public CardResolvePhase ResolvePhase;
    public CardResult Result;
    public CardConfigData ConfigData;

    private static CardEvent e;

    public static void Send(CardResolvePhase phase, CardResult result, CardConfigData configData)
    {
        e.ResolvePhase = phase;
        e.Result = result;
        e.ConfigData = configData;
        EventManager.TriggerEvent(e);
    }
}

public class CardContainer : MonoBehaviour, IEventListener<CardEvent>
{
    [SerializeField] private CardConfig _cardConfig;
    [SerializeField] private CardSlider _cardPrefab;
    [SerializeField] private CardSlider _msgCardPrefab;

    private CardSlider _curCard;
    private List<CardConfigData> _remainCards;
    private List<int> _resolvedCardIds;
    private int _stage;

    public void Shuffle(int stage)
    {
        if (stage == 1)
        {
            _resolvedCardIds = new List<int>();
        }
        Debug.Log("Shuffle " + stage);
        
        DestroyCurCard();
        _stage = stage;
        _remainCards = new List<CardConfigData>(_cardConfig.dataArray.Where(d => d.Stage.Contains(stage) && !_resolvedCardIds.Contains(d.Id)));
        _remainCards.Shuffle();
    }

    public void ShowNextCard()
    {
        _curCard = Instantiate(_cardPrefab, transform);
        _curCard.transform.localPosition = Vector3.zero;
        _curCard.ConfigData = GetNextCard();
        Debug.Log("ShowNextCard " + _curCard.ConfigData.Id);
        CardEvent.Send(CardResolvePhase.Prepare, CardResult.Yes, _curCard.ConfigData);
    }

    public void ShowMsgCard(string msg, CardConfigData configData = null)
    {
        _curCard = Instantiate(_msgCardPrefab, transform);
        _curCard.transform.localPosition = Vector3.zero;
        _curCard.ConfigData = configData ?? new CardConfigData();
        _curCard.ShowMsg(msg);
        CardEvent.Send(CardResolvePhase.Prepare, CardResult.Yes, _curCard.ConfigData);
    }

    // private void ShowCard(CardSlider card)
    // {
    //     var tweenTime = 0f;
    //     var jumpTime = 0.75f;
    //     var jumpDist = Random.value > 0.5f ? Vector3.left : Vector3.right;
    //     jumpDist *= 800;
    //     
    //     card.transform.position -= jumpDist;
    //     card.transform.localScale = Vector3.zero;
    //     
    //     DOTween.Sequence()
    //         .Join(card.transform.DOJump(jumpDist, 400, 1, jumpTime)
    //             .SetEase(Ease.OutSine)
    //             .SetRelative(true)
    //             .OnUpdate(() =>
    //             {
    //                 tweenTime += Time.deltaTime;
    //                 card.transform.eulerAngles = 2 * 360 * tweenTime / jumpTime * Vector3.forward;
    //             })
    //             .OnComplete(() => { card.transform.eulerAngles = Vector3.zero; }))
    //         .Join(card.transform.DOScale(1, jumpTime / 2)
    //             .SetEase(Ease.OutSine)
    //             .SetDelay(jumpTime / 2));
    // }

    public void Resolved()
    {
        _resolvedCardIds.Add(_curCard.ConfigData.Id);
        DestroyCurCard();
    }

    private void DestroyCurCard()
    {
        if (_curCard != null)
        {
            Destroy(_curCard.gameObject);
            _curCard = null;
        }
    }

    private CardConfigData GetNextCard()
    {
        var weights = new int[_remainCards.Count];
        var totalWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = totalWeight;
            var isFind = false;
            for (var j = 0; j < _remainCards[i].Stage.Length; j++)
            {
                if (_remainCards[i].Stage[j] == _stage)
                {
                    isFind = true;
                    totalWeight += _remainCards[i].Weight[j];
                    break;
                }
            }

            if (!isFind)
            {
                Debug.LogError($"Not fount stage! {_remainCards[i].Id}");
            }
        }

        var rand = Random.Range(0, totalWeight);
        var index = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            if (rand >= weights[i])
            {
                index = i;
                break;
            } 
        }

        var result = _remainCards[index];
        _remainCards.RemoveAt(index);
        return result;
    }

    private void OnEnable()
    {
        this.EventStartListening();   
    }

    private void OnDisable()
    {
        this.EventStopListening();
    }
    
    public void OnEvent(CardEvent e)
    {
        if (_curCard == null) return;
        if (e.ResolvePhase == CardResolvePhase.Resolved && e.ConfigData == _curCard.ConfigData)
        {
            Resolved();
        }
    }
}

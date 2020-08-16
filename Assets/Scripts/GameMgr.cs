using System;
using System.Collections;
using System.Collections.Generic;
using CjGameDevFrame.Common;
using DG.Tweening;
using UnityEngine;

public enum GameEvent
{
    RestartGame, GameOver, EndDay, NewDay
}

public class GameMgr : MonoSingleton<GameMgr>, IEventListener<CardEvent>
{
    public int StageAddInterval;
    public Transform StartBtnTrans;
    public AudioSource BgmSource;
    public AudioClip MainBgm;
    
    public CardContainer CardContainer { get; private set; }
    public List<BaseProperty> AllProperty { get; private set; }

    public TrustProperty Trust { get; private set; }
    public MoneyProperty Money { get; private set; }
    public MoodProperty Mood { get; private set; }
    public SatietyProperty Satiety { get; private set; }
    public EndDaysProperty EndDays { get; private set; }

    public TipsContainer Tips { get; private set; }
    public GameOverPanel GameOverPanel { get; private set; }

    public bool IsGameOver { get; private set; }
    public int PassDays { get; private set; }
    
    public int Stage { get; private set; }

    private bool _isShowCard;
    private int _msgIndex;
    private Dictionary<int, List<string>> _msgDict;
    
    private void OnEnable()
    {
        this.EventStartListening();
    }

    private void OnDisable()
    {
        this.EventStopListening();
    }

    void Start()
    {
        CardContainer = FindObjectOfType<CardContainer>();
        AllProperty = new List<BaseProperty>();
        AllProperty.AddRange(FindObjectsOfType<BaseProperty>());
        
        Trust = FindObjectOfType<TrustProperty>();
        Money = FindObjectOfType<MoneyProperty>();
        Mood = FindObjectOfType<MoodProperty>();
        Satiety = FindObjectOfType<SatietyProperty>();
        EndDays = FindObjectOfType<EndDaysProperty>();

        Tips = FindObjectOfType<TipsContainer>();
        GameOverPanel = FindObjectOfType<GameOverPanel>();
        GameOverPanel.Hide();

        _msgDict = new Dictionary<int, List<string>>();
        _msgDict.Add(0, new List<string>
        {
            "2333年，突如其来的蚂蚁病毒袭击了这座城市，人们不得不相互隔离以防传染。康涅狄格州的斯嘉丽是一个独居的18岁平民女孩，她的日常就是通过支持或反对一些事件使自己在蚁毒中存活。",
            "你需要注意自己的四项数值来保持自身属性的平衡。G代表政府信任度，E代表饱食度，M代表心情，R代表金钱，过高或过低都会导致游戏结束。",
            "你需要在蚁毒中存活30天，祝你成功。"
        });
        _msgDict.Add(10, new List<string>
        {
            "由于前期防控缺位，蚁毒在距疫苗研制成功的20天时发生了突变，传染力升级，您的属性变化将会加大，请谨慎做出选择。"
        });
        _msgDict.Add(20, new List<string>
        {
            "距离疫苗研制成功仅剩10天，蚁毒正在做最后的反扑，您的属性变化将会进一步加大，请谨慎做出选择。"
        });

        StartBtnTrans.DOScale(0.93f, 1.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame");
        PlayBgm(MainBgm);
        Stage = 1;
        PassDays = 0;
        _msgIndex = 0;
        GameOverPanel.Hide();
        StopAllCoroutines();
        IsGameOver = false;
        EventManager.TriggerEvent(GameEvent.RestartGame);
        foreach (var property in AllProperty)
        {
            property.ResetValue();
        }
        CardContainer.Shuffle(Stage);
        
        NewDay(true);
    }

    public void GameOver(bool failed, string des)
    {
        Debug.Log($"GameOver!! failed = {failed}, {des}");
        IsGameOver = true;
        EventManager.TriggerEvent(GameEvent.GameOver);
        GameOverPanel.Show(failed, des);
    }

    public void NewDay(bool isFirstDay = false)
    {
        if (IsGameOver) return;
        if (!isFirstDay) PassDays++;
        Debug.Log("NewDay PassDays = " + PassDays);
        _isShowCard = false;
        if (PassDays >= 5 && PassDays % 5 == 0)
        {
            CardContainer.ShowMsgCard("政府补给到了!", new CardConfigData{Yesmoney = 5, Nomoney = 5});
        }
        else if (_msgDict.ContainsKey(PassDays) && _msgDict[PassDays].Count > _msgIndex)
        {
            CardContainer.ShowMsgCard(_msgDict[PassDays][_msgIndex]);
            _msgIndex++;
        }
        else
        {
            _isShowCard = true;
            CardContainer.ShowNextCard();
        }
        EventManager.TriggerEvent(GameEvent.NewDay);
    }

    public void EndDay()
    {
        if (IsGameOver) return;
        _msgIndex = 0;
        Debug.Log("EndDay");
        EventManager.TriggerEvent(GameEvent.EndDay);
    }

    public void PlayBgm(AudioClip clip)
    {
        BgmSource.clip = clip;
        BgmSource.Play();
    }

    public void OnEvent(CardEvent e)
    {
        if (e.ResolvePhase == CardResolvePhase.Resolved)
        {
            StartCoroutine(DelayPassDay());
        }
    }

    private IEnumerator DelayPassDay()
    {
        yield return new WaitForSeconds(0.5f);
        if (_msgDict.ContainsKey(PassDays))
        {
            if (_msgDict[PassDays].Count > _msgIndex)
            {
                CardContainer.ShowMsgCard(_msgDict[PassDays][_msgIndex]);
                _msgIndex++;
                yield break;
            }
        }
        
        if (!_isShowCard)
        {
            _isShowCard = true;
            CardContainer.ShowNextCard();
            yield break;
        }
        
        EndDay();
        if (IsGameOver) yield break;
        Tips.ShowTips("一天结束了\n心情减少\n饱食减少");

        yield return new WaitForSeconds(1f);
        if (PassDays > 0 && PassDays % StageAddInterval == 0)
        {
            Stage++;
            CardContainer.Shuffle(Stage);
        } 
        NewDay();
    }
} 

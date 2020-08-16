
using CjGameDevFrame.Common;
using UnityEngine;

public class MoodProperty : BaseProperty, IEventListener<GameEvent>
{
    public int DailyExpend;
    [HideInInspector] public int DailyExtraExpend;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.AddListener<GameEvent>(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.RemoveListener<GameEvent>(this);
    }

    protected override int GetPropertyVal(CardConfigData configData, CardResult result)
    {
        return result == CardResult.Yes ? configData.Yesmood : configData.Nomood;
    }
    
    public override void ResolvedCard(CardConfigData configData, CardResult result)
    {
        CheckGameOver();
    }

    public void OnEvent(GameEvent e)
    {
        if (e == GameEvent.EndDay)
        {
            Value -= DailyExpend + DailyExtraExpend;
            DailyExtraExpend = 0;
            CheckGameOver();
        }
    }

    private void CheckGameOver()
    {
        if (Value == 0)
        {
            GameMgr.Instance.GameOver(true, "心情太差，抑郁而死");
        }
    }
}
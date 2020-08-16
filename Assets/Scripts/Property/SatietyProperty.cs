

using CjGameDevFrame.Common;
using UnityEngine;

public class SatietyProperty : BaseProperty, IEventListener<GameEvent>
{
    public int DailyExpend;
    public Vector2 BadMoodRange;
    public int ExtraMoodExpend;
    
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
        return result == CardResult.Yes ? configData.Yessatiety : configData.Nosatiety;
    }
    
    public override void ResolvingCard(CardConfigData configData, CardResult result)
    {
        base.ResolvingCard(configData, result);
        
        if (Value <= BadMoodRange.x || Value >= BadMoodRange.y)
        {
            GameMgr.Instance.Mood.DailyExtraExpend += ExtraMoodExpend;
        }
    }

    public override void ResolvedCard(CardConfigData configData, CardResult result)
    {
        CheckGameOver();
    }

    public void OnEvent(GameEvent e)
    {
        if (e == GameEvent.EndDay)
        {
            Value -= DailyExpend;
            CheckGameOver();
        }
    }

    private void CheckGameOver()
    {
        if (Value == 0)
        {
            GameMgr.Instance.GameOver(true, "你饿死了");
        }
    }
}

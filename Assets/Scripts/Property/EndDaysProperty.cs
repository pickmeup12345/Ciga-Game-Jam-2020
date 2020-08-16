

using CjGameDevFrame.Common;

public class EndDaysProperty : BaseProperty, IEventListener<GameEvent>
{
    private bool _isFirstDay;

    public override void ResetValue()
    {
        base.ResetValue();
        _isFirstDay = true;
    }

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
        return result == CardResult.Yes ? configData.Yesenddays : configData.Noenddays;
    }

    public void OnEvent(GameEvent e)
    {
        if (_isFirstDay)
        {
            _isFirstDay = false;
            return;
        }
        
        if (e == GameEvent.NewDay)
        {
            Value -= 1;
            CheckGameOver();
        }
    }

    private void CheckGameOver()
    {
        if (Value == 0)
        {
            GameMgr.Instance.GameOver(false, "疫情结束了，你赢了");
        }
    }
}



using CjGameDevFrame.Common;

public class MoneyProperty : BaseProperty, IEventListener<GameEvent>
{
    public int SupplyInterval;
    public int SupplyCount;
    
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
        return result == CardResult.Yes ? configData.Yesmoney : configData.Nomoney;
    }

    public override void ResolvedCard(CardConfigData configData, CardResult result)
    {
        base.ResolvedCard(configData, result);
        if (Value == 0)
        {
            GameMgr.Instance.GameOver(true, "你的钱耗尽了");
        }
    }


    public void OnEvent(GameEvent e)
    {
        // if (e == GameEvent.NewDay)
        // {
        //     if (GameMgr.Instance.PassDays >= SupplyInterval && GameMgr.Instance.PassDays % SupplyInterval == 0)
        //     {
        //         Value += SupplyCount;
        //         GameMgr.Instance.Tips.ShowTips("联盟补给到了");
        //     }
        // }
    }
}

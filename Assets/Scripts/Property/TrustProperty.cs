

using UnityEngine;

public class TrustProperty : BaseProperty
{
    public Vector2 BadMoodRange;
    public int ExtraMoodExpend;
    
    protected override int GetPropertyVal(CardConfigData configData, CardResult result)
    {
        return result == CardResult.Yes ? configData.Yestrust : configData.Notrust;
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

    private void CheckGameOver()
    {
        if (Value <= 0)
        {
            GameMgr.Instance.GameOver(true, "你违抗联盟，被关进监狱了");
        }

        if (Value >= MaxValue)
        {
            GameMgr.Instance.GameOver(true, "平民怀疑你成为了联盟的走狗，将你杀死了");
        }
    }
}

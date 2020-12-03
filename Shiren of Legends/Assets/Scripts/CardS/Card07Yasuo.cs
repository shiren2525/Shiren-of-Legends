public class Card07Yasuo : CardParent, IPassiveSkill
{
    public void PassiveSkill()
    {
        var cardStatus = this.gameObject.GetComponent<CardStatus>();
        cardStatus.AddShield((int)(cardStatus.MyAD*cardStatus.MyRatio));
    }
}

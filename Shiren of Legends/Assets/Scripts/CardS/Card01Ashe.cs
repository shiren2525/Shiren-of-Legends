public class Card01Ashe : CardParent, ISkill
{
    public void ActiveSkill(int myLane)
    {
        var cardStatus = this.gameObject.GetComponent<CardStatus>();
        var myPlayer = cardStatus.Player;        

        var fullSearch = FullSearch((enemyPlayer, enemyLane) => { return (enemyPlayer != myPlayer && enemyLane == myLane); });
        foreach (var card in fullSearch)
        {
            card.CardStatus.AddDamage((int)(cardStatus.MyAD * cardStatus.MyRatio), (int)EnumSkillType.SkillShot);
            card.CardStatus.IsStun = true;
        }
    }
}
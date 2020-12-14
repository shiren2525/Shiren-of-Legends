using UnityEngine;

public class Card06Senna : CardParent, ISkill
{
    [SerializeField] private CardStatus CardStatus = null;

    public void ActiveSkill(int myLane)
    {
        var myPlayer = CardStatus.Player;        

        var fullSearch = FullSearch((enemyPlayer, enemyLane) => { return (enemyPlayer != myPlayer && enemyLane == myLane); });
        foreach (var card in fullSearch)
        {
            card.CardStatus.AddDamage(myPlayer, (int)(CardStatus.MyAD * CardStatus.MyRatio), (int)EnumSkillType.SkillShot);
        }

        var fullSearchHeal = FullSearch((enemyPlayer, enemyLane) => { return (enemyPlayer == myPlayer && enemyLane == myLane); });
        foreach (var card in fullSearchHeal)
        {
            card.CardStatus.AddHeal((int)(CardStatus.MyAD * CardStatus.MyRatio));
        }
    }
}

using UnityEngine;

public class Card01Ashe : CardParent, ISkill
{
    [SerializeField] private CardStatus CardStatus = null;

    public void ActiveSkill(int myLane)
    {
        var myPlayer = CardStatus.Player;        

        var fullSearch = FullSearch((enemyPlayer, enemyLane) => { return (enemyPlayer != myPlayer && enemyLane == myLane); });
        foreach (var card in fullSearch)
        {
            card.CardStatus.AddDamage((int)(CardStatus.MyAD * CardStatus.MyRatio), (int)EnumSkillType.SkillShot);
            card.CardStatus.IsStun = true;
        }
    }
}
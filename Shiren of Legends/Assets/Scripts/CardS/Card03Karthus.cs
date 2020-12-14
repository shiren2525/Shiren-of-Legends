using UnityEngine;

public class Card03Karthus : CardParent, IHasSlain
{
    [SerializeField] private CardStatus CardStatus = null;
    private bool canSkill = true;

    public bool HasSlain(CardLanes cardLanes, bool player)
    {
        if (!canSkill)
            return canSkill;

        canSkill = false;
        var myPlayer = CardStatus.Player;

        var fullSearch = FullSearch((enemyPlayer, enemyLane) => enemyPlayer != myPlayer);
        foreach (var card in fullSearch)
        {
            card.CardStatus.AddDamage(myPlayer,(int)(CardStatus.MyAD * CardStatus.MyRatio), (int)EnumSkillType.AreaOfEffect);
        }

        canSkill = !canSkill;
        return true;
    }
}
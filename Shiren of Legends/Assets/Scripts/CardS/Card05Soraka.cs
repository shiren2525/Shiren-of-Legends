using UnityEngine;

public class Card05Soraka : CardParent, ISkill
{
    [SerializeField] private CardStatus CardStatus = null;

    public void ActiveSkill(int myLane)
    {
        var myPlayer = CardStatus.Player;

        var fullSearch = FullSearch((enemyPlayer, enemyLane) => enemyPlayer == myPlayer);
        foreach (var card in fullSearch)
        {
            card.CardStatus.AddHeal((int)(CardStatus.MyAD * CardStatus.MyRatio));
        }
    }
}
public class Card05Soraka : CardParent, ISkill
{
    public void ActiveSkill(int myLane)
    {
        var cardStatus = this.gameObject.GetComponent<CardStatus>();
        var myPlayer = cardStatus.Player;

        var fullSearch = FullSearch((enemyPlayer, enemyLane) => enemyPlayer == myPlayer);
        foreach (var card in fullSearch)
        {
            card.CardStatus.AddHeal((int)(cardStatus.MyAD * cardStatus.MyRatio));
        }
    }
}
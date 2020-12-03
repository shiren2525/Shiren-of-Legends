public class Card03Karthus : CardParent, IHasSlain
{
    private bool canSkill = true;

    public bool HasSlain(CardLanes cardLanes, bool player)
    {
        if (!canSkill)
            return canSkill;

        canSkill = false;
        var cardStatus = this.gameObject.GetComponent<CardStatus>();
        var myPlayer = cardStatus.Player;

        var fullSearch = FullSearch((enemyPlayer, enemyLane) => enemyPlayer != myPlayer);
        foreach (var card in fullSearch)
        {
            card.CardStatus.AddDamage((int)(cardStatus.MyAD * cardStatus.MyRatio), (int)EnumSkillType.AreaOfEffect);
        }

        canSkill = !canSkill;
        return true;
    }
}
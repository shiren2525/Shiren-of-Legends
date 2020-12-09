using UnityEngine;

public class Card00Anivia : MonoBehaviour, IHasSlain
{
    private bool canRespawn = true;

    public bool HasSlain(CardLanes cardLanes, bool player)
    {
        if (!canRespawn)
            return canRespawn;

        var cardStatus = this.gameObject.GetComponent<CardStatus>();
        cardStatus.MyHP = cardStatus.MyMaxHP;

        var cardManager = GameObject.FindWithTag(nameof(CardManager)).GetComponent<CardManager>();
        if (player)
        {
            cardManager.JustMovement(cardLanes, (int)EnumBoardLength.MaxBoardX);
        }
        else if (!player)
        {
            cardManager.JustMovement(cardLanes, (int)EnumBoardLength.MinBoard);
        }
        canRespawn = !canRespawn;
        return true;
    }
}
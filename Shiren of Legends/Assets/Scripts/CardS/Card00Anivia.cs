using UnityEngine;

public class Card00Anivia : MonoBehaviour, IHasSlain
{
    private bool canRespawn = true;

    public bool HasSlain(CardLanes cardLanes, bool player)
    {
        if (!canRespawn)
            return true;

        var CardManager = GameObject.Find("CardManager");
        var cardManager = CardManager.GetComponent<CardManager>();
        var cardStatus = this.gameObject.GetComponent<CardStatus>();
        cardStatus.MyHP = cardStatus.MyMaxHP;

        if (player)
        {
            cardManager.JustMovement(cardLanes, (int)EnumBoardLength.MaxBoardX);
        }
        else if (!player)
        {
            cardManager.JustMovement(cardLanes, (int)EnumBoardLength.MinBoard);
        }
        canRespawn = !canRespawn;
        return false;
    }
}
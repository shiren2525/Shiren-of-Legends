using UnityEngine;

public class Card00Anivia : MonoBehaviour, IHasSlain
{
    private bool canRespawn = true;

    public bool HasSlain(int lane, int secondLane, bool player)
    {
        if (!canRespawn)
            return true;

        var CardManager = GameObject.Find("CardManager");
        var cardManager = CardManager.GetComponent<CardManager>();
        var cardStatus = this.gameObject.GetComponent<CardStatus>();
        cardStatus.MyHP = cardStatus.MyMaxHP;

        if (player)
        {
            cardManager.JustMovement(lane, secondLane, 6);
        }
        else if (!player)
        {
            cardManager.JustMovement(lane, secondLane, 0);
        }
        canRespawn = !canRespawn;
        return false;
    }
}
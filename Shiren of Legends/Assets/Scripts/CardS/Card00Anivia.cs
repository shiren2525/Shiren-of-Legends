using UnityEngine;

public class Card00Anivia : MonoBehaviour, IHasSlain
{
    [SerializeField] private CardStatus CardStatus = null;
    private bool canRespawn = true;

    public bool HasSlain(CardLanes cardLanes, bool player)
    {
        if (!canRespawn)
            return canRespawn;

        CardStatus.MyHP = CardStatus.MyMaxHP;

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
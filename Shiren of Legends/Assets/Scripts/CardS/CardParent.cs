using System;
using System.Collections.Generic;
using UnityEngine;

public class CardParent : MonoBehaviour
{
    public IEnumerable<SendCard> FullSearch(Func<bool, int, bool> func)
    {
        var cardManager = GameObject.FindWithTag(nameof(CardManager)).GetComponent<CardManager>();

        for (int i = 0; i < cardManager.BoardList.GetLength(0); i++)
        {
            for (int j = 0; j < cardManager.BoardList.GetLength(1); j++)
            {
                if (cardManager.BoardList[i, j] == null)
                    continue;

                var card = cardManager.BoardList[i, j].GetComponent<CardStatus>();
                if (card == null)
                    continue;

                var enemyPlayer = cardManager.TurnPlayerList[i, j];

                if (func(enemyPlayer, j))
                {
                    yield return new SendCard { Lane = j, Player = enemyPlayer, CardStatus = card };
                }
            }
        }
    }
}

public class SendCard
{
    public int Lane { get; set; }
    public bool Player { get; set; }
    public CardStatus CardStatus { get; set; }
}
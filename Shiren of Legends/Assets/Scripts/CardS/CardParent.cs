using System;
using System.Collections.Generic;
using UnityEngine;

public class CardParent : MonoBehaviour
{
    public IEnumerable<SendCard> FullSearch(Func<bool, int, bool> func)
    {
        var CardManager = GameObject.Find("CardManager");
        var cardManager = CardManager.GetComponent<CardManager>();        

        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 7; j++)
            {
                if (cardManager.BoardList[i, j] == null)
                    continue;

                var card = cardManager.BoardList[i, j].GetComponent<CardStatus>();
                if (card == null)
                    continue;

                var enemyPlayer = cardManager.TurnPlayerList[i, j];

                if (func(enemyPlayer, i))
                {
                    yield return new SendCard { Lane = i, Player = enemyPlayer, CardStatus = card };         
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
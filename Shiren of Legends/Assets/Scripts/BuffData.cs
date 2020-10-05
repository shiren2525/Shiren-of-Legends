using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffData : MonoBehaviour
{
    private CardManager CreateCardManager()
    {
        var CardManager = GameObject.Find("CardManager");
        var cardManager = CardManager.GetComponent<CardManager>();        
        return cardManager;
    }

    private CardStatus CreateCardStatus(int lane, int secondLane)
    {
        var cardManager = CreateCardManager();
        var card = cardManager.BoardList[lane, secondLane].GetComponent<CardStatus>();
        return card;
    }

    public void InfernalBuff(int lane,int secondLane)
    {
        var card = CreateCardStatus(lane,secondLane);
        card.MyAD++;
    }

    public void MountainBuff(int lane,int secondLane)
    {
        var card = CreateCardStatus(lane, secondLane);
        card.MyHP++;
    }

    public void CloudBuff(int lane,int secondLane)
    {
        var cardManager = CreateCardManager();
        cardManager.Skill(lane, secondLane);        
    }

    public void OceanBuff(int lane,int secondLane)
    {
        var card = CreateCardStatus(lane, secondLane);
        card.IsOcean = true;
    }

    public void ElderBuff(int lane,int secondLane)
    {
        var card = CreateCardStatus(lane, secondLane);
        card.CreateBuff();
    }
}

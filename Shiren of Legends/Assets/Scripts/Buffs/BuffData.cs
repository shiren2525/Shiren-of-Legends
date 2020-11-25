using UnityEngine;

public class BuffData : MonoBehaviour
{
    private CardManager CreateCardManager()
    {
        var CardManager = GameObject.Find("CardManager");
        var cardManager = CardManager.GetComponent<CardManager>();        
        return cardManager;
    }

    private CardStatus CreateCardStatus(CardLanes cardLanes)
    {
        var cardManager = CreateCardManager();
        var card = cardManager.BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>();
        return card;
    }

    public void InfernalBuff(CardLanes cardLanes)
    {
        var card = CreateCardStatus(cardLanes);
        card.MyAD++;
    }

    public void MountainBuff(CardLanes cardLanes)
    {
        var card = CreateCardStatus(cardLanes);
        card.MyHP++;
    }

    public void CloudBuff(CardLanes cardLanes)
    {
        var cardManager = CreateCardManager();
        cardManager.Skill(cardLanes);
    }

    public void OceanBuff(CardLanes cardLanes)
    {
        var card = CreateCardStatus(cardLanes);
        card.IsOcean = true;
    }

    public void ElderBuff(CardLanes cardLanes)
    {
        var card = CreateCardStatus(cardLanes);
        card.CreateBuff();
    }
}
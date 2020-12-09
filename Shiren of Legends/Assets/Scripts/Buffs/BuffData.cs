using UnityEngine;

public class BuffData : MonoBehaviour
{
    [SerializeField] private CardManager CardManager = null;

    private CardStatus CreateCardStatus(CardLanes cardLanes)
    {
        return CardManager.BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>(); ;
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
        CardManager.Skill(cardLanes);
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
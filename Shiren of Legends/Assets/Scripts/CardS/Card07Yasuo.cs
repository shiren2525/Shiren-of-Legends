using UnityEngine;

public class Card07Yasuo : MonoBehaviour, IPassiveSkill
{
    public void PassiveSkill()
    {
        var cardStatus = this.gameObject.GetComponent<CardStatus>();
        cardStatus.AddShield((int)(cardStatus.MyAD*cardStatus.MyRatio));
    }
}

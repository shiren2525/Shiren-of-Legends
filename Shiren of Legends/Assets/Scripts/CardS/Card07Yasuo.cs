using UnityEngine;

public class Card07Yasuo : CardParent, IPassiveSkill
{
    [SerializeField] private CardStatus CardStatus = null;

    public void PassiveSkill()
    {
        CardStatus.AddShield((int)(CardStatus.MyAD * CardStatus.MyRatio));
    }
}

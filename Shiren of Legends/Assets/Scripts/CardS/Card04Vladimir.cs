using UnityEngine;

public class Card04Vladimir : MonoBehaviour, IPassiveSkill, ILifeSteal
{
    [SerializeField] private CardStatus CardStatus = null;

    public void PassiveSkill()
    {
        CardStatus.AddDamage((int)(CardStatus.MyAD * CardStatus.MyRatio), (int)EnumSkillType.AreaOfEffect);
    }

    public void LifeSteal()
    {
        CardStatus.AddHeal((int)(CardStatus.MyAD));
        CardStatus.MyAD += (int)(CardStatus.MyAD * CardStatus.MyRatio);
    }
}

using UnityEngine;

public class Card04Vladimir : MonoBehaviour, IPassiveSkill, ILifeSteal
{
    public void PassiveSkill()
    {
        var cardStatus = this.gameObject.GetComponent<CardStatus>();
        cardStatus.AddDamage((int)(cardStatus.MyAD * cardStatus.MyRatio), (int)EnumSkillType.AreaOfEffect);
    }

    public void LifeSteal()
    {
        var cardStatus = this.gameObject.GetComponent<CardStatus>();
        cardStatus.AddHeal((int)(cardStatus.MyAD));
        cardStatus.MyAD += (int)(cardStatus.MyAD * cardStatus.MyRatio);
    }
}

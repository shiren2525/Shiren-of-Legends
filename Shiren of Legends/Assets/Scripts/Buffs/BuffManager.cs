using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public List<int> RedBuffList { get; set; } = new List<int> { };
    public List<int> BlueBuffList { get; set; } = new List<int> { };

    public void Buff(CardLanes cardLanes, bool player)
    {        
        if (player)
        {
            foreach (var value in RedBuffList)
            {
                Buff(value, cardLanes);
            }
        }
        else if (!player)
        {
            foreach (var value in BlueBuffList)
            {
                Buff(value, cardLanes);
            }
        }
    }

    public void Buff(int buffNum, CardLanes cardLanes)
    {
        var buffData = gameObject.AddComponent<BuffData>();
        switch (buffNum)
        {
            case (int)EnumMonster.Infernal:
                buffData.InfernalBuff(cardLanes);
                break;
            case (int)EnumMonster.Mountain:
                buffData.MountainBuff(cardLanes);
                break;
            case (int)EnumMonster.Cloud:
                buffData.CloudBuff(cardLanes);
                break;
            case (int)EnumMonster.Ocean:
                buffData.OceanBuff(cardLanes);
                break;
            case (int)EnumMonster.Elder:
                buffData.ElderBuff(cardLanes);
                break;

            default:
                break;
        }
    }
}
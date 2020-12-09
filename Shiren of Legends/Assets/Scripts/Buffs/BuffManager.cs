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

    [SerializeField] private BuffData BuffData = null;
    public void Buff(int buffNum, CardLanes cardLanes)
    {
        switch (buffNum)
        {
            case (int)EnumMonster.Infernal:
                BuffData.InfernalBuff(cardLanes);
                break;
            case (int)EnumMonster.Mountain:
                BuffData.MountainBuff(cardLanes);
                break;
            case (int)EnumMonster.Cloud:
                BuffData.CloudBuff(cardLanes);
                break;
            case (int)EnumMonster.Ocean:
                BuffData.OceanBuff(cardLanes);
                break;
            case (int)EnumMonster.Elder:
                BuffData.ElderBuff(cardLanes);
                break;

            default:
                Debug.LogError("buff is Error");
                break;
        }
    }
}
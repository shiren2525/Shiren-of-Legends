using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public List<int> RedBuffList { get; set; } = new List<int> { };
    public List<int> BlueBuffList { get; set; } = new List<int> { };

    public void Buff(int i, int j, bool player)
    {        
        if (player)
        {
            foreach (var value in RedBuffList)
            {
                Buff(value, i, j);
            }
        }
        else if (!player)
        {
            foreach (var value in BlueBuffList)
            {
                Buff(value, i, j);
            }
        }
    }

    public void Buff(int buffNum, int lane, int secondLane)
    {
        var buffData = gameObject.AddComponent<BuffData>();
        switch (buffNum)
        {
            case (int)EnumMonster.Infernal:
                buffData.InfernalBuff(lane, secondLane);
                break;
            case (int)EnumMonster.Mountain:
                buffData.MountainBuff(lane, secondLane);
                break;
            case (int)EnumMonster.Cloud:
                buffData.CloudBuff(lane, secondLane);
                break;
            case (int)EnumMonster.Ocean:
                buffData.OceanBuff(lane, secondLane);
                break;
            case (int)EnumMonster.Elder:
                buffData.ElderBuff(lane, secondLane);
                break;

            default:
                break;
        }
    }
}
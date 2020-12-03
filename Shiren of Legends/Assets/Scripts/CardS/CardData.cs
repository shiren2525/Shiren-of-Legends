using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public Dictionary<int, int> KeyValuesHP { get; set; }
    public Dictionary<int, int> KeyValuesAD { get; set; }
    public Dictionary<int, float> KeyValuesRatio { get; set; }
    public Dictionary<int, string> KeyValuesName { get; set; }

    private void Start()
    {
        CardParameter[] cardParameters = new CardParameter[]
        {
           new CardParameter(){ ID=0, Name="Anivia",    HP=5, AD=5, Ratio=1.0f },
           new CardParameter(){ ID=1, Name="Ashe",      HP=5, AD=5, Ratio=0.2f },
           new CardParameter(){ ID=2, Name="NULL",      HP=5, AD=5, Ratio=0.2f },
           new CardParameter(){ ID=3, Name="Karthus",   HP=5, AD=5, Ratio=2.0f },
           new CardParameter(){ ID=4, Name="Vladimir",  HP=10, AD=10, Ratio=0.2f },
           new CardParameter(){ ID=5, Name="Soraka",    HP=5, AD=2, Ratio=1.0f },
           new CardParameter(){ ID=6, Name="Sena",      HP=5, AD=2, Ratio=0.5f },
           new CardParameter(){ ID=7, Name="Yasuo",     HP=10, AD=10, Ratio=1.0f },
        };

        KeyValuesHP = cardParameters.ToDictionary(value => value.ID, value => value.HP);
        KeyValuesAD = cardParameters.ToDictionary(value => value.ID, value => value.AD);
        KeyValuesRatio = cardParameters.ToDictionary(value => value.ID, value => value.Ratio);
        KeyValuesName = cardParameters.ToDictionary(value => value.ID, value => value.Name);
    }
}

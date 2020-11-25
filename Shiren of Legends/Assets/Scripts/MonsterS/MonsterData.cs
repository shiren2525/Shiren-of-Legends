using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterData : MonoBehaviour
{
    public Dictionary<int, int> KeyValuesHP { get; set; }
    public Dictionary<int, int> KeyValuesAD { get; set; }    
    public Dictionary<int, string> KeyValuesName { get; set; }

    private void Start()
    {
        CardParameter[] cardParameters = new CardParameter[]
        {
           new CardParameter(){ ID=0, Name="Infernal",    HP=5, AD=5},
           new CardParameter(){ ID=1, Name="Mountain",      HP=5, AD=5},
           new CardParameter(){ ID=2, Name="Cloud",      HP=5, AD=5},
           new CardParameter(){ ID=3, Name="Ocean",   HP=5, AD=5},
           new CardParameter(){ ID=4, Name="Elder",  HP=10, AD=10},
        };

        KeyValuesHP = cardParameters.ToDictionary(value => value.ID, value => value.HP);
        KeyValuesAD = cardParameters.ToDictionary(value => value.ID, value => value.AD);        
        KeyValuesName = cardParameters.ToDictionary(value => value.ID, value => value.Name);
    }
}
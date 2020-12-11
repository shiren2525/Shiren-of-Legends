using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private CardData CardDate = null;
    [SerializeField] private Text[] Texts = new Text[(int)EnumNumbers.Cards];
    [SerializeField] private Image[] Images = new Image[(int)EnumNumbers.Cards];
    [SerializeField] private Sprite[] FullCardSprites = new Sprite[(int)EnumNumbers.FullCards];

    public void Init(int[] cardIDs)
    {
        var loadtext = (Resources.Load("CardEffect", typeof(TextAsset)) as TextAsset).text;
        string[] spliteText = loadtext.Split('\n');
        var num = 0;

        foreach(var val in cardIDs)
        {
            Texts[num].text=
                CardDate.KeyValuesName[val] + ":HP" + CardDate.KeyValuesHP[val].ToString() + "/AD" + CardDate.KeyValuesAD[val].ToString() + "\n" +
                spliteText[val].Replace("ad", (CardDate.KeyValuesAD[val] * CardDate.KeyValuesRatio[val]).ToString())
                             .Replace("hp", CardDate.KeyValuesHP[val].ToString());
            Images[num].sprite = FullCardSprites[val];
            num++;
        }
    }

    [SerializeField] private GameObject Canvas = null;
    public void PanelChange()
    {
        if (Canvas.activeSelf)
        {
            Canvas.SetActive(false);
        }
        else if (!Canvas.activeSelf)
        {
            Canvas.SetActive(true);
        }
    }
}
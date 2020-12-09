using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private CardData CardDate = null;
    [SerializeField] private Text[] Texts = new Text[(int)EnumNumbers.Cards];

    private void Init()
    {
        var loadtext = (Resources.Load("CardEffect", typeof(TextAsset)) as TextAsset).text;
        string[] spliteText = loadtext.Split('\n');

        for (int i = 0; i < (int)EnumNumbers.Cards; i++)
        {
            Texts[i].text =
                CardDate.KeyValuesName[i] + ":HP" + CardDate.KeyValuesHP[i].ToString() + "/AD" + CardDate.KeyValuesAD[i].ToString() + "\n" +
                spliteText[i].Replace("ad", (CardDate.KeyValuesAD[i] * CardDate.KeyValuesRatio[i]).ToString())
                             .Replace("hp", CardDate.KeyValuesHP[i].ToString());
        }
    }

    private void Start()
    {
        Init();
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
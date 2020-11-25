using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] CardData CardDate = null;
    [SerializeField] Text[] Texts = new Text[8];    

    private void Init()
    {        
        var loadtext = (Resources.Load("CardEffect", typeof(TextAsset)) as TextAsset).text;
        string[] spliteText = loadtext.Split('\n');

        for (int i = 0; i < 8; i++)
        {
            Texts[i].text =
                CardDate.KeyValuesName[i] + ":HP" + CardDate.KeyValuesAD[i].ToString() + "/AD" + CardDate.KeyValuesHP[i].ToString() + "\n" +
                spliteText[i].Replace("ad", (CardDate.KeyValuesAD[i] * CardDate.KeyValuesRatio[i]).ToString())
                             .Replace("hp", CardDate.KeyValuesHP[i].ToString());
        }
    }

    private void Start()
    {
        Init();
    }

    [SerializeField] GameObject Canvas = null;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
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
}
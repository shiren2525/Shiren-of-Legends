using UnityEngine;
using UnityEngine.UI;

public class SetSpriteScript : MonoBehaviour
{
    [SerializeField] private Image[] Images = new Image[(int)EnumNumbers.Cards];
    [SerializeField] private Sprite[] Sprites = new Sprite[(int)EnumNumbers.FullCards];

    private int num = 0;
    public void ReceiveCardID(int cardID)
    {
        Images[num].sprite = Sprites[cardID];
        num++;
    }

    public void DeleteDeck()
    {
        foreach(var val in Images)
        {
            val.sprite = null;
        }
        num = 0;
    }
}

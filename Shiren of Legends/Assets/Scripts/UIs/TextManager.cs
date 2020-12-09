using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] private Text textPlayer0HP = null;
    [SerializeField] private Text textPlayer1HP = null;

    [SerializeField] private Text[] textTower0HP = new Text[(int)EnumBoardLength.MaxBoardLengthY];
    [SerializeField] private Text[] textTower1HP = new Text[(int)EnumBoardLength.MaxBoardLengthY];

    [SerializeField] private SpriteRenderer[] spriteRenderers = new SpriteRenderer[(int)EnumBoardLength.MaxBoardLengthY];
    [SerializeField] private SpriteRenderer[] spriteRenderers1 = new SpriteRenderer[(int)EnumBoardLength.MaxBoardLengthY];

    [SerializeField] private Sprite spriteBlack = null;

    [SerializeField] private Image ImagePanel0 = null;
    [SerializeField] private Image ImagePanel1 = null;

    [SerializeField] private Text TextTurnNum = null;

    public void SetText(int health, bool player)
    {
        if (player)
        {
            textPlayer0HP.text = "HP: " + health.ToString();
        }
        else if (!player)
        {
            textPlayer1HP.text = "HP: " + health.ToString();
        }
    }

    public void SetTowerHPText(int health, bool player, int laneY)
    {
        if (player)
        {
            textTower0HP[laneY].text = "HP: " + health.ToString();
            if (health <= 0)
            {
                textTower0HP[laneY].text = "";
                spriteRenderers[laneY].sprite = spriteBlack;
            }
        }
        else if (!player)
        {
            textTower1HP[laneY].text = "HP: " + health.ToString();
            if (health <= 0)
            {
                textTower1HP[laneY].text = "";
                spriteRenderers1[laneY].sprite = spriteBlack;
            }
        }
    }

    private Color colorOn = new Color32(255, 255, 255, 255);
    private Color colorOff = new Color32(255, 255, 255, 0);
    public void SetPanel(bool turn)
    {
        if (turn)
        {
            ImagePanel0.GetComponent<Image>().color = colorOff;
            ImagePanel1.GetComponent<Image>().color = colorOn;
        }
        else if (!turn)
        {
            ImagePanel0.GetComponent<Image>().color = colorOn;
            ImagePanel1.GetComponent<Image>().color = colorOff;
        }
    }

    public void SetTurnNum(int value)
    {
        TextTurnNum.text = "Dragon " + (5 - value).ToString();
    }

    [SerializeField] private GameObject[] dragonIcons = new GameObject[(int)EnumMonster.Elder + 1];

    public void CreateDragonIconinPanel(int dragonID, bool player)
    {
        var gameobject = Instantiate(dragonIcons[dragonID]);

        if (player)
        {
            gameobject.transform.SetParent(ImagePanel1.transform);
        }
        else if (!player)
        {
            gameobject.transform.SetParent(ImagePanel0.transform);
        }
        gameobject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
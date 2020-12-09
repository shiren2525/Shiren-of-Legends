using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] Text textPlayer0HP = null;
    [SerializeField] Text textPlayer1HP = null;

    [SerializeField] Text[] textTower0HP = new Text[4];
    [SerializeField] Text[] textTower1HP = new Text[4];

    [SerializeField] SpriteRenderer[] spriteRenderers = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] spriteRenderers1 = new SpriteRenderer[4];

    [SerializeField] Sprite spriteBlack = null;

    [SerializeField] Image ImagePanel0 = null;
    [SerializeField] Image ImagePanel1 = null;

    [SerializeField] Text TextTurnNum = null;

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

    public void SetPanel(bool turn)
    {
        var colorOn = new Color32(255, 255, 255, 200);
        var colorOff = new Color32(255, 255, 255, 0);
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

    [SerializeField] GameObject[] dragonIcons = new GameObject[5];

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
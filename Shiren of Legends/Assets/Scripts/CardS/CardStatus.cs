using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardStatus : MonoBehaviour
{
    private int myID;
    public int MyMaxHP { get; set; }
    public int MyHP { get; set; }
    public int MyAD { get; set; }
    public float MyRatio { get; private set; }
    public bool IsStun { get; set; }
    private int myShield;

    public bool Player { get; private set; }
    public CardLanes CardLanes { get; set; }

    public bool IsOcean { get; set; }

    public void Create(int ID, bool player, CardLanes cardLanes)
    {
        myID = ID;
        this.Player = player;
        CardLanes = cardLanes;

        var CardDate = GameObject.Find(nameof(CardData));
        var cardDate = CardDate.GetComponent<CardData>();
        MyHP = cardDate.KeyValuesHP[myID];
        MyAD = cardDate.KeyValuesAD[myID];
        MyRatio = cardDate.KeyValuesRatio[myID];
        MyMaxHP = MyHP;

        CreateBuff();

        ColorChange();
        SetText();
    }

    private int buffCount = 0;
    public void CreateBuff()
    {
        if (buffCount > 1)
            return;

        buffCount++;
        var BuffManagerObj = GameObject.Find(nameof(BuffManager));
        var buffManager = BuffManagerObj.GetComponent<BuffManager>();
        buffManager.Buff(CardLanes, Player);
    }

    private void ColorChange()
    {
        if (Player)
            this.GetComponent<Renderer>().material.color = Color.red;

        else if (!Player)
            this.GetComponent<Renderer>().material.color = Color.cyan;
    }

    public void AddDamage(int damage, int damageType)
    {
        if (GetComponent<Card07Yasuo>() && (int)EnumSkillType.SkillShot == damageType)
            return;

        if (damage < myShield)
        {
            myShield -= damage;
        }
        else
        {
            try
            {
                checked
                {
                    MyHP -= (damage - myShield);
                    myShield = 0;
                }
            }
            catch (OverflowException)
            {
                Debug.Log("オーバーフロー");
            }
        }
        Debug.Log("Dameged HP:" + MyHP + " / " + this.name);

        CheckArrive(damageType);
        SetText();
    }

    private void CheckArrive(int damageType)
    {
        if (MyHP <= 0)
        {
            Destroyer();
        }
        else if (MyHP >= 0)
        {
            if (IsOcean && MyHP < MyMaxHP)
                MyHP = MyMaxHP;

            var card = this.gameObject.GetComponent<ILifeSteal>();
            if (card == null)
                return;

            if ((int)EnumSkillType.AutoAttack == damageType)
            {
                card.LifeSteal();
            }
        }
    }

    public void AddHeal(int heal)
    {
        MyHP += heal;
        SetText();
    }

    public void AddShield(int shield)
    {
        if (myShield + shield >= 2)
        {
            myShield = 2;
        }
        else
        {
            myShield += shield;
        }
        SetText();
    }

    private void Destroyer()
    {
        var card = this.gameObject.GetComponent<IHasSlain>();
        if (card != null)
        {
            var canSlainSkill = card.HasSlain(CardLanes, Player);
            if (canSlainSkill && GetComponent<Card00Anivia>())
                return;
        }

        var CardManager = GameObject.Find("CardManager");
        var cardManager = CardManager.GetComponent<CardManager>();
        cardManager.Destroyer(CardLanes);
    }

    [SerializeField] Text Text = null;
    public void SetText()
    {
        Text.text = MyAD.ToString() + "/" + MyHP.ToString() + "+" + myShield.ToString();
    }
}
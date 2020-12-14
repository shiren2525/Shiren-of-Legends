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
    public int MyShield { get; set; }
    public bool IsStun { get; set; }

    public bool Player { get; private set; }
    public CardLanes CardLanes { get; set; }

    public bool IsOcean { get; set; }
    public bool IsMonsterCard { get; set; }

    [SerializeField] CardData CardData = null;
    [SerializeField] MonsterData MonsterData = null;

    public void Create(bool isMonsterCard, int ID, bool player, CardLanes cardLanes)
    {
        this.IsMonsterCard = isMonsterCard;
        myID = ID;
        this.Player = player;
        CardLanes = cardLanes;

        if (IsMonsterCard)
        {
            MyHP = MonsterData.KeyValuesHP[myID];
            MyAD = MonsterData.KeyValuesAD[myID];
        }
        else
        {
            MyHP = CardData.KeyValuesHP[myID];
            MyAD = CardData.KeyValuesAD[myID];
            MyRatio = CardData.KeyValuesRatio[myID];

            CreateBuff();
            ColorChange();
        }

        MyMaxHP = MyHP;
        SetText();
        Debug.Log("Summon ID " + myID + ":" + MyAD + "/" + MyHP + ":" + this.name);
    }

    private int buffCount = 0;
    public void CreateBuff()
    {
        if (buffCount > 1)
            return;

        buffCount++;
        var buffManager = GameObject.FindWithTag(nameof(BuffManager)).GetComponent<BuffManager>();
        buffManager.Buff(CardLanes, Player);
    }

    [SerializeField] Renderer Renderer = null;
    private void ColorChange()
    {
        if (Player)
            Renderer.material.color = Color.red;

        else if (!Player)
            Renderer.material.color = Color.cyan;
    }

    public void AddDamage(bool turn,int damage, int damageType)
    {
        if (GetComponent<Card07Yasuo>() && (int)EnumSkillType.SkillShot == damageType)
            return;

        if (damage < MyShield)
        {
            MyShield -= damage;
        }
        else
        {
            try
            {
                checked
                {
                    MyHP -= (damage - MyShield);
                    MyShield = 0;
                }
            }
            catch (OverflowException)
            {
                Debug.LogError("オーバーフロー");
            }
        }
        Debug.Log("<color=magenta>" + "Dameged HP:" + MyHP + " / " + this.name + "</color>");

        CheckArrive(turn,damageType);
        SetText();
    }

    private void CheckArrive(bool turn,int damageType)
    {
        if (MyHP <= 0)
        {
            Destroyer(turn);
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

        Debug.Log("<color=green>" + "Healed HP: " + MyHP + " : " + this.name + "</color>");
    }

    private readonly int MaxShield = 2;
    public void AddShield(int shield)
    {
        MyShield = MyShield + shield >= MaxShield ? MaxShield : MyShield += shield;
        SetText();
    }

    private void Destroyer(bool turn)
    {
        var card = this.gameObject.GetComponent<IHasSlain>();
        if (card != null)
        {
            var canSlainSkill = card.HasSlain(CardLanes, Player);
            if (canSlainSkill && GetComponent<Card00Anivia>())
                return;
        }

        if (IsMonsterCard)
        {
            SendBuff(turn);
        }

        var cardManager = GameObject.FindWithTag(nameof(CardManager)).GetComponent<CardManager>();
        cardManager.Destroyer(CardLanes);
    }

    private void SendBuff(bool turn)
    {
        var buffManager = GameObject.FindWithTag(nameof(BuffManager)).GetComponent<BuffManager>();
        var textManager = GameObject.FindWithTag(nameof(TextManager)).GetComponent<TextManager>();
        if (turn)
        {
            buffManager.RedBuffList.Add(myID);
            textManager.CreateDragonIconinPanel(myID, turn);
        }
        else if (!turn)
        {
            buffManager.BlueBuffList.Add(myID);
            textManager.CreateDragonIconinPanel(myID, turn);
        }
    }

    [SerializeField] Text Text = null;
    public void SetText()
    {
        Text.text = MyAD.ToString() + "/" + MyHP.ToString() + "+" + MyShield.ToString();
    }
}
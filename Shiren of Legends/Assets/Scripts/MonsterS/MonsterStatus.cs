using UnityEngine;
using UnityEngine.UI;

public class MonsterStatus : MonoBehaviour
{
    private int myID;
    public int MyHP { get; set; }
    public int MyAD { get; set; }
    public CardLanes CardLanes { get; set; }

    public void Create(int ID, CardLanes cardLanes)
    {
        var monsterData = GameObject.FindWithTag(nameof(MonsterData)).GetComponent<MonsterData>();
        myID = ID;
        CardLanes = cardLanes;
        MyHP = monsterData.KeyValuesHP[myID];
        MyAD = monsterData.KeyValuesAD[myID];

        SetText();
    }

    public void AddDamage(int damage, bool player)
    {
        MyHP -= damage;

        SetText();        

        if (MyHP <= 0)
        {
            Destroyer(player);
        }
    }

    private void Destroyer(bool player)
    {
        var buffManager = GameObject.FindWithTag(nameof(BuffManager)).GetComponent<BuffManager>();
        var textManager = GameObject.FindWithTag(nameof(TextManager)).GetComponent<TextManager>();
        if (player)
        {
            buffManager.RedBuffList.Add(myID);
            textManager.CreateDragonIconinPanel(myID, player);
        }
        else if (!player)
        {
            buffManager.BlueBuffList.Add(myID);
            textManager.CreateDragonIconinPanel(myID, player);
        }

        var cardManager = GameObject.FindWithTag(nameof(CardManager)).GetComponent<CardManager>();
        cardManager.Destroyer(CardLanes);
    }

    [SerializeField] Text Text = null;
    private void SetText()
    {
        Text.text = MyAD.ToString() + "/" + MyHP.ToString();
    }
}
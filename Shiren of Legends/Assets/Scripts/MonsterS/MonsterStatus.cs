using UnityEngine;
using UnityEngine.UI;

public class MonsterStatus : MonoBehaviour
{
    private int myID;
    public int MyHP { get; set; }
    public int MyAD { get; set; }

    private int lane;
    public int SecondLane { get; set; }    

    public void Create(int ID, int i, int j)
    {
        var MonsterDataObj = GameObject.Find("MonsterData");
        var monsterData = MonsterDataObj.GetComponent<MonsterData>();
        myID = ID;
        lane = i;
        SecondLane = j;        
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
            var BuffManager = GameObject.Find("BuffManager");
            var buffManager = BuffManager.GetComponent<BuffManager>();

            var TextManager = GameObject.Find("TextManager");
            var textManager = TextManager.GetComponent<TextManager>();
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
            Destroyer();
        }
    }

    public void AddHeal(int heal)
    {
        MyHP += heal;
        SetText();
        Debug.Log(this.name + " is Healed " + MyHP);
    }

    private void Destroyer()
    {
        var CardManager = GameObject.Find("CardManager");
        var cardManager = CardManager.GetComponent<CardManager>();

        cardManager.Destroyer(new CardLanes { X = lane, Y = SecondLane });
    }

    [SerializeField] Text Text = null;
    private void SetText()
    {
        Text.text = MyAD.ToString() + "/" + MyHP.ToString();
    }
}
using System;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] BoardManager BoardManager = null;
    [SerializeField] PlayerStatus PlayerStatus = null;
    [SerializeField] GameObject[] cardObjList = new GameObject[8];
    [SerializeField] GameObject[] monsterObjList = new GameObject[5];
    [SerializeField] Transform[] transformsHnad = new Transform[2];
    public GameObject[,] BoardList { get; private set; } = new GameObject[4, 7];
    public bool[,] TurnPlayerList { get; private set; } = new bool[4, 7];
    private readonly GameObject[] hand = new GameObject[2];

    public void Draw(int cardID, int cardID1)
    {
        hand[0] = Instantiate(cardObjList[cardID], transformsHnad[0].position, Quaternion.identity);
        hand[1] = Instantiate(cardObjList[cardID1], transformsHnad[1].position, Quaternion.identity);
    }

    public void DeleteHand()
    {
        Destroy(hand[0]);
        Destroy(hand[1]);
    }

    public bool CheckCanSummon(int lane, bool turn)
    {
        if (turn)
        {
            return BoardList[lane, 6] == null;
        }
        else if (!turn)
        {
            return BoardList[lane, 0] == null;
        }
        return turn;
    }

    public void Summon(int i, int j, int cardID, bool turn)
    {
        BoardList[i, j] = Instantiate(cardObjList[cardID], BoardManager.TransformList[i, j].position, Quaternion.identity) as GameObject;
        BoardList[i, j].GetComponent<CardStatus>().Create(cardID, i, j, turn);
        TurnPlayerList[i, j] = turn;
        Skill(i, j);
    }

    public void Skill(int i, int j)
    {
        var card = BoardList[i, j].GetComponent<ISkill>();
        if (card == null)
            return;

        card.ActiveSkill(i);
    }

    private void PassiveSkill(int i, int j)
    {
        var card = BoardList[i, j].GetComponent<IPassiveSkill>();
        if (card == null)
            return;

        card.PassiveSkill();
    }

    public void SummonMonster(int ID,int i,int j)
    {
        BoardList[i, j] = Instantiate(monsterObjList[ID], BoardManager.TransformList[i, j].position, Quaternion.identity) as GameObject;
        BoardList[i, j].GetComponent<MonsterStatus>().Create(ID, i, j);
    }

    public void Movement(int gap, bool turn)
    {
        for (int i = 0; i < BoardList.GetLength(0); i++)
        {
            // 連続で移動しないために探索順を分ける必要がある
            if (turn)
            {
                for (int j = 0; j < BoardList.GetLength(1); j++)
                {
                    Movement(i, j, j + gap);
                }
            }
            else if (!turn)
            {
                for (int j = BoardList.GetLength(1) - 1; j >= 0; j--)
                {
                    Movement(i, j, j + gap);
                }
            }
        }

        void Movement(int i, int j, int nextBoard)
        {
            if (BoardList[i, j] == null)
                return;

            if (!BoardList[i, j].GetComponent<CardStatus>())
                return;

            var card = BoardList[i, j].GetComponent<CardStatus>();
            if (card.IsStun)
                return;

            // 自分のカードだけを動かすため
            if (turn && TurnPlayerList[i, j] || !turn && !TurnPlayerList[i, j])
            {
                PassiveSkill(i, j);

                if (nextBoard == 0 || nextBoard == 6)
                {
                    PlayerStatus.AddDirectDamage(card.MyAD, turn);
                    Destroyer(i, j);
                    return;
                }
                else if (BoardList[i, nextBoard] == null)
                {
                    JustMovement(i, j, nextBoard);
                }
                else if (BoardList[i, nextBoard].GetComponent<CardStatus>())
                {
                    Battle<CardStatus>(i, j, nextBoard,CreateEnemy<CardStatus>(i,nextBoard));
                    return;
                }
                else if (BoardList[i, nextBoard].GetComponent<MonsterStatus>())
                {
                    // TODO slain monster
                    BattleMonster<MonsterStatus>(i, j, nextBoard, CreateEnemy<MonsterStatus>(i, nextBoard),turn);
                    return;
                }
            }
        }       
    }

    public Type CreateEnemy<Type>(int lane,int nextBoard)
    {
        var enemy= BoardList[lane, nextBoard].GetComponent<Type>();
        return enemy;
    }

    public void Battle<Type>(int i, int j, int nextBoard,Type type) where Type:CardStatus
    {
        var thisCard = BoardList[i, j].GetComponent<CardStatus>();
        var enemyCard = type;
//enemyCard = BoardList[i, nextBoard].GetComponent<CardStatus>();
        if (enemyCard.MyHP <= thisCard.MyAD)
        {
            thisCard.AddDamage(enemyCard.MyAD, (int)EnumSkillType.AutoAttack);
            enemyCard.AddDamage(thisCard.MyAD, (int)EnumSkillType.AutoAttack);

            JustMovement(i, j, nextBoard);
        }
        else if (thisCard.MyHP <= enemyCard.MyAD)
        {
            enemyCard.AddDamage(thisCard.MyAD, (int)EnumSkillType.AutoAttack);
            thisCard.AddDamage(enemyCard.MyAD, (int)EnumSkillType.AutoAttack);
        }
        else
        {
            Destroyer(i, j);
            Destroyer(i, nextBoard);
        }
    }

    public void BattleMonster<Type>(int i, int j, int nextBoard,Type type, bool player) where Type:MonsterStatus
    {
        var thisCard = BoardList[i, j].GetComponent<CardStatus>();
        var enemyMonster = type;

        if (enemyMonster.MyHP <= thisCard.MyAD)
        {
            thisCard.AddDamage(enemyMonster.MyAD, (int)EnumSkillType.AutoAttack);
            enemyMonster.AddDamage(thisCard.MyAD, player);

            JustMovement(i, j, nextBoard);
        }
        else if (thisCard.MyHP <= enemyMonster.MyAD)
        {
            enemyMonster.AddDamage(thisCard.MyAD, player);
            thisCard.AddDamage(enemyMonster.MyAD, (int)EnumSkillType.AutoAttack);
        }
        else
        {
            Destroyer(i, j);
            Destroyer(i, nextBoard);
        }
    }

    public void JustMovement(int i, int j, int nextBoard)
    {
        if (BoardList[i, nextBoard] != null)
            Destroyer(i, j);

        if (BoardList[i, j] == null)
            return;

        var cardStatus = BoardList[i, j].GetComponent<CardStatus>();
        if (cardStatus == null)
            return;

        BoardList[i, j].transform.position = BoardManager.TransformList[i, nextBoard].position;
        BoardList[i, nextBoard] = BoardList[i, j];
        TurnPlayerList[i, nextBoard] = TurnPlayerList[i, j];
        cardStatus.SecondLane = nextBoard;
        BoardList[i, j] = null;
        TurnPlayerList[i, j] = false;
    }

    public void Destroyer(int i, int j)
    {
        Destroy(BoardList[i, j]);
        BoardList[i, j] = null;
        TurnPlayerList[i, j] = false;
    }
}
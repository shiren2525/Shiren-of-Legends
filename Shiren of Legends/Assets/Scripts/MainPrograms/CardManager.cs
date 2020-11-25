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

    public void Summon(CardLanes cardLanes, int cardID, bool turn)
    {
        BoardList[cardLanes.X, cardLanes.Y] = Instantiate(cardObjList[cardID], BoardManager.TransformList[cardLanes.X, cardLanes.Y].position, Quaternion.identity) as GameObject;
        BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>().Create(cardID, turn, cardLanes);
        TurnPlayerList[cardLanes.X, cardLanes.Y] = turn;
        Skill(cardLanes);
    }

    public void Skill(CardLanes cardLanes)
    {
        var card = BoardList[cardLanes.X, cardLanes.Y].GetComponent<ISkill>();
        if (card == null)
            return;

        card.ActiveSkill(cardLanes.X);
    }

    private void PassiveSkill(CardLanes cardLanes)
    {
        var card = BoardList[cardLanes.X, cardLanes.Y].GetComponent<IPassiveSkill>();
        if (card == null)
            return;

        card.PassiveSkill();
    }

    public void SummonMonster(int ID, CardLanes cardLanes)
    {
        BoardList[cardLanes.X, cardLanes.Y] = Instantiate(monsterObjList[ID], BoardManager.TransformList[cardLanes.X, cardLanes.Y].position, Quaternion.identity) as GameObject;
        BoardList[cardLanes.X, cardLanes.Y].GetComponent<MonsterStatus>().Create(ID, cardLanes.X, cardLanes.Y);
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
                    Movement(new CardLanes { X = i, Y = j }, j + gap);
                }
            }
            else if (!turn)
            {
                for (int j = BoardList.GetLength(1) - 1; j >= 0; j--)
                {                    
                    Movement(new CardLanes { X = i, Y = j }, j + gap);
                }
            }
        }

        void Movement(CardLanes cardLanes, int nextBoard)
        {
            if (BoardList[cardLanes.X, cardLanes.Y] == null)
                return;

            if (!BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>())
                return;

            var card = BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>();
            if (card.IsStun)
                return;

            // 自分のカードだけを動かすため
            if (turn && TurnPlayerList[cardLanes.X, cardLanes.Y] || !turn && !TurnPlayerList[cardLanes.X, cardLanes.Y])
            {
                PassiveSkill(cardLanes);

                if (nextBoard == 0 || nextBoard == 6)
                {
                    PlayerStatus.AddDirectDamage(card.MyAD, turn);
                    Destroyer(cardLanes);
                    return;
                }
                else if (BoardList[cardLanes.X, nextBoard] == null)
                {
                    JustMovement(cardLanes, nextBoard);
                }
                else if (BoardList[cardLanes.X, nextBoard].GetComponent<CardStatus>())
                {
                    Battle<CardStatus>(cardLanes, nextBoard,CreateEnemy<CardStatus>(cardLanes.X, nextBoard));
                    return;
                }
                else if (BoardList[cardLanes.X, nextBoard].GetComponent<MonsterStatus>())
                {
                    // TODO slain monster
                    BattleMonster<MonsterStatus>(cardLanes, nextBoard, CreateEnemy<MonsterStatus>(cardLanes.X, nextBoard),turn);
                    return;
                }
            }
        }       
    }

    public Type CreateEnemy<Type>(int lane, int nextBoard)
    {
        var enemy= BoardList[lane, nextBoard].GetComponent<Type>();
        return enemy;
    }

    public void Battle<Type>(CardLanes cardLanes, int nextBoard,Type type) where Type:CardStatus
    {
        var thisCard = BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>();
        var enemyCard = type;

        if (enemyCard.MyHP <= thisCard.MyAD)
        {
            thisCard.AddDamage(enemyCard.MyAD, (int)EnumSkillType.AutoAttack);
            enemyCard.AddDamage(thisCard.MyAD, (int)EnumSkillType.AutoAttack);

            JustMovement(cardLanes, nextBoard);
        }
        else if (thisCard.MyHP <= enemyCard.MyAD)
        {
            enemyCard.AddDamage(thisCard.MyAD, (int)EnumSkillType.AutoAttack);
            thisCard.AddDamage(enemyCard.MyAD, (int)EnumSkillType.AutoAttack);
        }
        else
        {
            Destroyer(cardLanes);
            Destroyer(new CardLanes { X=cardLanes.X, Y=nextBoard});
        }
    }

    public void BattleMonster<Type>(CardLanes cardLanes, int nextBoard,Type type, bool player) where Type:MonsterStatus
    {
        var thisCard = BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>();
        var enemyMonster = type;

        if (enemyMonster.MyHP <= thisCard.MyAD)
        {
            thisCard.AddDamage(enemyMonster.MyAD, (int)EnumSkillType.AutoAttack);
            enemyMonster.AddDamage(thisCard.MyAD, player);

            JustMovement(cardLanes, nextBoard);
        }
        else if (thisCard.MyHP <= enemyMonster.MyAD)
        {
            enemyMonster.AddDamage(thisCard.MyAD, player);
            thisCard.AddDamage(enemyMonster.MyAD, (int)EnumSkillType.AutoAttack);
        }
        else
        {
            Destroyer(cardLanes);
            Destroyer(cardLanes);
        }
    }

    public void JustMovement(CardLanes cardLanes, int nextBoard)
    {
        if (BoardList[cardLanes.X, nextBoard] != null)
            Destroyer(cardLanes);

        if (BoardList[cardLanes.X, cardLanes.Y] == null)
            return;

        var cardStatus = BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>();
        if (cardStatus == null)
            return;

        BoardList[cardLanes.X, cardLanes.Y].transform.position = BoardManager.TransformList[cardLanes.X, nextBoard].position;
        BoardList[cardLanes.X, nextBoard] = BoardList[cardLanes.X, cardLanes.Y];
        TurnPlayerList[cardLanes.X, nextBoard] = TurnPlayerList[cardLanes.X, cardLanes.Y];
        cardStatus.CardLanes.Y = nextBoard;
        BoardList[cardLanes.X, cardLanes.Y] = null;
        TurnPlayerList[cardLanes.X, cardLanes.Y] = false;
    }

    public void Destroyer(CardLanes cardLanes)
    {
        Destroy(BoardList[cardLanes.X, cardLanes.Y]);
        BoardList[cardLanes.X, cardLanes.Y] = null;
        TurnPlayerList[cardLanes.X, cardLanes.Y] = false;
    }
}
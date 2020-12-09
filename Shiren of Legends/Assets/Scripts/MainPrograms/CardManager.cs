using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    [SerializeField] BoardManager BoardManager = null;
    [SerializeField] PlayerStatus PlayerStatus = null;
    [SerializeField] GameObject[] cardObjList = new GameObject[8];
    [SerializeField] GameObject[] monsterObjList = new GameObject[5];
    [SerializeField] Transform[] transformsHnad = new Transform[2];
    public GameObject[,] BoardList { get; private set; } = new GameObject[(int)EnumBoardLength.MaxBoardLengthX, (int)EnumBoardLength.MaxBoardLengthY];
    public bool[,] TurnPlayerList { get; private set; } = new bool[(int)EnumBoardLength.MaxBoardLengthX, (int)EnumBoardLength.MaxBoardLengthY];
    private readonly GameObject[] hand = new GameObject[2];
    private int[] deckIDs = new int[8];

    public void DeckCreator(GameObject[] deckObjs, int[] deckIDs)
    {
        cardObjList = deckObjs;
        this.deckIDs = deckIDs;
    }

    public void Draw(int cardID, int cardID1)
    {
        hand[0] = Instantiate(cardObjList[cardID], transformsHnad[0].position, Quaternion.identity);
        hand[1] = Instantiate(cardObjList[cardID1], transformsHnad[1].position, Quaternion.identity);
    }

    public void DeleteHand(int handID)
    {
        Destroy(hand[handID]);
    }

    public bool CheckCanSummon(CardLanes cardLanes)
    {
        return BoardList[cardLanes.X, cardLanes.Y] == null;
    }

    public void Summon(CardLanes cardLanes, int handID,int cardID, bool turn)
    {
        BoardList[cardLanes.X, cardLanes.Y] = hand[handID];
        BoardList[cardLanes.X, cardLanes.Y].transform.position = BoardManager.TransformList[cardLanes.X, cardLanes.Y].position;
        BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>().Create(deckIDs[cardID], turn, cardLanes);
        TurnPlayerList[cardLanes.X, cardLanes.Y] = turn;
        Skill(cardLanes);
    }

    public void Skill(CardLanes cardLanes)
    {
        var card = BoardList[cardLanes.X, cardLanes.Y].GetComponent<ISkill>();
        if (card == null)
            return;

        card.ActiveSkill(cardLanes.Y);
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
        BoardList[cardLanes.X, cardLanes.Y] =
            Instantiate(monsterObjList[ID], BoardManager.TransformList[cardLanes.X, cardLanes.Y].position, Quaternion.identity) as GameObject;
        BoardList[cardLanes.X, cardLanes.Y].GetComponent<MonsterStatus>().Create(ID, cardLanes);
    }

    public void Movement(int gap, bool turn)
    {
        for (int i = 0; i < BoardList.GetLength(1); i++)
        {
            // 連続で移動しないために探索順を分ける必要がある
            if (turn)
            {
                for (int j = 0; j < BoardList.GetLength(0); j++)
                {
                    Movement(new CardLanes { X = j, Y = i }, j + gap);
                }
            }
            else if (!turn)
            {
                for (int j = BoardList.GetLength(0) - 1; j >= 0; j--)
                {
                    Movement(new CardLanes { X = j, Y = i }, j + gap);
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

            // 自分のカードだけを動かすため
            if (turn && TurnPlayerList[cardLanes.X, cardLanes.Y] || !turn && !TurnPlayerList[cardLanes.X, cardLanes.Y])
            {
                if (card.IsStun)
                {
                    card.IsStun = false;
                    return;
                }

                PassiveSkill(cardLanes);

                if (nextBoard == (int)EnumBoardLength.MinBoard || nextBoard == (int)EnumBoardLength.MaxBoardX)
                {
                    PlayerStatus.AddDirectDamage(card.MyAD, turn,cardLanes.Y);
                    Destroyer(cardLanes);
                }
                else if (BoardList[nextBoard, cardLanes.Y] == null)
                {
                    JustMovement(cardLanes, nextBoard);
                }
                else if (BoardList[nextBoard, cardLanes.Y].GetComponent<CardStatus>())
                {
                    if (turn && TurnPlayerList[nextBoard, cardLanes.Y] || !turn && !TurnPlayerList[nextBoard, cardLanes.Y])
                        return;

                    Battle<CardStatus>(cardLanes, nextBoard, CreateEnemy<CardStatus>(nextBoard, cardLanes.Y));
                }
                else if (BoardList[nextBoard, cardLanes.Y].GetComponent<MonsterStatus>())
                {
                    BattleMonster<MonsterStatus>(cardLanes, nextBoard, CreateEnemy<MonsterStatus>(nextBoard, cardLanes.Y), turn);
                }
            }
        }
    }

    public Type CreateEnemy<Type>(int lane, int nextBoard)
    {
        return BoardList[lane, nextBoard].GetComponent<Type>();
    }

    public void Battle<Type>(CardLanes cardLanes, int nextBoard, Type type) where Type : CardStatus
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
            Destroyer(new CardLanes { X = nextBoard, Y = cardLanes.Y });
        }
    }

    public void BattleMonster<Type>(CardLanes cardLanes, int nextBoard, Type type, bool player) where Type : MonsterStatus
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
        if (BoardList[nextBoard, cardLanes.Y] != null)
            return;

        if (BoardList[cardLanes.X, cardLanes.Y] == null)
            return;

        var cardStatus = BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>();
        if (cardStatus == null)
            return;

        BoardList[cardLanes.X, cardLanes.Y].transform.position = BoardManager.TransformList[nextBoard, cardLanes.Y].position;
        BoardList[nextBoard, cardLanes.Y] = BoardList[cardLanes.X, cardLanes.Y];
        TurnPlayerList[nextBoard, cardLanes.Y] = TurnPlayerList[cardLanes.X, cardLanes.Y];
        
        BoardList[cardLanes.X, cardLanes.Y] = null;
        TurnPlayerList[cardLanes.X, cardLanes.Y] = false;
        
        cardStatus.CardLanes.X = nextBoard;
    }

    public void Destroyer(CardLanes cardLanes)
    {
        Destroy(BoardList[cardLanes.X, cardLanes.Y]);
        BoardList[cardLanes.X, cardLanes.Y] = null;
        TurnPlayerList[cardLanes.X, cardLanes.Y] = false;
    }
}
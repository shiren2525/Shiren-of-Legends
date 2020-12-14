using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] BoardManager BoardManager = null;
    [SerializeField] PlayerStatus PlayerStatus = null;
    [SerializeField] GameObject[] cardObjList = new GameObject[(int)EnumNumbers.Cards];
    [SerializeField] GameObject[] monsterObjList = new GameObject[(int)EnumNumbers.Monsters];
    [SerializeField] Transform[] transformsHnad = new Transform[(int)EnumNumbers.Hands];
    public GameObject[,] BoardList { get; private set; } = new GameObject[(int)EnumBoardLength.MaxBoardLengthX, (int)EnumBoardLength.MaxBoardLengthY];
    public CardStatus[,] CardStatuseList { get; private set; } = new CardStatus[(int)EnumBoardLength.MaxBoardLengthX, (int)EnumBoardLength.MaxBoardLengthY];
    public bool[,] TurnPlayerList { get; private set; } = new bool[(int)EnumBoardLength.MaxBoardLengthX, (int)EnumBoardLength.MaxBoardLengthY];
    private readonly GameObject[] hand = new GameObject[(int)EnumNumbers.Hands];
    private int[] deckIDs = new int[(int)EnumNumbers.Cards];

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

    public void Summon(CardLanes cardLanes, int handID, int cardID, bool turn)
    {
        BoardList[cardLanes.X, cardLanes.Y] = hand[handID];
        BoardList[cardLanes.X, cardLanes.Y].transform.position = BoardManager.TransformList[cardLanes.X, cardLanes.Y].position;
        CardStatuseList[cardLanes.X, cardLanes.Y] = BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>();
        CardStatuseList[cardLanes.X, cardLanes.Y].Create(false, deckIDs[cardID], turn, cardLanes);
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
        CardStatuseList[cardLanes.X, cardLanes.Y] = BoardList[cardLanes.X, cardLanes.Y].GetComponent<CardStatus>();
        CardStatuseList[cardLanes.X, cardLanes.Y].Create(true, ID, false, cardLanes);
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

            if (!CardStatuseList[cardLanes.X, cardLanes.Y])
                return;

            if (CardStatuseList[cardLanes.X, cardLanes.Y].IsMonsterCard)
                return;

            var card = CardStatuseList[cardLanes.X, cardLanes.Y];

            // 自分のカードだけを動かすため
            if (turn && TurnPlayerList[cardLanes.X, cardLanes.Y] || !turn && !TurnPlayerList[cardLanes.X, cardLanes.Y])
            {
                if (card.IsStun)
                {
                    card.IsStun = false;
                    return;
                }

                PassiveSkill(cardLanes);

                if (!CardStatuseList[cardLanes.X, cardLanes.Y])
                    return;

                if (nextBoard == (int)EnumBoardLength.MinBoard || nextBoard == (int)EnumBoardLength.MaxBoardX)
                {
                    PlayerStatus.AddDirectDamage(card.MyAD, turn, cardLanes.Y);
                    Destroyer(cardLanes);
                }
                else if (BoardList[nextBoard, cardLanes.Y] == null)
                {
                    JustMovement(cardLanes, nextBoard);
                }
                else if (CardStatuseList[nextBoard, cardLanes.Y].IsMonsterCard)
                {
                    Battle<CardStatus>(turn, cardLanes, nextBoard, CardStatuseList[nextBoard, cardLanes.Y]);
                }
                else if (CardStatuseList[nextBoard, cardLanes.Y])
                {
                    if (turn && TurnPlayerList[nextBoard, cardLanes.Y] || !turn && !TurnPlayerList[nextBoard, cardLanes.Y])
                        return;

                    Battle<CardStatus>(turn, cardLanes, nextBoard, CardStatuseList[nextBoard, cardLanes.Y]);
                }
            }
        }
    }

    public void Battle<Type>(bool turn, CardLanes cardLanes, int nextBoard, Type type) where Type : CardStatus
    {
        var thisCard = CardStatuseList[cardLanes.X, cardLanes.Y];
        var enemyCard = type;

        Debug.Log("<color=blue>" + thisCard.name + " VS " + enemyCard.name + "</color>");

        if (enemyCard.MyHP + enemyCard.MyShield <= thisCard.MyAD)
        {
            thisCard.AddDamage(turn, enemyCard.MyAD, (int)EnumSkillType.AutoAttack);
            enemyCard.AddDamage(turn, thisCard.MyAD, (int)EnumSkillType.AutoAttack);

            JustMovement(cardLanes, nextBoard);
        }
        else if (thisCard.MyHP + thisCard.MyShield <= enemyCard.MyAD)
        {
            enemyCard.AddDamage(turn, thisCard.MyAD, (int)EnumSkillType.AutoAttack);
            thisCard.AddDamage(turn, enemyCard.MyAD, (int)EnumSkillType.AutoAttack);
        }
        else
        {
            enemyCard.AddDamage(turn, thisCard.MyAD, (int)EnumSkillType.AutoAttack);
            thisCard.AddDamage(turn, enemyCard.MyAD, (int)EnumSkillType.AutoAttack);
        }
    }

    public void JustMovement(CardLanes cardLanes, int nextBoard)
    {
        if (BoardList[nextBoard, cardLanes.Y] != null)
            return;

        if (BoardList[cardLanes.X, cardLanes.Y] == null)
            return;

        var cardStatus = CardStatuseList[cardLanes.X, cardLanes.Y];
        if (cardStatus == null)
            return;

        BoardList[cardLanes.X, cardLanes.Y].transform.position = BoardManager.TransformList[nextBoard, cardLanes.Y].position;
        BoardList[nextBoard, cardLanes.Y] = BoardList[cardLanes.X, cardLanes.Y];
        CardStatuseList[nextBoard, cardLanes.Y] = CardStatuseList[cardLanes.X, cardLanes.Y];
        TurnPlayerList[nextBoard, cardLanes.Y] = TurnPlayerList[cardLanes.X, cardLanes.Y];

        BoardList[cardLanes.X, cardLanes.Y] = null;
        CardStatuseList[cardLanes.X, cardLanes.Y] = null;
        TurnPlayerList[cardLanes.X, cardLanes.Y] = false;

        cardStatus.CardLanes.X = nextBoard;
    }

    public void Destroyer(CardLanes cardLanes)
    {
        Destroy(BoardList[cardLanes.X, cardLanes.Y]);
        BoardList[cardLanes.X, cardLanes.Y] = null;
        CardStatuseList[cardLanes.X, cardLanes.Y] = null;
        TurnPlayerList[cardLanes.X, cardLanes.Y] = false;
    }
}
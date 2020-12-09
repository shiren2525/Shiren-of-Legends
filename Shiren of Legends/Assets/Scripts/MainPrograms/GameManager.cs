using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CardManager CardManager = null;
    [SerializeField] private SoundManager SoundManager = null;
    [SerializeField] private TextManager TextManager = null;
    [SerializeField] private TimeBarController TimeBarController = null;
    [SerializeField] private NarrationManager NarrationManager = null;
    [SerializeField] private LoadScene LoadScene = null;
    [SerializeField] private BoardManager BoardManager = null;
    [SerializeField] private GameObject Cursor = null;
    [SerializeField] Transform[] transformsHnad = new Transform[2];
    private bool turn = false;
    private int StartingLaneX = 0;
    private int handID = 0;
    private int faith = 0;
    private int cardID = 0, cardID0 = 0, cardID1 = 0;
    private int turnNum = 0;
    private bool isCPU = false;
    private int inputCount = 0; // 制御変数

    public bool IsTimeLimit { get; set; }

    private void NextFaith()
    {
        faith++;
        if (faith > (int)EnumFaith.TurnChange)
            faith = (int)EnumFaith.Startup;
    }

    private void FaithUpdate()
    {
        switch (faith)
        {
            case (int)EnumFaith.Startup:
                Startup();
                break;
            case (int)EnumFaith.Draw:
                Draw();
                break;
            case (int)EnumFaith.SelectHand:
                SelectHand();
                break;
            case (int)EnumFaith.Summon:
                SelectLane();
                break;
            case (int)EnumFaith.Skill:
                Skill();
                break;
            case (int)EnumFaith.Battle:
                Battle();
                break;
            case (int)EnumFaith.TurnChange:
                TurnChange();
                break;
            default:
                break;
        }
    }

    private void Startup()
    {
        TextManager.SetTurnNum(++turnNum);
        TextManager.SetPanel(turn);
        SoundManager.PlaySound((int)EnumAudioClips.NextTurn);
        NarrationManager.SetSerif((int)EnumNarrationTexts.NextPlayersTurn);
        Debug.Log("<color=red> --------------------------- </color>");

        SummonMonster((int)EnumBoardLength.MaxBoardY);

        NextFaith();
    }

    private void SummonMonster(int laneY)
    {
        if (turnNum % 5 == 0)
        {
            if (laneY < 0)
                return;

            var cardLanes = new CardLanes { X = (int)EnumBoardLength.MaxBoardX / 2, Y = laneY };
            var canSummon = CardManager.CheckCanSummon(cardLanes);
            if (!canSummon)
            {
                SummonMonster(--laneY);
            }
            else if (canSummon)
            {
                CardManager.SummonMonster(Random.Range(0, 4), cardLanes);
            }
            turnNum = 0;
        }
    }

    private void Draw()
    {
        cardID0 = Random.Range(0, (int)EnumNumbers.Cards);
        cardID1 = Random.Range(0, (int)EnumNumbers.Cards);
        CardManager.Draw(cardID0, cardID1);
        TimeBarController.StartCoroutine();
        NarrationManager.SetSerif((int)EnumNarrationTexts.SelectHand);
        NextFaith();
    }

    private void SelectHand()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            cardID = cardID0;
            handID = 0;
            Cursor.transform.position = transformsHnad[handID].transform.position;
            inputCount++;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            cardID = cardID1;
            handID = 1;
            Cursor.transform.position = transformsHnad[handID].transform.position;
            inputCount++;
        }
        else if (Input.GetKeyDown(KeyCode.E) || IsTimeLimit)
        {
            if (inputCount == 0)
            {
                cardID = cardID0;
                handID = 0;
            }
            IsTimeLimit = false;
            Cursor.transform.position = BoardManager.TransformList[StartingLaneX, laneY].transform.position;
            End();
        }
        else if (isCPU && turn)
        {
            cardID = cardID0;
            handID = 0;
            End();
        }

        void End()
        {
            if (handID == 0)
            {
                CardManager.DeleteHand(1);
            }
            else if (handID == 1)
            {
                CardManager.DeleteHand(0);
            }
            inputCount = 0;
            SoundManager.PlaySound((int)EnumAudioClips.Draw);
            NarrationManager.SetSerif((int)EnumNarrationTexts.SelectLane);
            TimeBarController.StopCoroutine();
            TimeBarController.StartCoroutine();
            NextFaith();
        }
    }

    int laneY = 0;
    private void SelectLane()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ++laneY;
            if (laneY == 4)
                laneY = 0;

            Cursor.transform.position = BoardManager.TransformList[StartingLaneX, laneY].transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            --laneY;
            if (laneY == -1)
                laneY = 3;

            Cursor.transform.position = BoardManager.TransformList[StartingLaneX, laneY].transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.E) || IsTimeLimit)
        {
            IsTimeLimit = false;
            End();
        }
        else if(isCPU && turn)
        {
            laneY = Random.Range(0, 4);
            End();
        }

        void End()
        {
            var canSummon = CardManager.CheckCanSummon(new CardLanes { X = StartingLaneX, Y = laneY });
            if (!canSummon)
            {
                faith = (int)EnumFaith.Summon;
            }
            else if (canSummon)
            {
                Summon(laneY);
                TimeBarController.StopCoroutine();
                NextFaith();
            }
        }
    }

    private void Summon(int laneY)
    {
        if (!turn)
        {
            CardManager.Summon(new CardLanes { X = (int)EnumBoardLength.MinBoard, Y = laneY }, handID, cardID, turn);
        }
        else if (turn)
        {
            CardManager.Summon(new CardLanes { X = (int)EnumBoardLength.MaxBoardX, Y = laneY }, handID, cardID, turn);
        }
        SoundManager.PlaySound((int)EnumAudioClips.SpecialSummon);
        NarrationManager.SetSerif((int)EnumNarrationTexts.Skill);
    }

    private void Skill()
    {
        if (Input.GetKeyDown(KeyCode.E))
            NextFaith();
    }

    private void Battle()
    {
        if (!turn)
        {
            CardManager.Movement(1, turn);
            NextFaith();
        }
        else if (turn)
        {
            CardManager.Movement(-1, turn);
            NextFaith();
        }
    }

    private void TurnChange()
    {
        turn = !turn;
        if (turn)
        {
            StartingLaneX = (int)EnumBoardLength.MaxBoardX;
        }
        else if (!turn)
        {
            StartingLaneX = (int)EnumBoardLength.MinBoard;
        }

        handID = 0;
        laneY = 0;
        Cursor.transform.position = transformsHnad[handID].transform.position;

        NextFaith();
    }

    private void Start()
    {
        LoadScene = new LoadScene();
        Application.targetFrameRate = 30;
    }

    private void Update()
    {
        FaithUpdate();

        if (Input.GetKeyDown(KeyCode.T))
        {
            isCPU = !isCPU;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadScene.ResetGames();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadScene.ExitGames();
        }
    }
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CardManager CardManager = null;
    [SerializeField] private SoundManager SoundManager = null;
    [SerializeField] private TextManager TextManager = null;
    [SerializeField] private TimeBarController TimeBarController = null;
    [SerializeField] private NarrationManager NarrationManager = null;
    [SerializeField] private LoadScene LoadScene = null;
    [SerializeField] private InfoPanel InfoPanel = null;
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
    private int laneY = 0;

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
        Debug.Log(" ----------------------------------------------- ");
        TextManager.SetTurnNum(++turnNum);
        TextManager.SetPanel(turn);
        SoundManager.PlaySound((int)EnumAudioClips.NextTurn);
        NarrationManager.SetSerif((int)EnumNarrationTexts.NextPlayersTurn);
        SummonMonster(randomNum);
        NextFaith();
    }

    private void SummonMonster(int randomNum)
    {
        if (turnNum % 5 == 0)
        {
            var laneY = randomNum < (int)EnumBoardLength.MaxBoardLengthY ? randomNum : (int)EnumBoardLength.MaxBoardY;
            if (laneY < (int)EnumBoardLength.MinBoard)
                return;

            var cardLanes = new CardLanes { X = (int)EnumBoardLength.MaxBoardX / 2, Y = laneY };
            var canSummon = CardManager.CheckCanSummon(cardLanes);
            if (!canSummon)
            {
                SummonMonster(--laneY);
            }
            else if (canSummon)
            {
                CardManager.SummonMonster(Random.Range(0, (int)EnumNumbers.Monsters - 1), cardLanes);
            }
            turnNum = 0;
        }
    }

    private int randomNum;
    private void Draw()
    {
        randomNum = Random.Range(0, (int)EnumNumbers.Cards);
        cardID0 = randomNum;
        cardID1 = randomNum + 1 == 8 ? 0 : randomNum + 1;
        CardManager.Draw(cardID0, cardID1);
        TimeBarController.StartCoroutine();
        NarrationManager.SetSerif((int)EnumNarrationTexts.SelectHand);
        NextFaith();
    }

    private bool isInput = false;
    private void SelectHand()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isInput = true;
            cardID = cardID0;
            handID = 0;
            Cursor.transform.position = transformsHnad[handID].transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            isInput = true;
            cardID = cardID1;
            handID = 1;
            Cursor.transform.position = transformsHnad[handID].transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.E) || IsTimeLimit)
        {
            if (!isInput)
            {
                cardID = cardID0;
                handID = 0;
            }
            isInput = false;
            IsTimeLimit = false;
            Cursor.transform.position = BoardManager.TransformList[StartingLaneX, laneY].transform.position;
            End();
        }
        else if (isCPU)
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
            SoundManager.PlaySound((int)EnumAudioClips.Draw);
            NarrationManager.SetSerif((int)EnumNarrationTexts.SelectLane);
            TimeBarController.StopCoroutine();
            TimeBarController.StartCoroutine();
            NextFaith();
        }
    }

    private void SelectLane()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ++laneY;
            if (laneY == (int)EnumBoardLength.MaxBoardLengthY)
                laneY = (int)EnumBoardLength.MinBoard;

            Cursor.transform.position = BoardManager.TransformList[StartingLaneX, laneY].transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            --laneY;
            if (laneY == (int)EnumBoardLength.MinBoard - 1)
                laneY = (int)EnumBoardLength.MaxBoardLengthY - 1;

            Cursor.transform.position = BoardManager.TransformList[StartingLaneX, laneY].transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.E) || IsTimeLimit)
        {
            IsTimeLimit = false;
            End();
        }
        else if (isCPU)
        {
            laneY = Random.Range(0, (int)EnumBoardLength.MaxBoardLengthY);
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
                TimeBarController.StartCoroutine();
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
        NarrationManager.SetSerif((int)EnumNarrationTexts.Battle);
    }

    private void Skill()
    {
        if (Input.GetKeyDown(KeyCode.E) || IsTimeLimit)
        {
            IsTimeLimit = false;
            TimeBarController.StopCoroutine();
            NextFaith();
        }
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            InfoPanel.PanelChange();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            isCPU = !isCPU;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            LoadScene.ResetGames();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadScene.ExitGames();
        }
    }
}
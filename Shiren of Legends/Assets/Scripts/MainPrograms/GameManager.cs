using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CardManager CardManager = null;
    [SerializeField] private SoundManager SoundManager = null;
    [SerializeField] private TextManager TextManager = null;
    [SerializeField] private TimeBarController TimeBarController = null;
    [SerializeField] private NarrationManager NarrationManager = null;
    private bool turn = false;
    private int handID = 0;
    private int faith = 0;
    private int hand = 0, hand1 = 0;
    private int turnNum = 0;

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
                CardManager.SummonMonster(Random.Range(0, 5), cardLanes);
            }
        }
    }

    private void Draw()
    {
        hand = Random.Range(0, (int)EnumNumbers.Cards);
        hand1 = Random.Range(0, (int)EnumNumbers.Cards);        
        CardManager.Draw(hand, hand1);
        TimeBarController.StartCoroutine();
        NarrationManager.SetSerif((int)EnumNarrationTexts.SelectHand);
        NextFaith();
    }

    private void SelectHand()
    {
        if (Input.GetKeyDown(KeyCode.Q) || IsTimeLimit)
        {
            IsTimeLimit = false;
            handID = hand;
            End();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            handID = hand1;
            End();
        }
        void End()
        {
            CardManager.DeleteHand();
            SoundManager.PlaySound((int)EnumAudioClips.Draw);
            NarrationManager.SetSerif((int)EnumNarrationTexts.SelectLane);
            TimeBarController.StopCoroutine();
            TimeBarController.StartCoroutine();
            NextFaith();
        }
    }

    private void SelectLane()
    {
        int laneY;
        if (Input.GetKeyDown(KeyCode.Q) || IsTimeLimit)
        {
            IsTimeLimit = false;
            laneY = 0;
            End();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            laneY = 1;
            End();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            laneY = 2;
            End();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            laneY = 3;
            End();
        }

        void End()
        {
            var laneX = 0;
            if (turn)
            {
                laneX = (int)EnumBoardLength.MaxBoardX;
            }
            else if (!turn)
            {
                laneX = (int)EnumBoardLength.MinBoard;
            }

            var canSummon = CardManager.CheckCanSummon(new CardLanes { X = laneX, Y = laneY });
            if (!canSummon)
            {
                faith = (int)EnumFaith.Summon;
            }
            else if (canSummon)
            {
                Summon(laneY, handID);
                TimeBarController.StopCoroutine();
                NextFaith();
            }
        }
    }

    private void Summon(int laneY, int cardID)
    {
        if (!turn)
        {
            CardManager.Summon(new CardLanes { X = (int)EnumBoardLength.MinBoard, Y = laneY }, cardID, turn);
        }
        else if (turn)
        {
            CardManager.Summon(new CardLanes { X = (int)EnumBoardLength.MaxBoardX, Y = laneY }, cardID, turn);
        }
        SoundManager.PlaySound((int)EnumAudioClips.SpecialSummon);
        NarrationManager.SetSerif((int)EnumNarrationTexts.Skill);
    }

    private void Skill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
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
        NextFaith();
    }

    private void Update()
    {
        FaithUpdate();
    }
}
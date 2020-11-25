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

        NarrationManager.SetSerif(faith);
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
        SoundManager.PlaySound(1);

        if (turnNum % 5 == 0)
        {            
            CardManager.SummonMonster(Random.Range(0, 5), new CardLanes { X = 3, Y = 3 });
        }
        NextFaith();
    }

    private void Draw()
    {
        hand = Random.Range(0, 8);
        hand1 = Random.Range(0, 8);
        CardManager.Draw(hand, hand1);
        TimeBarController.StartCoroutine();
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
            SoundManager.PlaySound(0);
            TimeBarController.StopCoroutine();
            TimeBarController.StartCoroutine();
            NextFaith();
        }
    }

    private void SelectLane()
    {
        int val;
        if (Input.GetKeyDown(KeyCode.Q) || IsTimeLimit)
        {
            IsTimeLimit = false;
            val = 0;
            End();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            val = 1;
            End();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            val = 2;
            End();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            val = 3;
            End();
        }

        void End()
        {
            var canSummon = CardManager.CheckCanSummon(val, turn);
            if (!canSummon)
            {
                faith = (int)EnumFaith.Summon;
            }
            else if (canSummon)
            {
                Summon(val, handID);
                TimeBarController.StopCoroutine();
                NextFaith();
            }
        }
    }

    private void Summon(int i, int cardID)
    {
        if (!turn)
        {
            CardManager.Summon(new CardLanes { X = i, Y = 0 }, cardID, turn);
        }
        else if (turn)
        { 
            CardManager.Summon(new CardLanes { X = i, Y = 6 }, cardID, turn);
        }
        SoundManager.PlaySound(2);
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
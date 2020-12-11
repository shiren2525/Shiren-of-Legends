using UnityEngine;

public class Card02Shyvana : MonoBehaviour, ISkill
{
    [SerializeField] private CardStatus CardStatus = null;

    public void ActiveSkill(int myLane)
    {
        var player = CardStatus.Player;

        var buffManager = GameObject.FindWithTag(nameof(BuffManager)).GetComponent<BuffManager>();

        if (player)
        {
            Shyvana(buffManager.RedBuffList.Count);
        }
        else if (!player)
        {
            Shyvana(buffManager.BlueBuffList.Count);
        }
        
        void Shyvana(int buffValue)
        {
            CardStatus.MyMaxHP += buffValue;
            CardStatus.MyHP += buffValue;
            CardStatus.MyAD += buffValue;
            
            CardStatus.SetText();
        }
    }
}
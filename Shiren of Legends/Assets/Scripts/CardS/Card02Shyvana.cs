using UnityEngine;

public class Card02Shyvana : MonoBehaviour, ISkill
{
    public void ActiveSkill(int myLane)
    {
        var cardStatus = this.gameObject.GetComponent<CardStatus>();
        var player = cardStatus.Player;

        var buffManager = GameObject.FindWithTag(nameof(BuffManager)).GetComponent<BuffManager>();

        if (player)
        {
            var buffValue = buffManager.RedBuffList.Count;
            Shyvana(buffValue);
        }
        else if (!player)
        {
            var buffValue = buffManager.BlueBuffList.Count;
            Shyvana(buffValue);
        }
        
        void Shyvana(int buffValue)
        {
            cardStatus.MyMaxHP += buffValue;
            cardStatus.MyHP += buffValue;
            cardStatus.MyAD += buffValue;
            
            cardStatus.SetText();
        }
    }
}
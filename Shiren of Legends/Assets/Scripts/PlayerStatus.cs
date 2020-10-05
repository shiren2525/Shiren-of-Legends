using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] TextManager TextManager = null;

    private int player0HP = 12;
    private int player1HP = 12;

    private void Start()
    {
        TextManager.SetText(player1HP, true);
        TextManager.SetText(player0HP, false);
    }

    internal void AddDirectDamage(int damage, bool player)
    {
        if (player)
        {
            player0HP -= damage;
            TextManager.SetText(player0HP, player);
        }
        else if (!player)
        {
            player1HP -= damage;
            TextManager.SetText(player1HP, player);
        }
    }
}

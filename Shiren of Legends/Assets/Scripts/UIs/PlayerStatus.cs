using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] TextManager TextManager = null;

    private int player0HP = 12;
    private int player1HP = 12;

    private int[] tower0HP = new int[4];
    private int[] tower1HP = new int[4];

    private void Start()
    {
        TextManager.SetText(player1HP, true);
        TextManager.SetText(player0HP, false);

        InitTower();
    }

    void InitTower()
    {
        for (int i = 0; i < 4; i++)
        {
            tower0HP[i] = 8;
            tower1HP[i] = 8;
        }
    }

    internal void AddDirectDamage(int damage, bool player, int laneY)
    {
        if (player)
        {
            if (tower0HP[laneY] > 0)
            {
                tower0HP[laneY] -= damage;
                TextManager.SetTowerHPText(tower0HP[laneY], player, laneY);
            }
            else
            {
                player0HP -= damage;
                TextManager.SetText(player0HP, player);
                if (player0HP <= 0)
                {
                    var loadScene = new LoadScene();
                    loadScene.ResetGames();
                }
            }
        }
        else if (!player)
        {
            if (tower1HP[laneY] > 0)
            {
                tower1HP[laneY] -= damage;
                TextManager.SetTowerHPText(tower1HP[laneY], player, laneY);
            }
            else
            {
                player1HP -= damage;
                TextManager.SetText(player1HP, player);
                if (player1HP <= 0)
                {
                    var loadScene = new LoadScene();
                    loadScene.ResetGames();
                }
            }
        }
    }
}
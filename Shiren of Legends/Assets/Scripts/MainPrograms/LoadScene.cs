public class LoadScene
{
    public void ResetGames()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void ExitGames()
    {
        UnityEngine.Application.Quit();
    }
}
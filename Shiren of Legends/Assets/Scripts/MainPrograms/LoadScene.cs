public class LoadScene
{
    public void ResetGames()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ExitGames()
    {
        UnityEngine.Application.Quit();
    }
}
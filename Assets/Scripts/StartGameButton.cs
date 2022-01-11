using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    public AsyncOperation asyncLoadLevel;

    public void OnButtonPressed()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
    }
}
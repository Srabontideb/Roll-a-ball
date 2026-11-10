using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // PLAY button on the main menu: starts the game level.
    public void PlayGame()
    {
        SceneManager.LoadScene("Minigame");
    }

    // QUIT button: closes the game (or stops Play mode inside Unity).
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

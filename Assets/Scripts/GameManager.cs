using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // The panel with Resume / Restart / Main Menu buttons.
    public GameObject pausePanel;

    // The small pause button shown during play.
    public GameObject pauseButton;

    private bool isPaused = false;

    // Becomes true when the player wins or loses. Pausing is then disabled.
    private bool gameEnded = false;

    void Start()
    {
        // Make sure the game runs at normal speed when the scene starts.
        Time.timeScale = 1f;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true);
    }

    void Update()
    {
        // No pausing after the game has ended.
        if (gameEnded)
        {
            return;
        }

        // Press SPACE to pause or resume.
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // PAUSE: freezes the game and shows the pause panel.
    public void PauseGame()
    {
        if (gameEnded) return;

        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null) pausePanel.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(false);

        ClearSelectedButton();
    }

    // RESUME: unfreezes the game and hides the panel.
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (pauseButton != null && !gameEnded) pauseButton.SetActive(true);

        ClearSelectedButton();
    }

    // RESTART / PLAY AGAIN: reloads the current level.
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // MAIN MENU (stop): leaves the level and goes back to the menu.
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Called by PlayerController when the player wins or loses.
    public void EndGame()
    {
        gameEnded = true;

        if (pauseButton != null) pauseButton.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    // After clicking a button, Unity keeps it "selected", and pressing Space
    // would click it again. This un-selects it so Space only pauses/resumes.
    void ClearSelectedButton()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
///
/// All the logic used in the game menu.
/// If this gets to be too much break apart into smaller components.
///

using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private SceneReference mainMenuScene;

    [SerializeField] private Button pauseButton;
    private Text pauseButtonText;

    private void Awake()
    {
        pauseButtonText = pauseButton.GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Loads the game scene.
    /// </summary>
    public void LoadMainMenu()
    {
        if (mainMenuScene == null)
        {
            Debug.LogError("No menu scene variable set!", this);
            return;
        }
        int index = mainMenuScene.Value.SceneIndex;
        SceneManager.LoadScene(index);
    }

    public void OnTogglePause()
    {
        var state = GameManager.Instance.GetGameState;
        switch (state)
        {
            case GameState.Running:
                pauseButtonText.text = "Pause Game";
                break;
            case GameState.Paused:
                pauseButtonText.text = "Resume Game";
                break;
            case GameState.End:
                pauseButton.interactable = false;
                pauseButtonText.text = "Game Over";
                break;
        }
    }
}

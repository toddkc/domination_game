///
/// All the logic used in the main menu.
/// If this gets to be too much break apart into smaller components.
///

using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private SceneReference gameScene;

    /// <summary>
    /// Closes the application.
    /// </summary>
    public void ExitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    /// <summary>
    /// Loads the game scene.
    /// </summary>
    public void StartNewGame()
    {
        if (gameScene == null)
        {
            Debug.LogError("No game scene variable set!", this);
            return;
        }
        int index = gameScene.Value.SceneIndex;
        SceneManager.LoadScene(index);
    }
}

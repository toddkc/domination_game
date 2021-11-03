///
/// All the logic used in the game menu.
/// If this gets to be too much break apart into smaller components.
///

using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private SceneReference mainMenuScene;

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
}

using UnityEngine;
using UnityEngine.SceneManagement; // Required for Scene Management

public class MenuScript : MonoBehaviour
{
    // This function will be called when the "Start Game" button is clicked
    public void Play()
    {
        // Load the game scene. Make sure the scene name matches the name of your game scene.
        SceneManager.LoadScene("Game"); // Replace "GameScene" with the actual name of your game scene
    }

    // Optional: You can also add an exit function
    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
        Debug.Log("Game is exiting"); // This will work in a built game, but in the editor, it won't quit.
    }
}

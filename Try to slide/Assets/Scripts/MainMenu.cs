using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GUISkin mainMenuSkin = null;
    private Rect newGameButton;
    private Rect continueButton;
    private Rect quitButton;
    private Rect gameNameLabel;


    private void Start()
    {
        newGameButton = new Rect(Screen.width / 2 - 45, Screen.height / 2 - 40, 90, 40);  // Play
        continueButton = new Rect(Screen.width / 2 - 45, Screen.height / 2 - 90, 90, 40);  // Continue
        quitButton = new Rect(Screen.width / 2 - 45, Screen.height / 2 + 10, 90, 40);  // Quit
        gameNameLabel = new Rect(Screen.width / 2 - 80, 10, 160, 150);  // Game Name
    }


    private void OnGUI()
    {
        GUI.skin = mainMenuSkin;
        GUI.Label(gameNameLabel, "Try to slide");
        if (PlayerPrefs.GetInt("Unlocked Level") > 0)
        {
            if (GUI.Button(continueButton, "Continue"))
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("Unlocked Level"));
            }
        }
        if (GUI.Button(newGameButton, "New Game"))
        {
            GameManager.NewGame();
        }
        if (GUI.Button(quitButton, "Quit"))
        {
            Debug.Log("Quit");
        }
    }
}

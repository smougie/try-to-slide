using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GUISkin mainMenuSkin = null;
    private Rect newGameButton;
    private Rect continueButton;
    private Rect quitButton;
    private Rect gameNameLabel;
    private Rect newGameConfirmRect;
    private Rect newGameWarningRect;
    private Rect yesButton;
    private Rect noButton;

    private float screenWidth;
    private float screenHeight;

    private bool newGameConfirm;

    private void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        newGameButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 - 40, 90, 40);  // Play
        continueButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 - 90, 90, 40);  // Continue
        quitButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 + 10, 90, 40);  // Quit
        gameNameLabel = new Rect(screenWidth / 2 - 80, screenHeight * .1f , 160, 150);  // Game Name
        newGameConfirmRect = new Rect(screenWidth / 2 - (screenWidth / 2) / 2, screenHeight / 2 - (screenHeight / 2) / 2, screenWidth * .5f, screenHeight * .5f);  // New Game confirmation rectangle for Box
        newGameWarningRect = new Rect();
        yesButton = new Rect();
        noButton = new Rect();
        newGameConfirm = false;
    }


    private void OnGUI()
    {
        GUI.skin = mainMenuSkin;
        GUI.Label(gameNameLabel, "Try to slide");

        if (newGameConfirm)
        {
            GUI.Box(new Rect(newGameConfirmRect), "japierdole");
        }

        if (PlayerPrefs.GetInt("Unlocked Level") > 0)
        {
            if (GUI.Button(continueButton, "Continue"))
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("Unlocked Level"));
            }
        }

        if (GUI.Button(newGameButton, "New Game"))
        {
            newGameConfirm = true;
            Debug.Log("japierdole");
        }

        if (GUI.Button(quitButton, "Quit"))
        {
            Debug.Log("Quit");
        }
    }
}

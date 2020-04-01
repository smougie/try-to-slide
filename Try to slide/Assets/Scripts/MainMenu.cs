using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GUISkin mainMenuSkin = null;
    private Rect newGameButton;
    private Rect continueButton;
    private Rect quitButton;
    private Rect gameNameLabel;
    private Rect newGameConfirmBoxRect;
    private Rect newGameWarningRect;
    private Rect yesButton;
    private Rect noButton;

    private float screenWidth;
    private float screenHeight;

    private float buttonWidth;
    private float buttonHeight;
    private float buttonSpacing;

    private bool guiEnabled;
    private bool newGameConfirm;

    private void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        buttonWidth = 90;
        buttonHeight = 40;
        buttonSpacing = 10;

        newGameButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 - buttonHeight, buttonWidth, buttonHeight);  // Play
        continueButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 - (2 * buttonHeight + buttonSpacing), buttonWidth, buttonHeight);  // Continue
        quitButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 + buttonSpacing, buttonWidth, buttonHeight);  // Quit
        gameNameLabel = new Rect(screenWidth / 2 - 80, screenHeight * .1f , 160, 150);  // Game Name
        newGameConfirmBoxRect = new Rect(screenWidth / 2 - (screenWidth * .4f) / 2, screenHeight / 2 - (screenHeight * .4f) / 2, 
            screenWidth * .4f, screenHeight * .4f);  // New Game confirmation rectangle for Box
        newGameWarningRect = new Rect();
        yesButton = new Rect(newGameConfirmBoxRect.x + newGameConfirmBoxRect.width * .8f, newGameConfirmBoxRect.y + newGameConfirmBoxRect.height * .8f,
            buttonWidth, buttonHeight);
        noButton = new Rect(newGameConfirmBoxRect.x + (newGameConfirmBoxRect.width * .2f) - buttonWidth, yesButton.y, buttonWidth, buttonHeight);
        guiEnabled = true;
        newGameConfirm = false;
}


    private void OnGUI()
    {
        GUI.skin = mainMenuSkin;
        GUI.Label(gameNameLabel, "Try to slide");

        if (newGameConfirm)
        {
            GUI.Box(new Rect(newGameConfirmBoxRect), "japierdole");
            if (GUI.Button(yesButton, "YES"))
            {
                guiEnabled = true;
                Debug.Log("yes");
            }
            if (GUI.Button(noButton, "NO"))
            {
                guiEnabled = true;
                Debug.Log("no");
            }
        }

        GUI.enabled = guiEnabled;
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
            guiEnabled = false;
        }

        if (GUI.Button(quitButton, "Quit"))
        {
            Debug.Log("Quit");
        }

    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Variables
    // GUI/rectangle section for buttons, boxes and labels
    [SerializeField] private GUISkin mainMenuSkin = null;
    private Rect newGameButton;
    private Rect continueButton;
    private Rect quitButton;
    private Rect gameNameLabel;
    private Rect newGameConfirmBoxRect;
    private Rect newGameWarningRect;
    private Rect yesButton;
    private Rect noButton;

    // Size section for buttons, boxes and labels
    private float screenWidth;
    private float screenHeight;

    private float buttonWidth;
    private float buttonHeight;
    private float buttonSpacing;

    // String section
    private string warningMessage;

    // Flag section
    private bool newGameConfirm;
    #endregion

    private void Start()
    {
        #region Initializing necessary variables
        // Initializing sizes
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        buttonWidth = 90;
        buttonHeight = 40;
        buttonSpacing = 10;

        // Labels
        gameNameLabel = new Rect(screenWidth / 2 - 80, screenHeight * .1f , 160, 150);  // Game Name

        // Rectangles for boxes
        newGameConfirmBoxRect = new Rect(screenWidth / 2 - (screenWidth * .4f) / 2, screenHeight / 2 - (screenHeight * .4f) / 2, 
            screenWidth * .4f, screenHeight * .4f);  // Rect for new game confirmation box
        newGameWarningRect = new Rect(newGameConfirmBoxRect.x + newGameConfirmBoxRect.x * .2f, newGameConfirmBoxRect.y + newGameConfirmBoxRect.y * .4f, 
            newGameConfirmBoxRect.x * .9f, newGameConfirmBoxRect.y * .9f);  // Rect for new game warning message label

        // Rectangles for buttons
        newGameButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 - buttonHeight, buttonWidth, buttonHeight);  // Play
        continueButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 - (2 * buttonHeight + buttonSpacing), buttonWidth, buttonHeight);  // Continue
        quitButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 + buttonSpacing, buttonWidth, buttonHeight);  // Quit
        yesButton = new Rect(newGameConfirmBoxRect.x + newGameConfirmBoxRect.width * .8f, newGameConfirmBoxRect.y + newGameConfirmBoxRect.height * .8f,
            buttonWidth, buttonHeight);  // Yes button in newGameConfirmBox
        noButton = new Rect(newGameConfirmBoxRect.x + (newGameConfirmBoxRect.width * .2f) - buttonWidth, 
            yesButton.y, buttonWidth, buttonHeight);  //button in newGameConfirmBox

        // Flags
        newGameConfirm = false;

        // Strings for boxes
        warningMessage = "You're trying to start new game, your current game progress will be reset. Are you sure?";  // Warning message for box
        #endregion
    }


    private void OnGUI()
    {
        GUI.skin = mainMenuSkin;  // skin prepared for main menu

        #region Main Menu
        GUI.Label(gameNameLabel, "Try to slide");  // game label in center of main menu
        // If newGameConfirm flag is not raised than in main menu scene player can see 2 buttons or 3 buttons if he made some progress before
        if (!newGameConfirm)
        {
            // if player made some progress, Continue button will be available and he will be able to continue from highest level achieved
            if (PlayerPrefs.GetInt("Unlocked Level") > 0)
            {
                if (GUI.Button(continueButton, "Continue"))
                {
                    SceneManager.LoadScene(PlayerPrefs.GetInt("Unlocked Level"));
                }
            }

            // if new game button will be pressed while there is no progress in PlayerPrefs, new game start immediately
            // if there is some progress in PLayerPrefs, than newGameConfirm flag is rising and New Game warning windows shows up
            // if player press yes button new game starts, if he press no main menu will show once again
            if (GUI.Button(newGameButton, "New Game"))
            {
                if (PlayerPrefs.GetInt("Unlocked Level") > 0)
                {
                    newGameConfirm = true;  // rising flag to pop up warning message
                }
                else
                {
                    GameManager.NewGame();
                }
            }

            // quit button prepared for application quit
            if (GUI.Button(quitButton, "Quit"))
            {
                Debug.Log("Quit");
                Application.Quit();
            }

        }
        #endregion

        #region New Game warning window
        // If newGameConfirm flag is raised, main menu buttons are disabled and player will see only Warning Window
        // if player press yes button new game starts, if he press no main menu will show once again
        if (newGameConfirm)
        {
            GUI.Box(new Rect(newGameConfirmBoxRect), "WARNING");
            GUI.Label(newGameWarningRect, warningMessage, mainMenuSkin.GetStyle("Warning Message"));
            if (GUI.Button(yesButton, "Yes"))
            {
                GameManager.NewGame();
            }
            if (GUI.Button(noButton, "No"))
            {
                newGameConfirm = false;  // lowering flag, main menu show up once again
            }
        }
        #endregion
    }
}

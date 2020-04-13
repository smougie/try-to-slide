﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Variables

    #region GUI/rectangle section for buttons, boxes and labels

    [SerializeField] private GUISkin mainMenuSkin = null;
    private Rect newGameButton;
    private Rect continueButton;
    private Rect quitButton;
    private Rect gameNameLabel;
    private Rect newGameConfirmBoxRect;
    private Rect newGameWarningRect;
    private Rect yesButton;
    private Rect noButton;
    private Rect playerNameBoxRect;
    private Rect playerNamePromptRect;
    private Rect playerNameTextFieldRect;
    private Rect playerNameWarningRect;
    private Rect confirmButton;
    private Rect backButton;
    private Rect playerExistBoxRect;
    private Rect playerExistWarningRect;

    #endregion

    #region  Size section for buttons, boxes and labels

    private float screenWidth;
    private float screenHeight;
    private float buttonWidth;
    private float buttonHeight;
    private float buttonSpacing;

    #endregion

    #region String section

    private string warningMessage;
    private string typedName;
    private string playerNamePrompt;
    private string playerNameWarning;
    private string playerExistsWarning;

    #endregion


    #region Flag section

    private bool mainMenuWindow;
    private bool newGameConfirm;
    private bool playerNameConfirm;
    private bool emptyPlayerName;
    private bool playerExists;

    #endregion
    
    #endregion

    private void Start()
    {
        #region Initializing necessary variables

        GameManager.numberOfLevels = SceneManager.sceneCountInBuildSettings - 2;  // variable storing number of levels

        // Initializing sizes
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        buttonWidth = 90;
        buttonHeight = 40;
        buttonSpacing = 10;

        // Labels
        gameNameLabel = new Rect(screenWidth / 2 - 75, screenHeight * .1f , 160, 150);  // Game Name

        // Rectangles for boxes
        newGameConfirmBoxRect = new Rect(screenWidth / 2 - (screenWidth * .4f) / 2, screenHeight / 2 - (screenHeight * .4f) / 2, 
            screenWidth * .4f, screenHeight * .4f);  // Rect for new game confirmation box
        newGameWarningRect = new Rect(newGameConfirmBoxRect.x + newGameConfirmBoxRect.x * .2f, newGameConfirmBoxRect.y + newGameConfirmBoxRect.y * .4f, 
            newGameConfirmBoxRect.x * .9f, newGameConfirmBoxRect.y * .9f);  // Rect for new game warning message label
        playerNameBoxRect = newGameConfirmBoxRect;
        playerNamePromptRect = new Rect(playerNameBoxRect.x + playerNameBoxRect.x * .25f, playerNameBoxRect.y + playerNameBoxRect.y * .1f,
            playerNameBoxRect.x * .7f, playerNameBoxRect.y * .1f);
        playerNameTextFieldRect = new Rect(playerNameBoxRect.x + playerNameBoxRect.x * .5f, playerNameBoxRect.y + playerNameBoxRect.y * .5f,
            playerNameBoxRect.x * .4f, playerNameBoxRect.y * .4f);
        playerNameWarningRect = new Rect(playerNameBoxRect.x + playerNameBoxRect.x * .40f, playerNameBoxRect.y + playerNameBoxRect.y * .9f,
            playerNameBoxRect.x * .7f, playerNameBoxRect.y * .1f);
        playerExistBoxRect = newGameConfirmBoxRect;
        playerExistWarningRect = newGameWarningRect;


        // Rectangles for buttons
        newGameButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 - buttonHeight, buttonWidth, buttonHeight);  // Play
        continueButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 - (2 * buttonHeight + buttonSpacing), buttonWidth, buttonHeight);  // Continue
        quitButton = new Rect(screenWidth / 2 - 45, screenHeight / 2 + buttonSpacing, buttonWidth, buttonHeight);  // Quit
        yesButton = new Rect(newGameConfirmBoxRect.x + newGameConfirmBoxRect.width * .8f, newGameConfirmBoxRect.y + newGameConfirmBoxRect.height * .8f,
            buttonWidth, buttonHeight);  // Yes button in newGameConfirmBox
        noButton = new Rect(newGameConfirmBoxRect.x + (newGameConfirmBoxRect.width * .2f) - buttonWidth, 
            yesButton.y, buttonWidth, buttonHeight);  //button in newGameConfirmBox
        confirmButton = yesButton;
        backButton = noButton;

        // Flags
        mainMenuWindow = true;
        newGameConfirm = false;
        playerNameConfirm = false;
        emptyPlayerName = false;
        playerExists = false;

        // Strings for boxes
        warningMessage = "You're trying to start new game, your current game progress will be reset. Are you sure?";  // Warning message for box
        typedName = "";
        playerNamePrompt = "Enter player name (maximum 10 characters).";
        playerNameWarning = "You must enter player name!";
        playerExistsWarning = "This name is already in score base, would you like to start game and overwrite scores?";
        
        #endregion
    }
    
    // Main menu GUI section
    private void OnGUI()
    {
        GUI.skin = mainMenuSkin;  // skin prepared for main menu

        #region Main Menu

        GUI.Label(gameNameLabel, "Try to slide");  // game label in center of main menu
        // If newGameConfirm flag is not raised than in main menu scene player can see 2 buttons or 3 buttons if he made some progress before
        if (mainMenuWindow)
        {
            // if player made some progress, Continue button will be available and he will be able to continue from highest level achieved
            if (PlayerPrefs.GetInt("Unlocked Level") > 1)
            {
                if (GUI.Button(continueButton, "Continue"))
                {
                    GameManager.ContinueGame();
                }
            }

            // if new game button will be pressed while there is no progress in PlayerPrefs, new game start immediately
            // if there is some progress in PLayerPrefs, than newGameConfirm flag is rising and New Game warning windows shows up
            // if player press yes button new game starts, if he press no main menu will show once again
            if (GUI.Button(newGameButton, "New Game"))
            {
                if (PlayerPrefs.GetInt("Unlocked Level") > 1)
                {
                    newGameConfirm = true;  // rising flag to pop up warning message
                    mainMenuWindow = false;
                }
                else
                {
                    playerNameConfirm = true;
                    mainMenuWindow = false;
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
            GUI.Box(newGameConfirmBoxRect, "WARNING");
            GUI.Label(newGameWarningRect, warningMessage, mainMenuSkin.GetStyle("Warning Message"));
            if (GUI.Button(yesButton, "Yes"))
            {
                newGameConfirm = false;
                playerNameConfirm = true;  // raising flag for player name confirm
            }
            if (GUI.Button(noButton, "No"))
            {
                newGameConfirm = false;  // lowering flag, main menu show up once again
                mainMenuWindow = true;
            }
        }
        #endregion

        #region Player Name input window

        // Player input window, player need to input 10 digit name
        // if player will not put any name, he will be announced with red prompt
        if (playerNameConfirm)
        {
            GUI.Box(playerNameBoxRect, "");
            GUI.Label(playerNamePromptRect, playerNamePrompt, mainMenuSkin.GetStyle("Player Name Prompt"));
            typedName = GUI.TextField(playerNameTextFieldRect, typedName, 10, mainMenuSkin.GetStyle("Player Name"));

            // if name window will be empty, player will be informed with red color prompt
            if (emptyPlayerName)
            {
                GUI.Label(playerNameWarningRect, playerNameWarning, mainMenuSkin.GetStyle("New Player Warning"));
            }

            // if player press confirm button few states will be checked
            if (GUI.Button(confirmButton, "Confirm"))
            {
                
                // state that checks if player already in any scoreboard
                if (GameManager.PlayerNameCheck(typedName))
                {
                    playerExists = true;
                    playerNameConfirm = false;
                }

                // state that checks if the player name field is empty
                else if (string.IsNullOrEmpty(typedName))
                {
                    emptyPlayerName = true;
                }

                // if field is not empty and player is not in any scoreboard new game will start with typed name
                else
                {
                    emptyPlayerName = false;
                    GameManager.NewGame(typedName);
                }
            }

            // if player press back, he will be moved to main menu
            if (GUI.Button(backButton, "Back"))
            {
                playerNameConfirm = false;
                mainMenuWindow = true;
            }
        }

        #endregion

        #region Player Exist window

        // after raising playerExist flag = if player is already in any scoreboard, warning window will show up and player will be notified that progress
        // will be lost
        if (playerExists)
        {
            GUI.Box(playerExistBoxRect, "WARNING!");
            GUI.Label(playerExistWarningRect, playerExistsWarning, mainMenuSkin.GetStyle("Player Exists Warning"));

            // if player press yes, flag will be lowered, scores with player name will be deleted and new game will start
            if (GUI.Button(yesButton, "Yes"))
            {
                playerExists = false;
                GameManager.RemovePlayerScore(typedName);
                GameManager.NewGame(typedName);
            }

            // if player press no, he will be moved to player name confirm window
            if (GUI.Button(noButton, "No"))
            {
                playerExists = false;
                playerNameConfirm = true;
            }
        }
        #endregion
    }
}

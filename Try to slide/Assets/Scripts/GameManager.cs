using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Life, Score, Level, Coin, Time section
    // Life variables section
    public static int life = 3;  // player current life
    public static int maxLife = 3;  // player max life

    // Score section
    private static float levelScore;  // level score var, showing after completing level
    private static float currentScore = 0f;  // current player score, reseting everytime when new game starts
    private static string playerName;  // var with player name
    private static string scoreBoard;  // var storing whole scoreboard string from PlayerPrefs.GetString("Scoreboard")
    private static string levelScoreboard;  // var storing level scoreboard string from PlayerPrefs.GetString("Scoreboard Level {levelNumber}")

    // Level section
    public static int currentLevel = 1;  // var storing current level, using it while saving progress, while loading next levels
    public static int currentLevelLabel;  // var storing current level label, even if some mehtods will change currentLevel this var stays the same for level

    // Coin section
    [SerializeField] private GameObject coinParent = null;  // object with coin parent, coin parent is box when storing coin object to count them
    [SerializeField] private float coinImportance = 0f;  // value from game manager inspector, setting how important are coins at level
    private static float coinImportanceCalc;  // var for storing coin importance necessary for score calculations
    private static float totalCoinCount;  // var storing total number of coins at scene 
    private static float currentCoinCount = 0f;  // current coin count at level
    private static float coinCompletion = 0f;  // var storing value with coin % completion at current scene 
    private static float coinScore;  // var storing coin score from current level

    // Time section
    [SerializeField] private float startTime = 0f;  // set time for player to complete level
    [SerializeField] private float timeImportance = 0f;  // value from game manager inspector, setting how important is time at level
    private static float timeImportanceCalc;  //  var for storing time importance necessary for score calculations
    private static float currentTime;  // var storing current time (start time - Time.deltaTime). Necessary for level timer
    private static float maxLevelTime;  // var storing maximum level time (start time), this var is necessary for counting time score
    private static float remainingTime;  // var storing remaining time after player complete level
    private static float timeCompletion = 0f;  // var storing value with time % completion at current scene
    private static float levelCompleteTime;  // var storing value of level completion time
    private static float timeScore;  // var storing time score from current level
    #endregion  

    #region GUI Section
    // GUI
    [SerializeField] private GUISkin levelSkin = null;  // GUI.skin set in inspector

    private Color normalTimerColor = new Color(0, 0, 0);  // black color for timer
    private Color warningTimerColor = new Color(255, 0, 0);  // red color for < 5 sec timer
    
    // Rectangles for buttons, labels, boxes
    private Rect timerRect;
    private Rect coinsRect;
    private Rect scoreRect;
    private Rect winScreenRect;
    private Rect LoseScreenRect;
    private Rect detailstWinScreenRect;
    private Rect continueButtonWinScreen;
    private Rect quitButtonWinScreen;
    private Rect scoreBoardButton;
    private Rect scoreBoardScreenRect;
    private Rect backButton;
    private Rect scoreListRect;
    private Rect playerListRect;
    private Rect lifeRect;
    private Rect levelScoreBoardButton;
    private Rect levelScoreBoardScreenRect;
    private Rect levelScoreListRect;
    private Rect levelPlayerListRect;

    // Initializing win screen window
    private static float winScreenBoxWidth = Screen.width * .7f;  // Width of win screen - 70% of screen width
    private static float winScreenBoxHeight = Screen.height * .7f;  // Height of win screen - 70% of screen height
    private float winScreenRectPositionX = Screen.width / 2 - winScreenBoxWidth / 2;  // x position, half of screen width - half of winScreenBox width ;
    private float winScreenRectPositionY = Screen.height / 2 - winScreenBoxHeight / 2; // y position, half of screen height - half of winScreenBox height ;
    #endregion

    // Flag section
    private static bool showWinScreen;  // win screen flag showing after player complete level
    private static bool showLoseScreen;  // lose screen flag showing after time ends, player waste all his lifes
    private static bool showScoreBoard;  // scoreboard flag showing after pressing Scoreboard button after lose
    private static bool showLevelScoreBoard;  // level scoreboard flag showing after pressing Level Scoreboard button after finishing level
    private static bool freezeFlag;  // freeze flag which raising will result with freezing game (Time.timeScale = 0f;)
    public static bool gameFinished;


    private void Start()
    {
        //print("1  " + PlayerPrefs.GetString("Scoreboard Level 1"));
        //print("2  " +PlayerPrefs.GetString("Scoreboard Level 2"));
        //print("3  " +PlayerPrefs.GetString("Scoreboard Level 3"));
        //print("Global  "  + PlayerPrefs.GetString("Scoreboard"));
        totalCoinCount = coinParent.transform.childCount;  // setting total coin count
        currentTime = startTime;  // seting start time as current time
        currentCoinCount = 0f;  // reseting coin count value when starting level
        coinCompletion = 0f;  // reseting coin completition value when starting level
        levelScore = 0f;  // reseting level score value when starting level
        coinImportanceCalc = coinImportance;  // setting coin imporance from inspector
        timeImportanceCalc = timeImportance;  // setting time importance from inspector
        maxLevelTime = startTime;  // setting start time as max level time
        showWinScreen = false;  // droping win screen flag
        showLoseScreen = false;  // dropping lose screen flag
        showScoreBoard = false;  // dropping scoreboard flag
        freezeFlag = false;  // dropping freeze flag
        gameFinished = false;

        // Initializing all rectangles for labels and boxes
        lifeRect = new Rect(10, 10, 10, 10);  
        timerRect = new Rect(Screen.width / 2 - 40, 10, 10, 10);
        coinsRect = new Rect(10, 30, 10, 10);
        scoreRect = new Rect(10, 50, 10, 10);
        winScreenRect = new Rect(winScreenRectPositionX, winScreenRectPositionY, winScreenBoxWidth, winScreenBoxHeight);
        LoseScreenRect = winScreenRect;
        detailstWinScreenRect = new Rect(winScreenRect.x + 20, winScreenRect.y + 40, 400, 200);
        scoreBoardScreenRect = winScreenRect;
        playerListRect = new Rect(scoreBoardScreenRect.x + scoreBoardScreenRect.x * 1.5f, scoreBoardScreenRect.y + scoreBoardScreenRect.y * .5f,
            100, 200);
        scoreListRect = new Rect(scoreBoardScreenRect.x + scoreBoardScreenRect.x * 3f, scoreBoardScreenRect.y + scoreBoardScreenRect.y * .5f,
            100, 200);
        levelScoreBoardScreenRect = scoreBoardScreenRect;
        levelPlayerListRect = playerListRect;
        levelScoreListRect = scoreListRect;


        // Initializing buttons rectangles
        continueButtonWinScreen = new Rect(winScreenRect.x + winScreenBoxWidth - 100, winScreenRect.y + winScreenBoxHeight - 55, 80, 35);
        quitButtonWinScreen = new Rect(winScreenRect.x + 20, winScreenRect.y + winScreenBoxHeight - 55, 80, 35);
        scoreBoardButton = continueButtonWinScreen;
        levelScoreBoardButton = new Rect(winScreenRect.x + winScreenBoxWidth - 240, winScreenRect.y + winScreenBoxHeight - 55, 120, 35);
        backButton = quitButtonWinScreen;

        // if level stored in unlocked level (current player progress) is higher than level 1 than set current level as unlocked level
        // added to set player name even if he press continue button
        if (PlayerPrefs.GetInt("Unlocked Level") > 1)
        {
            //currentLevel = PlayerPrefs.GetInt("Unlocked Level");
        }

    }


    private void Update()
    {
        if (!gameFinished)
        {
            if (currentTime > 0 && life > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                LoseLevel();
            }
        }
        
        // tracking freezeFlag status nad controlin conditions
        FreezeGame(freezeFlag);


        //DELETE AFTER TEST!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (Input.GetButtonDown("reset"))
        {
            PlayerPrefs.SetString("Scoreboard", "");
            PlayerPrefs.SetString("Scoreboard Level 1", "");
            PlayerPrefs.SetString("Scoreboard Level 2", "");
            PlayerPrefs.SetString("Scoreboard Level 3", "");
        }
    }


    // New game method responsible for staring a new game, loading level 1, clearing save in PlayerPrefs "Unlocked Level", setting player name. reseting
    // score and life values
    public static void NewGame(string typedName)
    {
        currentLevel = 1;
        currentScore = 0;
        life = 3;
        maxLife = 3;
        playerName = typedName;
        SceneManager.LoadScene(currentLevel);
        PlayerPrefs.SetInt("Unlocked Level", 0);
        PlayerPrefs.SetString("Player Name", playerName);
    }


    // Method responsible for ending the game by dropping all flags, destroying game manager object, loading main menu scene and reseting player progress
    public static void GameOver(GameObject gameObject)
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Main Menu");
        gameFinished = false;
        showWinScreen = false;
        showLoseScreen = false;
        freezeFlag = false;
        PlayerPrefs.DeleteKey("Player Name");
        PlayerPrefs.SetInt("Unlocked Level", 0);
    }


    // Method responsible for setting current level as Unlocked Level in PlayerPrefs
    public static void SaveLevel()
    {
        PlayerPrefs.SetInt("Unlocked Level", currentLevel);
    }


    // Method responsible for raising Lose Screen flag, setting current time to 0 and current score with player name to PlayerPrefs "Scoreboard" 
    public static void LoseLevel()
    {
        gameFinished = true;
        showLoseScreen = true;
        AddScore();
    }

    
    // Method responsible for raising Win Screen flag, setting current time as remaining time, calculating level score, incrementing level var and saving
    public static void CompleteLevel()
    {
        showWinScreen = true;
        remainingTime = currentTime;
        CalculateLevelScore();
        currentLevelLabel = currentLevel;
        AddLevelScore(currentLevelLabel);
        currentLevel++;
        SaveLevel();
    }


    // Method responsible for loading next scene
    public static void LoadNextLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }

     
    /*
     * Method responsible for calcualting level score
     * First calculating percentage coin completion (if there was 4 coins on map and player collected 1 than he has = 25% completition)
     * Next calculating percentage time completion (remaining time as percentage value of total time for level)
     * Third step is parsing data from float to int representation of % values
     * Than counting complete percentage values by importance which can be set in game manager, for example:
     * coins were more important than time so you can adjust importance to 120 and leave time importance at 100
     * Last step is addition coin score and time score and add level score to current score
     */
    public static void CalculateLevelScore()
    {
        // Remaining time and coin count % values, than converting them to int
        coinCompletion = (currentCoinCount * 100) / totalCoinCount;
        coinCompletion = (int)coinCompletion;  // Coin completion as percentage value

        timeCompletion = (remainingTime * 100) / maxLevelTime;
        timeCompletion = (int)timeCompletion;  // Time completion as percentage value

        // Level completition time
        levelCompleteTime = maxLevelTime - remainingTime;

        // Calculating Importance * % values
        coinScore = (coinImportanceCalc * coinCompletion) / 100;
        timeScore = (timeImportanceCalc * timeCompletion) / 100;

        levelScore = timeScore + coinScore;
        currentScore += levelScore;
    }


    // Method responsible for increment coin value after picking coin
    public static void CoinPickUp()
    {
        currentCoinCount += 1;
    }


    /*
     * Method resposible for concatenate current player and his score to scoreboard save in PlayerPrefs
     * first method is checking is there any record in player pref, if there is none method is starting new record
     * if there is some record in Scoreboard key, than method concatenate player and his score to this record
     */
    public static void AddScore()
    {
        if (PlayerPrefs.HasKey("Scoreboard"))
        {
            scoreBoard = PlayerPrefs.GetString("Scoreboard");
            PlayerPrefs.SetString("Scoreboard", scoreBoard + $"{playerName}:{currentScore};");
        }
        else
        {
            PlayerPrefs.SetString("Scoreboard", $"{playerName}:{currentScore};");
        }

    }


    /*
     * Method resposible for concatenate current player and his score to level scoreboard save in PlayerPrefs
     * first method is checking is there any record in player prefs, if there is none method is starting new record
     * if there is any record in Scoreboard Level {levelNumber} key, than method concatenate player and his score to this record 
     */
    public static void AddLevelScore(int levelNumber)
    {
        if (PlayerPrefs.HasKey($"Scoreboard Level {levelNumber}"))
        {
            levelScoreboard = PlayerPrefs.GetString($"Scoreboard Level {levelNumber}");
            PlayerPrefs.SetString($"Scoreboard Level {levelNumber}", levelScoreboard + $"{playerName}:{levelScore};");
        }
        else
        {
            PlayerPrefs.SetString($"Scoreboard Level {levelNumber}", $"{playerName}:{levelScore};");
        }
    }


    /*
     * Method responsible for creating template with players and their scores
     * Method takes places parameter - this one declare number of players that we want to get
     * and optional parameter level, which is tracked inside of method in condition, if we put level number
     * method will change from global scoreboard to specified world scoreboard
     * Method load record with scoreboard from PlayerPrefs
     * First creating two empty string for player names and second one for player scores
     * Next creating empy array for 2 string where mehtod store player names and scores and than return this array as a resault
     * listScores is a list of separated names and score - using list of separators to split this list
     * dictScores is a dict with key - player names and value - player score
     * sortedDict is a dict with number of {places} highest scores
     * First method checks is there optional parameter typed in call,
     * if there is optional parameter, method creates list of strings  with level scoreboard - {"player", "score", "player", "score"}
     * if there is none, method creates list of strings with global scoreboard - {"player", "score", "player", "score"}
     * Next method fills dictScores with key - "player" and value - score (parsing score from string to int) records
     * if there is already player with the same name, method will save achieved score
     * Next method is creating player and score string templates with place-player name-player score records
     * Last step is putting templates to playersAndScores array - index 0 are player names, index 1 are player scores
     * and returning array
     * ShowScores(20)[0] - shows names of best 20 players in total scoreboard (change 20 number to get other number of players)
     * ShowScore(20)[1] - show scores of best 20 players in total scoreboard (change 20 number to get other number of players)
     * ShowScore(10, 1)[0] - show score of 10 best player in level 1 scoreboards
     */
    public static string[] ShowScores(int places, int level = 0)
    {
        int showPlayerPlaces = places;
        string showTemplatePlayers = $"";
        string showTemplateScores = $"";
        string[] playersAndScores = new string[2];
        List<string> listScores = new List<string>();
        Dictionary<string, float> dictScores = new Dictionary<string, float>();
        char[] separator = { ':', ';' };
        if (level == 0)
        {
            listScores = PlayerPrefs.GetString("Scoreboard").Split(separator).ToList();
        }
        else
        {
            listScores = PlayerPrefs.GetString($"Scoreboard Level {level}").Split(separator).ToList();
        }

        for (int i = 0; i < listScores.Count() - 1 * 2; i += 2)
        {
            if (dictScores.ContainsKey(listScores[i]))
            {
                float valueStrToInt = float.Parse(listScores[i + 1]);
                dictScores[listScores[i]] = valueStrToInt;
            }
            else
            {
                float valueStrToInt = float.Parse(listScores[i + 1]);
                dictScores.Add(listScores[i], valueStrToInt);
            }
        }

        int playerPlace = 1;
        if (dictScores.Count() < showPlayerPlaces)
        {
            foreach (KeyValuePair<string, float> player in dictScores.OrderByDescending(key => key.Value))
            {
                showTemplatePlayers += $"{playerPlace}.) {player.Key}".PadRight(60, '.') + "\n";
                showTemplateScores += $"{player.Value}\n";
                playerPlace++;
            }
        }

        else
        {
            foreach (KeyValuePair<string, float> player in dictScores.OrderByDescending(key => key.Value))
            {
                showTemplatePlayers += $"{playerPlace}.) {player.Key}".PadRight(60, '.') + "\n";
                showTemplateScores += $"{player.Value}\n";
                playerPlace++;
                showPlayerPlaces--;
                if (showPlayerPlaces == 0)
                {
                    break;
                }
            }
        }


        playersAndScores[0] = showTemplatePlayers;
        playersAndScores[1] = showTemplateScores;

        return playersAndScores;
    }


    // Method resposnible for controlling Time.timeScale, after showing the win screen or lose screen flag is raising to true and game is frozen
    // When player press continue (win screen) or quit (win/lose screen) button, flag will be lowered to false and Time.timeScale will be set to 1f.
    private static void FreezeGame(bool freeze)
    {
        if (freeze)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }


    // GUI section responsible for showing win/lose/scoreboard boxes, life/timer/coins label
    private void OnGUI()
    {
        GUI.skin = levelSkin;

        // In game/Level labels
        // show timer, coins
        // if level is higher than 1 than show score 
        GUI.Label(lifeRect, $"Life: {life}/{maxLife}", levelSkin.GetStyle("Life"));
        GUI.Label(timerRect, $"Time: {currentTime.ToString("0.0")}/{startTime}", levelSkin.GetStyle("Timer"));
        GUI.Label(coinsRect, $"Coins: {currentCoinCount}/{totalCoinCount}", levelSkin.GetStyle("Coins"));
        string winScreenLevelInfo = $"Level completion time: {levelCompleteTime.ToString("0.0")}" +
            $"\nCoins collected {currentCoinCount}/{totalCoinCount}" +
            $"\nLevel score: {levelScore}\nTotal score: {currentScore}";
        string loseScreenLevelInfo = $"Coins collected {currentCoinCount}/{totalCoinCount}" +
            $"\nLevel score: {levelScore}\nTotal score: {currentScore}";


        if (currentScore > 0)
        {
            GUI.Label(scoreRect, $"Score: {currentScore}", levelSkin.GetStyle("Score"));
        }

        if (currentTime <= 5)
        {
            levelSkin.GetStyle("Timer").normal.textColor = warningTimerColor;
        }

        else
        {
            levelSkin.GetStyle("Timer").normal.textColor = normalTimerColor;
        }


        /*
         * Win/Lose/Scoreboard screen
         * if any of this flag is raised, than game is freezed
         * Show Win Screen:
         * showing level details and score
         * if player press continue button, loading next level, dropping Win Screen and Freeze flag
         * if player press quit, loading main menu scene, dropping Win Screen and Freeze flag, destroying game manager object
         * Show Lose Screen:
         * showing level details
         * if player press Scoreboard button, raising Scoreboard flag and showing player scores
         * if player press quit, calling GameOver method and destroying game object
         * Show Scoreboard:
         * if player press the Scoreboard button, hiding showLoseScreen by dropping flag and raising Scoreboard flag
         * creating two vertical labels, one with player names, second with player scores
         * if player press back button, showScoreboard flag is dropping and showLoseScreen flag is raising
         * Show Level Scoreboard:
         * if player press the Level Scoreboard button, hiding showWinScreen by dropping flag and raising showLevelScoreBoard flag
         * creating two vertical labels, one with player names, second with players scores
         * if player press back button, showLevelScoreBoard flag is dopping and showWinScreen flag is raising
         */
        if (showWinScreen || showLoseScreen || showScoreBoard || showLevelScoreBoard)
        {
            freezeFlag = true;
            if (showWinScreen)
            {
                // Show win screen with Level Completed sign at top of the box also show Continue button
                GUI.Box(winScreenRect, $"Level {currentLevelLabel} Completed");
                // show Level Details on win screen
                GUI.Label(detailstWinScreenRect, winScreenLevelInfo);
                if (GUI.Button(continueButtonWinScreen, "Continue"))
                {
                    LoadNextLevel();
                    showWinScreen = false;
                    freezeFlag = false;
                }
                if (GUI.Button(levelScoreBoardButton, $"Level Scoreboard"))
                {
                    showLevelScoreBoard = true;
                }
                if (GUI.Button(quitButtonWinScreen, "Quit"))
                {
                    SceneManager.LoadScene("Main Menu");
                    showWinScreen = false;
                    freezeFlag = false;
                    Destroy(gameObject);
                }
            }

            if (showLoseScreen)
            {
                // Show lose screen with Lose sigh at top of the box
                GUI.Box(LoseScreenRect, "Lose");
                // show Level Details on win screen
                GUI.Label(detailstWinScreenRect, loseScreenLevelInfo);
                if (GUI.Button(scoreBoardButton, "Scoreboard"))
                {
                    showScoreBoard = true;
                }
                if (GUI.Button(quitButtonWinScreen, "Quit"))
                {
                    GameOver(gameObject);
                }
            }

            if (showLevelScoreBoard)
            {
                showWinScreen = false;
                GUI.Box(levelScoreBoardScreenRect, "Level Scoreboard");
                GUI.Label(levelPlayerListRect, ShowScores(10, currentLevelLabel)[0], levelSkin.GetStyle("Scores"));
                GUI.Label(levelScoreListRect, ShowScores(10, currentLevelLabel)[1], levelSkin.GetStyle("Scores"));
                if (GUI.Button(backButton, "Back"))
                {
                    showLevelScoreBoard = false;
                    showWinScreen = true;
                }
            }

            if (showScoreBoard)
            {
                showLoseScreen = false;
                GUI.Box(scoreBoardScreenRect, "Scoreboard");
                GUI.Label(playerListRect, ShowScores(20)[0], levelSkin.GetStyle("Scores"));
                GUI.Label(scoreListRect, ShowScores(20)[1], levelSkin.GetStyle("Scores"));
                if (GUI.Button(backButton, "Back"))
                {
                    showScoreBoard = false;
                    showLoseScreen = true;
                }
            }
        }

        

        // DELETE AFTER TEST!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        GUI.Label(new Rect(Screen.width - 160, Screen.height - 90, 150, 150), $@"Name: {playerName}
PlayerPrefsName: {PlayerPrefs.GetString("Player Name")}
Current Level: {currentLevel}
Playerpref Unlocked Level: {PlayerPrefs.GetInt("Unlocked Level")}
LvLScore: {levelScore}
Score: {currentScore}
Coins: {currentCoinCount}/{totalCoinCount}", levelSkin.GetStyle("Test"));
    }
}

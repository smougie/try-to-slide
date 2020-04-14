using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    #region Life, Score, Level, Coin, Time section
    // Life variables section
    public static int life = 3;  // player current life
    public static int maxLife = 3;  // player max life

    // Score section
    private static float levelScore;  // level score var, showing after completing level
    private static float currentScore = 0f;  // current player score, reseting everytime when new game starts
    public static string playerName;  // var with player name
    private static string scoreBoard;  // var storing whole scoreboard string from PlayerPrefs.GetString("Scoreboard")
    private static string levelScoreboard;  // var storing level scoreboard string from PlayerPrefs.GetString("Scoreboard Level {levelNumber}")

    // Level section
    public static int currentLevel;  // var storing current level, using it while saving progress, while loading next levels
    public static int currentLevelLabel;  // var storing current level label, even if some mehtods will change currentLevel this var stays the same for level
    public static int numberOfLevels;  // var storing current number of levels ->   -2 means: - Main Menu scene, -Level Select

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
    private Rect currentScoreLevelSelectRect;
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

    #region Flag Section
    // Flag section
    private static bool showWinScreen;  // win screen flag showing after player complete level
    private static bool showLoseScreen;  // lose screen flag showing after time ends, player waste all his lifes
    private static bool showScoreBoard;  // scoreboard flag showing after pressing Scoreboard button after lose
    private static bool showLevelScoreBoard;  // level scoreboard flag showing after pressing Level Scoreboard button after finishing level
    private static bool freezeFlag;  // freeze flag which raising will result with freezing game (Time.timeScale = 0f;)
    public static bool gameFinished;
    [SerializeField] private bool isLevelSelect = false;
    #endregion

    #endregion

    private void Start()
    {
        #region Initializing necessary variables

        #region Reseting values at level start, tracking object parents, setting time and coin variables 

        if (!isLevelSelect)
        {
            totalCoinCount = coinParent.transform.childCount;  // setting total coin count
        }
        numberOfLevels = SceneManager.sceneCountInBuildSettings - 2;  // variable with number of levels
        currentTime = startTime;  // seting start time as current time
        currentCoinCount = 0f;  // reseting coin count value when starting level
        coinCompletion = 0f;  // reseting coin completition value when starting level
        levelScore = 0f;  // reseting level score value when starting level
        coinImportanceCalc = coinImportance;  // setting coin imporance from inspector
        timeImportanceCalc = timeImportance;  // setting time importance from inspector
        maxLevelTime = startTime;  // setting start time as max level time

        #endregion

        #region Flag section

        showWinScreen = false;  // droping win screen flag
        showLoseScreen = false;  // dropping lose screen flag
        showScoreBoard = false;  // dropping scoreboard flag
        freezeFlag = false;  // dropping freeze flag
        gameFinished = false;

        #endregion

        #region Initializing all rectangles for labels and boxes

        lifeRect = new Rect(10, 10, 10, 10);  
        timerRect = new Rect(Screen.width / 2 - 40, 10, 10, 10);
        coinsRect = new Rect(10, 30, 10, 10);
        scoreRect = new Rect(10, 50, 10, 10);
        currentScoreLevelSelectRect = timerRect;
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

        #endregion

        #region Initializing buttons rectangles

        continueButtonWinScreen = new Rect(winScreenRect.x + winScreenBoxWidth - 100, winScreenRect.y + winScreenBoxHeight - 55, 80, 35);
        quitButtonWinScreen = new Rect(winScreenRect.x + 20, winScreenRect.y + winScreenBoxHeight - 55, 80, 35);
        scoreBoardButton = continueButtonWinScreen;
        levelScoreBoardButton = new Rect(winScreenRect.x + winScreenBoxWidth - 240, winScreenRect.y + winScreenBoxHeight - 55, 120, 35);
        backButton = quitButtonWinScreen;

        #endregion

        #endregion
    }


    private void Update()
    {
        // if not in level select, so if in normal game level
        if (!isLevelSelect)
        {
            // if game is not finished
            if (!gameFinished )
            {
                // if current time is greater than0 and life is greater than 0, time counter will work and count down current time till hit 0
                if (currentTime > 0 && life > 0)
                {
                    currentTime -= Time.deltaTime;
                }
                else
                {
                    LoseLevel();
                }
            }
        }
        
        // tracking freezeFlag status and controling conditions
        FreezeGame(freezeFlag);
    }

    // New game method responsible for staring a new game, loading level select, clearing save in PlayerPrefs "Unlocked Level", setting player name, reseting
    // score and life values
    public static void NewGame(string typedName)
    {
        currentLevel = 0;
        currentScore = 0;
        life = 3;
        maxLife = 3;
        playerName = typedName;
        SceneManager.LoadScene("Level Select");  // before test was currentLevel
        PlayerPrefs.SetInt("Unlocked Level", 1);  // before test was 0
        PlayerPrefs.SetString("Player Name", playerName);
    }
    
    // Continue game method responsible for rebooting the game with player progress
    // Player will be moved to Level Select, current score and player name will be loaded
    public static void ContinueGame()
    {
        currentLevel = 0;
        SceneManager.LoadScene("Level Select");
        currentScore = float.Parse(PlayerPrefs.GetString("Current Score"));
        playerName = PlayerPrefs.GetString("Player Name");
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
        PlayerPrefs.DeleteKey("Current Score");
        PlayerPrefs.SetInt("Unlocked Level", 0);
    }

    // Method responsible for setting current level as Unlocked Level in PlayerPrefs
    // if save in player pref is less than current level, progress will be saved
    public static void SaveLevel()
    {
        if (PlayerPrefs.GetInt("Unlocked Level") < currentLevel + 1)
        {
            PlayerPrefs.SetInt("Unlocked Level", currentLevel + 1);
        }
        PlayerPrefs.SetString("Current Score", $"{currentScore}");
    }

    // Method responsible for raising Lose Screen flag, setting current time to 0 and current score with player name to PlayerPrefs "Scoreboard" 
    public static void LoseLevel()
    {
        gameFinished = true;
        showLoseScreen = true;
        AddScore();
    }
    
    // Method responsible for raising Win Screen flag, setting current time as remaining time, calculating level score, adding level score to scoreboard
    // and saving game
    public static void CompleteLevel()
    {
        showWinScreen = true;
        remainingTime = currentTime;
        CalculateLevelScore();
        AddLevelScore(currentLevel);
        SaveLevel();
    }

    // Method responsible for loading Level Select scene
    public static void LoadLevelSelect()
    {
        SceneManager.LoadScene("Level Select");
        currentLevel = 0;
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
     * Method resposible for removing player name and score from Level Scoreboard after pressing restart button by player in Select Level scene
     * also method will substract previous level score from currentScore
     */
    public static void RemoveLevelScore(string playerName, int levelNumberToRemove)
    {
        string modifiedScoreboard = "";
        Dictionary<string, string> playersAndScores = PlayerNamesScores($"Scoreboard Level {levelNumberToRemove}");
        
        // removing player level score from global scoreboard
        foreach (KeyValuePair<string, string> item in playersAndScores)
        {
            if (item.Key == playerName)
            {
                currentScore -= float.Parse(item.Value);
            }
        }

        // removing player key and value from dict
        playersAndScores.Remove(playerName);

        // creating new record for player pref scoreboard save
        foreach (KeyValuePair<string, string> item in playersAndScores)
        {
            modifiedScoreboard += $"{item.Key}:{item.Value};";
        }

        // setting new scoreboard record
        PlayerPrefs.SetString($"Scoreboard Level {levelNumberToRemove}", modifiedScoreboard);
    }

    /*
     * Method responsible for removing player name and score from global game scoreboard after choosing already existing player name
     * When player hit new game button in the main menu scene, and type player name that is already in some scoreboard record and confirm
     * that he wants to start the game with this name, his previous score in every scoreboard will be removed
     */
    public static void RemovePlayerScore(string playerToRemove, int levelToRemove = 0)
    {
        Dictionary<string, string> scoreboardDict = PlayerNamesScores("Scoreboard");
        List<string> scoreboardKeysToRemove = new List<string>();
        string modifiedRecord = "";

        // first checking global scoreboard for player name, if there will be name, we are adding name to remove it after loop
        foreach (KeyValuePair<string, string> entry in scoreboardDict)
        {
            if (entry.Key == playerToRemove)
            {
                scoreboardKeysToRemove.Add(entry.Key);
            }
        }

        // removing all key in dict equal to player name
        foreach (var key in scoreboardKeysToRemove)
        {
            scoreboardDict.Remove(key);
        }

        // if there was any record to remove, we are creating new string for player pref save without removed record
        if (scoreboardKeysToRemove.Any())
        {
            foreach (KeyValuePair<string, string> playerScore in scoreboardDict)
            {
                modifiedRecord += $"{playerScore.Key}:{playerScore.Value};";
            }
            PlayerPrefs.SetString("Scoreboard", modifiedRecord);
        }

        // now moving to Level X Scoreboard, creating list of levels which will have to remove player score record
        List<int> levelsToNuke = new List<int>();

        // now checking all level scoreboards for any score with player name as a key
        for (int levelNum = 1; levelNum <= numberOfLevels; levelNum++)
        {
            foreach (KeyValuePair<string, string> entry in PlayerNamesScores($"Scoreboard Level {levelNum}"))
            {
                if (entry.Key == playerToRemove)
                {
                    levelsToNuke.Add(levelNum);
                }
            }
        }

        // for each level number where player name was found, method will create modified record without player name and score
        foreach (var levelNumber in levelsToNuke)
        {
            Dictionary<string, string> levelScoreBoard = PlayerNamesScores($"Scoreboard Level {levelNumber}");
            string modifiedLevelRecord = "";

            // removing player as a key and score as a value from temp dict
            levelScoreBoard.Remove(playerToRemove);

            foreach (KeyValuePair<string, string> entry in levelScoreBoard)
            {
                modifiedLevelRecord += $"{entry.Key}:{entry.Value};";
            }

            // saving new record without player and score
            PlayerPrefs.SetString($"Scoreboard Level {levelNumber}", modifiedLevelRecord);
        }
    }

    /*
     * Method responsible for comparing player level score
     * If method find any record with player name, score will show up under best scores in Level X Scoreboard
     * If method will not find any record assigned to player name, player will get proper information
     */
    public static string LevelScoreComparer(int levelNumber, string playerName)
    {
        string playerScore = "You have not finished this level.";
        string levelScoreboard = PlayerPrefs.GetString($"Scoreboard Level {levelNumber}");
        char[] separator = { ':', ';' };
        List<string> listPlayerScores = levelScoreboard.Split(separator).ToList();

        int counter = 0;

        foreach (var item in listPlayerScores)
        {
            if (item == playerName)
            {
                return $"Your score on this level: {listPlayerScores[counter + 1]}";
            }

            else
            {
                counter++;
            }
        }

        return playerScore;
    }

    // Method responsible for checking if the player's name is already on Level X Scoreboard
    public static bool RestartAvailable(int levelToCheck)
    {
        Dictionary<string, string> playersAndScores = PlayerNamesScores($"Scoreboard Level {levelToCheck}");
        if (playersAndScores.ContainsKey(playerName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     * Method responsible for checking if the player's name is already in any of Level X Scoreboard
     */
    public static bool PlayerNameCheck(string typedName)
    {
        bool exist = false;
        for (int levelNum = 1; levelNum <= numberOfLevels; levelNum++)
        {
            if (PlayerNamesScores($"Scoreboard Level {levelNum}").ContainsKey(typedName))
            {
                exist = true;
            }
            else
            {
                continue;
            }
        }

        return exist;
    }

    /*
     * Method creating and returning dict with key - player name, value - player score, both are strings
     */
    public static Dictionary<string, string> PlayerNamesScores(string listToCheck)
    {
        char[] separator = { ':', ';' };
        List<string> listScores = new List<string>();
        listScores = PlayerPrefs.GetString(listToCheck).Split(separator).ToList();
        Dictionary<string, string> dictPlayersScores = new Dictionary<string, string>();

        for (int i = 0; i < listScores.Count() - 1 * 2; i += 2)
        {
            if (dictPlayersScores.ContainsKey(listScores[i]))
            {
                dictPlayersScores[listScores[i]] = listScores[i + 1];
            }
            else
            {
                dictPlayersScores.Add(listScores[i], listScores[i + 1]);
            }
        }

        return dictPlayersScores;
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
    public static string[] ShowScores(int places = 0, int level = 0)
    {
        int showPlayerPlaces = places;
        string showTemplatePlayers = $"";
        string showTemplateScores = $"";
        string[] playersAndScores = new string[2];
        Dictionary<string, string> notSortedDictScore = new Dictionary<string, string>();

        if (level == 0)
        {
            notSortedDictScore = PlayerNamesScores("Scoreboard");
        }

        else
        {
            notSortedDictScore = PlayerNamesScores($"Scoreboard Level {level}");
        }

        int playerPlace = 1;
        if (notSortedDictScore.Count() < showPlayerPlaces)
        {
            foreach (KeyValuePair<string, string> player in notSortedDictScore.OrderByDescending(key => float.Parse(key.Value)))
            {
                showTemplatePlayers += $"{playerPlace}.) {player.Key}".PadRight(63 - player.Key.Length - 2, '.') + "\n";
                showTemplateScores += $"{player.Value}\n";
                playerPlace++;
            }
        }

        else
        {
            foreach (KeyValuePair<string, string> player in notSortedDictScore.OrderByDescending(key => float.Parse(key.Value)))
            {
                showTemplatePlayers += $"{playerPlace}.) {player.Key}".PadRight(63 - player.Key.Length - 2, '.') + "\n";
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
        
        string winScreenLevelInfo = $"Level completion time: {levelCompleteTime.ToString("0.0")}" +
            $"\nCoins collected {currentCoinCount}/{totalCoinCount}" +
            $"\nLevel score: {levelScore}\nTotal score: {currentScore}";
        string loseScreenLevelInfo = $"Coins collected {currentCoinCount}/{totalCoinCount}" +
            $"\nLevel score: {levelScore}\nTotal score: {currentScore}";
        
        // In game level labels, timers, coins
        GUI.Label(lifeRect, $"Life: {life}/{maxLife}", levelSkin.GetStyle("Life"));

        // if it is level select show score in top center of screen
        if (isLevelSelect)
        {
            GUI.Label(currentScoreLevelSelectRect, $"Score: {currentScore}", levelSkin.GetStyle("Level Select Score"));
        }

        // if it is normal game level show time, coins, score labels
        if (!isLevelSelect)
        {
            GUI.Label(timerRect, $"Time: {currentTime.ToString("0.0")}/{startTime}", levelSkin.GetStyle("Timer"));
            GUI.Label(coinsRect, $"Coins: {currentCoinCount}/{totalCoinCount}", levelSkin.GetStyle("Coins"));

            // if score is greater than 0, show it
            if (currentScore > 0)
            {
                GUI.Label(scoreRect, $"Score: {currentScore}", levelSkin.GetStyle("Score"));
            }
        }

        // if time falls below 5 seconds, the timer color will change for red
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
                    LoadLevelSelect();
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

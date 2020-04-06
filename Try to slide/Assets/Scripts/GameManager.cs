using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int life = 3;
    public static int maxLife = 3;
    private Rect lifeRect;

    private static string playerName;
    private static string scoreBoard;

    // Score section
    private static float levelScore;
    private static float currentScore = 0f;
    private static int highscore;
    private static float totalScore;

    // Level section
    public static int currentLevel = 1;
    public static int unlockedLevel;

    // Coin section
    [SerializeField] private GameObject coinParent = null;
    [SerializeField] private float coinImportance = 0f;  // Value from Game Manager gameobject set in inspector
    private static float coinImportanceCalc;
    private static float totalCoinCount;
    private static float currentCoinCount = 0f;
    private static float coinCompletion = 0f;
    private static float coinScore;

    // Time section
    [SerializeField] private float startTime = 0f;
    [SerializeField] private float timeImportance = 0f;
    private static float timeImportanceCalc;
    private static float currentTime;
    private static float maxLevelTime;
    private static float remainingTime;
    private static float timeCompletion = 0f;
    private static float levelCompleteTime;
    private static float timeScore;

    // GUI Section
    [SerializeField] private GUISkin levelSkin = null;

    private Color normalTimerColor = new Color(0, 0, 0);  // black
    private Color warningTimerColor = new Color(255, 0, 0);  // red
    
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

    private static float winScreenBoxWidth = Screen.width * .7f;  // Width of win screen - 70% of screen width
    private static float winScreenBoxHeight = Screen.height * .7f;  // Height of win screen - 70% of screen height
    private float winScreenRectPositionX = Screen.width / 2 - winScreenBoxWidth / 2;  // x position, half of screen width - half of winScreenBox width ;
    private float winScreenRectPositionY = Screen.height / 2 - winScreenBoxHeight / 2; // y position, half of screen height - half of winScreenBox height ;


    // Flag section
    private static bool showWinScreen;
    private static bool showLoseScreen;
    private static bool showScoreBoard;
    private static bool freezeFlag;  // freeze flag which raising will result with freezing game (Time.timeScale = 0f;)

    private void Start()
    {
        currentTime = startTime;
        totalCoinCount = coinParent.transform.childCount;
        currentCoinCount = 0f;  // reseting coin count value when starting level
        coinCompletion = 0f;  // reseting coin completition value when starting level
        levelScore = 0f;  // reseting level score value when starting level
        coinImportanceCalc = coinImportance;
        timeImportanceCalc = timeImportance;
        maxLevelTime = startTime;
        showWinScreen = false;
        showLoseScreen = false;
        showScoreBoard = false;
        freezeFlag = false;

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

        continueButtonWinScreen = new Rect(winScreenRect.x + winScreenBoxWidth - 100, winScreenRect.y + winScreenBoxHeight - 55, 80, 35);
        quitButtonWinScreen = new Rect(winScreenRect.x + 20, winScreenRect.y + winScreenBoxHeight - 55, 80, 35);
        scoreBoardButton = continueButtonWinScreen;
        backButton = quitButtonWinScreen;

        if (PlayerPrefs.GetInt("Unlocked Level") > 1)
        {
            currentLevel = PlayerPrefs.GetInt("Unlocked Level");
        }
    }


    private void Update()
    {
        if (startTime > 0)
        {
            currentTime -= Time.deltaTime;
        }

        if (currentTime <= 0)
        {
            LoseLevel();
        }

        // Method which is responsible for tracking freezeFlag status, if freezeFlag is false than Time.timeScale is equal to 1f, game works
        // if freeeFlag is True than Time.timeScale is equal to 0f and game is frozen until flag will be equal to false
        FreezeGame(freezeFlag);
        //DELETE AFTER TEST!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (Input.GetButtonDown("reset"))
        {
            PlayerPrefs.SetString("Scoreboard", "");
        }
    }


    public static void NewGame(string typedName)
    {
        currentLevel = 1;
        currentScore = 0;
        life = 3;
        maxLife = 3;
        SceneManager.LoadScene(currentLevel);
        PlayerPrefs.SetInt("Unlocked Level", 0);
        playerName = typedName;
    }
    

    public static void SaveLevel()
    {
        PlayerPrefs.SetInt("Unlocked Level", currentLevel);
    }


    public static void LoseLevel()
    {
        showLoseScreen = true;
        currentTime = 0;
        AddScore();
    }


    public static void CompleteLevel()
    {
        showWinScreen = true;
        remainingTime = currentTime;
        CalculateLevelScore();
        currentLevel++;
        SaveLevel();
    }


    public static void LoadNextLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }


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


    public static void CoinPickUp()
    {
        currentCoinCount += 1;
    }


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


    public static string[] ShowScores()
    {
        string showTemplatePlayers = $"";
        string showTemplateScores = $"";
        string[] playersAndScores = new string[2];
        List<string> listScores = new List<string>();
        Dictionary<string, int> dictScores = new Dictionary<string, int>();
        Dictionary<string, int> sortedDict = new Dictionary<string, int>();
        char[] separator = { ':', ';' };
        listScores = PlayerPrefs.GetString("Scoreboard").Split(separator).ToList();
        for (int i = 0; i < listScores.Count() - 1 * 2; i += 2)
        {
            if (dictScores.ContainsKey(listScores[i]))
            {
                int valueStrToInt = int.Parse(listScores[i + 1]);
                dictScores[listScores[i]] = valueStrToInt;
            }
            else
            {
                int valueStrToInt = int.Parse(listScores[i + 1]);
                dictScores.Add(listScores[i], valueStrToInt);
            }
        }

        int playerPlace = 1;
        foreach (KeyValuePair<string, int> player in dictScores.OrderByDescending(key => key.Value))
        {
            showTemplatePlayers += $"{playerPlace}.) {player.Key}".PadRight(60, '.') + "\n";
            showTemplateScores += $"{player.Value}\n";
            sortedDict.Add(player.Key, player.Value);
            playerPlace++;
            if (playerPlace > 20)
            {
                break;
            }
        }
        playersAndScores[0] = showTemplatePlayers;
        playersAndScores[1] = showTemplateScores;
        return playersAndScores;
    }


    public static void GameOver()
    {
        SceneManager.LoadScene("Main Menu");
        showWinScreen = false;
        showLoseScreen = false;
        freezeFlag = false;
        PlayerPrefs.SetInt("Unlocked Level", 0);
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

        // Win Screen section after triggering goal tag
        if (showWinScreen || showLoseScreen || showScoreBoard)
        {
            freezeFlag = true;
            if (showWinScreen)
            {
                // Show win screen with Level Completed sign at top of the box also show Continue button
                GUI.Box(winScreenRect, "Level Completed");
                // show Level Details on win screen
                GUI.Label(detailstWinScreenRect, winScreenLevelInfo);
                if (GUI.Button(continueButtonWinScreen, "Continue"))
                {
                    LoadNextLevel();
                    showWinScreen = false;
                    freezeFlag = false;
                }
                if (GUI.Button(quitButtonWinScreen, "Quit"))
                {
                    SceneManager.LoadScene("Main Menu");
                    showWinScreen = false;
                    showLoseScreen = false;
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
                    GameOver();
                    Destroy(gameObject);
                }
            }

            if (showScoreBoard)
            {

                showLoseScreen = false;
                GUI.Box(scoreBoardScreenRect, "Scoreboard");
                GUI.Label(playerListRect, ShowScores()[0], levelSkin.GetStyle("Scores"));
                GUI.Label(scoreListRect, ShowScores()[1], levelSkin.GetStyle("Scores"));

                if (GUI.Button(backButton, "Back"))
                {
                    showScoreBoard = false;
                    showLoseScreen = true;
                }
            }
        }

        

        // DELETE AFTER TEST!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        GUI.Label(new Rect(Screen.width - 160, Screen.height - 70, 150, 150), $@"Name: {playerName}
Current Level: {currentLevel}
Playerpref Unlocked Level: {PlayerPrefs.GetInt("Unlocked Level")}
LvLScore: {levelScore}
Score: {currentScore}
Coins: {currentCoinCount}/{totalCoinCount}", levelSkin.GetStyle("Test"));
    }
}

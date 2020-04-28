using UnityEngine;
using UnityEngine.SceneManagement;

// Class responsible for tracking collision with World Node in World Select level.
public class LevelSelect : MonoBehaviour
{
    #region Variables
    
    #region Variables accessible from inspector

    [SerializeField] private int levelToLoad = 0;  // variable storing value of level to load, accesible from inspector level node
    [SerializeField] private GameObject padlock = null;  // variable storing padlock object for level node
    [SerializeField] private GUISkin skinLevelSelect = null;  // variable storing level GUI skin

    #endregion


    #region Rectangles

    private float screenWidth;  // rect with screen width
    private float screenHeight;  // rect with screen height
    private Rect levelScoreBoardRect;  // rect for Level X Scoreboard window box with 10 best player scores poping up in Level Select near level node
    private Rect levelPlayerListRect;  // rect for Player names in Level X Scoreboard window
    private Rect levelScoreListRect;  // rect for Player score in Level X Scoreboard window
    private Rect playerLevelScoreRect;  // rect for Player Level X score which will appear in Level X Scoreboard window after completing X level
    private Rect levelDetailsRect;  // rect for world details label
    private Rect restartButton;  // rect for restart button
    private Rect submitButton;  // rect for restart button

    #endregion


    #region Labels

    private string unlockedLevelDetails;  // variable storing label with info how to load level if player is in range of level node
    private string lockedLevelDetails;  // variable storing label with info which level need to complete before moving to this level node

    #endregion

    #region Flags

    private bool inRange;  // flag will raise when player collide with World Node and lower flag when player exit trigger
    private bool locked;  // flag will raise if player progress in PlayerPrefs Unlocked Level is less than Level to Load
    private bool restartAvailable;  // flag will raise if player will be in range of level to load and he is already in scoreboard record

    #endregion

    #endregion

    void Start()
    {
        #region Initializing necessary variables

        // Initializing flags
        inRange = false;
        locked = false;
        restartAvailable = false;

        // Initializing rectangles
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        levelScoreBoardRect = new Rect(screenWidth - screenWidth * .95f, screenHeight - screenHeight * .95f, screenWidth * .3f, screenHeight * .6f);
        levelPlayerListRect = new Rect(levelScoreBoardRect.x + levelScoreBoardRect.x * 1f, levelScoreBoardRect.y + levelScoreBoardRect.y * 1f, 100, 400);
        levelScoreListRect = new Rect(levelScoreBoardRect.x + levelScoreBoardRect.x * 4.5f, levelScoreBoardRect.y + levelScoreBoardRect.y * 1f, 100, 400);
        playerLevelScoreRect = new Rect(levelScoreBoardRect.x + levelScoreBoardRect.width * .28f, levelScoreBoardRect.height, 200, 100);
        levelDetailsRect = new Rect(Screen.width * .01f, Screen.height * .95f, 400, 150);
        restartButton = new Rect(levelScoreBoardRect.x + levelScoreBoardRect.width / 2 - 30, levelScoreBoardRect.y + levelScoreBoardRect.height * .8f, 60, 30);
        submitButton = new Rect(levelScoreBoardRect.x + levelScoreBoardRect.width / 2 - 50, levelScoreBoardRect.y + levelScoreBoardRect.height * .91f, 100, 30);

        // Initializing labels
        unlockedLevelDetails = $"Press [SPACE] or [E]  to load level {levelToLoad}.";
        lockedLevelDetails = $"Complete level {levelToLoad - 1} to unlock.";

        #endregion

        // Instantiating padlock object on level node and raising locked flag  
        // if level to load on level node is greater than player save instantiate padlock object on level node
        if (levelToLoad > PlayerPrefs.GetInt("Unlocked Level"))
        {
            Instantiate(padlock,new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z - 1), Quaternion.identity);
        }

        // if level to load on level node is greater than player save or level is equal to number of all levels + 1 (empty place for global scoreboard) raise locked flag
        if (levelToLoad > PlayerPrefs.GetInt("Unlocked Level") || levelToLoad == GameManager.numberOfLevels + 1)
        {
            locked = true;
        }
    }

    // when player enter World Node sphere collider this method will raise inRagne flag showing worldDetails.
    // also checking whether the player has an opportunity for restarting level, restart is available if player is already in level scoreboard
    // when player hit Action button (space/e) and locked or restart flag is not raised, LoadLevel method will be called and player will be moved to X level.
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            restartAvailable = GameManager.RestartAvailable(levelToLoad);
            if (Input.GetButtonDown("Action") && !locked && !restartAvailable)
            {
                LoadLevel();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            inRange = true;
        }
    }
    // when player leaves world node range, inRange flag will be lowered
    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }

    // method responsible for loading level which is stored in levelToLoad variable
    public void LoadLevel()
    {
        SceneManager.LoadScene(levelToLoad);
        GameManager.currentLevel = levelToLoad;
    }

    // GUI section responsible for showing world details only when flag is raised
    private void OnGUI()
    {
        GUI.skin = skinLevelSelect;
        // if inRange flag is raised = player collides with level select sphere collider
        if (inRange)
        {
            // Boxes and labels with player info for level 0 - global scoreboard
            if (levelToLoad == GameManager.numberOfLevels + 1 && GameManager.gameDone)
            {
                GUI.Box(levelScoreBoardRect, $"Scoreboard");
                GUI.Label(levelPlayerListRect, GameManager.ShowScores(20, levelToLoad)[0], skinLevelSelect.GetStyle("Scores"));
                GUI.Label(levelScoreListRect, GameManager.ShowScores(20, levelToLoad)[1], skinLevelSelect.GetStyle("Scores"));
                if (GUI.Button(submitButton, "Submit Score"))
                {
                    inRange = false;
                    GameManager.CongratulationsWindow();
                }
            }
            else if (levelToLoad == GameManager.numberOfLevels + 1)
            {
                GUI.Box(levelScoreBoardRect, $"Scoreboard");
                GUI.Label(levelPlayerListRect, GameManager.ShowScores(20, levelToLoad)[0], skinLevelSelect.GetStyle("Scores"));
                GUI.Label(levelScoreListRect, GameManager.ShowScores(20, levelToLoad)[1], skinLevelSelect.GetStyle("Scores"));
            }
            // Boxes and labels with player info for locked level X node
            else if (locked)
            {
                GUI.Box(levelScoreBoardRect, $"Level {levelToLoad} Scoreboard");
                GUI.Label(levelPlayerListRect, GameManager.ShowScores(10, levelToLoad)[0], skinLevelSelect.GetStyle("Scores"));
                GUI.Label(levelScoreListRect, GameManager.ShowScores(10, levelToLoad)[1], skinLevelSelect.GetStyle("Scores"));
                GUI.Label(levelDetailsRect, lockedLevelDetails);
            }
            // Boxes, labels and button for restart available state of level X node
            else if (restartAvailable)
            {
                GUI.Box(levelScoreBoardRect, $"Level {levelToLoad} Scoreboard");
                GUI.Label(levelPlayerListRect, GameManager.ShowScores(10, levelToLoad)[0], skinLevelSelect.GetStyle("Scores"));
                GUI.Label(levelScoreListRect, GameManager.ShowScores(10, levelToLoad)[1], skinLevelSelect.GetStyle("Scores"));
                GUI.Label(playerLevelScoreRect, GameManager.LevelScoreComparer(levelToLoad, GameManager.playerName), 
                    skinLevelSelect.GetStyle("Player Level Score"));
                // if player press restart button he will be moved to level X and his scoreboard record will be removed making place for new score
                if (GUI.Button(restartButton, "Restart"))
                    {
                        GameManager.RemoveLevelScore(GameManager.playerName, levelToLoad);
                        LoadLevel();
                    }
            }
            // Boxes, labels for unlocked level X node without restart option 
            else
            {
                GUI.Box(levelScoreBoardRect, $"Level {levelToLoad} Scoreboard");
                GUI.Label(levelPlayerListRect, GameManager.ShowScores(10, levelToLoad)[0], skinLevelSelect.GetStyle("Scores"));
                GUI.Label(levelScoreListRect, GameManager.ShowScores(10, levelToLoad)[1], skinLevelSelect.GetStyle("Scores"));
                GUI.Label(playerLevelScoreRect, GameManager.LevelScoreComparer(levelToLoad, GameManager.playerName), 
                    skinLevelSelect.GetStyle("Player Level Score"));
                GUI.Label(levelDetailsRect, unlockedLevelDetails);
            }
        }
    }
}
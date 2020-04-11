using UnityEngine;
using UnityEngine.SceneManagement;

// Class responsible for tracking collision with World Node in World Select level.
public class LevelSelect : MonoBehaviour
{
    [SerializeField] private int levelToLoad = 0;  // variable storing value of level to load, accesible from inspektor/World Node
    [SerializeField] private GameObject padlock = null;
    [SerializeField] private GUISkin skinLevelSelect = null;
    private string unlockedLevelDetails;
    private string lockedLevelDetails;
    private bool inRange;  // flag, we will raise flag when player is colliding with World Node and lower flag when player exit trigger
    private bool locked;
    private Rect levelDetailsRect;  // rect for world details label

    private float screenWidth;
    private float screenHeight;
    private Rect levelScoreBoardRect;
    private Rect levelPlayerListRect;
    private Rect levelScoreListRect;
    private Rect playerLevelScoreRect;
    private Color playerScoreColor = new Color(0, 0, 0);
    private Color playerNoScoreColor = new Color(255, 0, 0);
    
    void Start()
    {
        inRange = false;
        locked = false;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        levelDetailsRect = new Rect(Screen.width * .01f, Screen.height * .95f, 400, 150);
        unlockedLevelDetails = $"Press [SPACE] or [E]  to load level {levelToLoad}.";
        lockedLevelDetails = $"Complete level {levelToLoad - 1} to unlock.";

        levelScoreBoardRect = new Rect(screenWidth - screenWidth * .95f, screenHeight - screenHeight * .95f, screenWidth * .3f, screenHeight * .6f);
        levelPlayerListRect = new Rect(levelScoreBoardRect.x + levelScoreBoardRect.x * 1f, levelScoreBoardRect.y + levelScoreBoardRect.y * 1f, 100, 400);
        levelScoreListRect = new Rect(levelScoreBoardRect.x + levelScoreBoardRect.x * 4.5f, levelScoreBoardRect.y + levelScoreBoardRect.y * 1f, 100, 400);
        playerLevelScoreRect = new Rect(levelScoreBoardRect.x + levelScoreBoardRect.width * .28f, levelScoreBoardRect.height, 200, 100);

        if (levelToLoad > PlayerPrefs.GetInt("Unlocked Level"))
        {
            Instantiate(padlock,new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z - 1), Quaternion.identity);
        }
        if (levelToLoad > PlayerPrefs.GetInt("Unlocked Level") || levelToLoad == 0)
        {
            locked = true;
        }
    }

    // when player enter World Node sphere collider this method will raise flag and show worldDetails
    // when player hit Action button (space/e), LoadWorld method will be called and player will be moved to X level.
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            inRange = true;
            if (Input.GetButtonDown("Action") && !locked)
            {
                LoadWorld();
            }
        }
    }

    // when player leaves world node range, flag will be lowered
    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }

    // method responsible for loading level which is stored in worldToLoad variable
    public void LoadWorld()
    {
        SceneManager.LoadScene(levelToLoad);
        GameManager.currentLevel = levelToLoad;
    }


    // GUI section responsible for showing world details only when flag is raised
    private void OnGUI()
    {
        GUI.skin = skinLevelSelect;
        if (inRange)
        {
            if (levelToLoad == 0)
            {
                // if levelToLoad is equal to 0, don't use any label, leave empty place.
            }
            else if (locked)
            {
                GUI.Label(levelDetailsRect, lockedLevelDetails);
            }
            else
            {
                GUI.Label(levelDetailsRect, unlockedLevelDetails);
            }
            if (levelToLoad == 0)
            {
                GUI.Box(levelScoreBoardRect, $"Scoreboard");
                GUI.Label(levelPlayerListRect, GameManager.ShowScores(20, levelToLoad)[0], skinLevelSelect.GetStyle("Scores"));
                GUI.Label(levelScoreListRect, GameManager.ShowScores(20, levelToLoad)[1], skinLevelSelect.GetStyle("Scores"));
            }
            else
            {
                GUI.Box(levelScoreBoardRect, $"Level {levelToLoad} Scoreboard");
                GUI.Label(levelPlayerListRect, GameManager.ShowScores(10, levelToLoad)[0], skinLevelSelect.GetStyle("Scores"));
                GUI.Label(levelScoreListRect, GameManager.ShowScores(10, levelToLoad)[1], skinLevelSelect.GetStyle("Scores"));
            }
            GUI.Label(playerLevelScoreRect, GameManager.LevelScoreComparer(levelToLoad, GameManager.playerName), skinLevelSelect.GetStyle("Player Level Score"));
        }
    }
}

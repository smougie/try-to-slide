using UnityEngine;
using UnityEngine.SceneManagement;

// Class responsible for tracking collision with World Node in World Select level.
public class LevelSelect : MonoBehaviour
{
    [SerializeField] private int levelToLoad = 0;  // variable storing value of level to load, accesible from inspektor/World Node
    [SerializeField] private GameObject padlock = null;
    private string levelDetails;
    private bool inRange;  // flag, we will raise flag when player is colliding with World Node and lower flag when player exit trigger
    private Rect levelDetailsRect;  // rect for world details label
    
    void Start()
    {
        inRange = false;
        levelDetailsRect = new Rect(Screen.width * .01f, Screen.height * .95f, 400, 150);
        levelDetails = $"Press [SPACE] or [E]  to load level {levelToLoad}.";
        if (levelToLoad > PlayerPrefs.GetInt("Unlocked Level"))
        {
            Instantiate(padlock,new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z - 1), Quaternion.identity);

        }
    }

    // when player enter World Node sphere collider this method will raise flag and show worldDetails
    // when player hit Action button (space/e), LoadWorld method will be called and player will be moved to X level.
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            inRange = true;
            if (Input.GetButtonDown("Action"))
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
    }

    // GUI section responsible for showing world details only when flag is raised
    private void OnGUI()
    {
        if (inRange)
        {
            GUI.Label(levelDetailsRect, levelDetails);
        }
    }
}

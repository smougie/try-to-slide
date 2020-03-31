using UnityEngine;
using UnityEngine.SceneManagement;

// Class responsible for tracking collision with World Node in World Select level.
public class WorldSelect : MonoBehaviour
{
    [SerializeField] private int worldToLoad = 0;  // variable storing value of level to load, accesible from inspektor/World Node
    private string worldDetails;
    private bool showWorldDetails;  // flag, we will raise flag when player is colliding with World Node and lower flag when player exit trigger
    private Rect worldDetailsRect;  // rect for world details label
    
    void Start()
    {
        showWorldDetails = false;
        worldDetailsRect = new Rect(10, Screen.height - 30, 400, 150);
        worldDetails = $"Press [SPACE] or [E]  to load level {worldToLoad}.";
    }

    // when player enter World Node sphere collider this method will raise flag and show worldDetails
    // when player hit Action button (space/e), LoadWorld method will be called and player will be moved to X level.
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            showWorldDetails = true;
            if (Input.GetButtonDown("Action"))
            {
                LoadWorld();
            }
        }
    }

    // when player leaves world node range, flag will be lowered
    private void OnTriggerExit(Collider other)
    {
        showWorldDetails = false;
    }

    // method responsible for loading level which is stored in worldToLoad variable
    public void LoadWorld()
    {
        SceneManager.LoadScene(worldToLoad);
    }

    // GUI section responsible for showing world details only when flag is raised
    private void OnGUI()
    {
        if (showWorldDetails)
        {
            GUI.Label(worldDetailsRect, worldDetails);
        }
    }
}

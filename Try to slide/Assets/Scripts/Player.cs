using UnityEngine;

// Player class responsible for moving player, track collisions and triggers, destroying player and controlling sound effects.
public class Player : MonoBehaviour
{
    #region Variables

    [SerializeField] private float moveSpeed = 0;  // player movement speed, accesible from inspector
    [SerializeField] private GameObject deathParticles = null;  // insert death particles object here
    [SerializeField] private GameObject physicalOrbPrefab = null;  // variable storing Physical Orb Prefab 
    [SerializeField] private GameObject fireOrbPrefab = null;  // variable storing Fire Orb Prefab
    [SerializeField] private GameObject gasOrbPrefab = null;  // variable storing Gas Orb Prefab
    [SerializeField] private GameObject iceOrbPrefab = null;  // variable storing Ice Orb Prefab
    [SerializeField] private Material normalStance = null;  // material for normal stance
    [SerializeField] private Material InviolableStance = null;  // material for invictible stance
    [SerializeField] private AudioClip[] audioClips = null;  // audio clips array for player sounds

    private Rigidbody playerRb;  // player rigidbody variable
    private Vector3 input;  // variable create to manipulate player position by using arrow keys
    private Vector3 spawnPoint;  // spawn point for player, use game object inside Unity to change it by moving
    private Renderer playerRenderer;  // variable storing player renderer necessary
    private GameObject playerBuff;  // variable storing currently spawned player buff object
    private GameObject shrineObject;  // variable storing current shrine object untill game will respawn shrine

    private float maxSpeed = 7.5f;  // maximum movement speed value
    private float InviolableTime;  // period of time for invictible buff
    private float elementalProtTime;  // period of time for elemental protection buff
    private string currentElementalProt;  // string storing string with tag

    private bool lifeIsFull;  // life is full flag, raising when player try to collect life node and life is full
    private bool gasImmune;  // flag for Gas resistance
    private bool fireImmune;  // flag for Fire resistance
    private bool iceImmune;  // flag for Ice resistance
    private bool physicalImmune;  // Flag for Physical resistance

    string gasProt = "Gas Prot";  // variable storing gas tag
    string fireProt = "Fire Prot";  // variable storing fire tag
    string iceProt = "Ice Prot";  // variable storing ice tag
    string physicalProt = "Physical Prot";  // variable storing physical tag

    private float shrineCounter;  // variable storing shrine respawn counter

    #endregion

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();  // initializing player rigid body
        spawnPoint = transform.position;  // initializing player spawn point by taking current position of game object
        playerRenderer = GetComponent<Renderer>();  // initializing player renderer
    }


    private void Update()
    {
        // Tracking status of buff counter, if greater than 0, count down to 0, if less than 0, set to 0 and turn off buff using ElementalProtOff() method
        if (elementalProtTime > 0)
        {
            elementalProtTime -= Time.deltaTime;
        }
        if (elementalProtTime < 0)
        {
            elementalProtTime = 0;
            ElementalProtOff();
        }
        // Tracking status of shrine respawn counter, if greater than 0, count down to 0, if less than 0, set to 0 and turn on shrine object (respawn it)
        if (shrineCounter > 0)
        {
            shrineCounter -= Time.deltaTime;
        }
        if (shrineCounter < 0)
        {
            shrineCounter = 0;
            shrineObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        // variable storing Unity Input position in vector3, necessary to manipulate with add force and create player movement
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // statement responsible for making player movement smooth and skip progression in move
        if (playerRb.velocity.magnitude < maxSpeed)
        {
            playerRb.AddForce(input * moveSpeed);  // if we press left arrow, we will multiply input value by moveSpeed
        }

        // killing player when he fall from floor
        if (transform.position.y <= -2)
        {
            Die();
        }

    }

    // tracking player collisions with other objects - currently checking name and physical immune, if player collide with enemy and he is not phys immune he will die
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Enemy" && !physicalImmune)
        {
            Die();
        }
    }

    // tracking player triggering with door objects, goal
    private void OnTriggerEnter(Collider other)
    {
        // Goal trigger CompleteLevel method and playing sound
        if (other.transform.tag == "Goal")
        {
            PlaySound(2);
            GameManager.CompleteLevel();
        }

        if (other.transform.tag == "Key")
        {
            PlaySound(9);
        }

        if (other.transform.tag == "Switch")
        {
            PlaySound(10);
        }

        if (other.transform.tag == "Platform")
        {
            PlaySound(11);
        }

        if (other.transform.tag == "Bar")
        {
            PlaySound(12);
        }
    }
    // tracking player triggering other objects
    private void OnTriggerStay(Collider other)
    {

        // Coin trigger CoinPickUp method and destroy coin object and play sound
        if (other.transform.tag == "Coin")
        {
            PlaySound(1);
            Destroy(other.gameObject);
            GameManager.CoinPickUp();
        }

        // Adding one life to player when he collides with life object and he's life is not full and destroy life node
        if (other.transform.tag == "Life")
        {
            if (GameManager.life < GameManager.maxLife)
            {
                GameManager.life++;
                PlaySound(3);
                Destroy(other.gameObject);
            }

            // if hp is full raising flag to display proper label with information
            else
            {
                lifeIsFull = true;
            }
        }

        // Adding 1 point to maximum player life and destroy game object
        if (other.transform.tag == "Max Life")
        {
            GameManager.maxLife++;
            GameManager.life++;
            PlaySound(4);
            Destroy(other.gameObject);
        }

        //  section with statements which are responsible for killing player if he is not resistant to elemental and he trigger trap
        if (other.transform.tag == "Gas" && !gasImmune)
        {
            Die();
        }

        if (other.transform.tag == "Fire" && !fireImmune)
        {
            Die();
        }

        if (other.transform.tag == "Ice" && !iceImmune)
        {
            Die();
        }

        if (other.transform.tag == "Physical" && !physicalImmune)
        {
            Die();
        }

        // section with statements repsonsible for picking up buff shrine, playing sound and triggering buff on player using ElementalProtOn() method
        if (other.transform.tag == "Gas Prot" || other.transform.tag == "Fire Prot" || other.transform.tag == "Ice Prot" || other.transform.tag == "Physical Prot")
        {
            if (other.transform.tag == "Gas Prot")
            {
                ElementalProtOn(other.transform.tag, other.transform.GetComponent<ElementalProtShrine>().elementalProtTime);
                PlaySound(7);
            }
            else if (other.transform.tag == "Fire Prot")
            {
                ElementalProtOn(other.transform.tag, other.transform.GetComponent<ElementalProtShrine>().elementalProtTime);
                PlaySound(5);
            }
            else if (other.transform.tag == "Ice Prot")
            {
                ElementalProtOn(other.transform.tag, other.transform.GetComponent<ElementalProtShrine>().elementalProtTime);
                PlaySound(6);
            }
            else if (other.transform.tag == "Physical Prot")
            {
                ElementalProtOn(other.transform.tag, other.transform.GetComponent<ElementalProtShrine>().elementalProtTime);
                PlaySound(8);
            }
            // if shrine is set to respawn, turning off shrine game object and starting counter
            if (other.transform.GetComponent<ElementalProtShrine>().respawnShrine)
            {
                other.gameObject.SetActive(false);
                StartShrineCounter(other.gameObject, other.transform.GetComponent<ElementalProtShrine>().elementalProtTime);

            }
            // if shrine is not set to respawn, destroying shrine game object
            if (!other.transform.GetComponent<ElementalProtShrine>().respawnShrine)
            {
                Destroy(other.gameObject);
            }
        }
    }

    // if player leaves life node while he is on full hp, drop flag and stop showing label with info
    private void OnTriggerExit(Collider other)
    {
        lifeIsFull = false;
    }

    // Method responsible for playing sound from array with sounds
    private void PlaySound(int clipIndex)
    {
        AudioSource sound = GetComponent<AudioSource>();
        sound.PlayOneShot(audioClips[clipIndex]);
    }

    // Method responsible for instantiate death particles in place of player death, moving player to spawn point and playing sound
    private void Die()
    {
        PlaySound(0);
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        transform.position = spawnPoint;
        GameManager.life -= 1;
    }

    // Method responsible for starting shrine counter, setting shrine object and changing counter time from 0 to shrineRespawnTime
    private void StartShrineCounter(GameObject shrine, float shrineRespawnTime)
    {
        shrineObject = shrine;
        shrineCounter = shrineRespawnTime;
    }

    // Method responsible for setting resistant buff type, time, spawning buff effect around player object
    // Also method is responsible for dealing with collecting another shrine while already one is active
    // If that happens, method is setting latest buff by removing old one
    private void ElementalProtOn(string elementalTag, int protTime)
    {
        elementalProtTime = protTime;  // setting buff time 
        Destroy(playerBuff);  // destroying current buff effect
        playerRenderer.material = normalStance;  // setting player.material for normal stance
        if (elementalTag == gasProt)
        {
            gasImmune = true;
            fireImmune = false;
            iceImmune = false;
            currentElementalProt = gasProt;
            playerBuff = SpawnPlayerBuff(gasOrbPrefab);
        }
        if (elementalTag == fireProt)
        {
            fireImmune = true;
            gasImmune = false;
            iceImmune = false;
            currentElementalProt = fireProt;
            playerBuff = SpawnPlayerBuff(fireOrbPrefab);
        }
        if (elementalTag == iceProt)
        {
            iceImmune = true;
            fireImmune = false;
            gasImmune = false;
            currentElementalProt = iceProt;
            playerBuff = SpawnPlayerBuff(iceOrbPrefab);
        }
        if (elementalTag == physicalProt)
        {
            physicalImmune = true;
            gasImmune = false;
            fireImmune = false;
            iceImmune = false;
            currentElementalProt = physicalProt;
            playerRenderer.material = InviolableStance;
            ApplyIgnoreCollision();
            playerBuff = SpawnPlayerBuff(physicalOrbPrefab);
        }
    }

    // Method responsible for turning off Elemental Protection Buff
    private void ElementalProtOff()
    {

        Destroy(playerBuff.gameObject);  // destroying player buff effect
        // turning off resistance
        if (currentElementalProt == gasProt)
        {
            gasImmune = false;
        }
        if (currentElementalProt == fireProt)
        {
            fireImmune = false;
        }
        if (currentElementalProt == iceProt)
        {
            iceImmune = false;
        }
        if (currentElementalProt == physicalProt)
        {
            physicalImmune = false;
            playerRenderer.material = normalStance;  // reseting player material to normal
            RemoveIgnoreCollision();  // turning collision with Enemy on
        }
        currentElementalProt = "";

    }

    // Method responsible for spawning player buff effect
    private GameObject SpawnPlayerBuff(GameObject buffToSpawn)
    {
        return Instantiate(buffToSpawn, gameObject.transform);
    }

    // Method responsible for turning off collision by player. Player can now move through enemies
    private void ApplyIgnoreCollision()
    {
        // setting ignore collision to all "Enemy" objects
        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name == "Enemy")
            {
                Physics.IgnoreCollision(gameObj.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            }
        }
    }

    // Method responsible for turning on player collision. Player can't move though enemies anymore
    private void RemoveIgnoreCollision()
    {
        // turning off ignore collision from all "Enemy" objects
        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name == "Enemy")
            {
                Physics.IgnoreCollision(gameObj.GetComponent<Collider>(), gameObject.GetComponent<Collider>(), false);
            }
        }
    }

    // Method responsible for showing life is full while player is on full hp and try to collect life node
    // also shows timer for invictible buff in bot center of screen
    private void OnGUI()
    {
        string gasResString = $"Gas resistant for {elementalProtTime.ToString("0.0")} seconds.";
        string fireResString = $"Fire resistant for {elementalProtTime.ToString("0.0")} seconds.";
        string iceResString = $"Ice resistant for {elementalProtTime.ToString("0.0")} seconds.";
        string physicalResString = $"Invulnerable for {elementalProtTime.ToString("0.0")} seconds.";

        if (lifeIsFull)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height * .95f, 200, 20), "Life is full");
        }
        if (gasImmune)
        {
            GUI.Label(new Rect(Screen.width / 2 - 70, Screen.height * .90f, 400, 400), gasResString);
        }
        if (fireImmune)
        {
            GUI.Label(new Rect(Screen.width / 2 - 70, Screen.height * .90f, 400, 400), fireResString);
        }
        if (iceImmune)
        {
            GUI.Label(new Rect(Screen.width / 2 - 70, Screen.height * .90f, 400, 400), iceResString);
        }
        if (physicalImmune)
        {
            GUI.Label(new Rect(Screen.width / 2 - 70, Screen.height * .90f, 400, 400), physicalResString);
        }
    }
}

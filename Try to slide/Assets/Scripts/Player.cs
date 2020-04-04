using UnityEngine;

// Player class responsible for moving player, track collisions and triggers, destroying player and controlling sound effects.
public class Player : MonoBehaviour
{
    private Vector3 spawnPoint;  // spawn point for player, use game object inside Unity to change it by moving
    [SerializeField] private GameObject deathParticles = null;  // insert death particles object here
    [SerializeField] private AudioClip[] audioClips = null;
    private Rigidbody playerRb;  // player rigidbody variable
    private Vector3 input;  // variable create to manipulate player position by using arrow keys

    [SerializeField] private float moveSpeed = 0;  // player movement speed, accesible from inspector
    private float maxSpeed = 7.5f;  // maximum movement speed value
    private bool lifeIsFull;

    private bool invictibleIsActive;
    private float invictibleTime;
    private Renderer playerRenderer;
    [SerializeField] private Material normalStance;
    [SerializeField] private Material invictibleStance;
    

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();  // initializing player rigid body
        spawnPoint = transform.position;  // initializing player spawn point by taking current position of game object
        playerRenderer = GetComponent<Renderer>();
    }


    private void Update()
    {
        // when player falls down to 0 lifes, game ends
        if (GameManager.life <= 0)
        {
            GameManager.LoseLevel();
        }

        if (invictibleTime > 0)
        {
            invictibleTime -= Time.deltaTime;
            playerRenderer.material.SetColor("_Color", Color.black);
            
        }

        if (invictibleTime <= 0)
        {
            InvictibleOff();
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


    // tracking player collisions with other objects
    void OnCollisionEnter(Collision collision)
    {
        // collision with enemy 
        if (collision.transform.tag == "Enemy")
        {
            if (!invictibleIsActive)
            {
                Die();
            }
        }
    }


    // tracking player triggering other objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Invictible")
        {
            InvictibleOn();
            Destroy(other.gameObject);

        }
        // Goal trigger CompleteLevel method
        if (other.transform.tag == "Goal")
        {
            PlaySound(2);
            GameManager.CompleteLevel();
        }

        // Trap trigger Die method
        if (other.transform.tag == "Trap")
        {
            if (!invictibleIsActive)
            {
                Die();
            }
        }

        // Coin trigger CoinPickUp method and destroy coin object
        if (other.transform.tag == "Coin")
        {
            PlaySound(1);
            Destroy(other.gameObject);
            GameManager.CoinPickUp();
        }

        // Adding one life to player when he collides with life object and he's life is not full
        if (other.transform.tag == "Life")
        {
            if (GameManager.life < GameManager.maxLife)
            {
                GameManager.life++;
                Destroy(other.gameObject);
            }

            // if hp is full just notice that
            else
            {
                lifeIsFull = true;
            }
        }

        if (other.transform.tag == "Max Life")
        {
            GameManager.maxLife++;
            Destroy(other.gameObject);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        lifeIsFull = false;
    }


    private void PlaySound(int clipIndex)
    {
        AudioSource sound = GetComponent<AudioSource>();
        sound.PlayOneShot(audioClips[clipIndex]);
    }


    // Method responsible for instantiate death particles in place of player death and moving player to spawn point
    private void Die()
    {
        PlaySound(0);
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        transform.position = spawnPoint;
        GameManager.life -= 1;
    }

    private void InvictibleOn()
    {
        invictibleTime = 5f;
        invictibleIsActive = true;
        playerRenderer.material = invictibleStance;
    }

    private void InvictibleOff()
    {
        invictibleIsActive = false;
        playerRenderer.material = normalStance;
    }

    private void OnGUI()
    {
        if (lifeIsFull)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height * .95f, 200, 20), "Life is full");
        }
        if (invictibleIsActive)
        {
            GUI.Label(new Rect(Screen.width / 2 - 70, Screen.height * .90f, 400, 400), $"Invictible for {invictibleTime.ToString("0.0")} seconds.");
        }
    }
}

﻿using UnityEngine;

// Player class responsible for moving player, track collisions and triggers, destroying player and controlling sound effects.
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject deathParticles = null;  // insert death particles object here
    [SerializeField] private AudioClip[] audioClips = null;  // audio clips array for player sounds
    [SerializeField] private Material normalStance = null;  // material for normal stance
    [SerializeField] private Material invictibleStance = null;  // material for invictible stance
    [SerializeField] private float moveSpeed = 0;  // player movement speed, accesible from inspector

    private Rigidbody playerRb;  // player rigidbody variable
    private Vector3 input;  // variable create to manipulate player position by using arrow keys
    private Vector3 spawnPoint;  // spawn point for player, use game object inside Unity to change it by moving
    private Renderer playerRenderer;  // variable storing player renderer necessary

    private float maxSpeed = 7.5f;  // maximum movement speed value
    private float invictibleTime;  // period of time for invictible buff
    private float elementalProtTime;  // period of time for elemental protection buff
    private string currentElementalProt;

    private bool lifeIsFull;  // life is full flag, raising when player try to collect life node and life is full
    private bool invictibleIsActive;  // invictible buff flag, raising when player collect shield node

    [SerializeField] private GameObject shieldPrefab = null;
    [SerializeField] private GameObject fireOrbPrefab = null;
    [SerializeField] private GameObject gasOrbPrefab = null;
    [SerializeField] private GameObject iceOrbPrefab = null;
    private GameObject playerBuff;

    private bool gasImmune;
    private bool fireImmune;
    private bool iceImmune;

    string gasProt = "Gas Prot";
    string fireProt = "Fire Prot";
    string iceProt = "Ice Prot";


    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();  // initializing player rigid body
        spawnPoint = transform.position;  // initializing player spawn point by taking current position of game object
        playerRenderer = GetComponent<Renderer>();  // initializing player renderer
    }


    private void Update()
    {
        // if player pick up invictible buff this counter will start counting down buff time until time of buff will be greater than 0
        // Player cube will change color for the buff time
        if (invictibleTime > 0)
        {
            invictibleTime -= Time.deltaTime;
        }

        // if invictible time falls to 0, running method to turn off this buff
        if (invictibleTime <= 0)
        {
            InvictibleOff();
        }

        if (elementalProtTime > 0)
        {
            elementalProtTime -= Time.deltaTime;
        }
        if (elementalProtTime <= 0)
        {
            ElementalProtOff();
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
            // if invictible buff is active player can't collide with Enemy, he can pass through Enemy
            if (invictibleIsActive)
            {
                Physics.IgnoreCollision(collision.transform.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            }

            // if not while invictible buff is triggered player dies when collides with enemy
            else
            {
                Die();
            }
        }
    }


    // tracking player triggering other objects
    private void OnTriggerEnter(Collider other)
    {

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

        // shield node trigger invictible buff and destroys shield node object
        if (other.transform.tag == "Invictible")
        {
            InvictibleOn();
            Destroy(other.gameObject);
        }
        if (other.transform.tag == "Gas Prot" || other.transform.tag == "Fire Prot" || other.transform.tag == "Ice Prot")
        {
            Destroy(other.gameObject);
            if (other.transform.tag == "Gas Prot")
            {
                ElementalProtOn(other.transform.tag);
                PlaySound(7);
            }
            else if (other.transform.tag == "Fire Prot")
            {
                ElementalProtOn(other.transform.tag);
                PlaySound(5);
            }
            else if (other.transform.tag == "Ice Prot")
            {
                ElementalProtOn(other.transform.tag);
                PlaySound(6);
            }
        }
        // Goal trigger CompleteLevel method and playing sound
        if (other.transform.tag == "Goal")
        {
            PlaySound(2);
            GameManager.CompleteLevel();
        }

        // Trap trigger Die method
        if (other.transform.tag == "Trap")
        {
            // if not while invictible buff is triggered, player dies from trap spikes
            if (!invictibleIsActive)
            {
                Die();
            }
        }

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
            PlaySound(4);
            Destroy(other.gameObject);
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


    // Method responsible for turning on invictible buff, initializing buff time, raising flag and setting player color
    private void InvictibleOn()
    {
        invictibleTime = 5f;
        invictibleIsActive = true;
        playerRenderer.material = invictibleStance;
        GameObject playerBuff = Instantiate(shieldPrefab, gameObject.transform) as GameObject;
    }

    // Method resposible for turning off ivictible buff, dropping flag and setting normal color
    private void InvictibleOff()
    {
        invictibleIsActive = false;
        playerRenderer.material = normalStance;
        Destroy(playerBuff);
    }

    private void ElementalProtOn(string elementalTag)
    {
        elementalProtTime = 10f;
        Destroy(playerBuff);
        if (elementalTag == gasProt)
        {
            gasImmune = true;
            fireImmune = false;
            iceImmune = false;
            currentElementalProt = gasProt;
            
        }
        if (elementalTag == fireProt)
        {
            fireImmune = true;
            gasImmune = false;
            iceImmune = false;
            currentElementalProt = fireProt;
            //playerBuff = SpawnPlayerBuff(fireOrbPrefab);
            //playerBuff = Instantiate(fireOrbPrefab, gameObject.transform);
        }
        if (elementalTag == iceProt)
        {
            iceImmune = true;
            fireImmune = false;
            gasImmune = false;
            currentElementalProt = iceProt;
            //playerBuff = SpawnPlayerBuff(iceOrbPrefab);
        }
    }

    private void ElementalProtOff()
    {
        Destroy(playerBuff);
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
        currentElementalProt = "";
    }

    private GameObject SpawnPlayerBuff(GameObject buffToSpawn)
    {
        return Instantiate(buffToSpawn, gameObject.transform);
    }

    private void DestroyPlayerBuff(GameObject buffToDestroy)
    {
        Destroy(playerBuff);
    }

    // Method responsible for showing life is full while player is on full hp and try to collect life node
    // also shows timer for invictible buff in bot center of screen
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
        GUI.Label(new Rect(Screen.width / 2, Screen.height * .15f, 200, 20), $"Current Prot: {currentElementalProt}");
        GUI.Label(new Rect(Screen.width / 2, Screen.height * .17f, 200, 20), $"Buff time: {elementalProtTime}");
        GUI.Label(new Rect(Screen.width / 2, Screen.height * .19f, 200, 20), $"Gas: {gasImmune} | Fire: {fireImmune} | Ice: {iceImmune}");
    }
}

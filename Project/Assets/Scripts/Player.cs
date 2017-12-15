using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Getting the players rigidbody
    Rigidbody2D rb = null;

    // player travel too fast in air
    public float maxHorizontalSpeed = 8.0f;

    // Speed of the player
    public float PlayerSpeed = 0;
    GameManager scpGameManager = null;
    MapLoader ml;
    // Force of jump
    public float PlayerJump = 0;

    // Did the player jump
    private bool Jump = false;
    private float JumpCount = 0.0f;
    public float JumpTime = 0.0f;
    private bool CanJump = false;
    bool inair = false;
    bool collidingwithground = false;

    // Use this for initialization
    void Start()
    {
        // finding the GameManager because you cannot assign it to the prefab
        if (GameObject.Find("GameManager"))
            scpGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        ml = GameObject.FindObjectOfType<MapLoader>();
        rb = GetComponent<Rigidbody2D>();
        scpGameManager.LevelSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 RayVector2 = (Vector2)gameObject.transform.position - new Vector2(0.0f, 0.555f);

        RaycastHit2D hit2D = Physics2D.Raycast(RayVector2, gameObject.transform.up * -1);

        Debug.Log(scpGameManager.LevelSpawn);
        if (scpGameManager.LevelSpawn)
        {
            // world white, platform black player wont land so set world to black
            if (!scpGameManager.isDark && !hit2D)
            {
                Debug.Log("found platform");
                scpGameManager.isDark = true;
                scpGameManager.LevelSpawn = false;
                scpGameManager.FragmentCount = 0;
            }
            // world white, platform white player will land
            else if (!scpGameManager.isDark && hit2D.transform.tag == "PlatformWhite")
            {
                Debug.Log("found platform");
                scpGameManager.isDark = false;
                scpGameManager.LevelSpawn = false;
                scpGameManager.FragmentCount = 0;
            }
            // world black, platform black player wont land so set world to white
            if (scpGameManager.isDark && !hit2D)
            {
                Debug.Log("found platform");
                scpGameManager.isDark = false;
                scpGameManager.LevelSpawn = false;
                scpGameManager.FragmentCount = 0;
            }
            // world black, platform black player will land
            else if (scpGameManager.isDark && hit2D.transform.tag == "PlatformBlack")
            {
                Debug.Log("found platform");
                scpGameManager.isDark = true;
                scpGameManager.LevelSpawn = false;
                scpGameManager.FragmentCount = 0;
            }
        }
        Debug.Log(inair + " is inair");
        // stop x velocity if not inteding
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) || inair && !collidingwithground)
        {
            Vector2 v2;
            Vector2 v2x;
            float vx = Mathf.Lerp(rb.velocity.x, 0.0f, Time.deltaTime*5);
            inair = true;
            v2x.x = vx;
            v2.x = v2x.x;
            v2.y = rb.velocity.y;
            rb.velocity = v2;

            if (rb.velocity.x > 0.5f)
                inair = false;
        }

        // stop the player moving too fast left and right
        if (rb.velocity.x > maxHorizontalSpeed)
        {
            Vector2 vv2;
            vv2.x = maxHorizontalSpeed;
            vv2.y = rb.velocity.y;
            rb.velocity = vv2;
        }

        // movement controls
        // move left
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(Vector2.left * PlayerSpeed, ForceMode2D.Impulse);

        // move right
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(-Vector2.left * PlayerSpeed, ForceMode2D.Impulse);

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && CanJump)
        {
            Jump = true;
            // in the air so they cannot jump again
            CanJump = false;
        }

        // jump counter (jump for a certain time)
        if (Jump && JumpCount < JumpTime)
        {
            // moving the player upwards (jumping)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + PlayerJump);
            // counting the jump time
            JumpCount += Time.deltaTime;
        }
        else if (JumpCount > JumpTime)
        {
            // finished jumping
            Jump = false;
            JumpCount = 0.0f;
        }

        if (transform.position.y < -15.0f)
        {
            ml.RestartMap();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // if colliding with ground the player can jump
        if (col.transform.tag.Contains("Platform"))
        {
            collidingwithground = true;
            CanJump = true;
        }

        if (col.transform.tag == "Fragment")
        {
            scpGameManager.FragmentCount++;
            Destroy(col.gameObject);
        }
        if (col.gameObject.GetComponent<Enemy>() != null)
        {
            ml.RestartMap();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Portal")
        {
            if (scpGameManager.FragmentCount == scpGameManager.RemFragments.Count)
            {
                //Bad code. But is for a jam. so meh.........
                MapLoader ml = GameObject.FindObjectOfType<MapLoader>();
                ml.NextMap();
            }
        }

    }
}

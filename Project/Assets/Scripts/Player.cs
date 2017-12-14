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
    float maxHorizontalSpeed = 8.0f;

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


    // Use this for initialization
    void Start()
    {
        // finding the GameManager because you cannot assign it to the prefab
        if (GameObject.Find("GameManager"))
            scpGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        ml = GameObject.FindObjectOfType<MapLoader>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 RayVector2 = (Vector2)transform.position - new Vector2(0.0f, 0.555f);

        RaycastHit2D hit = Physics2D.Raycast(RayVector2, gameObject.transform.up * -1);
        if (hit && scpGameManager.LevelSpawn)
        {
            scpGameManager.isDark = true;
            scpGameManager.LevelSpawn = false;
            scpGameManager.FragmentCount = 0;
        }
        if (!hit && scpGameManager.LevelSpawn)
        {
            scpGameManager.isDark = false;
            scpGameManager.LevelSpawn = false;
            scpGameManager.FragmentCount = 0;
        }

        // stop the player moving too fast left and right
        if (rb.velocity.x > maxHorizontalSpeed)
        {
            Vector2 v2;
            v2.x = maxHorizontalSpeed;
            v2.y = rb.velocity.y;
            rb.velocity = v2;
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
            CanJump = true;

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

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Getting the players rigidbody
    Rigidbody2D rb = null;

    // Speed of the player
    public float PlayerSpeed = 0;

    // Force of jump
    public float PlayerJump = 0;

    // Did the player jump
    private bool Jump = false;
    private float JumpCount = 0.0f;
    public float JumpTime = 0.0f;
    private bool CanJump = false;

    // Number of fragments that the player has
    private int FragmentCount = 0;

    // REM Fragment count
    public Text FragmentsText = null;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Updating how many fragments the player has (visually)
        FragmentsText.text = "Fragments " + FragmentCount;

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

        // shift worlds
        //if (Input.GetKey(KeyCode.LeftShift))
        //Debug.Log("Change Light/Dark");
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // if colliding with ground the player can jump
        if (col.transform.tag.Contains("Platform"))
            CanJump = true;

        if (col.transform.tag == "Fragment")
        {
            FragmentCount++;
            Destroy(col.gameObject);
        }
        if (col.transform.tag == "Killzone")
            Destroy(gameObject);
    }
}

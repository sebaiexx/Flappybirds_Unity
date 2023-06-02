using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Bird : MonoBehaviour
{
    private const float jump_amount = 85f;
    public event EventHandler OnDied;
    private static Bird instance;

    public bool isDead = false;
    public static Bird GetInstance()
    {
        return instance;
    }

    public event EventHandler OnStartedPlaying;

    private Rigidbody2D birdrigidbody2D;

    private State state;
    private enum State
    {
        WaitingToStart
            ,Playing
            ,Dead
    }
    private void Awake()
    {
        instance = this;
        birdrigidbody2D = GetComponent<Rigidbody2D>();
        birdrigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }
    private void jump()
    {
        birdrigidbody2D.velocity = Vector2.up * jump_amount;
    }
    private void Update()
    {
        OffScene();

        switch (state) 
        {
            default:
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    birdrigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    jump();
                    if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
                }
                break;
             case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    jump();
                }
                break;
            case State.Dead:
            {
                break;
            }
                
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        isDead = true;
        birdrigidbody2D.bodyType = RigidbodyType2D.Static;
        if (OnDied != null) OnDied(this, EventArgs.Empty);
    }
    private void OffScene()
    {
        if(transform.position.y < -Camera.main.orthographicSize)
        {
            isDead = true;
            if (OnDied != null) OnDied(this, EventArgs.Empty);
        }
    }
    
}

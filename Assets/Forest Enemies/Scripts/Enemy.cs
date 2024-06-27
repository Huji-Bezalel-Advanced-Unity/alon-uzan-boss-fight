using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    
    public Animator animator;
    public bool isFlying;
    public bool walk;
    public bool run;

    private float xAxis;
    private float yAxis;
    private string currentAnimaton;
    private Rigidbody2D rb2d;
    public float smoothTime = 0.3f;

    public string PLAYER_IDLE;
    public string PLAYER_ATTACK;
    public string PLAYER_WALK;
    public string PLAYER_RUN;
    public string PLAYER_DEATH;
    public string PLAYER_FLY;
    public string PLAYER_FLY_ATTACK;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); 

    }

    private void Update()
    {
        Vector2 vel = new Vector2(1, 0);

        if (walk)
        {
            rb2d.velocity = vel * walkSpeed;
        }
        else if (run)
        {
            rb2d.velocity = vel * runSpeed;

        }

        if (transform.position.x > 7.74f) 
        {
            transform.position = new Vector2(-10.25f, transform.position.y);
        }

    }

    public void ChangeAnimationState(string newAnimation, bool setWalk, bool setRun)
    {
        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
        walk = setWalk;
        run = setRun;
    }

 
}
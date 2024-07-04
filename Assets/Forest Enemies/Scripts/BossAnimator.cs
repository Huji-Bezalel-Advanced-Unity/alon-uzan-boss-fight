using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossAnimator : MonoBehaviour
{
    public static BossAnimator Instance { get; private set; }

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void ChangeAnimationState(string newAnimation, bool setWalk, bool setRun)
    {
        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
        walk = setWalk;
        run = setRun;
    }
}
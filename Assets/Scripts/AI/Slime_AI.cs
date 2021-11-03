using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_AI : MonoBehaviour
{
    //Public Fields
    public float explodeRange = 1;
    public GameObject explosionPrefab;


    private MoveAndRotate chaserScript;
    private Vector2 startingLocation;

    Animator animator;
    Health healthScript;
    //Rigidbody2D rb;

    //State Machine
    private enum State
    {
        idle, chasing, returning,explode
    }

    private Vector2 startingPosition;
    [SerializeField] private State state;

    private void Awake()
    {
        chaserScript = GetComponent<MoveAndRotate>();
        animator = GetComponent<Animator>();
        healthScript = GetComponent<Health>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startingLocation = transform.position;
        state = State.idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.idle:

                if (chaserScript.distancetoTarget() <= chaserScript.activationRange)
                {
                    state = State.chasing;
                }
                break;
            case State.chasing:
                animator.SetBool("Chasing", true);
                if (chaserScript.distancetoTarget() > chaserScript.activationRange * 2)
                {
                    animator.SetBool("Chasing", false);
                    state = State.returning;
                } else if (chaserScript.distancetoTarget() < explodeRange)
                {
                    animator.SetBool("Chasing", false);
                    state = State.explode;
                } else
                    chaserScript.ChaseTarget();
                break;
            case State.returning:
                //TODO IMPLEMENT THIS
                state = State.idle;
                break;
            case State.explode:
                //animator.SetTrigger("Explode");
                animator.Play("slime_explode");
                break;
               
        }
    }

    void Kamikaze()
    {
        //Debug.Log("REEEEEEEEEEEEEEEEE");
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        healthScript.Die();
    }
}

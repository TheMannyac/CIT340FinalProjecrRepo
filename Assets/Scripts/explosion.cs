using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public int damage = 25;
    public float knockbackPower = 8;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SoundManager.instance.PlaySound("squish");
    }


    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {  //If normalizedTime is 0 to 1 means animation is playing, if greater than 1 means finished
            Debug.Log("not playing");

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("playing");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Health>().changeHP(damage);
            Rigidbody2D enemyRB = collision.GetComponent<Rigidbody2D>();
  
            Vector2 dir = ((collision.transform.position - this.transform.position).normalized) * knockbackPower;
            enemyRB.AddForce(dir, ForceMode2D.Impulse);

        }
    }
}

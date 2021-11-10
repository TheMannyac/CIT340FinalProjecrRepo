using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    static PlayerControls playerReference;
    public float speed = 5.0f;
    public int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        //if (playerReference == null)
            //playerReference = FindObjectOfType<PlayerControls>();

        //SoundManager.PlaySound("Gunshot");
        //SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //sr.color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(new Vector3(Time.deltaTime * speed, 0, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //gameObject; //refers to the object this script is on (the laser)
        //collision.gameObject; //refers to the object we collided with 

        if(collision.gameObject.tag == "Asteroid")
        {
            Destroy(collision.gameObject);
        } 

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
           Debug.Log("hit has occured");
           collision.gameObject.GetComponent<Health>().takeDamage(damage);

        }
           
        Destroy(gameObject);
    }
}

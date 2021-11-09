using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    //be careful:
    //C# Collider2D[] array;
    //C++ Collider2D array[];

    private static Collider2D[] colliders = new Collider2D[50];

    //A ContactFilter is used to filter out collisions so that we only check
    //the objects we care about.
    private static ContactFilter2D contactFilter = new ContactFilter2D();

    public float bulletsPerSecond = 2;
    public GameObject bulletPrefab;
    
    private Transform target;
    private CircleCollider2D detectionArea;

    // Start is called before the first frame update
    void Start()
    {
        detectionArea = GetComponent<CircleCollider2D>();
        contactFilter.useTriggers = true;
        contactFilter.SetLayerMask(LayerMask.GetMask("Enemies"));
        StartCoroutine(TargetAndShoot());
    }

    //CoRoutine
    IEnumerator TargetAndShoot()
    {
        while (true)
        {
            target = null;
            int numCollisions = detectionArea.OverlapCollider(contactFilter, colliders);

            if (numCollisions == 0)
            {
                yield return new WaitForSeconds(.25f);//Look for enemies in .25 seconds
                continue;
            }
            else
            {
                target = colliders[0].gameObject.transform;
                Vector3 direction = target.position - transform.position;
                direction.z = 0;
                transform.up = direction;
                Instantiate(bulletPrefab, transform.position, transform.rotation);
                yield return new WaitForSeconds(1/bulletsPerSecond);
            }
        }
    }
}

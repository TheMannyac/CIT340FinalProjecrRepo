using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathos : MonoBehaviour
{
    public static Pathos current;

    SoulShard_AttackManager stateHandler;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
    private void Awake()
    {
        current = this;
        stateHandler = GetComponent<SoulShard_AttackManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}

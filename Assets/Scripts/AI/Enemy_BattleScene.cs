using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BattleScene : MonoBehaviour
{

    [HideInInspector] public Vector2 battleStation; //where this enemy returns to between turns

    // Start is called before the first frame update
    void Start()
    {
        //All gameObjects with this script have the Enemies Layer mask
        gameObject.layer = LayerMask.NameToLayer("Enemies");

        BattleManager.instance.OnEnemyTurnEnter += OnEnemyTurnEnter;
        BattleManager.instance.OnEnemyTurnExit += OnEnemyTurnExit;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnemyTurnEnter()
    {
        GetComponent<WaypointManager>().enabled = true;
        enemy_ghost ghost = GetComponent<enemy_ghost>();
        ghost.enabled = true;
    }

    void OnEnemyTurnExit()
    {
        GetComponent<enemy_ghost>().enabled = false;
        GetComponent<WaypointManager>().enabled = false;
        transform.position = battleStation;
    }
}

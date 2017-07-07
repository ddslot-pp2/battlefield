using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    GameObject[] enemyTank;
    Vector3[] pos;
    public GameObject[] enemy;
	// Use this for initialization
	void Start () {
        pos = new Vector3[4];

        pos[0] = new Vector3(0, 0, 60);
        pos[1] = new Vector3(-60, 0, 0);
        pos[2] = new Vector3(60, 0, 0);
        pos[3] = new Vector3(0, 0, -60);
    }

    // Update is called once per frame
    void Update () {
        enemyTank = GameObject.FindGameObjectsWithTag("EnemyTank");

        if (enemyTank.Length <= 5)
        {
            Instantiate(enemy[0], pos[Random.Range(0, 4)], transform.rotation);
        }
	}
}

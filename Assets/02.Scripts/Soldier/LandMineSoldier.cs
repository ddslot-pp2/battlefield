using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LandMineSoldier : SoldierState {
    private GameObject attackTank; // 공격 대상자
    private GameObject targetPos; // 따라갈 타겟 주변 위치 좌표
    private NavMeshAgent nvAgent;

    private bool owner;

    private float nextfire;
    private int count;

    public GameObject Mine;
    void Awake()
    {
        hp = 50;
        owner = false;
    }

    void Start()
    {
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        moveSpeed = 10.0f;
        owner = false;
        nextfire = 0.0f;
        fireRate = 3.0f;
    }

    void Update()
    {
        if (owner == true)
        {

            NavAgent(targetPos.transform.position, 10.0f, 0.0f);
            transform.rotation = Quaternion.LookRotation((targetPos.transform.position - transform.position).normalized);

            nextfire += Time.deltaTime;
            if (nextfire >= fireRate)
            { 
                Repair();
                nextfire = 0;
            }
        }
    }

    public void NavAgent(Vector3 pos, float speed, float stopDistance)
    {
        nvAgent.destination = pos;
        nvAgent.speed = speed;
        nvAgent.stoppingDistance = stopDistance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (owner == false && other.tag == ("Tank"))
        {        
            for (int i = 0; i < other.GetComponent<Tank_State>().soldier.Length; i++)
            {
                if (other.GetComponent<Tank_State>().soldier[i] == null)
                {
                    other.GetComponent<Tank_State>().soldier[i] = gameObject;
                    targetPos = other.transform.Find("Soldier" + (i + 1)).gameObject;
                    Debug.Log("Soldier" + (i + 1));
                    break;
                }
            }
            owner = true;
        }
    }



    void Repair()
    {
        Instantiate(Mine, transform.position, transform.rotation);
    }
}

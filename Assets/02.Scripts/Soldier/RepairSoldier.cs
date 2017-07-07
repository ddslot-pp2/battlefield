using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RepairSoldier : SoldierState {
    private GameObject target; // 따라갈 타겟의 트랜스 폼
    private GameObject attackTank; // 공격 대상자
    private NavMeshAgent nvAgent;
    private GameObject targetPos; // 따라갈 타겟 주변 위치 좌표

    private bool owner;

    private float nextfire;
    private int count;
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
        fireRate = 2.0f;
    }

    void Update()
    {
        if (owner == true)
        {
            NavAgent(targetPos.transform.position, 10.0f, 0.0f);
            transform.rotation = Quaternion.LookRotation((targetPos.transform.position - transform.position).normalized);

            if (Time.time >= nextfire)
            {
                nextfire = Time.time + fireRate;
                Repair();
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
            target = other.gameObject;

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
        if(target.GetComponent<Tank_State>().hp < target.GetComponent<Tank_State>().maxHp)
        {
            
            if (target.GetComponent<Tank_State>().hp + 3 > target.GetComponent<Tank_State>().maxHp)
                target.GetComponent<Tank_State>().hp = target.GetComponent<Tank_State>().maxHp;
            else
                target.GetComponent<Tank_State>().hp += 3;
        }
    }
}

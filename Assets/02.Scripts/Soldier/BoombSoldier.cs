using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoombSoldier : SoldierState {
    private GameObject target; // 따라갈 타겟의 트랜스 폼
    private GameObject attackTank; // 공격 대상자
    private GameObject targetPos; // 따라갈 타겟 주변 위치 좌표
    private NavMeshAgent nvAgent;

    private float radius = 10.0f;
    private bool owner;
    private bool attack;

    private int count;

    public GameObject destroyParticle;
    public int Hp;
    void Awake()
    {
        hp = 50;
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        owner = false;
        attack = false;
    }

    void Update()
    {
        if (owner == true)
        {
            if (attack == false)
            {
                NavAgent(targetPos.transform.position, 10.0f, 0.0f);
                transform.rotation = Quaternion.LookRotation((targetPos.transform.position - transform.position).normalized);
                if (Vector3.Distance(transform.position, target.transform.position) <= 10.0f)
                {
                    AttackCheck();
                }
            }
            else if (attack == true)
            {
                try
                {
                    NavAgent(attackTank.transform.position, 10.0f, 0.0f);
                    transform.rotation = Quaternion.LookRotation((attackTank.transform.position - transform.position).normalized);

                    if (Vector3.Distance(transform.position, target.transform.position) >= 10.0f)
                    {
                        attack = false;
                    }else if(Vector3.Distance(transform.position, attackTank.transform.position) <= 3.0f)
                    {
                        Fire();
                    }
                }
                catch (MissingReferenceException)
                {
                    attack = false;
                }
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

    public void AttackCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if ((colliders[i].tag == "EnemyTank" || colliders[i].tag == "Tank") && colliders[i].gameObject != target)
            {
                attack = true;
                attackTank = colliders[i].gameObject;
                break;
            }
        }
    }

    void Fire()
    {
        GameObject destroy = Instantiate(destroyParticle,transform.position, transform.rotation);
        Destroy(destroy, 1.0f);

        attackTank.GetComponent<Tank_State>().hp -= attackTank.GetComponent<Tank_State>().hp / 2;
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaSoldier : SoldierState {
    private GameObject target; // 따라갈 타겟의 트랜스 폼
    private GameObject attackTank; // 공격 대상자

    private float speed = 10.0f;
    private float radius = 10.0f;
    private bool owner;
    private bool attack;

    private float nextfire;
    private int count;

    public GameObject muzzleFlash_1;
    public Transform firePos_p1;
    public int Hp;
    void Awake()
    {
        hp = 50;
    }

    void Start()
    {
        owner = false;
        nextfire = 0.0f;
        fireRate = 10.0f;
        attack = false;
    }

    void Update()
    {
        if (owner == true)
        {
            if (Vector3.Distance(transform.position, target.transform.position) >= count * 3.0f)
            {
                if (attack == false)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) >= count * 3.0f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, 1.0f, target.transform.position.z), speed * Time.deltaTime);
                        transform.rotation = Quaternion.LookRotation((target.transform.position - transform.position).normalized);
                        AttackCheck();
                    }
                }
                else
                {
                    try
                    {
                        if (Vector3.Distance(transform.position, attackTank.transform.position) >= count * 10.0f)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, new Vector3(attackTank.transform.position.x, 1.0f, attackTank.transform.position.z), speed * Time.deltaTime);
                        }
                        transform.rotation = Quaternion.LookRotation((attackTank.transform.position - transform.position).normalized);

                        if (Vector3.Distance(transform.position, target.transform.position) >= count * 7.0f)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, 1.0f, target.transform.position.z), speed * Time.deltaTime);
                            transform.rotation = Quaternion.LookRotation((target.transform.position - transform.position).normalized);
                            attack = false;
                        }
                        Fire();
                    }
                    catch (MissingReferenceException)
                    {
                        attack = false;
                    }
                }
            }
        }

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
                    count = i + 1;
                    Debug.Log(count);
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
            if (colliders[i].tag == "Tank" && colliders[i].gameObject != target)
            {
                attack = true;
                attackTank = colliders[i].gameObject;
                break;
            }
        }
    }

    void Fire()
    {
        if (Time.time >= nextfire)
        {
            nextfire = Time.time + fireRate;
            CreateBullet();

            StartCoroutine(this.ShowMuzzleFlash());
        }
    }

    void CreateBullet()
    {
        //Bullet 프리팹을 동적으로 생성
        GameObject bulletLocalSize = Instantiate(bullet, firePos_p1.position, firePos_p1.rotation);
        bulletLocalSize.GetComponent<PistorBullet>().GetDamageType(1, 10.0f);
    }

    IEnumerator ShowMuzzleFlash()
    {
        //MuzzleFlash 스케일을 불규칙하게 변경
        float scale = Random.Range(2.0f, 4.0f);
        muzzleFlash_1.transform.localScale = Vector3.one * scale;

        //활성화해서 보이게 함
        muzzleFlash_1.GetComponent<MeshRenderer>().enabled = true;
        //불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));

        //비활성화해서 보이지 않게 함
        muzzleFlash_1.GetComponent<MeshRenderer>().enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank4 : Tank_State {
    private GameObject attackTank; // 공격 대상자
    private float radius = 10.0f;

    private bool attack;
    private bool posSearch;

    private UnityEngine.AI.NavMeshAgent nvAgent;
    private float nextfire;
    private Vector3 randomPos;

    public Vector3 firstPos;
    public GameObject muzzleFlash_1;
    public Transform firePos_p1;
    public GameObject muzzleFlash_2;
    public Transform firePos_p2;
    public GameObject muzzleFlash_3;
    public Transform firePos_p3;
    public GameObject muzzleFlash_4;
    public Transform firePos_p4;
    public GameObject topFire;
    public GameObject ray;
    // Use this for initialization

    void Start()
    {
        posSearch = true;
        nvAgent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        moveSpeed = 10.0f;
        hp = 100;
        maxHp = 100;
        attack = false;
        firstPos = transform.position;
        nextfire = 0.0f;
        fireRate = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (attack == false)
        {
            if (posSearch == false)
            {
                NavAgent(randomPos, 5, 1);
                if (Vector3.Distance(transform.position, randomPos) <= 2.0f)
                {
                    posSearch = true;
                }
            }
            else if (posSearch == true)
            {
                if (RandomPos())
                {
                    posSearch = false;
                }
            }

            AttackCheck();
        }
        else if (attack == true)
        {
            try
            {
                if (Vector3.Distance(attackTank.transform.position, transform.position) <= 30)
                {
                    NavAgent(attackTank.transform.position, 5, 5);
                    topFire.transform.rotation = Quaternion.LookRotation((attackTank.transform.position - transform.position).normalized);
                    Fire();
                }
                else
                {
                    NavAgent(firstPos, 20, 1);
                    attack = false;
                }
            }
            catch (MissingReferenceException)
            {
                attack = false;
            }
        }
    }

    public bool RandomPos()
    {
        randomPos = new Vector3(firstPos.x + Random.Range(-10, 10), 1, firstPos.z + Random.Range(-10, 10));
        Collider[] colliders = Physics.OverlapSphere(new Vector3(randomPos.x, 4, randomPos.z), 2.0f);

        if (colliders.Length == 0)
        {

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RayCast(GameObject target)
    {

        ray.transform.rotation = Quaternion.LookRotation((target.transform.position - transform.position).normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray.transform.position, ray.transform.forward, out hit, 20.0F))
        {
            Debug.Log(hit.transform.name);

            if (hit.collider.gameObject == target.gameObject)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void NavAgent(Vector3 pos, float speed, int Stop)
    {
        nvAgent.destination = pos;
        nvAgent.speed = speed;
        nvAgent.stoppingDistance = Stop;
    }

    public void AttackCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Tank" || colliders[i].tag == "Soldier")
            {
                Debug.Log("as");
                if (RayCast(colliders[i].gameObject))
                {
                    attack = true;
                    attackTank = colliders[i].gameObject;
                    break;
                }

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
        bulletLocalSize.GetComponent<PistorBullet>().GetDamageType(damage, 10.0f);

        GameObject bulletLocalSize2 = Instantiate(bullet, firePos_p2.position, firePos_p2.rotation);
        bulletLocalSize2.GetComponent<PistorBullet>().GetDamageType(damage, 10.0f);

        GameObject bulletLocalSize3 = Instantiate(bullet, firePos_p3.position, firePos_p3.rotation);
        bulletLocalSize3.GetComponent<PistorBullet>().GetDamageType(damage, 10.0f);

        GameObject bulletLocalSize4 = Instantiate(bullet, firePos_p4.position, firePos_p4.rotation);
        bulletLocalSize4.GetComponent<PistorBullet>().GetDamageType(damage, 10.0f);
    }

    IEnumerator ShowMuzzleFlash()
    {
        //MuzzleFlash 스케일을 불규칙하게 변경
        float scale = Random.Range(2.0f, 4.0f);
        muzzleFlash_1.transform.localScale = Vector3.one * scale;
        muzzleFlash_2.transform.localScale = Vector3.one * scale;
        muzzleFlash_3.transform.localScale = Vector3.one * scale;
        muzzleFlash_4.transform.localScale = Vector3.one * scale;

        //활성화해서 보이게 함
        muzzleFlash_1.GetComponent<MeshRenderer>().enabled = true;
        muzzleFlash_2.GetComponent<MeshRenderer>().enabled = true;
        muzzleFlash_3.GetComponent<MeshRenderer>().enabled = true;
        muzzleFlash_4.GetComponent<MeshRenderer>().enabled = true;

        //불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));

        //비활성화해서 보이지 않게 함
        muzzleFlash_1.GetComponent<MeshRenderer>().enabled = false;
        muzzleFlash_2.GetComponent<MeshRenderer>().enabled = false;
        muzzleFlash_3.GetComponent<MeshRenderer>().enabled = false;
        muzzleFlash_4.GetComponent<MeshRenderer>().enabled = false;

    }
}

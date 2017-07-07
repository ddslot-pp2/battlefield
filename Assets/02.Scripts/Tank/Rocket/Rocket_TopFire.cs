using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_TopFire : MonoBehaviour {

    //총알 발사좌표
    public Transform firePos_p1;
    //총알 발사좌표
    public Transform firePos_p2;

    public GameObject BombRangeEffect;
    public GameObject[] haveBullet;

    RaycastHit TFire;
    Vector3 Click;
    Quaternion dir;

    public float nextfire = 3.0f;//다음 총알 발사시간

	Tank_State state;


    void Start()
    {
		state = gameObject.GetComponentInParent<Tank_State>();
        //최초에 MuzzleFlash MeshRenderer를 비활성화
        haveBullet = new GameObject[2];
        haveBullet[0] = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
        haveBullet[0].transform.parent = gameObject.transform;
        haveBullet[1] = Instantiate(state.bullet, firePos_p2.position, firePos_p2.rotation);
        haveBullet[1].transform.parent = gameObject.transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out TFire);
            Click = TFire.point;
            Click.y = transform.position.y;
            dir = Quaternion.LookRotation((Click - transform.position).normalized);

            transform.rotation = dir;

            int l = TFire.transform.gameObject.layer;

            if (l == 8 && (Vector3.Distance(transform.position, TFire.point) <= state.range))
            {
                GameObject bulletLocalSize = Instantiate(BombRangeEffect, TFire.point, gameObject.transform.rotation);
                bulletLocalSize.transform.position = new Vector3(bulletLocalSize.transform.position.x, bulletLocalSize.transform.position.y + 1, bulletLocalSize.transform.position.z);
                StartCoroutine(this.DestroyTargetRange(bulletLocalSize));
                Fire();
            }
        }

    }

    void Fire()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().CoolTimeCounter(state.fireRate);
        StartCoroutine("CreateBullet");
    }

    IEnumerator CreateBullet()
    {
        if (haveBullet[0] != null || haveBullet[0].GetComponent<RocketBullet>().start == false)
        {
            haveBullet[0].transform.parent = null;
            haveBullet[0].GetComponent<RocketBullet>().GetDamageType(state.damage, TFire.point, transform.position, transform.parent.gameObject);
            haveBullet[0].GetComponent<RocketBullet>().start = true;

            yield return new WaitForSeconds(nextfire);
            haveBullet[0] = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
            haveBullet[0].transform.parent = gameObject.transform;
        }
        else if (haveBullet[1] != null || haveBullet[1].GetComponent<RocketBullet>().start == false)
        {
            haveBullet[1].transform.parent = null;
            haveBullet[1].GetComponent<RocketBullet>().GetDamageType(state.damage, TFire.point, transform.position, transform.parent.gameObject);
            haveBullet[1].GetComponent<RocketBullet>().start = true;

            yield return new WaitForSeconds(nextfire);
            haveBullet[1] = Instantiate(state.bullet, firePos_p2.position, firePos_p2.rotation);
            haveBullet[1].transform.parent = gameObject.transform;
        }
    }

    IEnumerator DestroyTargetRange(GameObject obj)
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(obj);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankKiller_TopFire : MonoBehaviour {

    //총알 발사좌표
    public Transform[] firePos_rocket;
    //MuzzleFlash의 MeshRenderer 컴포넌트 연결 변수
    public MeshRenderer[] muzzleFlash_rocket;
    public GameObject bullet;

    public Transform firePos_Valkan;
    public MeshRenderer muzzleFlash_Valkan;

    RaycastHit TFire;
    Vector3 Click;
    Quaternion dir;

    public float nextfire = 0.0f;//다음 총알 발사시간

	Tank_State state;

    void Start()
    {
		state = gameObject.GetComponentInParent<Tank_State>();
        //최초에 MuzzleFlash MeshRenderer를 비활성화
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

            Fire();
        }
    }



    void Fire()
    {
        if (Time.time >= nextfire)
        {
            nextfire = Time.time + state.fireRate;
            GameObject.Find("GameManager").GetComponent<GameManager>().CoolTimeCounter(state.fireRate);
            StartCoroutine("CreateBullet");
            StartCoroutine("CreateBullet2");

        }
    }

    IEnumerator CreateBullet2()
    {
        for(int i = 0; i < 12; i++)
        {
            GameObject bulletLocalSize = Instantiate(bullet, firePos_Valkan.position, firePos_Valkan.rotation);
            bulletLocalSize.transform.localScale = new Vector3(bulletLocalSize.transform.localScale.x * state.bulletSize, bulletLocalSize.transform.localScale.y * state.bulletSize, bulletLocalSize.transform.localScale.z * state.bulletSize);
            bulletLocalSize.GetComponent<BalKanBullet>().GetDamageType(state.damage,transform.parent.gameObject, state.range, state.bulletSpeed);

            StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_Valkan));

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator CreateBullet()
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject bulletLocalSize = Instantiate(state.bullet, firePos_rocket[i % 2].position, firePos_rocket[i % 2].rotation);
            bulletLocalSize.transform.localScale = new Vector3(bulletLocalSize.transform.localScale.x * state.bulletSize, bulletLocalSize.transform.localScale.y * state.bulletSize, bulletLocalSize.transform.localScale.z * state.bulletSize);
            bulletLocalSize.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);

            StartCoroutine(ShowMuzzleFlash(muzzleFlash_rocket[i % 2]));

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator ShowMuzzleFlash(MeshRenderer muzzleFlash_1)
    {
        //MuzzleFlash 스케일을 불규칙하게 변경
        float scale = Random.Range(2.0f, 4.0f);
        muzzleFlash_1.transform.localScale = Vector3.one * scale;

        //활성화해서 보이게 함
        muzzleFlash_1.enabled = true;

        //불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));

        //비활성화해서 보이지 않게 함
        muzzleFlash_1.enabled = false;
    }
}


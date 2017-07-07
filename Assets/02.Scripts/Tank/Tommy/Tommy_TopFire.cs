using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tommy_TopFire : MonoBehaviour {
    //총알 발사좌표
    public Transform firePos_p1;
    //MuzzleFlash의 MeshRenderer 컴포넌트 연결 변수
    public MeshRenderer muzzleFlash_1;

    //총알 발사좌표
    public Transform firePos_p2;
    //MuzzleFlash의 MeshRenderer 컴포넌트 연결 변수
    public MeshRenderer muzzleFlash_2;

    //총알 발사좌표
    public Transform firePos_p3;
    //MuzzleFlash의 MeshRenderer 컴포넌트 연결 변수
    public MeshRenderer muzzleFlash_3;



    RaycastHit TFire;
    Vector3 Click;
    Quaternion dir;
    Tank_State state;

    public float nextfire = 0.0f;//다음 총알 발사시간

    void Start()
    {

		state = gameObject.GetComponentInParent<Tank_State>();
        //최초에 MuzzleFlash MeshRenderer를 비활성화
        muzzleFlash_1.enabled = false;
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
            CreateBullet();

            //잠시 기다리는 루틴을 위해 코루틴 함수로 호출
            StartCoroutine(this.ShowMuzzleFlash());
        }
    }

    void CreateBullet()
    {
        //Bullet 프리팹을 동적으로 생성
        GameObject bulletLocalSize1 = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
        bulletLocalSize1.transform.localScale = new Vector3(bulletLocalSize1.transform.localScale.x * state.bulletSize, bulletLocalSize1.transform.localScale.y * state.bulletSize, bulletLocalSize1.transform.localScale.z * state.bulletSize);
        bulletLocalSize1.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);

        GameObject bulletLocalSize2 = Instantiate(state.bullet, firePos_p2.position, firePos_p2.rotation);
        bulletLocalSize2.transform.localScale = new Vector3(bulletLocalSize2.transform.localScale.x * state.bulletSize, bulletLocalSize2.transform.localScale.y * state.bulletSize, bulletLocalSize2.transform.localScale.z * state.bulletSize);
        bulletLocalSize2.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);

        GameObject bulletLocalSize3 = Instantiate(state.bullet, firePos_p3.position, firePos_p3.rotation);
        bulletLocalSize3.transform.localScale = new Vector3(bulletLocalSize3.transform.localScale.x * state.bulletSize, bulletLocalSize3.transform.localScale.y * state.bulletSize, bulletLocalSize3.transform.localScale.z * state.bulletSize);
        bulletLocalSize3.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
    }

    IEnumerator ShowMuzzleFlash()
    {
        //MuzzleFlash 스케일을 불규칙하게 변경
        float scale = Random.Range(2.0f, 4.0f);
        muzzleFlash_1.transform.localScale = Vector3.one * scale;
        muzzleFlash_2.transform.localScale = Vector3.one * scale;
        muzzleFlash_3.transform.localScale = Vector3.one * scale;


        //활성화해서 보이게 함
        muzzleFlash_1.enabled = true;
        muzzleFlash_2.enabled = true;
        muzzleFlash_3.enabled = true;

        //불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));

        //비활성화해서 보이지 않게 함
        muzzleFlash_1.enabled = false;
        muzzleFlash_2.enabled = false;
        muzzleFlash_3   .enabled = false;

    }
}

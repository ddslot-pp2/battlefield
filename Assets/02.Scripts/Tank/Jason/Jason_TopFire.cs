using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jason_TopFire : MonoBehaviour {
    //총알 발사좌표
    public Transform firePos_p1;
    //총알 발사좌표
    public Transform firePos_p2;
    //총알 발사좌표
    public Transform firePos_p3;
    //총알 발사좌표
    public Transform firePos_p4;

    //MuzzleFlash의 MeshRenderer 컴포넌트 연결 변수
    public MeshRenderer muzzleFlash_1;
    //MuzzleFlash의 MeshRenderer 컴포넌트 연결 변수
    public MeshRenderer muzzleFlash_2;
    //MuzzleFlash의 MeshRenderer 컴포넌트 연결 변수
    public MeshRenderer muzzleFlash_3;
    //MuzzleFlash의 MeshRenderer 컴포넌트 연결 변수
    public MeshRenderer muzzleFlash_4;



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
            StartCoroutine("CreateBullet");

        }
    }

    IEnumerator CreateBullet()
    {
        //Bullet 프리팹을 동적으로 생성
        GameObject bulletLocalSize1 = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
        bulletLocalSize1.transform.localScale = new Vector3(bulletLocalSize1.transform.localScale.x * state.bulletSize, bulletLocalSize1.transform.localScale.y * state.bulletSize, bulletLocalSize1.transform.localScale.z * state.bulletSize);
        bulletLocalSize1.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_1));


        yield return new WaitForSeconds(0.3f);

        GameObject bulletLocalSize2 = Instantiate(state.bullet, firePos_p2.position, firePos_p2.rotation);
        bulletLocalSize2.transform.localScale = new Vector3(bulletLocalSize2.transform.localScale.x * state.bulletSize, bulletLocalSize2.transform.localScale.y * state.bulletSize, bulletLocalSize2.transform.localScale.z * state.bulletSize);
        bulletLocalSize2.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_2));

        yield return new WaitForSeconds(0.3f);

        GameObject bulletLocalSize3 = Instantiate(state.bullet, firePos_p3.position, firePos_p3.rotation);
        bulletLocalSize3.transform.localScale = new Vector3(bulletLocalSize3.transform.localScale.x * state.bulletSize, bulletLocalSize3.transform.localScale.y * state.bulletSize, bulletLocalSize3.transform.localScale.z * state.bulletSize);
        bulletLocalSize3.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_3));

        yield return new WaitForSeconds(0.3f);

        GameObject bulletLocalSize4 = Instantiate(state.bullet, firePos_p4.position, firePos_p4.rotation);
        bulletLocalSize4.transform.localScale = new Vector3(bulletLocalSize4.transform.localScale.x * state.bulletSize, bulletLocalSize4.transform.localScale.y * state.bulletSize, bulletLocalSize4.transform.localScale.z * state.bulletSize);
        bulletLocalSize4.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_4));


        yield return new WaitForSeconds(0.3f);

        bulletLocalSize1 = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
        bulletLocalSize1.transform.localScale = new Vector3(bulletLocalSize1.transform.localScale.x * state.bulletSize, bulletLocalSize1.transform.localScale.y * state.bulletSize, bulletLocalSize1.transform.localScale.z * state.bulletSize);
        bulletLocalSize1.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_1));

        yield return new WaitForSeconds(0.3f);

        bulletLocalSize2 = Instantiate(state.bullet, firePos_p2.position, firePos_p2.rotation);
        bulletLocalSize2.transform.localScale = new Vector3(bulletLocalSize2.transform.localScale.x * state.bulletSize, bulletLocalSize2.transform.localScale.y * state.bulletSize, bulletLocalSize2.transform.localScale.z * state.bulletSize);
        bulletLocalSize2.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_2));

        yield return new WaitForSeconds(0.3f);

        bulletLocalSize3 = Instantiate(state.bullet, firePos_p3.position, firePos_p3.rotation);
        bulletLocalSize3.transform.localScale = new Vector3(bulletLocalSize3.transform.localScale.x * state.bulletSize, bulletLocalSize3.transform.localScale.y * state.bulletSize, bulletLocalSize3.transform.localScale.z * state.bulletSize);
        bulletLocalSize3.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_3));

        yield return new WaitForSeconds(0.3f);

        bulletLocalSize4 = Instantiate(state.bullet, firePos_p4.position, firePos_p4.rotation);
        bulletLocalSize4.transform.localScale = new Vector3(bulletLocalSize4.transform.localScale.x * state.bulletSize, bulletLocalSize4.transform.localScale.y * state.bulletSize, bulletLocalSize4.transform.localScale.z * state.bulletSize);
        bulletLocalSize4.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_4));

        yield return new WaitForSeconds(0.3f);

        bulletLocalSize1 = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
        bulletLocalSize1.transform.localScale = new Vector3(bulletLocalSize1.transform.localScale.x * state.bulletSize, bulletLocalSize1.transform.localScale.y * state.bulletSize, bulletLocalSize1.transform.localScale.z * state.bulletSize);
        bulletLocalSize1.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_1));

        yield return new WaitForSeconds(0.3f);

        bulletLocalSize2 = Instantiate(state.bullet, firePos_p2.position, firePos_p2.rotation);
        bulletLocalSize2.transform.localScale = new Vector3(bulletLocalSize2.transform.localScale.x * state.bulletSize, bulletLocalSize2.transform.localScale.y * state.bulletSize, bulletLocalSize2.transform.localScale.z * state.bulletSize);
        bulletLocalSize2.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_2));

        yield return new WaitForSeconds(0.3f);

        bulletLocalSize3 = Instantiate(state.bullet, firePos_p3.position, firePos_p3.rotation);
        bulletLocalSize3.transform.localScale = new Vector3(bulletLocalSize3.transform.localScale.x * state.bulletSize, bulletLocalSize3.transform.localScale.y * state.bulletSize, bulletLocalSize3.transform.localScale.z * state.bulletSize);
        bulletLocalSize3.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_3));

        yield return new WaitForSeconds(0.3f);

        bulletLocalSize4 = Instantiate(state.bullet, firePos_p4.position, firePos_p4.rotation);
        bulletLocalSize4.transform.localScale = new Vector3(bulletLocalSize4.transform.localScale.x * state.bulletSize, bulletLocalSize4.transform.localScale.y * state.bulletSize, bulletLocalSize4.transform.localScale.z * state.bulletSize);
        bulletLocalSize4.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_4));

        yield return new WaitForSeconds(0.3f);

        bulletLocalSize1 = Instantiate(state.bullet, firePos_p1.position, firePos_p1.rotation);
        bulletLocalSize1.transform.localScale = new Vector3(bulletLocalSize1.transform.localScale.x * state.bulletSize, bulletLocalSize1.transform.localScale.y * state.bulletSize, bulletLocalSize1.transform.localScale.z * state.bulletSize);
        bulletLocalSize1.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_1));

        yield return new WaitForSeconds(0.3f);

        bulletLocalSize2 = Instantiate(state.bullet, firePos_p2.position, firePos_p2.rotation);
        bulletLocalSize2.transform.localScale = new Vector3(bulletLocalSize2.transform.localScale.x * state.bulletSize, bulletLocalSize2.transform.localScale.y * state.bulletSize, bulletLocalSize2.transform.localScale.z * state.bulletSize);
        bulletLocalSize2.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_2));

        yield return new WaitForSeconds(0.3f);

        bulletLocalSize3 = Instantiate(state.bullet, firePos_p3.position, firePos_p3.rotation);
        bulletLocalSize3.transform.localScale = new Vector3(bulletLocalSize3.transform.localScale.x * state.bulletSize, bulletLocalSize3.transform.localScale.y * state.bulletSize, bulletLocalSize3.transform.localScale.z * state.bulletSize);
        bulletLocalSize3.GetComponent<DirectBullet>().GetDamageType(state.damage, 1, transform.parent.gameObject, state.range, state.bulletSpeed);
        StartCoroutine(this.ShowMuzzleFlash(muzzleFlash_3));


    }

    IEnumerator ShowMuzzleFlash(MeshRenderer muzzleFlash)
    {
        //MuzzleFlash 스케일을 불규칙하게 변경
        float scale = Random.Range(2.0f, 4.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        //활성화해서 보이게 함
        muzzleFlash.enabled = true;

        //불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));

        //비활성화해서 보이지 않게 함
        muzzleFlash.enabled = false;

    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class P_Tank_1_TopFire : MonoBehaviour
{
    //총알 프리팹
    public GameObject bullet;
    //총알 발사좌표
    public Transform firePos_p1;
    //MuzzleFlash의 MeshRenderer 컴포넌트 연결 변수
    public MeshRenderer muzzleFlash_1;

    //회전 속도 변수
    public float toprotSpeed = 100.0f;
    //게임 매니저
    public GameObject gameManager;

    RaycastHit TFire;
    Vector3 Click;
    Quaternion dir;

	public float fireRate = 5.0f;//총알 지연 시간 설정
	public float nextfire = 0.0f;//다음 총알 발사시간

    void Start()
    {
        //최초에 MuzzleFlash MeshRenderer를 비활성화
        muzzleFlash_1.enabled = false;

    }

    void Update()
    {
        
        int count = Input.touchCount;
            for (int i = 0; i < count; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(i) == false)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {

                    Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(i).position), out TFire);
                    Click = TFire.point;
                    dir = Quaternion.LookRotation((Click - transform.position).normalized);
                    dir.x = 0;
                    dir.z = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, dir, toprotSpeed * Time.deltaTime);

                    Fire();
                    }
                }
            }  
        }

    

    void Fire()
    {
		if (Time.time >= nextfire) 
		{
			nextfire = Time.time + fireRate;
            gameManager.GetComponent<GameManager>().CoolTimeCounter(fireRate);
            CreateBullet();
			
			//잠시 기다리는 루틴을 위해 코루틴 함수로 호출
			StartCoroutine(this.ShowMuzzleFlash());
		}
    }

    void CreateBullet()
    {
        //Bullet 프리팹을 동적으로 생성
		Instantiate(bullet, firePos_p1.position, firePos_p1.rotation);
    }

    IEnumerator ShowMuzzleFlash()
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

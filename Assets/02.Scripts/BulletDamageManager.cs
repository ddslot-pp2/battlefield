using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageManager : MonoBehaviour {
    float decreaseSpeed;
    public GameObject fireEffect;
    public GameObject dustEffect;
    public GameObject iceEffect;
    public GameObject slimeEffect;
    public GameObject waterEffect;
    public GameObject windEffect;

    public GameObject destroyParticle;
    public GameManager gameManager;
    public GameLoad gameLoad;

	static 	BulletDamageManager _Instance;


	public static BulletDamageManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = GameObject.FindObjectOfType(typeof(BulletDamageManager)) as BulletDamageManager;

				if (_Instance == null)
				{
					GameObject kNewObject = GameObject.Instantiate( Resources.Load("Prefab/Global/BulletDamageManager") ) as GameObject;
					kNewObject.name = "BulletDamageManager";

					DontDestroyOnLoad( kNewObject );

					_Instance = kNewObject.GetComponent<BulletDamageManager>();
				}
			}

			return _Instance;
		}
	}
		
    void Awake()
    {
		DontDestroyOnLoad(this);

        gameLoad = GameObject.Find("Main Camera").GetComponent<GameLoad>();
        Debug.Log("asdsa");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    
    public void GetDamage(int damage, GameObject hit)
    {
        if(hit.tag == "Tank" || hit.tag == "EnemyTank")
        {
            hit.GetComponent<Tank_State>().hp -= (damage - hit.GetComponent<Tank_State>().tankDefensive);
            Debug.Log(hit.GetComponent<Tank_State>().hp);
            if (hit.GetComponent<Tank_State>().hp <= 0)
            {
                Destroy(hit.GetComponent<HpBar>().healthBarObj);
                Destroy(hit.gameObject);

                GameObject destroy = Instantiate(destroyParticle, new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z), hit.transform.rotation);
                Destroy(destroy, 1.0f);

                if(hit.gameObject == gameLoad.userTank)
                {
                    gameManager.GameOver();
                }
            }
        }else if(hit.tag == "Soldier")
        {
            hit.GetComponent<SoldierState>().hp -= damage;
            Debug.Log(hit.GetComponent<SoldierState>().hp);
            if (hit.GetComponent<SoldierState>().hp <= 0)
            {
                Destroy(hit.gameObject);

                GameObject destroy = Instantiate(destroyParticle, new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z), hit.transform.rotation);
                Destroy(destroy, 1.0f);
            }
        }
    }

    public void GetDamage(int damage, GameObject hit, GameObject attacker)
    {
        if (hit.tag == "Tank" || hit.tag == "EnemyTank")
        {
            Debug.Log(hit.name);
            hit.GetComponent<Tank_State>().hp -= (damage - hit.GetComponent<Tank_State>().tankDefensive);
            Debug.Log(hit.GetComponent<Tank_State>().hp);
            if (hit.GetComponent<Tank_State>().hp <= 0)
            {
                Destroy(hit.GetComponent<HpBar>().healthBarObj);
                Destroy(hit.gameObject);

                gameManager.addExperience(attacker, 10);

                GameObject destroy = Instantiate(destroyParticle, new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z), hit.transform.rotation);
                Destroy(destroy, 1.0f);

                if (hit.gameObject == gameLoad.userTank)
                {
                    gameManager.GameOver();
                }
            }
        }
        else if (hit.tag == "Soldier")
        {
            hit.GetComponent<SoldierState>().hp -= damage;
            Debug.Log(hit.GetComponent<SoldierState>().hp);
            if (hit.GetComponent<SoldierState>().hp <= 0)
            {
                Destroy(hit.gameObject);

                gameManager.addExperience(attacker, 10);

                GameObject destroy = Instantiate(destroyParticle, new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z), hit.transform.rotation);
                Destroy(destroy, 1.0f);
            }
        }
    }

    public void GetDustEffect(GameObject hit)
    {
        GameObject dust;
        if(hit.GetComponent<Tank_State>().attAply == 1)
        {
            return;
        }

        dust = Instantiate(dustEffect, transform.position, transform.rotation);
        dust.transform.parent = hit.transform;
        dust.transform.localPosition = new Vector3(0, 0, 0);

        hit.GetComponent<Tank_State>().attAply = 1;
        Debug.Log("진입");
        decreaseSpeed = hit.GetComponent<Tank_State>().moveSpeed * 0.3f;
        hit.GetComponent<Tank_State>().moveSpeed -= decreaseSpeed;
        StartCoroutine(this.DustEffect(hit, dust));

    }

    IEnumerator DustEffect(GameObject hit, GameObject dust)
    {
        yield return new WaitForSeconds(3.0f);
        hit.GetComponent<Tank_State>().moveSpeed += decreaseSpeed;
        hit.GetComponent<Tank_State>().attAply = 0;
        Destroy(dust);
    }

    public void GetIceEffect(GameObject hit)
    {
        if (hit.GetComponent<Tank_State>().attAply == 1)
        {
            return;
        }
        GameObject ice = Instantiate(iceEffect, transform.position, transform.rotation);
        ice.transform.parent = hit.transform;
        ice.transform.localPosition = new Vector3(0, 0, 0);

        hit.GetComponent<Tank_State>().attAply = 1;
        Debug.Log("진입");
        hit.GetComponent<Tank_State>().fireRate = 2.0f;
        StartCoroutine(this.IceEffect(hit, ice));

    }

    IEnumerator IceEffect(GameObject hit, GameObject ice)
    {
        yield return new WaitForSeconds(2.0f);
        hit.GetComponent<Tank_State>().fireRate = 0.5f;
        hit.GetComponent<Tank_State>().attAply = 0;

        Destroy(ice);
    }

    public void GetWindEffect(GameObject hit)
    {
        if (hit.GetComponent<Tank_State>().attAply == 1)
        {
            return;
        }

        GameObject wind = Instantiate(windEffect, transform.position, transform.rotation);
        wind.transform.parent = hit.transform;
        wind.transform.localPosition = new Vector3(0, 0, 0);

        hit.GetComponent<Tank_State>().attAply = 1;
        Debug.Log("진입");
        hit.GetComponent<Tank_State>().direct = -1f;
        StartCoroutine(this.WindEffect(hit, wind));

    }

    IEnumerator WindEffect(GameObject hit, GameObject wind)
    {
        yield return new WaitForSeconds(3.0f);
        hit.GetComponent<Tank_State>().direct = 1f;
        hit.GetComponent<Tank_State>().attAply = 0;
        Destroy(wind);

    }

    public void GetFireEffect(GameObject hit, int fireDamage, GameObject attacker)
    {
        GameObject fire;

        if (hit.GetComponent<Tank_State>().attAply == 1)
        {
            return;
        }
        fire = Instantiate(fireEffect, transform.position, transform.rotation);
        fire.transform.parent = hit.transform;
        fire.transform.localPosition = new Vector3(0, 0, 0);


        hit.GetComponent<Tank_State>().attAply = 1;
        Debug.Log(fireDamage);
        StartCoroutine(this.FireEffect(hit, fireDamage, fire, attacker));
    }

    IEnumerator FireEffect(GameObject hit, int fireDamage, GameObject fire, GameObject attacker)
    {
        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit, attacker);

        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit, attacker);

        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit, attacker);

        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit, attacker);

        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit, attacker);

        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit, attacker);

        hit.GetComponent<Tank_State>().attAply = 0;

        Destroy(fire);
    }

    public void GetFireEffect(GameObject hit, int fireDamage)
    {
        GameObject fire;

        if (hit.GetComponent<Tank_State>().attAply == 1)
        {
            return;
        }
        fire = Instantiate(fireEffect, transform.position, transform.rotation);
        fire.transform.parent = hit.transform;
        fire.transform.localPosition = new Vector3(0, 0, 0);

        hit.GetComponent<Tank_State>().attAply = 1;
        Debug.Log(fireDamage);
        StartCoroutine(this.FireEffect(hit, fireDamage, fire));
    }

    IEnumerator FireEffect(GameObject hit, int fireDamage, GameObject fire)
    {
        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit);

        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit);

        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit);

        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit);

        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit);

        yield return new WaitForSeconds(0.5f);
        GetDamage(fireDamage, hit);

        hit.GetComponent<Tank_State>().attAply = 0;

        Destroy(fire);
    }


    public void GetSlimEffect(GameObject hit)
    {
        if (hit.GetComponent<Tank_State>().attAply == 1)
        {
            return;
        }

        GameObject slime = Instantiate(slimeEffect, transform.position, transform.rotation);
        slime.transform.parent = hit.transform;
        slime.transform.localPosition = new Vector3(0, 0, 0);

        hit.GetComponent<Tank_State>().attAply = 1;
        Debug.Log("진입");
        hit.GetComponent<Tank_State>().forward = Vector3.zero;
        StartCoroutine(this.SlimEffect(hit, slime));

    }

    IEnumerator SlimEffect(GameObject hit, GameObject slime)
    {
        yield return new WaitForSeconds(3.0f);
        hit.GetComponent<Tank_State>().forward = Vector3.forward;
        hit.GetComponent<Tank_State>().attAply = 0;

        Destroy(slime);
    }

    public void GetWaterEffect(GameObject hit, Vector3 firstPos)
    {
        GameObject water = Instantiate(waterEffect, transform.position, transform.rotation);
        water.transform.parent = hit.transform;
        water.transform.localPosition = new Vector3(0, 0, 0);

        hit.GetComponent<Tank_State>().attAply = 1;

        Vector3 dir = (firstPos - hit.transform.position).normalized;
        hit.GetComponent<Rigidbody>().AddForce(-dir * 100, ForceMode.Impulse);

        if (hit.GetComponent<Tank_State>().attAply == 1)
        {
            return;

        }
        hit.GetComponent<Tank_State>().range /= 2;

        StartCoroutine(this.WaterEffect(hit, water));
    }

    IEnumerator WaterEffect(GameObject hit, GameObject water)
    {
        yield return new WaitForSeconds(3.0f);
        hit.GetComponent<Tank_State>().range *= 2;
        hit.GetComponent<Tank_State>().attAply = 0;
        Destroy(water);
    }

    public void UsePowerItem()
    {
        float firstDamage = gameLoad.userTank.GetComponent<Tank_State>().firstDamage;
        float Damage = gameLoad.userTank.GetComponent<Tank_State>().damage;

        gameLoad.userTank.GetComponent<Tank_State>().firstDamage *= 2;
        gameLoad.userTank.GetComponent<Tank_State>().damage *= 2;

        StartCoroutine(this.PowerItem(firstDamage, Damage));
    }

    IEnumerator PowerItem(float firstDamage, float Damage)
    {
        yield return new WaitForSeconds(30.0f);
        gameLoad.userTank.GetComponent<Tank_State>().firstDamage = (int)firstDamage;
        gameLoad.userTank.GetComponent<Tank_State>().damage = (int)Damage;
    }

    public void UseDefendItem()
    {
        float firstDamage = gameLoad.userTank.GetComponent<Tank_State>().firstDamage;
        float Damage = gameLoad.userTank.GetComponent<Tank_State>().damage;

        gameLoad.userTank.GetComponent<Tank_State>().firstDamage *= 2;
        gameLoad.userTank.GetComponent<Tank_State>().damage *= 2;

        StartCoroutine(this.DefendItem(firstDamage, Damage));
    }

    IEnumerator DefendItem(float firstDamage, float Damage)
    {
        yield return new WaitForSeconds(30.0f);
        gameLoad.userTank.GetComponent<Tank_State>().firstDamage = (int)firstDamage;
        gameLoad.userTank.GetComponent<Tank_State>().damage = (int)Damage;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectManager : MonoBehaviour {
	
    public GameObject normalBox;
    public GameObject fireBox;
    public GameObject prison;
    public GameObject ItemBox;
    public GameObject corn;
    public GameObject barricade;

	static 	ObjectManager _Instance;


	public static ObjectManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = GameObject.FindObjectOfType(typeof(ObjectManager)) as ObjectManager;

				if (_Instance == null)
				{
					GameObject kNewObject = GameObject.Instantiate( Resources.Load("Prefab/Global/ObjectManager") ) as GameObject;
					kNewObject.name = "ObjectManager";

					DontDestroyOnLoad( kNewObject );

					_Instance = kNewObject.GetComponent<ObjectManager>();
				}
			}

			return _Instance;
		}
	}
		
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

    public void CreateObject(Vector3 pos, Quaternion rot, int type)
    {
        StartCoroutine(DustEffect(pos, rot, type));
    }

    IEnumerator DustEffect(Vector3 pos, Quaternion rot, int type)
    {
        GameObject box = null;

        yield return new WaitForSeconds(18.0f);

		Debug.Log ("DustEffect");

        if (type == 1)
        {
			//box = normalBox.Spawn ();
            box = Instantiate(normalBox);
        }else if(type == 2)
        {
			//box = fireBox.Spawn ();
            box = Instantiate(fireBox);
        }
        else if (type == 3)
        {
			//box = ItemBox.Spawn ();
            box = Instantiate(ItemBox);
        }
        else if (type == 4)
        {
			//box = prison.Spawn ();
           box = Instantiate(prison);
        }
        else if (type == 5)
        {
			//box = corn.Spawn ();
            box = Instantiate(corn);
        }
        else if (type == 6)
        {
			//box = barricade.Spawn ();
            box = Instantiate(barricade);
        }

        box.transform.position = pos;
        box.transform.rotation = rot;
        
    }
}

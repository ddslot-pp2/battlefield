using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPointManager : MonoBehaviour {
    public bool levelUp;
    public bool exit;
    float height;
    //float width;
    GameObject obj;
    public GameObject[] levelPoint;
    // Use this for initialization
    void Start () {
        height = 2 * Camera.main.orthographicSize;
        //width = height * Camera.main.aspect;
        levelUp = false;
        exit = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(levelUp == true)
        { 
            transform.position += new Vector3(0, 100 * Time.deltaTime, 0);
            Debug.Log("asd");
            
            if (transform.position.y >= height + 75)
            {
                levelUp = false;
            }
        }

        if(exit == true)
        {
            levelUp = false;
            transform.position -= new Vector3(0, 100 * Time.deltaTime, 0);
            if (transform.position.y <= height - 120 )
            {
                exit = false;
                DestroyLevelIpPoint();
            }
        }

    }

    public void LevelUpPoint()
    {
        levelUp = true;
        int[] num = { 0, 1, 2, 3, 4 };
        for(int i = 0; i < 10; i++)
        {
            int k = Random.Range(0, 5);
            int j = Random.Range(0, 5);

            int p = num[k];
            num[k] = num[j];
            num[j] = p;
        }

        obj = Instantiate(levelPoint[num[0]]);
        obj.transform.parent = gameObject.transform;
        obj.transform.localScale = new Vector3(1, 1, 1);

        obj = Instantiate(levelPoint[num[1]]);
        obj.transform.parent = gameObject.transform;
        obj.transform.localScale = new Vector3(1, 1, 1);

        obj = Instantiate(levelPoint[num[2]]);
        obj.transform.parent = gameObject.transform;
        obj.transform.localScale = new Vector3(1, 1, 1);

        Debug.Log("asd");
    }

    public void DestroyLevelIpPoint()
    {
        Transform[] childList = GetComponentsInChildren<Transform>(true);
        if (childList != null)
        {
            for (int i = 0; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    Destroy(childList[i].gameObject);
            }
        }
    }
}

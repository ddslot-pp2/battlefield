using UnityEngine;
using System.Collections;
using System;

public class FollowCam : MonoBehaviour {

    private GameObject Player;

    public float offsetX;
    public float offsetY;
    public float offsetZ;

    public float followSpeed = 0.25f;

    Vector3 comeraPosition;

    void Start()
    {
        Player = GameObject.Find("Main Camera").GetComponent<GameLoad>().userTank;
    }
        
    void LateUpdate()
    {
        try
        {
            comeraPosition.x = Player.transform.position.x + offsetX;
            comeraPosition.y = Player.transform.position.y + offsetY;
            comeraPosition.z = Player.transform.position.z + offsetZ;

            transform.position = Vector3.Lerp(transform.position, comeraPosition, followSpeed * Time.deltaTime);
        }
        catch(MissingReferenceException)
        {
            Player = GameObject.Find("Main Camera").GetComponent<GameLoad>().userTank;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controller : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {


	public Image joystickImg;
    private Vector3 inputVector;
    private Vector3 first;
    public Vector3 dir;
    public Vector2 pos;
    public float distance;

	public delegate void ControllDelegate(Vector3 pos);

	public ControllDelegate DirDelegate;

    // Use this for initialization
    
	static 	Controller _Instance;

	public static Controller Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = GameObject.FindObjectOfType(typeof(Controller)) as Controller;

				if (_Instance == null)
				{
					GameObject kNewObject = GameObject.Instantiate( Resources.Load("Prefab/Global/Controller") ) as GameObject;
					kNewObject.name = "Controller";

					DontDestroyOnLoad( kNewObject );

					_Instance = kNewObject.GetComponent<Controller>();
				}
			}

			return _Instance;
		}
	}


    void Start () {
       
        first = joystickImg.transform.position;
    }
	
    public void OnDrag(PointerEventData ped)
    {
        dir = (new Vector3(ped.position.x, ped.position.y) - first).normalized;
        pos = ped.position;
        distance = Vector3.Distance(first, pos);
        if (distance >= gameObject.transform.localScale.x*50)
        {
            joystickImg.transform.position = first + (dir * gameObject.transform.localScale.x * 50);
        }
        else
        {
            joystickImg.transform.position = ped.position;
        }
    }

	public void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

   public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
        dir = new Vector3(0 , 0,0);
    }

    public float GetHorizontalValue()
    {
        return inputVector.x;
    }

    public float GetVerticalValue()
    {
        return inputVector.y;
    }

    public void OnClick()
    {
        Debug.Log("123");
    }

	public void Update()
	{
		if (null != DirDelegate) DirDelegate(dir);

		if (Input.GetMouseButtonDown(0))
		{

		}
	}

}

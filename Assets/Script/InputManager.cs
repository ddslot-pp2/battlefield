using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InputManager : MonoBehaviour {


	public delegate void TestDelegate();
	public TestDelegate test1dele;
	public TestDelegate test2dele;

	static 	InputManager _Instance;

	public delegate void ControllDelegate(Vector3 pos);
	public ControllDelegate ClickDelegate;
	public ControllDelegate DirDelegate;

	private bool CheckUiBegin = false;
	private bool CheckUiBegin2 = false;

	public static InputManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = GameObject.FindObjectOfType(typeof(InputManager)) as InputManager;

				if (_Instance == null)
				{
					GameObject kNewObject = GameObject.Instantiate( Resources.Load("Prefab/Global/InputManager") ) as GameObject;
					kNewObject.name = "test";

					DontDestroyOnLoad( kNewObject );

					_Instance = kNewObject.GetComponent<InputManager>();
				}
			}

			return _Instance;
		}
	}

	public void Update()
	{

		if (IsTouchBegan())
		{

		}

		if (IsTouchEnded())
		{
			if (null != ClickDelegate) ClickDelegate(GetTouchPosition(0));
		}
	}


	public static Vector3 GetTouchPosition(int touchIndex)
	{
		#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX )
		return Input.mousePosition;
		#else
		return Input.touches[touchIndex].position;
		#endif
	}


	bool IsPointerOverUiObjectTouch(int touchIndex)
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
		eventDataCurrentPosition.position = new Vector2 (Input.touches[touchIndex].position.x, Input.touches[touchIndex].position.y);

		List<RaycastResult> results = new List<RaycastResult> ();

		EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	bool IsPointerOverUiObjectMouse()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
		eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

		List<RaycastResult> results = new List<RaycastResult> ();

		EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	bool IsTouchEnded()
	{
		#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX )

		if( IsPointerOverUiObjectMouse() )
		{
			return false;
		}

		if( CheckUiBegin ) 
		{
			CheckUiBegin = false;
			return false;
		}

		return Input.GetButtonUp("Fire1");
		#else

		if( Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
		{
		if( CheckUiBegin ) 
		{
		CheckUiBegin = false;
		}
		else
		{
		if (null != ClickDelegate) ClickDelegate(GetTouchPosition(0));
		}
		}

		if( Input.touchCount > 1 && (Input.touches[1].phase == TouchPhase.Ended || Input.touches[1].phase == TouchPhase.Canceled)
		{
		if( CheckUiBegin2 ) 
		{
		CheckUiBegin2 = false;
		}
		else
		{
		if (null != ClickDelegate) ClickDelegate(GetTouchPosition(1));
		}		
		}

		return false
		#endif
	}

	bool IsTouchBegan()
	{
		#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX )
		if (Input.GetButtonDown("Fire1"))
		{

			if(IsPointerOverUiObjectMouse() )
			{
				Debug.Log("IsPointerOverUiObjectMouse");
				CheckUiBegin = true;
				return false;
			}

			return true;
		}

		return false;
		#else

		bool checkBegin = false;
		if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
		{
		if( IsPointerOverUiObjectTouch(0))
		{
		CheckUiBegin = true;
		}
		else
		{

		_beginPosition = Input.touches[0].position;
		checkBegin = true;
		}
		}

		if (Input.touchCount > 1 && Input.touches[1].phase == TouchPhase.Began)
		{
		if( IsPointerOverUiObjectTouch(1))
		{
		CheckUiBegin2 = true;
		}
		else
		{
		_beginPosition2 = Input.touches[1].position;
		checkBegin = true;
		}
		}

		return checkBegin;

		#endif
	}
}

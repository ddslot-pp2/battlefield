using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace GameCore
{
    /// <summary>
    /// 사용자의 입력을 분석해서 Began, Moved, Ended 등으로 구분해서 알려준다.
    /// 유니티툴이나 모바일 디바이스 구분없이 사용할수 있다.
    /// </summary>
    public class TouchDispatcher
    {
        public delegate void TouchDelegate(Vector3 pos);

        public TouchDelegate BeganDelegate;
        public TouchDelegate MovedDelegate;
        public TouchDelegate PressedDelegate;
        public TouchDelegate EndedDelegate;
		public TouchDelegate DoublePressedDelegate;

        private Vector3 _beginPosition;
        private Vector3 _prevPosition;
		private Vector3 _positionForDBClick = Vector3.zero;
		private int pointId = -1;
		private float _prevTouchTime;
		private bool CheckUiBegin = false;


	
		public bool UsableDBClickMode = false;

		void Awake()
		{

			#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX)
				pointId	 = -1;
			#else
				pointId = 0;
			#endif
		}
        // Update is called once per frame
        public void Update()
        {
	       if( Application.platform == RuntimePlatform.Android && Input.GetKey(KeyCode.Escape) )
			{
				Application.Quit();
				return;
			}

		
            if (IsTouchBegan())
            {
                //Debug.Log("Touch Began! : " + GetTouchPosition().ToString());
				
				if (null != BeganDelegate) BeganDelegate(GetTouchPosition());
            }
            else if (IsTouchMoved())
            {
                //Debug.Log("Touch Moved! : " + GetTouchPosition().ToString());
                if (null != MovedDelegate) MovedDelegate(GetTouchPosition());
            }
            else if (IsTouchPressed())
            {
                if (null != PressedDelegate) PressedDelegate(GetTouchPosition());
            }
            else if (IsTouchEnded())
            {
				bool bExistDBClick = false;
				if( Time.realtimeSinceStartup - _prevTouchTime <= 0.3f )
					//&& ( _positionForDBClick - GetTouchPosition() ).magnitude <= 2.0f )
				{
					if( UsableDBClickMode && DoublePressedDelegate != null )	
					{
						DoublePressedDelegate( GetTouchPosition() );
						bExistDBClick = true;
					}
				}
				
				if( bExistDBClick == false )
				{
                	//Debug.Log("Touch Ended! : " + GetTouchPosition().ToString());
                	if (null != EndedDelegate) EndedDelegate(GetTouchPosition());
				}
				
				_prevTouchTime = Time.realtimeSinceStartup;
				_positionForDBClick = GetTouchPosition();
            }
        }

        public void LateUpdate()
        {
			


#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX)
            _prevPosition = Input.mousePosition;
#else
            if (Input.touchCount > 0)
            {
                _prevPosition = Input.touches[0].position;
            }
#endif
        }

		bool IsPointerOverUiObjectTouch()
		{
			PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
			eventDataCurrentPosition.position = new Vector2 (Input.touches[0].position.x, Input.touches[0].position.y);

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


			if( IsPointerOverUiObjectTouch() )
			{
				return false;
			}
		
			if( CheckUiBegin ) 
			{
				CheckUiBegin = false;
				return false;
			}

        	return Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled);
#endif
        }


        bool IsTouchPressed()
        {
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX )
            return Input.GetButton("Fire1");
#else
            return Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Stationary;
#endif
        }



		bool IsPointerOverGameObject( int fingerId )
		{

			EventSystem eventSystem = EventSystem.current;
			 //return ( eventSystem.IsPointerOverGameObject( fingerId ) && eventSystem.currentSelectedGameObject != null );
			return ( eventSystem.IsPointerOverGameObject( fingerId ) );
		}

		bool IsPointerOverGameObject()
		{

			EventSystem eventSystem = EventSystem.current;
			return ( eventSystem.IsPointerOverGameObject() && eventSystem.currentSelectedGameObject != null );
		}
		

        bool IsTouchMoved()
        {
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX )

			if (Input.GetButton("Fire1") )
            {
                bool moved = _prevPosition != Input.mousePosition;
                return moved;
            }

            return false;
#else
        return Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved;
#endif
        }

        bool IsTouchBegan()
        {
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX )
            if (Input.GetButtonDown("Fire1"))
            {

				if(IsPointerOverUiObjectMouse() )
				{
					CheckUiBegin = true;
				}

                _beginPosition = Input.mousePosition;
                return true;
            }

            return false;
#else


            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {

				if(IsPointerOverUiObjectTouch() )
				{
					CheckUiBegin = true;
				}

                _beginPosition = Input.touches[0].position;
                return true;
            }
            return false;
#endif
        }

        public static Vector3 GetTouchPosition()
        {
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX )
            return Input.mousePosition;
#else
		return Input.touches[0].position;
#endif
        }
		
		public static Vector3 GetNextTouchPosition()
        {
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX )
            return Vector3.zero;
#else
			if( Input.touchCount >= 2 )
				return Input.touches[1].position;
			else
				return Vector3.zero;
#endif
        }
		
		public static int GetTouchCount()	
		{
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX )
            return 1;
#else
		return Input.touchCount;
#endif	
		}

        public Vector3 BeginPosition
        {
            get { return _beginPosition; }
        }

        public Vector3 PrevTouchPosition
        {
            get { return _prevPosition; }
        }

        public static Vector2 GetTouchDeltaPosition()
        {
            Vector2 delta = Vector2.zero;
            
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX )
            delta.x = -Input.GetAxis("Mouse X");
            delta.y = -Input.GetAxis("Mouse Y");
#else
            if (Input.touchCount > 0)
            {
                delta = -Input.touches[0].deltaPosition;
            }
#endif
            return delta;
        }
    }




}

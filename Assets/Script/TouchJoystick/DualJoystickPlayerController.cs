using UnityEngine;

public class DualJoystickPlayerController : MonoBehaviour
{
    public LeftJoystick leftJoystick; // the game object containing the LeftJoystick script
    public RightJoystick rightJoystick; // the game object containing the RightJoystick script
   
    private Vector3 leftJoystickInput; // holds the input of the Left Joystick
    private Vector3 rightJoystickInput; // hold the input of the Right Joystick
   

	static 	DualJoystickPlayerController _Instance;

	public static DualJoystickPlayerController Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = GameObject.FindObjectOfType(typeof(DualJoystickPlayerController)) as DualJoystickPlayerController;

				if (_Instance == null)
				{
					GameObject kNewObject = GameObject.Instantiate( Resources.Load("Prefab/Global/DualJoystickPlayerController") ) as GameObject;
					kNewObject.name = "DualJoystickPlayerController";

					DontDestroyOnLoad( kNewObject );

					_Instance = kNewObject.GetComponent<DualJoystickPlayerController>();
				}
			}

			return _Instance;
		}
	}

    void Start()
    {

		if (leftJoystick == null)
		{
			Debug.LogError("The left joystick is not attached.");
		}

		if (rightJoystick == null)
		{
			Debug.LogError("The right joystick is not attached.");
		}
		 
	
    }


	public Vector3 GetleftJoystickDirection()
	{
		return leftJoystick.GetInputDirection();
	}

	public Vector3 GetRightJoystickDirection()
	{
		return rightJoystick.GetInputDirection();
	}


   
}
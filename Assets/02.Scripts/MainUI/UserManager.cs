using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour {
    public string nickName;
    public int getGem;
    public int getGold;

    public List<int> haveTank = new List<int>();
    public int selectTank;
    public int Maxscore;
    public int beforScore;
    public int aliveTime;
    public int beforeAliveTime;
    public int rescue;
    public int beforeRescue;
    public List<int> useItem;
    public List<int> TouchItem;

	static 	UserManager _Instance;


	public static UserManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = GameObject.FindObjectOfType(typeof(UserManager)) as UserManager;

				if (_Instance == null)
				{
					GameObject kNewObject = GameObject.Instantiate( Resources.Load("Prefab/Global/UserManager") ) as GameObject;
					kNewObject.name = "UserManager";

					DontDestroyOnLoad( kNewObject );

					_Instance = kNewObject.GetComponent<UserManager>();
				}
			}

			return _Instance;
		}
	}
    void Awake()
    {
        DontDestroyOnLoad(this);

        Maxscore = 400000;
        beforScore = 2000;
        getGem = 10000 ;
        getGold = 10000;
        haveTank.Add(1);
        haveTank.Add(3);
        haveTank.Add(4);
        selectTank = 2;
        aliveTime = 12600;
        beforeAliveTime = 1000;
        rescue = 800;
        beforeRescue = 800;

    }
    
}

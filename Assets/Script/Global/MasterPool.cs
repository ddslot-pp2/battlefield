using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class MasterPool : MonoBehaviour
{
	public enum StartupPoolMode { Awake, Start, CallManually };

	public static bool NEW_CREATE = true;
	
	[System.Serializable]
	public class StartupPool
	{
		public string Name;
		public GameObject prefab;
		public int size;
	}
	
	static MasterPool _instance;
	static List<GameObject> tempList = new List<GameObject>();
	
	Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();
	Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();
	
	public StartupPoolMode startupPoolMode;
	public StartupPool[] startupPools;
	
	bool startupPoolsCreated;
	
	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);

			if (startupPoolMode == StartupPoolMode.Awake)
				CreateStartupPools();
		}
		else
			Destroy(gameObject);
	}

	public void RemoveAll()
	{

	}
	
	void Start()
	{
		if (startupPoolMode == StartupPoolMode.Start)
			CreateStartupPools();
	}

	public void CheckRefillPool()
	{
		CreateStartupPools();

		RecycleAll();

		var pools = instance.startupPools;
		if (pools != null && pools.Length > 0)
		{
			for (int i = 0; i < pools.Length; ++i)
			{
				CreatePoolRefill(pools[i].prefab, pools[i].size );
			}
		}
	}

	public static void CreatePoolRefill(GameObject prefab, int initialPoolSize)
	{
		if (prefab != null )
		{
			var list = instance.pooledObjects[prefab];
		
			if (initialPoolSize > 0)
			{
				bool active = prefab.activeSelf;
				//prefab.SetActive(false);
				Transform parent = instance.transform;
				while (list.Count < initialPoolSize)
				{
					var obj = (GameObject)Object.Instantiate(prefab);
					obj.transform.parent = parent;
					obj.SetActive(false);
					list.Add(obj);
				}

				//prefab.SetActive(active);
			}
		}
	}



	public static void CreateStartupPools()
	{
		if (!instance.startupPoolsCreated)
		{
			instance.startupPoolsCreated = true;
			var pools = instance.startupPools;
			if (pools != null && pools.Length > 0)
				for (int i = 0; i < pools.Length; ++i)
					CreatePool(pools[i].prefab, pools[i].size);
		}
	}
	
	public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
	{
		CreatePool(prefab.gameObject, initialPoolSize);
	}
	public static void CreatePool(GameObject prefab, int initialPoolSize)
	{
		if (prefab != null && !instance.pooledObjects.ContainsKey(prefab))
		{
			var list = new List<GameObject>();
			instance.pooledObjects.Add(prefab, list);
			
			if (initialPoolSize > 0)
			{
				bool active = prefab.activeSelf;
				//prefab.SetActive(false);
				Transform parent = instance.transform;
				while (list.Count < initialPoolSize)
				{
					var obj = (GameObject)Object.Instantiate(prefab);
					obj.transform.parent = parent;
					obj.SetActive(false);
					list.Add(obj);
				}
				//prefab.SetActive(active);
			}
		}
	}
	
	public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
	{
		return Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
	}
	public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
	}
	public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
	{
		return Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
	}
	public static T Spawn<T>(T prefab, Vector3 position) where T : Component
	{
		return Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
	}
	public static T Spawn<T>(T prefab, Transform parent) where T : Component
	{
		return Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
	}
	public static T Spawn<T>(T prefab) where T : Component
	{
		return Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
	}
	public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		List<GameObject> list;
		Transform trans;
		GameObject obj;
		if (instance.pooledObjects.TryGetValue (prefab, out list)) 
		{
			obj = null;
			if (list.Count > 0) {
				while (obj == null && list.Count > 0) {
					obj = list [0];
					list.RemoveAt (0);
				}
				if (obj != null) {
					trans = obj.transform;
					trans.parent = parent;
					trans.localPosition = position;
					trans.localRotation = rotation;
					obj.SetActive (true);
					instance.spawnedObjects.Add (obj, prefab);
					return obj;
				}
			}

			if (NEW_CREATE) {
				obj = (GameObject)Object.Instantiate (prefab);
				trans = obj.transform;
				trans.parent = parent;
				trans.localPosition = position;
				trans.localRotation = rotation;
				instance.spawnedObjects.Add (obj, prefab);
				return obj;
			} else
				return null;
		} 
		else 
		{
			CreatePool (prefab, 5);
			if (instance.pooledObjects.TryGetValue (prefab, out list))
			{
				obj = null;
				if (list.Count > 0) {
					while (obj == null && list.Count > 0) {
						obj = list [0];
						list.RemoveAt (0);
					}
					if (obj != null) {
						trans = obj.transform;
						trans.parent = parent;
						trans.localPosition = position;
						trans.localRotation = rotation;
						obj.SetActive (true);
						instance.spawnedObjects.Add (obj, prefab);
						return obj;
					}
				}
			}

			return null;
		}
		/*
		else
		{
			Debug.Log ("add spawnobject");

			CreatePool (prefab, 5);

			obj = (GameObject)Object.Instantiate(prefab);
			//Debug.Log("spawn object: " + obj.name );
			trans = obj.GetComponent<Transform>();
			trans.parent = parent;
			trans.localPosition = position;
			trans.localRotation = rotation;
			//instance.spawnedObjects.Add(obj, prefab);
			return obj;
		}
		*/

	}
	public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
	{
		return Spawn(prefab, parent, position, Quaternion.identity);
	}
	public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return Spawn(prefab, null, position, rotation);
	}
	public static GameObject Spawn(GameObject prefab, Transform parent)
	{
		return Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}
	public static GameObject Spawn(GameObject prefab, Vector3 position)
	{
		return Spawn(prefab, null, position, Quaternion.identity);
	}
	public static GameObject Spawn(GameObject prefab)
	{
		return Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}
	
	public static void Recycle<T>(T obj) where T : Component
	{
		Recycle(obj.gameObject);
	}
	public static void Recycle(GameObject obj)
	{
		GameObject prefab;
		if (instance.spawnedObjects.TryGetValue(obj, out prefab))
			Recycle(obj, prefab);
		else
			Object.Destroy(obj);
	}
	static void Recycle(GameObject obj, GameObject prefab)
	{
		if (obj == null) return;

		instance.pooledObjects[prefab].Add(obj);
		instance.spawnedObjects.Remove(obj);
		obj.transform.parent = instance.transform;
		obj.transform.localScale = prefab.transform.localScale;
		obj.SetActive(false);
	}
	
	public static void RecycleAll<T>(T prefab) where T : Component
	{
		RecycleAll(prefab.gameObject);
	}
	public static void RecycleAll(GameObject prefab)
	{
		foreach (var item in instance.spawnedObjects)
			if (item.Value == prefab)
				tempList.Add(item.Key);
		for (int i = 0; i < tempList.Count; ++i)
			Recycle(tempList[i]);
		tempList.Clear();
	}
	public static void RecycleAll()
	{
		tempList.AddRange(instance.spawnedObjects.Keys);
		for (int i = 0; i < tempList.Count; ++i)
			Recycle(tempList[i]);
		tempList.Clear();
	}
	
	public static bool IsSpawned(GameObject obj)
	{
		return instance.spawnedObjects.ContainsKey(obj);
	}
	
	public static int CountPooled<T>(T prefab) where T : Component
	{
		return CountPooled(prefab.gameObject);
	}
	public static int CountPooled(GameObject prefab)
	{
		List<GameObject> list;
		if (instance.pooledObjects.TryGetValue(prefab, out list))
			return list.Count;
		return 0;
	}
	
	public static int CountSpawned<T>(T prefab) where T : Component
	{
		return CountSpawned(prefab.gameObject);
	}
	public static int CountSpawned(GameObject prefab)
	{
		int count = 0 ;
		foreach (var instancePrefab in instance.spawnedObjects.Values)
			if (prefab == instancePrefab)
				++count;
		return count;
	}
	
	public static int CountAllPooled()
	{
		int count = 0;
		foreach (var list in instance.pooledObjects.Values)
			count += list.Count;
		return count;
	}
	
	public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
			list = new List<GameObject>();
		if (!appendList)
			list.Clear();
		List<GameObject> pooled;
		if (instance.pooledObjects.TryGetValue(prefab, out pooled))
			list.AddRange(pooled);
		return list;
	}
	public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
			list = new List<T>();
		if (!appendList)
			list.Clear();
		List<GameObject> pooled;
		if (instance.pooledObjects.TryGetValue(prefab.gameObject, out pooled))
			for (int i = 0; i < pooled.Count; ++i)
				list.Add(pooled[i].GetComponent<T>());
		return list;
	}
	
	public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
			list = new List<GameObject>();
		if (!appendList)
			list.Clear();
		foreach (var item in instance.spawnedObjects)
			if (item.Value == prefab)
				list.Add(item.Key);
		return list;
	}
	public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
			list = new List<T>();
		if (!appendList)
			list.Clear();
		var prefabObj = prefab.gameObject;
		foreach (var item in instance.spawnedObjects)
			if (item.Value == prefabObj)
				list.Add(item.Key.GetComponent<T>());
		return list;
	}

	public static void DestroyPoolAll()
	{
		foreach( KeyValuePair< GameObject, List<GameObject> > kPair in instance.pooledObjects )
		{
			for (int i = 0; i < kPair.Value.Count; ++i)
				GameObject.Destroy(kPair.Value[i]);

			kPair.Value.Clear();
		}

		instance.startupPoolsCreated = false;
	}
	
	public static void DestroyPooled(GameObject prefab)
	{
		List<GameObject> pooled;
		if (instance.pooledObjects.TryGetValue(prefab, out pooled))
		{
			for (int i = 0; i < pooled.Count; ++i)
				GameObject.Destroy(pooled[i]);
			pooled.Clear();
		}
	}
	public static void DestroyPooled<T>(T prefab) where T : Component
	{
		DestroyPooled(prefab.gameObject);
	}
	
	public static void DestroyAll(GameObject prefab)
	{
		RecycleAll(prefab);
		DestroyPooled(prefab);
	}
	public static void DestroyAll<T>(T prefab) where T : Component
	{
		DestroyAll(prefab.gameObject);
	}
	
	public static MasterPool instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType(typeof(MasterPool)) as MasterPool;
				
				if (_instance == null)
				{
					GameObject kNewObject = GameObject.Instantiate( Resources.Load("Prefab/Global/MasterPool") ) as GameObject;
					kNewObject.name = "MasterPool";

					DontDestroyOnLoad( kNewObject );

					_instance = kNewObject.GetComponent<MasterPool>();
				}
			}
			
			return _instance;
		}
	}
}

public static class MasterPoolExtensions
{
	public static void CreatePool<T>(this T prefab) where T : Component
	{
		MasterPool.CreatePool(prefab, 0);
	}
	public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component
	{
		MasterPool.CreatePool(prefab, initialPoolSize);
	}
	public static void CreatePool(this GameObject prefab)
	{
		MasterPool.CreatePool(prefab, 0);
	}
	public static void CreatePool(this GameObject prefab, int initialPoolSize)
	{
		MasterPool.CreatePool(prefab, initialPoolSize);
	}
	
	public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
	{
		return MasterPool.Spawn(prefab, parent, position, rotation);
	}
	public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return MasterPool.Spawn(prefab, null, position, rotation);
	}
	public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component
	{
		return MasterPool.Spawn(prefab, parent, position, Quaternion.identity);
	}
	public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
	{
		return MasterPool.Spawn(prefab, null, position, Quaternion.identity);
	}
	public static T Spawn<T>(this T prefab, Transform parent) where T : Component
	{
		return MasterPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}
	public static T Spawn<T>(this T prefab) where T : Component
	{
		return MasterPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}
	public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		return MasterPool.Spawn(prefab, parent, position, rotation);
	}
	public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return MasterPool.Spawn(prefab, null, position, rotation);
	}
	public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
	{
		return MasterPool.Spawn(prefab, parent, position, Quaternion.identity);
	}
	public static GameObject Spawn(this GameObject prefab, Vector3 position)
	{
		return MasterPool.Spawn(prefab, null, position, Quaternion.identity);
	}
	public static GameObject Spawn(this GameObject prefab, Transform parent)
	{
		return MasterPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}
	public static GameObject Spawn(this GameObject prefab)
	{
		return MasterPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}
	
	public static void Recycle<T>(this T obj) where T : Component
	{
		MasterPool.Recycle(obj);
	}
	public static void Recycle(this GameObject obj)
	{
		MasterPool.Recycle(obj);
	}
	
	public static void RecycleAll<T>(this T prefab) where T : Component
	{
		MasterPool.RecycleAll(prefab);
	}
	public static void RecycleAll(this GameObject prefab)
	{
		MasterPool.RecycleAll(prefab);
	}
	
	public static int CountPooled<T>(this T prefab) where T : Component
	{
		return MasterPool.CountPooled(prefab);
	}
	public static int CountPooled(this GameObject prefab)
	{
		return MasterPool.CountPooled(prefab);
	}
	
	public static int CountSpawned<T>(this T prefab) where T : Component
	{
		return MasterPool.CountSpawned(prefab);
	}
	public static int CountSpawned(this GameObject prefab)
	{
		return MasterPool.CountSpawned(prefab);
	}
	
	public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list, bool appendList)
	{
		return MasterPool.GetSpawned(prefab, list, appendList);
	}
	public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list)
	{
		return MasterPool.GetSpawned(prefab, list, false);
	}
	public static List<GameObject> GetSpawned(this GameObject prefab)
	{
		return MasterPool.GetSpawned(prefab, null, false);
	}
	public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component
	{
		return MasterPool.GetSpawned(prefab, list, appendList);
	}
	public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component
	{
		return MasterPool.GetSpawned(prefab, list, false);
	}
	public static List<T> GetSpawned<T>(this T prefab) where T : Component
	{
		return MasterPool.GetSpawned(prefab, null, false);
	}
	
	public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list, bool appendList)
	{
		return MasterPool.GetPooled(prefab, list, appendList);
	}
	public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list)
	{
		return MasterPool.GetPooled(prefab, list, false);
	}
	public static List<GameObject> GetPooled(this GameObject prefab)
	{
		return MasterPool.GetPooled(prefab, null, false);
	}
	public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component
	{
		return MasterPool.GetPooled(prefab, list, appendList);
	}
	public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component
	{
		return MasterPool.GetPooled(prefab, list, false);
	}
	public static List<T> GetPooled<T>(this T prefab) where T : Component
	{
		return MasterPool.GetPooled(prefab, null, false);
	}
	
	public static void DestroyPooled(this GameObject prefab)
	{
		MasterPool.DestroyPooled(prefab);
	}
	public static void DestroyPooled<T>(this T prefab) where T : Component
	{
		MasterPool.DestroyPooled(prefab.gameObject);
	}
	
	public static void DestroyAll(this GameObject prefab)
	{
		MasterPool.DestroyAll(prefab);
	}
	public static void DestroyAll<T>(this T prefab) where T : Component
	{
		MasterPool.DestroyAll(prefab.gameObject);
	}
}

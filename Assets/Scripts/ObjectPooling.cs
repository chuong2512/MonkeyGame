//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuongBaoToanTools
{
	public class ObjectPooling : MonoBehaviour
	{
		public static ObjectPooling INSTANCE;

		public GameObject bullet,
		boom,
		blood,
		enemy1,
		enemy2,
		enemy3,
		enemy4,
		enemy5,
		enemy6;
		public List<GameObject> pooledBullet,
		pooledBoom,
		pooledBlood,
		pooledEnemy1,
		pooledEnemy2,
		pooledEnemy3,
		pooledEnemy4,
		pooledEnemy5,
		pooledEnemy6;

		private	void Awake ()
		{
			INSTANCE = this;
		}

		void Start ()
		{
			FillPoolObject (10, bullet, pooledBullet);
			FillPoolObject (10, enemy1, pooledEnemy1);
			FillPoolObject (10, enemy2, pooledEnemy2);
			FillPoolObject (10, enemy3, pooledEnemy3);
			FillPoolObject (10, enemy4, pooledEnemy4);
			FillPoolObject (10, enemy5, pooledEnemy5);
			FillPoolObject (10, enemy6, pooledEnemy6);
			FillPoolObject (10, boom, pooledBoom);
			FillPoolObject (5, blood, pooledBlood);
		}
		///<Summary>
		///Implement this method to instatiate pool object and fill them to a list object
		///</Summary>
		public void FillPoolObject(int _fillAmount, GameObject _objectToPool, List<GameObject> _pooledList)
		{
			for (int i = 0; i < _fillAmount; i++) {
				GameObject poolObj;
				poolObj = Instantiate (_objectToPool, Vector3.zero, Quaternion.identity);
				if (_objectToPool.GetComponent<EnemyContronller> () != null) {
					GameManager.Instance.enemyList.Add (poolObj);
				}
				poolObj.transform.SetParent (transform);
				_pooledList.Add (poolObj);
				poolObj.SetActive (false);
			}
		}

		///<Summary>
		///Implement this method to get object from poolist
		///</Summary>
		public GameObject GetObjectFromPoolList(List<GameObject> _pooledList, Vector3 newObjectPos)
		{
			for (int i = 0; i < _pooledList.Count; i++) {
				if (!_pooledList [i].activeInHierarchy) {
					_pooledList [i].transform.position = newObjectPos;
					_pooledList [i].SetActive (true);
					return _pooledList [i];
				}
			}
			return null;
		}
	}
}

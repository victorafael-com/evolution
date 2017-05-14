using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPool<T> where T:MonoBehaviour, IPoolBehaviour{
	private GameObject prefab;
	private int incrementCount;
	private List<T> items;

	private Transform poolRoot;

	private static Transform parentObject;

	public PrefabPool(Transform parentObject, GameObject _prefab, int _startAmmount, int _incrementAmmount){
		poolRoot = new GameObject (_prefab.name + " pool").transform;
		poolRoot.transform.position = new Vector3(0,0,-20);
		poolRoot.parent = parentObject;
		prefab = _prefab;
		incrementCount = _incrementAmmount;
		items = new List<T> ();
		Increment (_startAmmount);
	}

	private void Increment(int count){
		for (int i = 0; i < count; i++) {
			GameObject g = GameObject.Instantiate (prefab);
			items.Add (g.GetComponent<T>());
			g.SetActive (false);
			g.transform.SetParent (poolRoot);
		}
	}

	public T Take(){
		if (items.Count == 0) {
			Increment (incrementCount);
		}
		T ret = items [0];
		items.RemoveAt (0);

		return ret;
	}
	public void Return(T g){
		items.Add (g);
		g.Dettach ();
		g.transform.parent = poolRoot;
		g.transform.position = poolRoot.transform.position;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayList : MonoBehaviour {
    public GameObject[] allGameObjects;
	// Use this for initialization
	void Start () {
        //ArrayList aList = new ArrayList();
        List<GameObject> aList = new List<GameObject>();
        Object[] allObjects = GameObject.FindObjectsOfType(typeof(Object)) as Object[];

        foreach (Object o in allObjects)
        {
            GameObject go = o as GameObject;
            
            if (go != null)
            {
                aList.Add(go);
            }
            Debug.Log(o);
            aList.Add(o as GameObject) ;
        }
        allGameObjects = new GameObject[aList.Count];
        aList.CopyTo(allGameObjects);

        string s = "Primera palabraSegunda palabra";
        print(s);
        bool b = s.Contains("algo");
        print(b);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

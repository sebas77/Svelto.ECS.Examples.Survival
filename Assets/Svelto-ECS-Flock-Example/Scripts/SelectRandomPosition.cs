using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRandomPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (Input.GetKeyDown(KeyCode.Space))	
        {
            var t = transform;
            t.position += (Random.insideUnitSphere) * 1;
        }
	}
}

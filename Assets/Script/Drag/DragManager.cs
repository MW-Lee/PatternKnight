using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{

    public static DragManager instance = null;
    public GameObject Target;

	// Use this for initialization
	void Start ()
    {
        if (instance == null)
            instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButtonUp(0) && Target != null)
        {
            Vector3 newPos = Target.transform.position;
            newPos.z += 1f;
            Target.transform.position = newPos;
            Target = null;
        }
	}
}

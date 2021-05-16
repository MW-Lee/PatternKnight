using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Vil : MonoBehaviour
{
    public float speed;

	void Start ()
    {
		
	}

	void Update ()
    {
        float PosX = transform.position.x;

        transform.position = new Vector3(
            PosX += Time.deltaTime * speed,
            transform.position.y
            );

        if (transform.position.x >= 11)
            transform.position = new Vector3(-11, transform.position.y);
	}
}

using UnityEngine;
using System.Collections;

public class Practice : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;

    void Start()
    {

    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, endMarker.position, 0.05f);

        //if (transform.position == endMarker.position)
        if (Vector3.Distance(transform.position, endMarker.position) <= 0.03f)
        {
            transform.position = endMarker.position;

            if (Input.GetKeyDown(KeyCode.R))
                transform.position = startMarker.position;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGManager : MonoBehaviour
{
    static public BGManager instance;

    public GameObject Sky;

    public bool bBGMove = false;

    private void Awake()
    {
        instance = this;

        //DontDestroy.a.FadeInStart();
    }

    private void Update()
    {
        if(bBGMove)
        {
            Sky.transform.Translate(Vector3.left * Time.deltaTime * 2);

            if (Sky.transform.position.x <= -38)
            {
                Sky.transform.position = new Vector3(0, 0, 0);
            }
        }
    }

}

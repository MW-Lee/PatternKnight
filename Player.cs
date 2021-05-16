using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("LeftArrow");
            InputManager.instance.AddKey(KeyCode.LeftArrow);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("RightArrow");
            InputManager.instance.AddKey(KeyCode.RightArrow);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SpaceBar");
            InputManager.instance.AddKey(KeyCode.Space);
            InputManager.instance.PrintStack();
            InputManager.instance.CheckCommand();
        }
    }
}

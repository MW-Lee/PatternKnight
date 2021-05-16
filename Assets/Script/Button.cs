using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public char key;

    public void OnClick_Character()
    {
        InputManager.instance.AddKey(key);
        InputManager.instance.CheckCommand();
    }

    public void OnClick_Enemy()
    {
        if(!Character.Attack)
           Enemy.Attack = true;
    }
}

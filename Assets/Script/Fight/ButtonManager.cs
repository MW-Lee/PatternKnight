using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ButtonType
{
    Fight,
    Heal,
    Buff
}

public class ButtonManager : MonoBehaviour
{
    public bool IsClear;

    public ButtonManager prevButton;
    public ButtonManager nextButton;

    public int Type;

    private void Start()
    {

    }

}

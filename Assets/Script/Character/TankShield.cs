using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShield : MonoBehaviour
{
    public Animator _Ani;
    public int Count;

    private void Awake()
    {
        _Ani = GetComponent<Animator>();
    }

    private void Start()
    {
        Count = 2;
    }

    public void SetHitFalse()
    {
        _Ani.SetBool("Hit", false);
    }
}

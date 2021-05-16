using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    // 적들이 공통으로 가질 변수들
    public bool     Attack;
    public bool     Hit;
    public bool     Move;
    public Image    TimeBar;
    public float    Filltime;
    public float    Attacktime;
    public float    _HitTime;
    public Image    HPBar;
    public int      hp;
    public float    Fullhp;
    public bool     IsDie;
    public AudioSource _AS;
    public AudioClip _AC;

    public SpriteRenderer EnmSpriteRenderer;

    // 매니저에서 관리할 적
    public GameObject Enemy;

    private GameObject pEnemy;


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    // 변수 선언 부분
    static public bool Attack;      // 현재 공격중인지 나타내주는 변수
    private bool Move;              // 현재 움직이는중인지 나타내주는 변수

    //private Rigidbody2D Body;       // 캐릭터에 입혀져있는 RigidBody를 사용하기 위한 변수.
    public Animator _Ani_s;         // 캐릭터 자체 애니메이터 (힐러)
    public Animator _Ani_d;         // 캐릭터 자체 애니메이터 (딜러)

    public Text _Missmatch;


    // 함수 선언

	void Start ()
    {
        Attack = false;             // 시작할 때 기본 오프셋으로 공격중이지 않다고 설정.

        //Body = GetComponent<Rigidbody2D>();     // 캐릭터에 입혀져있는 Rigidbody를 받아온다.
	}
	
	void Update ()
    {
        
	}

    public void MakeAttackFalse()
    {
        Attack = false;                         // 공격이 끝나면 공격중이 아님을 나타내줍니다
        _Ani_s.SetInteger("Attack", 0);           // 공격 상태를 0(기본)상태롤 초기화합니다.
        //InputManager._MissMatch.gameObject.SetActive(false);  // 공격이 끝나면 글씨를 지움.
        _Missmatch.gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D Col)
    {
        Debug.Log("!#@!$!@#$");
    }
}

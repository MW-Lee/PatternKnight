using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 변수 선언
    static public bool Attack;      // 현재 공격중인지 나타내주는 변수
    static public bool Hit;
    private bool Move;              // 현재 움직이는중인지 나타내주는 변수

    public Image TimeBar;           // 몬스터는 일정 시간이 지나면 공격을 해야한다.
    private float Filltime;         // 타임 게이지가 차는걸 담당
    private float Attacktime;       // 타임 게이지의 끝부분을 담당

    public Image HPBar;
    static public int hp;
    private float Fullhp;

    private float _time;

    static public Animator _Ani;    // 캐릭터 자체 애니메이터

    public GameObject _EnmBody;     // 피격 판정을 위한 오브젝트

    public GameObject _EnmAll;      // 죽은 판정을 위한 것

    // 함수 선언

    void Start()
    {
        Attack = false;             // 기본 오프셋 - 공격중이지 않음
        Filltime = 0.0f;            // 기본 오프셋 - 시작은 0초
        Attacktime = 2.5f;          // 기본 오프셋 - 2.5초 지나면 공격
        hp = 200;
        Fullhp = 200;
        _time = 0;

       // Body = GetComponent<Rigidbody2D>();     // 캐릭터에 입혀져있는 Rigidbody를 받아온다.
        _Ani = GetComponent<Animator>();        // 캐릭터의 애니메이터를 받아온다.
    }

    void Update()
    {
        TimeBar.fillAmount = Filltime / Attacktime;     // 시간 게이지 시각효과.
        HPBar.fillAmount = hp / Fullhp;                 // Hp 게이지 시각효과

        if (!Attack && !_Ani.GetBool("Die")) Filltime += Time.deltaTime;        // 시간 게이지를 deltatime에 맞춰 증가시킴

        if (Filltime >= Attacktime)                     // 만약 정해놓은 공격할 시간이 되면
        {
            if (!Command.bAttack)                      // 주인공이 공격중이 아니면
            {
                _Ani.SetBool("Attack", true);
                Attack = true;
            }
        }

        if(Hit)
        {
            StartCoroutine(HitAnimation());
            Hit = false;
        }
    }

    public void MakeAttackFalse()
    {
        Filltime = 0;
        Attack = false;
        _Ani.SetBool("Attack", false);
    }

    public void SetHitFalse()
    {
        Hit = false;
        _Ani.SetBool("Hit", false);
    }

    public void SetCharacterHit()
    {
        if (Command.Front_Obj.name == "Character_D")
        {
            Dealler.bHit = true;
            Dealler.hp -= 30;
            if (Dealler.hp <= 0) Dealler.hp = 0;
        }
        else if (Command.Front_Obj.name == "Character_T")
        {
            Tanker.bHit = true;
            Tanker.hp -= 30;
            if (Tanker.hp <= 0) Tanker.hp = 0;
            Command.FillMP++;
        }
        else if (Command.Front_Obj.name == "Character_S")
        {
            Supporter.bHit = true;
            Supporter.hp -= 30;
            if (Supporter.hp <= 0) Supporter.hp = 0;
        }
    }

    public void SetDie()
    {
        //_EnmAll.SetActive(false);

        string sceneName = "Board_1";
        DontDestroy.a.LoadScene(sceneName);
    }

    IEnumerator HitAnimation()
    {
        _EnmBody.transform.position = new Vector3
            (0.58f,
            _EnmBody.transform.position.y,
            _EnmBody.transform.position.z);
        yield return null;

        _time = 0;
        while (_time <= 0.5f)
        {
            _time += Time.deltaTime;

            _EnmBody.transform.position = new Vector3(
                Mathf.Lerp(0.58f, transform.position.x, _time / 0.5f),
                _EnmBody.transform.position.y,
                0
                );


            if (_EnmBody.transform.position.x <= 0.01f)
            {
                _EnmBody.transform.position = new Vector3(
                    transform.position.x, 
                    _EnmBody.transform.position.y,
                    _EnmBody.transform.position.z);
            }

            yield return null;
        }
        yield return null;
    }
}

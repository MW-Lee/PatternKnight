using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Supporter : MonoBehaviour
{
    // 변수 선언
    static public bool bHit;
    static public bool Move;                  // 현재 움직이는가
    public bool sAttack;          // 캐릭터가 공격중인가
    public bool bAttack;

    //private Rigidbody2D _Body;          // RigidBody를 받아옴
    public Animator _Ani;        // Animator 를 받아옴

    private float Filltime;
    private float Attacktime;

    public Image HPBar;
    static public int hp;                   // HP
    static public float Fullhp;

    public bool IsFront;

    private float _time;
    public GameObject _Body;

    public GameObject ShieldEffect;

    public AudioSource _AS;
    public AudioClip[] _AC;


    // 함수 선언

    void Start()
    {
        Move = false;
        sAttack = false;
        bAttack = false;
        bHit = false;

        //_Body = GetComponent<Rigidbody2D>();
        _Ani = GetComponent<Animator>();

        Filltime = 0f;
        Attacktime = 1.0f;

        hp = 150;
        Fullhp = 150.0f;

        IsFront = false;
    }

    void Update()
    {
        //_Pos = transform.position;

        HPBar.fillAmount = hp / Fullhp;

        if (Command._fight)
        {
            if (!SpawnManager.instance.FrontEnm.Move)
            {
                if (_Ani.GetBool("Move"))
                    _Ani.SetBool("Move", false);

                if (IsFront)
                {
                    Filltime += Time.deltaTime;

                    if (Filltime >= Attacktime)
                    {
                        if (!sAttack && !SpawnManager.instance.FrontEnm.Attack)
                        {
                            bAttack = true;
                            _Ani.SetBool("Base", true);
                        }
                    }

                    if (bHit)
                    {
                        StartCoroutine(HitAnimation(Command.FRONT));
                        bHit = false;
                    }
                }
                else
                {
                    Filltime = 0;

                    if(bHit)
                    {
                        if (Command.MID_Obj == Command.Instance.cSup.gameObject)
                            StartCoroutine(HitAnimation(Command.MID));
                        else
                            StartCoroutine(HitAnimation(Command.BACK));

                        bHit = false;
                    }
                }
            }
            else
            {
                Filltime = 0;
                _Ani.SetBool("Move", true);
            }
        }
    }

    public void MakeAttackFalse()
    {
        Command.bAttack = false;
        sAttack = false;
        bAttack = false;
        Filltime = 0;
        _Ani.SetInteger("Attack", 0);
        _Ani.SetBool("Base", false);
    }

    public void SetEnemyHit()
    {
        SpawnManager.instance.FrontEnm.Hit = true;
        SpawnManager.instance.FrontEnm.hp -= 8;
        if (SpawnManager.instance.FrontEnm.hp <= 0)
            SpawnManager.instance.FrontEnm.IsDie = true;
    }

    public void SetHeal()
    {
        hp += 20;
        Dealler.hp += 20;
        Tanker.hp += 20;

        if (hp > Fullhp) hp = (int)Fullhp;
        if (Dealler.hp > (int)Dealler.Fullhp) Dealler.hp = (int)Dealler.Fullhp;
        if (Tanker.hp > (int)Tanker.Fullhp) Tanker.hp = (int)Tanker.Fullhp;
    }

    public void AllHeal()
    {
        hp = (int)Fullhp;
        Dealler.hp = (int)Dealler.Fullhp;
        Tanker.hp = (int)Tanker.Fullhp;
    }

    public void UseMP()
    {
        if (_Ani.GetInteger("Attack") == 1)
            Command.FillMP -= 2;
        else
            Command.FillMP -= 6;
    }

    public void StartSound_B()
    {
        _AS.PlayOneShot(_AC[0], SoundSlider.EffectVolume);
    }

    public void StartSound_1()
    {
        _AS.PlayOneShot(_AC[1], SoundSlider.EffectVolume);
    }

    public void StartSound_2()
    {
        _AS.PlayOneShot(_AC[2], SoundSlider.EffectVolume);
    }

    IEnumerator HitAnimation(Vector3 Pos)
    {
        _Body.transform.position = new Vector3
            (Pos.x - 0.4f,
            _Body.transform.position.y,
            _Body.transform.position.z);
        yield return null;

        _time = 0;
        while (_time <= 0.5f)
        {
            _time += Time.deltaTime;

            _Body.transform.position = new Vector3(
                Mathf.Lerp(Pos.x - 0.4f, transform.position.x, _time / 0.5f),
                _Body.transform.position.y,
                0
                );


            if (_Body.transform.position.x >= transform.position.x - 0.1f)
            {
                _Body.transform.position = new Vector3(
                    transform.position.x,
                    _Body.transform.position.y,
                    _Body.transform.position.z);
            }

            yield return null;
        }
        yield return null;
    }

    public IEnumerator ShieldOn()
    {
        yield return new WaitForSeconds(1.1f);

        ShieldEffect.SetActive(true);
        ShieldEffect.GetComponent<Animator>().SetTrigger("Create");
        yield return new WaitForSecondsRealtime(6.0f);

        ShieldEffect.SetActive(false);
        yield return null;
    }
}

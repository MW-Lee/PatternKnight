using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dealler : MonoBehaviour {

    // 변수 선언
    public bool sAttack;            // 캐릭터가 공격중인가
    public bool bAttack;
    static public bool Move;        // 현재 움직이는가
    static public bool bHit;

    public bool IsFront;

    private float _time;

    private float Filltime;         // 타임 게이지가 차는걸 담당
    private float Attacktime;       // 타임 게이지의 끝부분을 담당

    public Animator _Ani;           // Animator 를 받아옴

    public Image HPBar;
    static public int hp;           // HP
    static public float Fullhp;     // 최대 hp

    public GameObject ShieldEffect;

    public GameObject _Body;

    public SpawnManager CreEnm;

    public AudioSource _AS;
    public AudioClip[] _AC;


    // 함수 선언

	void Start ()
    {
        Move = false;
        sAttack = false;
        bAttack = false;
        IsFront = true;
        bHit = false;

        Filltime = 0.0f;            // 기본 오프셋 - 시작은 0초
        Attacktime = 1.0f;

        _time = 0;

        //_Body = GetComponent<Rigidbody2D>();
        _Ani = GetComponent<Animator>();

        hp = 200;
        Fullhp = 200.0f;
	}
	
	void Update ()
    {
        HPBar.fillAmount = hp / Fullhp;

        if (Command._fight)
        {
            if (!SpawnManager.instance.FrontEnm.Move)
            {
                if (_Ani.GetBool("Move"))
                    _Ani.SetBool("Move", false);

                if (IsFront)
                {
                    if (!Command.bMove)
                        Filltime += Time.deltaTime;
                    else return;

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
                        if (Command.MID_Obj == Command.Instance.cDeal.gameObject)
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
        //Destroy(SpawnManager.instance.CreEnmM.gameObject);
        //Enemy._Ani.SetBool("Hit", true);
        if (bAttack == true)
            SpawnManager.instance.FrontEnm.hp -= 20;
        else if (sAttack)
            SpawnManager.instance.FrontEnm.hp -= 50;

        if (SpawnManager.instance.FrontEnm.hp <= 0)
            SpawnManager.instance.FrontEnm.IsDie = true;

        if (_Ani.GetInteger("Attack") == 0 && _Ani.GetBool("Base") == true)
        {
            Command.FillMP++;
        }
    }

    public void SetAllHit()
    {
        for (int i = 0; i < SpawnManager.instance.EnmCount; i++)
        {
            SpawnManager.instance.CreEnmM[i].Hit = true;

            SpawnManager.instance.CreEnmM[i].hp -= 80;

            if (SpawnManager.instance.CreEnmM[i].hp <= 0)
                SpawnManager.instance.CreEnmM[i].IsDie = true;
        }
    }

    public void UseMP()
    {
        if(_Ani.GetInteger("Attack")==1)
        {
            Command.FillMP -= 2;
        }
        else
        {
            Command.FillMP -= 5;
        }
    }

    public void StartSound_B()
    {
        _AS.PlayOneShot(_AC[0], SoundSlider.EffectVolume);
    }

    public void StratSound_1()
    {
        _AS.PlayOneShot(_AC[1], SoundSlider.EffectVolume);
    }

    public void StratSound_2()
    {
        _AS.PlayOneShot(_AC[2], SoundSlider.EffectVolume);
    }

    IEnumerator HitAnimation(Vector3 Pos)
    {
        _Body.transform.position = new Vector3
            (Pos.x-0.4f,
            _Body.transform.position.y,
            _Body.transform.position.z);
        yield return null;

        _time = 0;
        while (_time <= 0.5f)
        {
            _time += Time.deltaTime;

            _Body.transform.position = new Vector3(
                Mathf.Lerp(Pos.x-0.4f, transform.position.x, _time / 0.5f),
                _Body.transform.position.y,
                0
                );


            if (_Body.transform.position.x >= -0.01f)
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

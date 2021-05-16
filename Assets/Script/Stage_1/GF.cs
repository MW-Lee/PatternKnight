using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GF : EnemyManager
{
    public Animator _Ani;

    public GameObject _EnmBody;

    private void Awake()
    {
        _Ani = GetComponent<Animator>();
    }

    private void Start()
    {
        Attack = false;
        Hit = false;
        Move = true;
        IsDie = false;

        Filltime = 0;
        Attacktime = 1.5f;

        _HitTime = 0;

        hp = 100;
        Fullhp = 100;

        _AS = GetComponent<AudioSource>();
        _AS.clip = _AC;
    }

    private void Update()
    {
        TimeBar.fillAmount = Filltime / Attacktime;     // 시간 게이지 시각효과.
        HPBar.fillAmount = hp / Fullhp;                 // Hp 게이지 시각효과

        if (!Move)
            Filltime += Time.deltaTime;
        if (Filltime >= Attacktime)
        {
            if (!Command.bAttack && !Command.bMove)
            {
                _Ani.SetBool("Attack", true);
                Attack = true;
            }
        }

        if (Hit)
        {
            StartCoroutine(HitAnimation(_EnmBody.transform.position.x + 0.4f));
            Hit = false;
        }
    }


    //////////////////////////////////////////////////////////////////////////////////
    //
    // 함수

    public void MakeAttackFalse()
    {
        Filltime = 0;
        Attack = false;
        _Ani.SetBool("Attack", false);
    }

    public void SetCharacterHit()
    {
        _AS.PlayOneShot(_AS.clip, SoundSlider.EffectVolume);

        if (Command.TankerShield.gameObject.activeSelf)
        {
            Command.TankerShield.Count--;
            Command.TankerShield._Ani.SetBool("Hit", true);

            return;
        }

        if (Command.Front_Obj.name == "Character_D")
        {
            Dealler.bHit = true;

            if (Command.IsCharShield)
            {
                Tanker.hp -= 9 / 2;
                if (Tanker.hp <= 0) Tanker.hp = 0;
            }
            else
            {
                Dealler.hp -= 9;
                if (Dealler.hp <= 0) Dealler.hp = 0;
            }
        }
        else if (Command.Front_Obj.name == "Character_T")
        {
            Tanker.bHit = true;

            if (Command.IsCharShield)
                Tanker.hp -= 9 / 2;
            else
                Tanker.hp -= 9;

            if (Tanker.hp <= 0) Tanker.hp = 0;

            Command.FillMP++;
        }
        else if (Command.Front_Obj.name == "Character_S")
        {
            Supporter.bHit = true;

            if (Command.IsCharShield)
            {
                Tanker.hp -= 9 / 2;
                if (Tanker.hp <= 0) Tanker.hp = 0;
            }
            else
            {
                Supporter.hp -= 9;
                if (Supporter.hp <= 0) Supporter.hp = 0;
            }
        }
    }

    public void SetMidHit()
    {
        if (Command.TankerShield.gameObject.activeSelf)
        {
            if (Command.TankerShield.Count <= 0)
                Command.TankerShield.gameObject.SetActive(false);

            return;
        }

        if (Command.MID_Obj.name == "Character_D")
        {
            Dealler.bHit = true;

            if (Command.IsCharShield)
            {
                Tanker.hp -= 9 / 2;
                if (Tanker.hp <= 0) Tanker.hp = 0;
            }
            else
            {
                Dealler.hp -= 9;
                if (Dealler.hp <= 0) Dealler.hp = 0;
            }
        }
        else if (Command.MID_Obj.name == "Character_T")
        {
            Tanker.bHit = true;

            if (Command.IsCharShield)
                Tanker.hp -= 9 / 2;
            else
                Tanker.hp -= 9;

            if (Tanker.hp <= 0) Tanker.hp = 0;

            Command.FillMP++;
        }
        else if (Command.MID_Obj.name == "Character_S")
        {
            Supporter.bHit = true;

            if (Command.IsCharShield)
            {
                Tanker.hp -= 9 / 2;
                if (Tanker.hp <= 0) Tanker.hp = 0;
            }
            else
            {
                Supporter.hp -= 9;
                if (Supporter.hp <= 0) Supporter.hp = 0;
            }
        }
    }

    IEnumerator HitAnimation(float x)
    {
        _EnmBody.transform.position = new Vector3
            (x,
            _EnmBody.transform.position.y,
            _EnmBody.transform.position.z);
        yield return null;

        _HitTime = 0;
        while (_HitTime <= 0.5f)
        {
            _HitTime += Time.deltaTime;

            _EnmBody.transform.position = new Vector3(
                Mathf.Lerp(x, transform.position.x, _HitTime / 0.5f),
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

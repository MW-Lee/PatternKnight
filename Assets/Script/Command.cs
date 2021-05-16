using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Command : MonoBehaviour {

    // 터치가 눌렸는지 확인해주는 BOOL변수
    private bool _touch;

    // 현재 전투중인지 확인해주는 BOOL 변수
    static public bool _fight;
    public GameObject SpawnMG;

    public GameObject MpBar;

    // 캐릭터가 움질일 때 필요한 변수
    static public bool bMove;
    static public bool bAttack;

    // 몇번 커맨드가 실행됐는지 보여주는 변수
    private int iCom;

    // 화면 전환을 위한 메인카메라
    public Camera MCamera;

    // 캐릭터들의 각 클래스를 사용하기 위한 변수선언
    public Dealler cDeal;
    public Supporter cSup;
    public Tanker cTank;

    private bool DealDie = false;
    private bool SupDie = false;
    private bool TankDie = false;

    static public TankShield TankerShield;

    // 커맨드 사용을 위한 리스트 선언
    private string KeyStack;         // 입력한 키들을 저장하는 스택
    List<string> CommandTable;       // 저장해놓을 커맨드 모음

    private string PressButton;      // 먼저 저장된 버튼

    // MP구현 시스템
    public Image Bar;
    static public float FillMP;
    private float FullMP;

    // 싱글턴 생성을 위한 Instance값
    public static Command Instance;

    // 보드판 움직임을 위한 생성
    public BoardManager BoardMG;
    public GameObject BoardChar;
    public GameObject Popup;
    public Text Popuptext;
    public GameObject MovePopup;
    static public bool MoveOrNot = false;

    // 이동 포인트를 표시할 텍스트
    public GameObject MovePointPopup;
    public Text MovePointText;
    public static int MovePoint;

    // 포지션 오프셋
    static public Vector3 FRONT, MID, BACK;
    private float _time;

    // 승리 사운드
    static public AudioSource _AS;
    public AudioClip _AC;

    static public GameObject Front_Obj, MID_Obj, Back_Obj;

    static public bool IsCharShield = false;

    void Awake()
    {
        DontDestroy.SceneName = "";

        Front_Obj = cDeal.gameObject;
        MID_Obj = cSup.gameObject;
        Back_Obj = cTank.gameObject;

        bMove = false;
        bAttack = false;
        _touch = false;
        _fight = false;
        iCom = -1;

        MpBar.SetActive(false);

        FillMP = 0;
        FullMP = 10;

        FRONT = new Vector3(0, -2.07f);
        MID = new Vector3(-2.6f, -2.07f);
        BACK = new Vector3(-5.2f, -2.07f);

        _time = 0;

        KeyStack = "";
        PressButton = "";

        CommandTable = new List<string>
        {
            // 사전에 커맨드들을 미리 저장해놓음
            "D",
            "S",
            "T",

            // 전사스킬 1
            "DTS",

            // 탱커스킬 1
            "TSD",

            // 힐러스킬 1
            "STD",

            // 전사스킬 2
            "DST",

            // 탱커스킬 2
            "TDS",

            // 힐러스킬 2
            "SDT"
        };

        _AS = GetComponent<AudioSource>();
        _AS.clip = _AC;
    }

	void Start ()
    {
        // 싱글턴 제작
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        SpawnMG.SetActive(false);

        MovePoint = 3;

        TankerShield = GameObject.Find("TankShield").GetComponent<TankShield>();
        TankerShield.gameObject.SetActive(false);

        DontDestroy.a.FadeInStart();
        BGMManager.instance.Play();
    }
	
	void Update ()
    {
        if (_fight)
        {
            if (!MpBar.activeSelf)
            {
                MpBar.SetActive(true);
                MovePointPopup.SetActive(false);
            }
            Bar.fillAmount = FillMP / FullMP;

            if (FillMP >= 10) FillMP = 10;

            if (Dealler.hp <= 0 && DealDie == false)
            {
                DealDie = true;
                cDeal.HPBar.fillAmount = 0;
                cDeal.gameObject.SetActive(false);
            }
            if (Supporter.hp <= 0 && SupDie == false)
            {
                SupDie = true;
                cSup.HPBar.fillAmount = 0;
                cSup.gameObject.SetActive(false);
            }
            if (Tanker.hp <= 0 && TankDie == false)
            {
                TankDie = true;
                cTank.gameObject.SetActive(false);
            }

            if (!Front_Obj.activeSelf)
                StartCoroutine(PullToFront(MID_Obj, Back_Obj));
            else if (!MID_Obj.activeSelf)
                StartCoroutine(PullToFront(Front_Obj, Back_Obj));

            SpawnMG.SetActive(true);
        }
        else
        {
            if (MpBar.activeSelf)
            {
                MpBar.SetActive(false);
                MovePointPopup.SetActive(true);
            }
            SpawnMG.SetActive(false);
            MCamera.transform.position = new Vector3(0, -15, -10);

            MovePointText.text = MovePoint.ToString();

            if (!cDeal._Ani.GetBool("Move"))
            {
                cDeal._Ani.SetBool("Move", true);
                cSup._Ani.SetBool("Move", true);
                cTank._Ani.SetBool("Move", true);
            }
        }

        // 터치가 되면
        if (Input.GetMouseButtonDown(0))
        {
            // 터치 상태를 true로 변경
            if (!Option.instance.OptionWindow.activeSelf)
                _touch = true;

            KeyStack = "";
            PressButton = "";
        }

        // 터치 중이면
        if (_touch && !bMove && !bAttack)
        {
            // 터치 커서가 버튼에 들어가는지 확인
            CheckCursor();

            // 터치가 풀리면
            if (Input.GetMouseButtonUp(0))
            {
                // 입력된 커맨드가 맞는지 체크 
                iCom = CommandCheck();

                // 터치 상태를 false로 변경
                _touch = false;
            }
        }
        else
        {
            _touch = false;
        }

        // 움직이고
        if (bMove && !bAttack)
            StartMove(iCom);
        // 공격
        if (bAttack && !bMove)
            SetAni(iCom);
    }

    // 커서가 버튼에 들어갔는지 확인
    public void CheckCursor()
    {
        // Raycast가 작동하기 위한 기본 세팅
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray2D ray2D = new Ray2D(
            new Vector2(ray.origin.x, ray.origin.y),
            new Vector2(ray.direction.x, ray.direction.y)
            );
        RaycastHit2D hits = Physics2D.Raycast(ray2D.origin, ray2D.direction);

        if (hits.collider != null)
        {
            if (_fight)
            {
                if (KeyStack.Length == 0)
                    KeyStack += hits.collider.name;
                else if (KeyStack[KeyStack.Length - 1] != hits.collider.name[0])
                    KeyStack += hits.collider.name;
            }
            else
            {
                if (PressButton.Length == 0)
                    PressButton += hits.collider.name;
                else if (PressButton == hits.collider.name)
                    return;

                if (int.Parse(hits.collider.name) - BoardManager.instance.PlayerPos > MovePoint)
                {
                    if(hits.collider.name == "11" || hits.collider.name == "15")
                    {
                        BoardManager.instance.PlayerPos = int.Parse(hits.collider.name);
                        SpawnManager.instance.CanSpawn = true;
                        _fight = true;
                        DontDestroy.a.FadeInStart();
                        BGMManager.instance.ChangeBGM(1);
                        BGManager.instance.bBGMove = true;
                        MCamera.transform.position = new Vector3(0, 0, -10);
                        return;
                    }

                    StartCoroutine(ShowPopup("이동포인트가 부족합니다"));
                }
                else if (int.Parse(hits.collider.name) - BoardManager.instance.PlayerPos < 0)
                {
                    StartCoroutine(ShowPopup("뒤로는 이동할 수 없습니다"));
                }
                else
                {
                    if (MovePoint <= 0)
                    {
                        StartCoroutine(ShowPopup("더이상 이동할 수 없습니다"));
                        return;
                    }
                    else
                        MovePoint -= int.Parse(hits.collider.name) - BoardManager.instance.PlayerPos;

                    if (!hits.collider.GetComponent<ButtonManager>().IsClear)
                    {
                        switch (hits.collider.GetComponent<ButtonManager>().Type)
                        {
                            case 0:
                                SpawnManager.instance.CanSpawn = true;
                                _fight = true;
                                DontDestroy.a.FadeInStart();
                                BGMManager.instance.ChangeBGM(1);
                                BGManager.instance.bBGMove = true;
                                MCamera.transform.position = new Vector3(0, 0, -10);
                                break;

                            case 1:
                                Dealler.hp += (int)((Dealler.Fullhp - Dealler.hp) * 0.4f);
                                Supporter.hp += (int)((Supporter.Fullhp - Supporter.hp) * 0.4f);
                                Tanker.hp += (int)((Tanker.Fullhp - Tanker.hp) * 0.4f);

                                if(DealDie)
                                {
                                    DealDie = false;
                                    cDeal.gameObject.SetActive(true);
                                }
                                if(SupDie)
                                {
                                    SupDie = false;
                                    cSup.gameObject.SetActive(true);
                                }
                                if(TankDie)
                                {
                                    TankDie = false;
                                    cTank.gameObject.SetActive(true);
                                }

                                StartCoroutine(ShowPopup("체력이 회복되었습니다"));
                                BoardManager.instance.Buttonlist[BoardManager.instance.PlayerPos].IsClear = true;
                                break;

                            case 2:
                                FillMP += 3;
                                if (FillMP >= 10) FillMP = 10;
                                StartCoroutine(ShowPopup("마나가 회복되었습니다"));
                                BoardManager.instance.Buttonlist[BoardManager.instance.PlayerPos].IsClear = true;
                                break;

                            default:
                                break;
                        }

                        BoardManager.instance.PlayerPos = int.Parse(hits.collider.name);
                        BoardChar.transform.position = BoardManager.instance.Buttonlist[BoardManager.instance.PlayerPos].transform.position;
                    }
                    else
                    {
                        StartCoroutine(ShowPopup("이미 클리어 하였습니다"));
                    }                    
                }
            }
        }
    }

    public bool ChangePos(GameObject _obj, GameObject _front)
    {
        Vector3 tmpPos = new Vector3(0,0,0);
        if (_obj.gameObject == MID_Obj) tmpPos = MID;
        else if (_obj.gameObject == Back_Obj) tmpPos = BACK;

        bool bMove1 = false;
        bool bMove2 = false;
       
        _obj.transform.position = Vector3.Lerp(_obj.transform.position, FRONT, 0.1f);

        if(Vector3.Distance(_obj.transform.position,FRONT)<=0.06f)
        {
            _obj.transform.position = FRONT;
            bMove1 = true;
        }

        _front.transform.position = Vector3.Lerp(_front.transform.position, tmpPos, 0.1f);
        if(Vector3.Distance(_front.transform.position,tmpPos) <= 0.06f)
        {
            _front.transform.position = tmpPos;
            bMove2 = true;
        }

        if (bMove1 && bMove2)
        {
            if (tmpPos == MID) MID_Obj = _front;
            else if (tmpPos == BACK) Back_Obj = _front;

            return true;
        }
        return false;
    }

    public void SetMove(GameObject _obj)
    {
        if (Front_Obj == _obj)
        {
            bMove = false;
            bAttack = true;
            return;
        }

        bool temp;

        temp = ChangePos(_obj, Front_Obj);

        if (temp)
        {
            Front_Obj = _obj;
            bMove = false;
            bAttack = true;

            WhoIsFront();

            return;
        }
    }

    public void StartMove(int i)
    {
        switch(i)
        {
            case 0:
                if (!DealDie)
                    SetMove(cDeal.gameObject);
                else
                    bMove = false;
                break;
            case 1:
                if (!SupDie)
                    SetMove(cSup.gameObject);
                else
                    bMove = false;
                break;
            case 2:
                if (!TankDie)
                    SetMove(cTank.gameObject);
                else
                    bMove = false;
                break;

            case 3:
                if (!DealDie)
                    SetMove(cDeal.gameObject);
                else
                    bMove = false;
                break;
            case 4:
                if (!TankDie)
                    SetMove(cTank.gameObject);
                else
                    bMove = false;
                break;
            case 5:
                if (!SupDie)
                    SetMove(cSup.gameObject);
                else
                    bMove = false;
                break;
            case 6:
                if (!DealDie)
                    SetMove(cDeal.gameObject);
                else
                    bMove = false;
                break;
            case 7:
                if (!TankDie)
                    SetMove(cTank.gameObject);
                else
                    bMove = false;
                break;
            case 8:
                if (!SupDie)
                    SetMove(cSup.gameObject);
                else
                    bMove = false;
                break;

            default:
                break;
        }
    }

    public void SetAni(int i)
    {
        switch(i)
        {
            case 3:
                if (FillMP >= 3)
                {
                    cDeal._Ani.SetInteger("Attack", 1);
                    cDeal.UseMP();
                    cDeal.sAttack = true;
                }
                break;
            case 4:
                if (FillMP >= 2)
                {
                    cTank._Ani.SetInteger("Attack", 1);
                    cTank.UseMP();
                    cTank.sAttack = true;
                }
                break;
            case 5:
                if (FillMP >= 2)
                {
                    cSup._Ani.SetInteger("Attack", 1);
                    cSup.UseMP();
                    cSup.sAttack = true;
                }
                break;
            case 6:
                if (FillMP >= 5)
                {
                    cDeal._Ani.SetInteger("Attack", 2);
                    cDeal.UseMP();
                    cDeal.sAttack = true;
                }
                break;
            case 7:
                if (FillMP >= 4)
                {
                    cTank._Ani.SetInteger("Attack", 2);
                    cTank.UseMP();

                    StartCharShield();

                    cTank.sAttack = true;
                }
                break;
            case 8:
                if (FillMP >= 6)
                {
                    cSup._Ani.SetInteger("Attack", 2);
                    cSup.UseMP();

                    cDeal._Ani.SetTrigger("Heal");
                    cTank._Ani.SetTrigger("Heal");
                    cSup.sAttack = true;
                }
                break;

            default:
                bAttack = false;
                return;
        }

        iCom = -1;
        bAttack = false;
    }

    void WhoIsFront()
    {
        if (Front_Obj == cDeal.gameObject)
        {
            cDeal.IsFront = true;
            cSup.IsFront = false;
            cTank.IsFront = false;
        }
        else if (Front_Obj == cTank.gameObject)
        {
            cDeal.IsFront = false;
            cTank.IsFront = true;
            cSup.IsFront = false;
        }
        else if(Front_Obj == cSup.gameObject)
        {
            cSup.IsFront = true;
            cDeal.IsFront = false;
            cTank.IsFront = false;
        }
    }

    public int CommandCheck()
    {
        int index;

        for (index = 0; index < (CommandTable.Count); index++) 
        {
            if(KeyStack == CommandTable[index])
            {
                bMove = true;
                KeyStack = "";

                return index;
            }
        }

        KeyStack = "";
        return -1;
    }

    public void StartCharShield()
    {
        IsCharShield = true;

        cDeal.StartCoroutine(cDeal.ShieldOn());
        cSup.StartCoroutine(cSup.ShieldOn());
        cTank.StartCoroutine(cTank.ShieldOn());
    }

    IEnumerator ShowPopup(string text)
    {
        Popuptext.text = text;
        Popup.SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);

        Popup.SetActive(false);
        yield return null;
    }

    IEnumerator PullToFront(GameObject mid_obj, GameObject back_obj)
    {
        GameObject temp = new GameObject();

        if (mid_obj != Front_Obj)
            temp = Front_Obj;
        else if (mid_obj == Front_Obj)
            temp = MID_Obj;

        _time = 0;
        while (_time <= 0.5f)
        {
            _time += Time.deltaTime;

            mid_obj.transform.position = new Vector3(
                Mathf.Lerp(MID.x, FRONT.x, _time / 0.5f), mid_obj.transform.position.y);
            back_obj.transform.position = new Vector3(
                Mathf.Lerp(BACK.x, MID.x, _time / 0.5f), back_obj.transform.position.y);

            if(mid_obj.transform.position.x >= -0.05f)
            {
                mid_obj.transform.position = FRONT;
                back_obj.transform.position = MID;
                temp.transform.position = BACK;

                if (Front_Obj != mid_obj)
                {
                    Front_Obj = mid_obj;
                    MID_Obj = back_obj;
                    Back_Obj = temp;
                }
                else if(Front_Obj == mid_obj)
                {
                    MID_Obj = back_obj;
                    Back_Obj = temp;
                }

                WhoIsFront();
            }
            yield return null;
        }
        yield return null;
    }
}

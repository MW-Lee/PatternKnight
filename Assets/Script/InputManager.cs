using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    // 싱글턴 생성
    static public InputManager instance = null;

    // 커맨드 사용을 위한 리스트 선언
    List<char> KeyStack;         // 입력한 키들을 저장하는 스택
    List<char>[] CommandTable;   // 저장해놓을 커맨드 모음

    // 텍스트 출력을 위함
    public Text _MissMatch;

    float fTime = 0.5f;             // 커맨드 입력 제한시간 >> 0.5초
    bool bInput = false;            // 하나이상 버튼이 눌렸는지 확인

	void Start ()
    {
        // 커맨드를 저장해놓을 리스트 배열도 생성해줌
        CommandTable = new List<char>[2];    // 일단 2개만 만들어놓음..
        CommandTable[0] = new List<char>();
        CommandTable[1] = new List<char>();

        // 사전에 커맨드들을 미리 저장해놓음
        // 기본공격
        CommandTable[0].Add('a');
        CommandTable[0].Add('b');

        // 스킬공격
        CommandTable[1].Add('c');
        CommandTable[1].Add('b');
        CommandTable[1].Add('a');

        // 처음에는 보이지 않아야함.
        _MissMatch.gameObject.SetActive(false);
    }

    void Awake()
    {
        // 싱글턴 생성
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // List를 사용하기 전 생성 해줘야함
        KeyStack = new List<char>();
    }
	
	void Update ()
    {
        if (bInput)
        {
            // 버튼을 한개 이상 입력받은 상태에서
            // 0.5초이상 아무 입력이 없을 시에
            // 저장해놨던 입력받은 버튼들을 초기화 시킵니다.
            fTime -= Time.deltaTime;

            if (fTime < 0.0f)
            {
                if(_MissMatch.gameObject.activeSelf)
                    _MissMatch.gameObject.SetActive(false);
                ClearStack();
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}

    // 버튼이 눌렸을 때 누른 버튼을 스택에 넣어줍니다.
    public void AddKey(char key)
    {
        KeyStack.Add(key);
        bInput = true;
        fTime = 0.5f;
    }

    // 눌렸던 버튼들을 저장해놓은 스택을 깔끔하게 비워주는 역할.
    // 다른 시간과 input설정도 초기상태로 돌려줍니다.
    public void ClearStack()
    {
        KeyStack.Clear();
        fTime = 0.5f;
        bInput = false;
    }

    // 커맨드를 비교해주는 함수.
    // 맞는 것이 있으면 CommandTable에서 실행시킵니다.
    public void CheckCommand()
    {
        int i, j;           // for문 돌리는 용도
        int index;          // 입력받은 KeyStack의 현재 칸에 뭐가 있는지 확인해주는 변수
        bool bFlag;         // 커맨드가 저장된 것과 틀릴 경우 틀린 것을 확인하고 for문을 나오는 용도

        if (KeyStack.Count == 1) return;

        for(i = 0;i<CommandTable.Length;i++)
        {
            index = KeyStack.Count - 1;
            bFlag = true;

            for(j=CommandTable[i].Count-1;j>=0;j--)
            {
                // 전체 키가 몇개인지 확인 후 전부 확인하면 나오는 용도
                if (index < 0) break;

                if (CommandTable[i][j] != KeyStack[index])
                    bFlag = false;

                index--;
            }

            // 커맨드를 비교하여 계속 일치하면 bflag가 true로 보존되고
            // 받은 키가 길어져 CommandTable보다 길어질 수도 있으므로
            // CommandTable의 끝까지 보았는지 확인하여 출력해줍니다.
            if(bFlag && j<0)
            {
                _MissMatch.text = "Match";

                // 일치하는 경우 입력된 커맨드에 맞는 애니메이션이 출력되도록
                // Attack의 값을 설정해주고 자신이 공격중임을 설정해줍니다.
                if (!Enemy.Attack)
                {
                    Character.Attack = true;
                    //Character._Ani.SetInteger("Attack", i + 1);
                }

                ClearStack();
                break;
            }
        }

        // 맞는 커맨드가 없어 끝까지 돌았을때의 경우.
        if(i==CommandTable.Length)
        {
            _MissMatch.text = "Miss";
        }

        // 커맨드 비교후 결과값을 나타내줍니다.
        _MissMatch.gameObject.SetActive(true);

        // 확인 후 받았던 스택은 비워줍니다.
        //ClearStack();

        return;
    }
}

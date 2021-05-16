using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // 싱글턴 생성
    static public InputManager instance = null;

    // 리스트 선언
    List<KeyCode> KeyStack;         // 입력한 키들을 저장하는 리스트
    List<KeyCode>[] CommandTable;   // 지정된 커맨드들을 모아놓음 -> 입력받은 것과 비교

    float fTime = 0.5f; // 커맨드 입력 제한시간
    bool bInput = false;    // 버튼이 하나 이상 눌렸는지 확인하는 변수

    void Awake()
    {
        // 싱글턴 제작
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // List는 사용 전 생성을 해줘야 합니다
        KeyStack = new List<KeyCode>();
    }

    void Start()
    {
        // 배열도 사용 전 생성을 해줘야 합니다
        CommandTable = new List<KeyCode>[2];
        CommandTable[0] = new List<KeyCode>();
        CommandTable[1] = new List<KeyCode>();

        CommandTable[0].Add(KeyCode.LeftArrow);
        CommandTable[0].Add(KeyCode.Space);

        CommandTable[1].Add(KeyCode.RightArrow);
        CommandTable[1].Add(KeyCode.Space);

    }

    void Update()
    {
        // 입력받은 상태에서 일정 시간동안 추가 입력이 없으면 입력을 비워줍니다
        if(bInput)
        {
            fTime -= Time.deltaTime;

            if (fTime < 0.0f)
                ClearStack();
        }
    }

    public void AddKey(KeyCode key)
    {
        // 입력받은 키를 추가해준 후 상태를 갱신해줍니다
        KeyStack.Add(key);
        bInput = true;
        fTime = 0.5f;
    }

    public void CheckCommand()
    {
        // 미리 조합해 둔 커맨드와 입력받은 키를 비교합니다
        int i, j;
        int index;
        bool bFlag;

        for (i = 0; i < CommandTable.Length; ++i)
        {
            index = KeyStack.Count - 1;
            bFlag = true;

            for (j = CommandTable[i].Count - 1; j >= 0; j--)
            {
                // 입력받은 키를 다 비교했다면 반복문을 나옵니다
                if (index < 0)
                    break;

                // 만약 다르다면 Flag를 false로 변경해줍니다
                if (CommandTable[i][j] != KeyStack[index])
                    bFlag = false;
                
                --index;
            }

            // 다른 경우가 없었다면 bFlag가 true일 것이고
            // 입력받은 키가 커맨드 테이블보다 짧을 경우를 대비해
            // 커맨드 테이블 또한 비교를 다 했는지 확인을 해 줍니다
            if (bFlag && j < 0)
            {
                Debug.Log(i + " Match");
                break;
            }
        }

        // 맞는 커맨드가 없을 경우입니다
        if (i == CommandTable.Length)
            Debug.Log("MisMatch");

        // 비교를 다한 후 비워줍니다
        ClearStack();
    }

    public void ClearStack()
    {
        // 입력받은 키를 전부 지우고 상태를 갱신합니다
        KeyStack.Clear();
        fTime = 0.5f;
        bInput = false;
    }

    public void PrintStack()
    {
        string s = null;

        for (int i = 0; i < KeyStack.Count; ++i)
        {
            s = s + KeyStack[i].ToString() + " ";
        }

        Debug.Log(s);
    }
}

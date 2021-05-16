using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    //////////////////////////////////////////////////////
    // 변수 선언 부분
    static public SpawnManager instance;

    //public GameObject PreEnm1;
    public List<GameObject> PreEnm;
    public List<GameObject> PreEnmHP;

    public int EnmNum;
    public int EnmCount;

    public bool CanSpawn = false;

    public EnemyManager FrontEnm;
    public List<EnemyManager> CreEnmM;
    public List<GameObject> CreEnmHP;
    private GameObject MainCanvas;

    public int Wave;

    private void Awake()
    {
        instance = this;

        CreEnmM = new List<EnemyManager>();
        CreEnmHP = new List<GameObject>();
    }

    private void Start()
    {
        //CreEnm = Instantiate(PreEnm1, new Vector3(11, -1.73f, 0), Quaternion.identity);
        //CreEnmM = CreEnm.GetComponent<EnemyManager>();

        MainCanvas = GameObject.Find("Canvas");

        Wave = 0;
    }

    private void Update()
    {
        if (!CanSpawn)
            return;

        if (EnmCount == 0)
        {
            BGManager.instance.bBGMove = true;
            Wave++;
            if(Wave == 4)
            {
                if (BoardManager.instance.Buttonlist[BoardManager.instance.PlayerPos].nextButton == null)
                {
                    DontDestroy.a.LoadScene("Stage_2");
                    return;
                }
                BGManager.instance.bBGMove = false;
                BGMManager.instance.BGM.Pause();
                Command._AS.PlayOneShot(Command._AS.clip, SoundSlider.EffectVolume);
                Result_Button.instance.ShowResult();

                //FrontEnm.Move = false;
                //Command.MovePoint = 3;
                //Command._fight = false;
                //DontDestroy.a.FadeInStart();
                //BGMManager.instance.ChangeBGM(0);
                //BGManager.instance.bBGMove = false;
                //BGManager.instance.Sky.transform.position = new Vector3(0, 0, 0);
                //BoardManager.instance.Buttonlist[BoardManager.instance.PlayerPos].IsClear = true;
                return;
            }

            CreEnmM.Clear();

            if (BoardManager.instance.Buttonlist[BoardManager.instance.PlayerPos].nextButton == null && Wave == 3)
            {
                EnmCount = 1;
                CreEnmM.Add(Instantiate(PreEnm[2], new Vector3(11, -2.07f, 0), Quaternion.identity).GetComponent<EnemyManager>());
                CreEnmM[0].EnmSpriteRenderer.sortingOrder = EnmCount;

                CreEnmHP.Add(Instantiate(PreEnmHP[2]));
                CreEnmHP[0].transform.SetParent(MainCanvas.transform);
                CreEnmHP[0].transform.SetAsFirstSibling();
                CreEnmHP[0].transform.localPosition = new Vector3(380, 328);

                CreEnmM[0].TimeBar = CreEnmHP[0].transform.GetChild(1).GetComponent<Image>();
                CreEnmM[0].HPBar = CreEnmHP[0].transform.GetChild(3).GetComponent<Image>();
            }
            else
            {
                EnmCount = Random.Range(1, 4);
                for (int i = 0; i < EnmCount; i++)
                {
                    EnmNum = Random.Range(0, 2);
                    CreEnmM.Add(Instantiate(PreEnm[EnmNum], new Vector3(11 + (i * 2), -2.07f, 0), Quaternion.identity).GetComponent<EnemyManager>());
                    CreEnmM[i].EnmSpriteRenderer.sortingOrder = EnmCount - i;

                    CreEnmHP.Add(Instantiate(PreEnmHP[EnmNum]));
                    CreEnmHP[i].transform.SetParent(MainCanvas.transform);
                    CreEnmHP[i].transform.SetAsFirstSibling();
                    CreEnmHP[i].transform.localPosition = new Vector3(380, 328 - (i * 50));

                    CreEnmM[i].TimeBar = CreEnmHP[i].transform.GetChild(1).GetComponent<Image>();
                    CreEnmM[i].HPBar = CreEnmHP[i].transform.GetChild(3).GetComponent<Image>();
                }
            }

            FrontEnm = CreEnmM[0];
        }
        else
        {
            for (int i = 0; i < EnmCount; i++)
            {
                if (CreEnmM[i].transform.position.x <= 2.4f)
                {
                    BGManager.instance.bBGMove = false;
                    CreEnmM[i].transform.position = new Vector3(2.4f, CreEnmM[i].transform.position.y, 0);
                    CreEnmM[i].Move = false;
                }
                else
                {
                    CreEnmM[i].transform.Translate(Vector3.left * Time.deltaTime * 3);
                }
            }
        }
        
    }

    private void LateUpdate()
    {
        if (FrontEnm.IsDie)
        {
            if (EnmCount == 0)
            {
                if (Wave == 4)
                {
                    CanSpawn = false;
                    Wave = 0;
                }
                return;
            }

            EnmCount--;
            Destroy(CreEnmM[0].gameObject);
            CreEnmM.RemoveAt(0);

            Destroy(CreEnmHP[0].gameObject);
            CreEnmHP.RemoveAt(0);

            FrontEnm = CreEnmM[0];
        }
    }

    public void SetProto()
    {
        FrontEnm.Move = false;
        Command.MovePoint = 3;
        Command._fight = false;
        DontDestroy.a.FadeInStart();
        BGMManager.instance.ChangeBGM(0);
        BGManager.instance.bBGMove = false;
        BGManager.instance.Sky.transform.position = new Vector3(0, 0, 0);
        BoardManager.instance.Buttonlist[BoardManager.instance.PlayerPos].IsClear = true;
    }
}

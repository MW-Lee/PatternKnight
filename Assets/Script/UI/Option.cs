using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    static public Option instance;

    public GameObject OptionWindow;
    public Text BackVilText;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (Command._fight) BackVilText.text = "보드판";
        else BackVilText.text = "마을";
    }

    public void OnClick()
    {
        OptionWindow.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnClick_Back()
    {
        OptionWindow.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OnClick_Up()
    {
        OptionWindow.SetActive(false);
        Time.timeScale = 1.0f;

        if(Command._fight)
        {
            SpawnManager.instance.FrontEnm.Move = false;
            SpawnManager.instance.FrontEnm = null;

            for(int i=0;i<SpawnManager.instance.EnmCount;i++)
            {
                Destroy(SpawnManager.instance.CreEnmM[i].gameObject);
                Destroy(SpawnManager.instance.CreEnmHP[i].gameObject);
            }
            SpawnManager.instance.CreEnmM.Clear();
            SpawnManager.instance.CreEnmHP.Clear();
            SpawnManager.instance.CanSpawn = false;
            SpawnManager.instance.Wave = 0;
            SpawnManager.instance.EnmCount = 0;

            Command._fight = false;
            DontDestroy.a.FadeInStart();
            BGMManager.instance.ChangeBGM(0);
            BGManager.instance.bBGMove = false;
            BGManager.instance.Sky.transform.position = new Vector3(0, 0, 0);
            return;
        }
        else
        {
            DontDestroy.a.LoadScene("StartViliage");
            return;
        }
    }
}

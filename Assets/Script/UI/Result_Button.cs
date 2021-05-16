using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result_Button : MonoBehaviour
{
    static public Result_Button instance;

    public GameObject ResultWindow;

    private void Start()
    {
        instance = this;
    }

    public void ShowResult()
    {
        //Time.timeScale = 0;
        Time.timeScale = 0;
        ResultWindow.SetActive(true);
    }

    public void OnContinue()
    {
        ResultWindow.SetActive(false);
        Time.timeScale = 1;
        SpawnManager.instance.SetProto();
    }

    public void OnVil()
    {
        ResultWindow.SetActive(false);
        Time.timeScale = 1;
        DontDestroy.a.LoadScene("Start_Viliage");
    }
}

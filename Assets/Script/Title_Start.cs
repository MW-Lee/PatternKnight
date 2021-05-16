using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_Start : MonoBehaviour
{
    private float _time;
    private Text StartText;
    private bool TextOn;

    public void Start()
    {
        _time = 0;
        StartText = GetComponent<Text>();
        TextOn = true;
    }

    public void Update()
    {
        _time += Time.deltaTime;
        if (TextOn)
        {
            StartText.color = new Color(StartText.color.r, StartText.color.g, StartText.color.b,
                  Mathf.Lerp(1, 0, _time));
            if (StartText.color.a <= 0)
            {
                TextOn = false;
                _time = 0;
            }
        }
        else
        {
            StartText.color = new Color(StartText.color.r, StartText.color.g, StartText.color.b,
              Mathf.Lerp(0, 1, _time));
            if (StartText.color.a >= 1)
            {
                TextOn = true;
                _time = 0;
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            string sceneName;

            sceneName = "StartViliage";

            DontDestroy.a.LoadScene(sceneName);
            
        
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour {

    public Image FadeBlack;
    private float _time;

    public static DontDestroy a = null;
    public static string SceneName;

	void Start ()
    {
        // 싱글턴
        if (a == null)
            a = this;
        else if (a != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);

        Input.backButtonLeavesApp = true;
	}

    void Update ()
    {

    }

    public void LoadScene(string _Name)
    {
        SceneName = _Name;

        StartCoroutine(FadeOut());
    }

    public void FadeInStart()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeOutStart()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Image Fade = Instantiate(FadeBlack).GetComponent<Image>();
        //Fade.transform.parent = GameObject.Find("Canvas").transform;
        Fade.transform.SetParent(GameObject.Find("Canvas").transform);
        Fade.transform.SetAsLastSibling();
        Fade.transform.localPosition = Vector3.zero;
        float _Time = 0; 
        while (Fade.color.a < 1)
        {
            _Time += Time.deltaTime;
            Fade.color = new Color(
              Fade.color.r,
              Fade.color.g,
              Fade.color.b,
              Mathf.Lerp(0, 1, _Time));

            if (Fade.color.a >= 1)
            {
                SceneManager.LoadScene("Loading");
                StopAllCoroutines();
            }

            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        Image Fade = Instantiate(FadeBlack).GetComponent<Image>();
        //Fade.transform.parent = GameObject.Find("Canvas").transform;
        Fade.transform.SetParent(GameObject.Find("Canvas").transform);
        Fade.transform.SetAsFirstSibling();
        Fade.transform.localPosition = Vector3.zero;
        
        Fade.color = new Color(
            Fade.color.r,
            Fade.color.g,
            Fade.color.b,
            1);
        float _Time = 0;
        while (Fade.color.a > 0)
        {
            _Time += Time.deltaTime;
            Fade.color = new Color(
              Fade.color.r,
              Fade.color.g,
              Fade.color.b,
              Mathf.Lerp(1,0, _Time));

            if (Fade.color.a <= 0)
            {
                Destroy(Fade);
                StopCoroutine(FadeIn());
            }

            yield return null;
        }
    }
}

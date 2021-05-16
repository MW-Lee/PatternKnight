using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Manager : MonoBehaviour {

    public static Dialogue_Manager instance;

    public Text _name;
    public Text text;
    public Image Face;
    public Image DialogueWindow;

    public List<string> listSentences;

    public int count; // 대화 진행 상황

    public Animator animNameWindow;
    public Animator animSprite;
    public Animator animDialogueWindow;

    private bool ShowTu = false;

    void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            //DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion Singleton
    }

    void Start ()
    {
        count = 0;

        _name.text = "";
        text.text = "";

        listSentences = new List<string>();

    }
	
	void Update ()
    {
        if (ShowTu)
        {
            if (Input.GetMouseButtonDown(0))
            {
                count++;
                text.text = "";
                _name.text = "";

                if (count == listSentences.Count)
                {
                    TutorialDialogue.GoTutorial = false;
                    StopAllCoroutines();
                    ExitDialogue();

                    // 씬전환
                    string Scenename;
                    Scenename = "Tutorial_B";
                    DontDestroy.a.LoadScene(Scenename);
                    return;
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartDialogue());
                }
            }
        }
	}

    public void ShowDialogue(Dialogue dialogue)
    {
        for(int i=0;i<dialogue.sentences.Length;i++)
        {
            listSentences.Add(dialogue.sentences[i]);
        }

        ShowTu = true;

        animNameWindow.SetBool("Appear", true);
        animSprite.SetBool("Appear", true);
        animDialogueWindow.SetBool("Appear", true);
        StartCoroutine(StartDialogue());
    }

    public void ExitDialogue()
    {
        text.text = "";
        _name.text = "";
        count = 0;

        listSentences.Clear();

        animNameWindow.SetBool("Appear", false);
        animSprite.SetBool("Appear", false);
        animDialogueWindow.SetBool("Appear", false);
    }

    IEnumerator StartDialogue()
    {
        _name.text = "신관";
        text.text = listSentences[count];
        yield return null;
       
    }
}

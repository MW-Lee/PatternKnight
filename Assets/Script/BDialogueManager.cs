using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BDialogueManager : MonoBehaviour
{

    public static BDialogueManager instance;

    public Text _name;
    public Text text;

    public Image _Button;
    public Image DialogueWindow;

    public List<string> listSentences;
    public List<string> listNames;
    public List<Sprite> listButtons;

    private int count; // 대화 진행 상황

    bool show;
    public Animator animDialogueWindow;

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
        show = false;
        count = 0;

        text.text = "";
        _name.text = "";

        listSentences = new List<string>();
        listNames = new List<string>();
        listButtons = new List<Sprite>();
        _Button.enabled = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && show)
        {
            count++;
            text.text = "";
            _name.text = "";
            if(count == listSentences.Count)
            {
                StopAllCoroutines();
                ExitDialogue();

                // Scene change
                string Scenename;
                Scenename = "Tutorial_F";
                DontDestroy.a.LoadScene(Scenename);
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(StartDialogue());
            }
        }
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
            listNames.Add(dialogue.Name[i]);
            listButtons.Add(dialogue.sprites[i]);
        }

        _Button.enabled = true;
        animDialogueWindow.SetBool("Appear", true);
        show = true;
        StartCoroutine(StartDialogue());
    }

    public void ExitDialogue()
    {
        count = 0;
        text.text = "";
        _name.text = "";

        listSentences.Clear();
        listNames.Clear();
        listButtons.Clear();

        _Button.enabled = false;

        animDialogueWindow.SetBool("Appear", false);
    }

    IEnumerator StartDialogue()
    {
        _Button.sprite = listButtons[count];
        _name.text = listNames[count];
        text.text = listSentences[count];
        
        yield return null;
        
    }
}

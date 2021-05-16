using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogue : MonoBehaviour {

    [SerializeField]
    public Dialogue dialogue;

    private Dialogue_Manager theDM;

    static public bool GoTutorial = false;
    static public bool GoStage = false;

    private bool ShowTu = false;

	// Use this for initialization
	void Start ()
    {
        GoTutorial = false;

        theDM = FindObjectOfType<Dialogue_Manager>();

        DontDestroy.a.FadeInStart();
    }

    // Update is called once per frame
    void Update()
    {
        ShowTu = GoTutorial;

        if (ShowTu)
        {
            theDM.ShowDialogue(dialogue);
            GoTutorial = false;
            ShowTu = false;
        }

        if (!GoTutorial)
            if (GoStage)
            {
                GoStage = false;
                DontDestroy.a.LoadScene("Stage_1");
            }
    }
}

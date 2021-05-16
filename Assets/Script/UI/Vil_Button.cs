using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vil_Button : MonoBehaviour
{
    public GameObject SkillWindow;

    public void OnStart()
    {
        TutorialDialogue.GoStage = true;
    }

    public void OnSkill()
    {
        SkillWindow.SetActive(true);
    }

    public void OnSkillClose()
    {
        SkillWindow.SetActive(false);
    }

    public void OnTutorial()
    {
        TutorialDialogue.GoTutorial = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTutorialDialogue : MonoBehaviour {

    [SerializeField]
    public Dialogue dialogue;

    private BDialogueManager theDM;

    // Use this for initialization
    void Start()
    {
        DontDestroy.a.FadeInStart();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(theDM == null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                theDM = FindObjectOfType<BDialogueManager>();

                theDM.ShowDialogue(dialogue);
            }
        }
    }
}

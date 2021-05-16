using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    public List<ButtonManager> Buttonlist;
    public int PlayerPos;

    private void Start()
    {
        instance = this;

        for (int i = 0; i < Buttonlist.Count; i++)
        {
            if (i != Buttonlist.Count - 1)
            {
                if (Buttonlist[i + 1] != null && i != Buttonlist.Count - 1)
                    Buttonlist[i].nextButton = Buttonlist[i + 1];
            }

            if (i != 0)
            {
                if (Buttonlist[i - 1] != null)
                    Buttonlist[i].prevButton = Buttonlist[i - 1];
            }
        }

        PlayerPos = 0;
    }

    private void Update()
    {
        
    }
}

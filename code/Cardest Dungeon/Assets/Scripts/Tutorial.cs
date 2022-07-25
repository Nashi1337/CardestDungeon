using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    DialogueManager dm;
    private void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        if (dm != null)
        {
            dm.Tutorial1();
        }
    }
}

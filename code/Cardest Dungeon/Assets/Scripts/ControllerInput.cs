using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    private enum State
    {
        NOCONTROLLER,
        SELECTFIRSTMAPPIECE,
        SELECTSECONDMAPPIECE
    }

    private State state;

    [SerializeField]
    private GameObject selectionPointerPrefab;

    private void Start()
    {
        //Auswahlzeiger und Referenz??????
    }

    private void Update()
    {
        //Joystick Abfrage, was passiert wenn man was aufm controller drückt
        //OK Button (Input.Controller.Action) + InputController.GetActionDown()
        /*if (confirm)
        {
            switch (state)
            {
                case State.NOCONTROLLER:
                    //if controllerinput, goto selectfirstmappiece, selection pointer will be created and reference saved
                case State.SELECTFIRSTMAPPIECE:
                    //change state, first map piece will be highlighted
                break;
                case State.SELECTSECONDMAPPIECE:
                    //change state, highlighted map piece will be replaced with second piece if eligible
                break;
            }
        }*/
        //Cancel Button (InputController.Cancel) + InputController.GetActionDown()
    }
}

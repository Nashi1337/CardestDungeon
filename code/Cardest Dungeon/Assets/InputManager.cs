using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Current { get { return current; } }

    private static InputManager current = null;

    [SerializeField]
    private KeyCode[] forward;
    [SerializeField]
    private KeyCode[] backward;
    [SerializeField]
    private KeyCode[] left;
    [SerializeField]
    private KeyCode[] right;
    [SerializeField]
    private KeyCode[] action1;

    public delegate void Vec2Input(Vector2 move);
    public delegate void ActionInput();

    public Vec2Input onMove;
    public ActionInput onAction1Down;
    public ActionInput onAction1Held;
    public ActionInput onAction1Up;

    private bool isAction1Down = false;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else if (current != this)
        {
            throw new System.Exception("Es ist mehr als ein InputManager aktiv. Das sollte nicht sein!");
            Destroy(this);
        }
    }

    private void Update()
    {
        TryCallOnAction1();
    }

    private void FixedUpdate()
    {
        TryCallOnMove();
    }

    /// <summary>
    /// Checks if there is a movement input and Calls OnMove, if there is one.
    /// </summary>
    private void TryCallOnMove()
    {
        Vector2 movement = new Vector2(0, 0);
        //Falls eine Vorwärts Eingabe aktiv ist, addiere den movement Vektor mit (0, 1)
        foreach (KeyCode key in forward)
        {
            if (Input.GetKey(key))
            {
                movement.y += 1;
                break;
            }
        }
        //Falls eine Rückwärts Eingabe aktiv ist, addiere den movement Vektor mit (0, -1)
        foreach (KeyCode key in backward)
        {
            if (Input.GetKey(key))
            {
                movement.y -= 1;
                break;
            }
        }

        //Selbes Spiel mit links und rechts
        foreach (KeyCode key in right)
        {
            if (Input.GetKey(key))
            {
                movement.x += 1;
                break;
            }
        }

        foreach (KeyCode key in left)
        {
            if (Input.GetKey(key))
            {
                movement.x -= 1;
                break;
            }
        }

        if (movement != default)
        {
            onMove?.Invoke(movement);
        }
    }

    private void TryCallOnAction1()
    {
        if (!isAction1Down)
        {
            foreach (KeyCode key in action1)
            {
                if (Input.GetKeyDown(key))
                {
                    onAction1Down?.Invoke();
                    isAction1Down = true;
                    break;
                }
            }
        }
        else
        {
            foreach (KeyCode key in action1)
            {
                if (Input.GetKeyUp(key))
                {
                    onAction1Up?.Invoke();
                    isAction1Down = false;
                    return;
                }
            }

            //Fall Action1 gedrückt ist, und Action1 nicht losgelassen wurde
            onAction1Held?.Invoke();
        }
    }

    //private void TryCallOnAction1()
    //{
    //    if (!isAction1Down)
    //    {
    //        foreach (KeyCode key in action1)
    //        {
    //            if (Input.GetKeyDown(key))
    //            {
    //                onAction1Down?.Invoke();
    //                isAction1Down = true;
    //                break;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        foreach (KeyCode key in action1)
    //        {
    //            if (Input.GetKeyUp(key))
    //            {
    //                onAction1Up?.Invoke();
    //                isAction1Down = false;
    //                return;
    //            }
    //        }

    //        //Fall Action1 gedrückt ist, und Action1 nicht losgelassen wurde
    //        onAction1Held?.Invoke();
    //    }
    //}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used for anything input related. It especially maps different buttons to  more abstract actions.
/// It is unclear at the moment how much this class is actually used. Therefore it's existence should be up to debate.
/// </summary>
public class InputManager : MonoBehaviour
{
    public static KeyCode[] forward = new KeyCode[] { KeyCode.W };
    public static KeyCode[] backward = new KeyCode[] { KeyCode.S };
    public static KeyCode[] left = new KeyCode[] { KeyCode.A };
    public static KeyCode[] right = new KeyCode[] { KeyCode.D };
    public static KeyCode[] map = new KeyCode[] { KeyCode.M, KeyCode.Joystick1Button3};
    public static KeyCode[] inventory = new KeyCode[] { KeyCode.I, KeyCode.Joystick1Button2 };
    public static KeyCode[] cancel = new KeyCode[] { KeyCode.B };
    public static KeyCode[] action = new KeyCode[] { KeyCode.E };
    public static KeyCode[] attack = new KeyCode[] { KeyCode.Space, KeyCode.Joystick1Button0};
    public static KeyCode[] fireball = new KeyCode[] { KeyCode.F, KeyCode.Joystick1Button1 };
    public static KeyCode[] heal = new KeyCode[] { KeyCode.H };

    /// <summary>
    /// Calculates the movement by checking the forward, backward, left and right keys.
    /// </summary>
    /// <returns>Returns a Vector2 which contains values for x and y between -1 and 1</returns>
    public static Vector2 CalculateMovement()
    {
        Vector2 movement = new Vector2(0, 0);

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        if (movement.x == 0 && movement.y == 0)
        {
            movement.y += GetActionDown(forward) ? 1 : 0;
            movement.y -= GetActionDown(backward) ? 1 : 0;
            movement.x += GetActionDown(right) ? 1 : 0;
            movement.x -= GetActionDown(left) ? 1 : 0;
        }
        return movement;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="action">Use static variables from Inputmanager for that. (e.g. inventory, map,...)</param>
    /// <returns>Returns true, if a button for a certain action got pressed down. Else, false.</returns>
    public static bool GetAction(KeyCode[] action)
    {
        foreach (KeyCode key in action)
        {
            if (Input.GetKey(key))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="action">Use static variables from Inputmanager for that. (e.g. inventory, map,...)</param>
    /// <returns>Returns true, if a button for a certain action was released. Else, false.</returns>
    public static bool GetActionDown(KeyCode[] action)
    {
        foreach (KeyCode key in action)
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="action">Use static variables from Inputmanager for that. (e.g. inventory, map,...)</param>
    /// <returns>Returns true, if a button for a certain action is pressed. Else, false.</returns>
    public static bool GetActionUp(KeyCode[] action)
    {
        foreach (KeyCode key in action)
        {
            if (Input.GetKeyUp(key))
            {
                return true;
            }
        }
        return false;
    }
}

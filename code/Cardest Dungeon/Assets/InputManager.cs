using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class InputManager : MonoBehaviour
{
    public static KeyCode forward = KeyCode.W;
    public static KeyCode backward = KeyCode.S;
    public static KeyCode left = KeyCode.A;
    public static KeyCode right = KeyCode.D;
    public static KeyCode map = KeyCode.M;
    public static KeyCode inventory = KeyCode.I;
    public static KeyCode cancel = KeyCode.B;
    public static KeyCode action = KeyCode.Space;

    /// <summary>
    /// Calculates the movement by checking the forward, backward, left and right keys.
    /// </summary>
    /// <returns>Returns a Vector2 which contains values for x and y between -1 and 1</returns>
    public static Vector2 CalculateMovement()
    {
        Vector2 movement = new Vector2(0, 0);

        movement.y += Input.GetKey(forward) ? 1 : 0;
        movement.y -= Input.GetKey(backward) ? 1 : 0;
        movement.x += Input.GetKey(right) ? 1 : 0;
        movement.x -= Input.GetKey(left) ? 1 : 0;
        return movement;
    }
}

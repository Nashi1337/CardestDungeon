using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used for anything input related. It especially maps different buttons to  more abstract actions.
/// It is unclear at the moment how much this class is actually used. Therefore it's existence should be up to debate.
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
    public static KeyCode action = KeyCode.E;
    public static KeyCode attack = KeyCode.Space;
    public static KeyCode fireball = KeyCode.F;
    public static KeyCode heal = KeyCode.H;

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

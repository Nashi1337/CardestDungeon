using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBehaviour : MonoBehaviour
{
    public Element ElementWeakness { get { return elementWeakness; } }

    [SerializeField]
    private Element elementWeakness;
}

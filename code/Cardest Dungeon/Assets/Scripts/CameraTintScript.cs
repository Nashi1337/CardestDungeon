using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

public class CameraTintScript : MonoBehaviour
{
    public static CameraTintScript Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CameraTintScript>();
            }
            return instance;
        }
    }

    private static CameraTintScript instance;

    [System.Serializable]
    public enum Colors
    {
        RED, BLUE, DEFAULT
    }

    [SerializeField]
    private GameObject tintPlane;
    [SerializeField]
    private Color defaultTint;
    [SerializeField]
    private Color redTint;
    [SerializeField]
    private Color blueTint;

    private Color currentTint;
    private StateMachine stateMachine;
    private float lerpProgress;
    private SpriteRenderer tintRenderer;
    private void Start()
    {
        tintRenderer = tintPlane.GetComponent<SpriteRenderer>();
        currentTint = defaultTint;

        stateMachine = new StateMachine("Idle", Idle);
        stateMachine.AddState("TintingRed", TintingRed);
        stateMachine.AddState("TintingBlue", TintingBlue);
        stateMachine.AddState("TintingIdle", TintingIdle);

    }

    private void FixedUpdate()
    {
        stateMachine.Run();
    }

    #region StateMachine

    private void Idle()
    {

    }

    private void TintingRed()
    {
        lerpProgress += Time.fixedDeltaTime;

        tintRenderer.color = Color.Lerp(currentTint, redTint, lerpProgress);

        if(lerpProgress >= 1)
        {
            lerpProgress = 0;
            currentTint = redTint;
            stateMachine.TransitionToState("Idle");
        }
    }

    private void TintingBlue()
    {
        lerpProgress += Time.fixedDeltaTime;

        tintRenderer.color = Color.Lerp(currentTint, blueTint, lerpProgress);

        if (lerpProgress >= 1)
        {
            lerpProgress = 0;
            currentTint = blueTint;
            stateMachine.TransitionToState("Idle");
        }
    }

    private void TintingIdle()
    {
        lerpProgress += Time.fixedDeltaTime;

        tintRenderer.color = Color.Lerp(currentTint, defaultTint, lerpProgress);

        if (lerpProgress >= 1)
        {
            lerpProgress = 0;
            currentTint = defaultTint;
            stateMachine.TransitionToState("Idle");
        }
    }
    #endregion

    public void SwitchColor(Element element)
    {
        switch(element)
        {
            case Element.NONE:
                stateMachine.TransitionToState("TintingIdle");
                break;
            case Element.ICE:
                stateMachine.TransitionToState("TintingBlue");
                break;
            case Element.FIRE:
                stateMachine.TransitionToState("TintingRed");
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : IState
{
    public AudioClip battlemusic;
    AudioSource audioData;

    private GameObject battlescreen;
    public BattleState(GameObject battlescreen)
    {
        this.battlescreen = battlescreen;
    }

    public void Enter()
    {
        battlescreen.SetActive(!battlescreen.activeSelf);
        audioData = GameObject.Find("BattleMusic").GetComponent<AudioSource>();
        audioData.Play(0);
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        audioData.Stop();
    }

    // Start is called before the first frame update
}

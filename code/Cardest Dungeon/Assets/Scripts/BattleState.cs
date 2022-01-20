using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : IState
{
    public AudioClip battlemusic;
    AudioSource audioData;

    private GameObject battlescreen;

    private CharacterStatus playerStatus;
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

    private void setBattleData(Collider2D other)
    {
        //Spieler Position wird gespeichert
        playerStatus.position = PlayerController.Current.transform.position;

        //Gegner Daten
        //CharacterStatus status = other.gameObject.GetComponent<EnemyStatus>().enemyStatus;
    }

}

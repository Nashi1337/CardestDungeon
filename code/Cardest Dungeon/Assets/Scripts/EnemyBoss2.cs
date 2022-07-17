using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

public class EnemyBoss2 : Enemy
{
    [SerializeField]
    private float restPhaseTime;
    [SerializeField]
    private float doubleShootSmallTimeDelta;
    [SerializeField]
    private float doubleShootBigTimeDelta;
    [SerializeField]
    private float doubleShoots_Amount;

    private float doubleShoot_shotBullets;
    private float doubleShoot_doubleShotsCounter;
    private float timer;
    private StateMachine statemachine;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        boss = true;
        
        statemachine = new StateMachine("InitialIdle", InitialIdle);
        statemachine.AddState("Idle", Idle);
        statemachine.AddState("DoubleShoot", DoubleShoot);

        statemachine.AddTransition("InitialIdle", "Idle", AnyToIdle);
        statemachine.AddTransition("DoubleShoot", "Idle" ,AnyToIdle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        statemachine.Run();
        
        if(timer > 0)
        {
            timer -= Time.fixedDeltaTime;
        }
    }

    #region StateMachine
    private void InitialIdle()
    {
        if (Vector2.Distance(PlayerController.Current.transform.position, transform.position) <= detectRange)
        {
            statemachine.TransitionToState("Idle");
        }
    }

    private void Idle()
    {
        
    }

    private void DoubleShoot()
    {
        if (timer <= 0)
        {
            
            Vector2 thisToPlayer = PlayerController.Current.transform.position - transform.position;
            float angleInDegree = Mathf.Atan2(thisToPlayer.y, thisToPlayer.x) * Mathf.Rad2Deg;
            GameObject fireball = Instantiate(fireballProjectile, transform.position, Quaternion.Euler(0, 0, angleInDegree));
            EvilProjectile evil = fireball.GetComponent<EvilProjectile>();
            
            evil.damage = enemyStats.Magic;
            evil.targetDir = thisToPlayer.normalized;
            doubleShoot_shotBullets++;
            
            if(doubleShoot_shotBullets == 2)
            {
                doubleShoot_shotBullets = 0;
                doubleShoot_doubleShotsCounter++;
                timer = doubleShootBigTimeDelta;

                if (doubleShoot_doubleShotsCounter == doubleShoots_Amount)
                {
                    doubleShoot_doubleShotsCounter = 0;
                    timer = 0;
                    statemachine.TransitionToState("Idle");
                }
            }
            else
            {
                timer = doubleShootSmallTimeDelta;
            }
        }
    }


    private void AnyToIdle()
    {
        StartCoroutine(SwitchToRandomAttackPhaseAfterWait());
    }

    private IEnumerator SwitchToRandomAttackPhaseAfterWait()
    {
        yield return new WaitForSeconds(restPhaseTime);
        switch (Random.Range(0, 1))
        {
            case 0:
                statemachine.TransitionToState("DoubleShoot");
                break;
            case 1:

                break;
            case 2:

                break;
        }
    }

    #endregion
}

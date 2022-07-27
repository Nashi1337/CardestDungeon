using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MethodStateMachine;

public class EnemyBoss2 : Enemy
{
    [System.Serializable]
    public struct MinionWave
    {
        public GameObject[] minions;
        public float waitingTime;
        public Vector3[] spawnPositions;
    }

    [SerializeField]
    private float restPhaseTime;
    [SerializeField]
    private float doubleShootSmallTimeDelta;
    [SerializeField]
    private float doubleShootBigTimeDelta;
    [SerializeField]
    private float doubleShoots_Amount;

    [SerializeField]
    private float spreadShoots_TimeDelta;
    [SerializeField]
    private float spreadShoots_Amount;
    [SerializeField]
    private float spreadShoots_angleDelta;
    [SerializeField]
    private int spreadShoots_BulletsPerShot;

    [SerializeField]
    private MinionWave[] minionwaves;

    [SerializeField]
    private Transform bulletSpawnPoint;

    private float doubleShoot_shotBullets;
    private float counter;
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
        statemachine.AddState("SpreadShoot", SpreadShoot);
        statemachine.AddState("SpawnMinions", SpawnMinions);
        statemachine.AddState("Dead", Dead);

        statemachine.AddTransition("InitialIdle", "Idle", InitialIdleToIdle);
        statemachine.AddTransition("DoubleShoot", "Idle" ,AnyToIdle);
        statemachine.AddTransition("SpreadShoot", "Idle", AnyToIdle);
        statemachine.AddTransition("SpawnMinions", "Idle", AnyToIdle);
        statemachine.AddTransition("Idle", "SpawnMinions", AnyToSpawnMinions);
        statemachine.AddTransition("InitialIdle", "SpawnMinions", AnyToSpawnMinions);

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
        if(timer <= 0)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    statemachine.TransitionToState("DoubleShoot");
                    break;
                case 1:
                    statemachine.TransitionToState("SpreadShoot");
                    break;
                case 2:
                    statemachine.TransitionToState("SpawnMinions");
                    break;
            }
        }
    }

    private void DoubleShoot()
    {
        if (timer <= 0)
        {
            Vector2 thisToPlayer = PlayerController.Current.transform.position - bulletSpawnPoint.position;
            float angleInDegree = Mathf.Atan2(thisToPlayer.y, thisToPlayer.x) * Mathf.Rad2Deg;
            GameObject fireball = Instantiate(fireballProjectile, bulletSpawnPoint.position, Quaternion.Euler(0, 0, angleInDegree));
            EvilProjectile evil = fireball.GetComponent<EvilProjectile>();
            
            evil.damage = enemyStats.Magic;
            evil.targetDir = thisToPlayer.normalized;
            evil.enemy = this;
            doubleShoot_shotBullets++;
            
            if(doubleShoot_shotBullets == 2)
            {
                doubleShoot_shotBullets = 0;
                counter++;
                timer = doubleShootBigTimeDelta;

                if (counter == doubleShoots_Amount)
                {
                    counter = 0;
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

    private void SpreadShoot()
    {
        if(timer <= 0)
        {
            Vector2 thisToPlayer = PlayerController.Current.transform.position - transform.position;
            float angleInDegree = Mathf.Atan2(thisToPlayer.y, thisToPlayer.x) * Mathf.Rad2Deg;

            for (int i = 0; i < spreadShoots_BulletsPerShot; i++)
            {
                float bulletAngle = angleInDegree - ((spreadShoots_BulletsPerShot - 1) / 2f) * spreadShoots_angleDelta + spreadShoots_angleDelta * i;
                GameObject fireball = Instantiate(fireballProjectile, bulletSpawnPoint.position, Quaternion.Euler(0, 0, bulletAngle));
                EvilProjectile evil = fireball.GetComponent<EvilProjectile>();
                evil.damage = enemyStats.Magic;
                evil.targetDir = new Vector2(Mathf.Cos(bulletAngle * Mathf.Deg2Rad), Mathf.Sin(bulletAngle * Mathf.Deg2Rad)).normalized;
                evil.enemy = this;
                fireball.GetComponent<AudioSource>().volume *= 0.25f;
            }
            
            counter++;

            if(counter == spreadShoots_Amount)
            {
                counter = 0;

                statemachine.TransitionToState("Idle");
            }
            else
            {
                timer = spreadShoots_TimeDelta;
            }
        }
    }
    
    private void SpawnMinions()
    {
        if(timer <= 0)
        {
            statemachine.TransitionToState("Idle");
        }
    }

    private void AnyToIdle()
    {
        timer = restPhaseTime;
    }
    private void AnyToSpawnMinions()
    {
        int waveIndex = Random.Range(0, minionwaves.Length);

        for (int i = 0; i < minionwaves[waveIndex].minions.Length; i++)
        {
            GameObject minion = Instantiate(minionwaves[waveIndex].minions[i], transform.position + minionwaves[waveIndex].spawnPositions[i], Quaternion.identity);
            minion.transform.SetParent(transform, true);
        }

        timer = minionwaves[waveIndex].waitingTime;
    }

    private void InitialIdleToIdle()
    {
        if (!MusicLooper.Instance.IsActive)
        {
            MusicLooper.Instance.ActivateLooper(1.2f, 58.78f);
            MusicLooper.Instance.GameMusic.clip = bossMusic;
            MusicLooper.Instance.GameMusic.Play();
        }

        timer = restPhaseTime;
    }
    private void Dead()
    {

    }

    #endregion

    public void KillstateMachine()
    {
        statemachine.TransitionToState("Dead");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    public Transform bulletSpawnPoint;
    [SerializeField] private Animator anim;
    private PlayerController player;
    private State currentState;
    private bool isDied = false;
    private Transform[] wayPoints;
    [SerializeField] private Canvas can;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] private Slider healthBar;
    private float health = 100;

    public void SetUpEnemy(PlayerController player, Transform[] wayPoints)
    {
        this.player = player;
        this.wayPoints = wayPoints;
        SetHealthBar();
        currentState = new Idle(this, agent, anim, player.transform);
    }

    private void Start()
    {
        //currentState = new Idle(this, agent, anim, player.transform);
    }

    private void Update()
    {
        if(!isDied && GameManager.Instance.IsGameRunning)
        {
            //if(currentState == null) { currentState = new Idle(this, agent, anim, player.transform); }
            currentState = currentState.Process();
        }
    }

    public Vector3 GetWayPointAt(int index)
    {
        return wayPoints[index].position;
    }

    public int GetWayPointsLength()
    {
        return wayPoints.Length;
    }

    public void PlayShootAudio()
    {
        enemyAudioSource.PlayOneShot(fireSound);
    }

    public void DealDamage(int damagePoint)
    {
        if (isDied) return;
        health -= damagePoint;
        health = Mathf.Max(0, health);
        healthBar.value = health;

        if (currentState.name != State.STATE.CounterAttack || currentState.name != State.STATE.Attack )
        {
            currentState = new CounterAttack(this, agent, anim, player.transform);
        }
        if (health <= 0)
        {
            currentState = new Die(this, agent, anim, player.transform);
        }
    }

    public void Die()
    {
        isDied = true;
        player.IncreamentKillCount();
        GameManager.Instance.EnemyDied(this);
        can.gameObject.SetActive(false);
        Destroy(gameObject, 2);
    }

    public void SetHealthBar()
    {
        healthBar.minValue = 0;
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public float GetHealth()
    {
        return health;
    }
}

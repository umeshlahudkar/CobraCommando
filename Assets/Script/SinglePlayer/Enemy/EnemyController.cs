using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.SinglePlayer
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        public Transform bulletSpawnPoint;
        [SerializeField] private Animator anim;
        private PlayerController player;
        private State currentState;
        private HealthController healthController;
        private bool isDied = false;
        private Transform[] wayPoints;
        [SerializeField] private Canvas can;
        [SerializeField] private AudioClip fireSound;
        [SerializeField] private AudioSource enemyAudioSource;


        public void SetUpEnemy(PlayerController player, Transform[] wayPoints)
        {
            this.player = player;
            this.wayPoints = wayPoints;
        }

        private void Start()
        {
            healthController = gameObject.GetComponent<HealthController>();
            currentState = new Idle(this, agent, anim, player.transform);
        }

        private void Update()
        {
            if(!isDied && GameManager.Instance.isLevelStart)
            {
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

        public void DealDamage(float weaponDamage)
        {
            if (isDied) return;
            healthController.DecreamentHealth(weaponDamage);
            if(currentState.name != State.STATE.CounterAttack || currentState.name != State.STATE.Attack )
            {
                currentState = new CounterAttack(this, agent, anim, player.transform);
            }
            if (healthController.GetHealth() <= 0)
            {
                currentState = new Die(this, agent, anim, player.transform);
            }
        }

        public void Die()
        {
            isDied = true;
            player.IncreamentKillCount();
            GameManager.Instance.EnemyDied();
            can.gameObject.SetActive(false);
            Destroy(gameObject, 2);
        }
    }
}

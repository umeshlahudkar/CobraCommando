using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.SinglePlayer
{
    public class CounterAttack : State
    {
        public float rotSpeed = 2.0f;
        private float timeBetweenShoot = 1.0f;
        private float elapcedTime = 0;
        //private EnemyController enemyController;
        public CounterAttack(EnemyController _enemy, NavMeshAgent _agent, Animator _anim, Transform _player) :
              base(_enemy, _agent, _anim, _player)
        {
            name = STATE.CounterAttack;
            agent.speed = 5;
            agent.isStopped = false;
        }

        public override void Enter()
        {
            anim.SetTrigger("isRunning");
            base.Enter();
        }

        public override void Update()
        {
            agent.SetDestination(player.position);

            if(agent.remainingDistance < 5)
            {
                nextState = new Attack(enemy, agent, anim, player);
                stage = EVENT.Exit;
            }
        }

        private void CanShoot()
        {
            Ray ray = new Ray(enemy.bulletSpawnPoint.position, enemy.bulletSpawnPoint.forward);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                PlayerController player = hit.collider.GetComponent<PlayerController>();
                if (player && player.GetHealth() > 0)
                {
                    enemy.PlayShootAudio();
                    player.DealDamage(10);
                }
            }
        }

        public override void Exit()
        {
            anim.ResetTrigger("isRunning");
            base.Exit();
        }
    }
}


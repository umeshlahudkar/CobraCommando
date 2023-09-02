using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.SinglePlayer
{
    public class Attack : State
    {
        public float rotSpeed = 2.0f;
        private float timeBetweenShoot = 1.0f;
        private float elapcedTime = 0;
        //private EnemyController enemyController;
        public Attack(EnemyController _enemy, NavMeshAgent _agent, Animator _anim, Transform _player) :
              base(_enemy, _agent, _anim, _player)
        {
            name = STATE.Attack;
        }
        
        public override void Enter()
        {
            //enemyController = enemy.GetComponent<EnemyController>();
            anim.SetTrigger("isShooting");
            agent.isStopped = true;
            base.Enter();
        }

        public override void Update()
        {
            Vector3 direction = player.position - enemy.transform.position;
            float angle = Vector3.Angle(direction, enemy.transform.position);
            direction.y = 0;

            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                         Time.deltaTime * rotSpeed);

            elapcedTime += Time.deltaTime;
            if(elapcedTime >= timeBetweenShoot)
            {
                CanShoot();
                elapcedTime = 0;
            }

           
            //if (!CanSeePlayer())
            //{
            //    nextState = new Idle(enemy, agent, anim, player);
            //    stage = EVENT.Exit;
            //}

            if (!CanAttackPlayer())
            {
                nextState = new Suspicious(enemy, agent, anim, player);
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
            anim.ResetTrigger("isShooting");
            base.Exit();
        }
    }
}

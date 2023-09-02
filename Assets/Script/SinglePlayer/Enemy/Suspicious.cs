using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.SinglePlayer
{
    public class Suspicious : State
    {
        private float timeElapced = 0;
        private float timeToMove = 3.0f;
        private bool flag = false;
        private Vector3 playerLastPosition;

        public Suspicious(EnemyController _enemy, NavMeshAgent _agent, Animator _anim, Transform _player) :
            base(_enemy, _agent, _anim, _player)
        {
            name = STATE.Suspicious;
            agent.speed = 1f;
            agent.isStopped = true;
        }

        public override void Enter()
        {
            anim.SetTrigger("isIdle");
            base.Enter();
        }

        public override void Update()
        {
            timeElapced += Time.deltaTime;
            if(timeElapced >= timeToMove)
            {
                if(!flag)
                {
                    flag = true;
                    playerLastPosition = player.position;
                    agent.isStopped = false;
                    anim.SetTrigger("isWalking");
                }

                agent.SetDestination(playerLastPosition);
                if (agent.remainingDistance < 1)
                {
                    nextState = new Idle(enemy, agent, anim, player);
                    stage = EVENT.Exit;
                }
                //} else if (!CanSeePlayer())
                //{
                //    nextState = new Persue(enemy, agent, anim, player);
                //    stage = EVENT.Exit;
                //}
            }

            if (CanSeePlayer())
            {
                nextState = new Persue(enemy, agent, anim, player);
                stage = EVENT.Exit;
            }
        }

        public override void Exit()
        {
            timeElapced = 0;
            playerLastPosition = Vector3.zero;
            anim.ResetTrigger("isIdle");
            anim.ResetTrigger("isWalking");
            base.Exit();
        }
    }
}

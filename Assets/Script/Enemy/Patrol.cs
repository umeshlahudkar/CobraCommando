using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    public class Patrol : State
    {
        private int currentIndex = -1;
        public Patrol(EnemyController _enemy, NavMeshAgent _agent, Animator _anim, Transform _player) :
             base(_enemy, _agent, _anim, _player)
        {
            name = STATE.Patrol;
            agent.speed = 1;
            agent.isStopped = false;
        }

        public override void Enter()
        {
            currentIndex = 0;
            anim.SetTrigger("isWalking");
            base.Enter();
        }

        public override void Update()
        {
            if(agent.remainingDistance < 1)
            {
                if(currentIndex >= enemy.GetWayPointsLength() - 1)
                {
                    currentIndex = 0;
                } 
                else
                {
                    currentIndex++;
                }

                agent.SetDestination(enemy.GetWayPointAt(currentIndex));
            }

            if (CanSeePlayer())
            {
                nextState = new Persue(enemy, agent, anim, player);
                stage = EVENT.Exit;
            }
        }

        public override void Exit()
        {
            anim.ResetTrigger("isWalking");
            base.Exit();
        }
    }

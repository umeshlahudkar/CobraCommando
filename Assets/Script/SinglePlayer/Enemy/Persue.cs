using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.SinglePlayer
{
    public class Persue : State
    {
        public Persue(EnemyController _enemy, NavMeshAgent _agent, Animator _anim, Transform _player) :
             base(_enemy, _agent, _anim, _player)
        {
            name = STATE.Persue;
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
            if (agent.hasPath)
            {
                if (CanAttackPlayer())
                {
                    nextState = new Attack(enemy, agent, anim, player);
                    stage = EVENT.Exit;
                }
                else if (!CanSeePlayer())
                {
                    nextState = new Suspicious(enemy, agent, anim, player);
                    stage = EVENT.Exit;
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

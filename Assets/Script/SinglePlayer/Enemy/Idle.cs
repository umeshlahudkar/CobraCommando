using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.SinglePlayer
{
    public class Idle : State
    {
        public Idle(EnemyController _enemy, NavMeshAgent _agent, Animator _anim, Transform _player) :
            base (_enemy, _agent, _anim, _player) {
            name = STATE.Idle;
        }

        public override void Enter()
        {
            anim.SetTrigger("isIdle");
            base.Enter();
        }

        public override void Update()
        {
            if(CanSeePlayer())
            {
                nextState = new Persue(enemy, agent, anim, player);
                stage = EVENT.Exit;
            }
            else  if(Random.Range(0,100) < 10)
            {
                nextState = new Patrol(enemy, agent, anim, player);
                stage = EVENT.Exit;
            }
        }

        public override void Exit()
        {
            anim.ResetTrigger("isIdle");
            base.Exit();
        }
    }

}
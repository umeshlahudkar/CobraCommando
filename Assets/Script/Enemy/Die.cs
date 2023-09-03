using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    public class Die : State
    {
        private float timeToVanish = 5f;
        private float elapcedTime = 0; 

        public Die(EnemyController _enemy, NavMeshAgent _agent, Animator _anim, Transform _player) :
            base(_enemy, _agent, _anim, _player)
        {
            name = STATE.Die;
            agent.speed = 0;
        }

        public override void Enter()
        {
            agent.enabled = false;
            anim.SetTrigger("isSleeping");
            base.Enter();
        }

        public override void Update()
        {
            elapcedTime += Time.deltaTime;
            if(elapcedTime > timeToVanish)
            {
                enemy.Die();
                Exit();
            }
        }

        public override void Exit()
        {
            anim.ResetTrigger("isSleeping");
            base.Exit();
        }
    }

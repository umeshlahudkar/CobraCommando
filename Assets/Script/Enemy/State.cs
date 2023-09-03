using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    public class State
    {
        public enum STATE
        {
            Idle, Patrol, Persue, Attack,Suspicious, Die, CounterAttack
        }

        public enum EVENT
        {
            Enter, Update, Exit
        }

        public STATE name;
        protected EVENT stage;
        protected EnemyController enemy;
        protected NavMeshAgent agent;
        protected Animator anim;
        protected Transform player;
        protected State nextState;

        private float visDistance = 10.0f;
        private float visAngle = 50.0f;
        private float shootDistance = 5.0f;

        public State(EnemyController _enemy, NavMeshAgent _agent, Animator _anim, Transform _player)
        {
            enemy = _enemy;
            agent = _agent;
            anim = _anim;
            player = _player;
            stage = EVENT.Enter;
        }

        public virtual void Enter() { stage = EVENT.Update; }
        public virtual void Update() { stage = EVENT.Update; }
        public virtual void Exit() { stage = EVENT.Exit; }

        public State Process()
        {
            if (stage == EVENT.Enter) Enter();
            if (stage == EVENT.Update) Update();
            if (stage == EVENT.Exit)
            {
                Exit();
                return nextState;
            }
            return this;
        }

        public bool CanSeePlayer()
        {
            Vector3 direction = player.position - enemy.transform.position;
            float angle = Vector3.Angle(direction, enemy.transform.forward);
            Ray ray = new Ray(enemy.transform.position, direction);
            //ray.origin = enemy.transform.position + enemy.transform.forward * 1 + new Vector3(0,1,0);
            RaycastHit hit;
            
            if(direction.magnitude < visDistance && angle < visAngle && Physics.Raycast(ray, out hit))
            {
                if(hit.collider.GetComponent<PlayerController>())
                {
                   return true;
                }
            }
            return false;
        }

        public bool CanAttackPlayer()
        {
            Vector3 direction = player.position - enemy.transform.position;
            Ray ray = new Ray(enemy.transform.position, direction);
            RaycastHit hit;


            if (direction.magnitude < shootDistance && Physics.Raycast(ray, out hit)) //&& CanShoot())
            {
                if(hit.collider.GetComponent<PlayerController>())
                {
                    return true;
                }
            }
            return false;
        }

        
    }

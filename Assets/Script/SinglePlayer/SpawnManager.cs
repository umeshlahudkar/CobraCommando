using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.SinglePlayer
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        [SerializeField] private Transform[] wayPoints1;
        [SerializeField] private Transform[] wayPoints2;
        [SerializeField] private Transform[] wayPoints3;

        private int index = 0;

        public Transform[] GetWayPoints ()
        {
            index = (index + 1) % 3;

            if (index == 0)
            {
                return wayPoints1;
            }

            if (index == 1)
            {
                return wayPoints2;
            }

            if (index == 2)
            {
                return wayPoints3;
            }
            return null;
        }
    }

}
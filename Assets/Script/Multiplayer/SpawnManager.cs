using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace FPS.Multiplayer
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private PlayerController playerControllerPrefab;

        private PlayerController playerController;
        private Slider healthBar;
        private DynamicJoystick rotateJoystick;
        private DynamicJoystick moveJoystick;
        private Button fireButton;

        public void SpawnPlayer(Slider healthBar, DynamicJoystick _rotateJoystick, DynamicJoystick _moveJoystick, Button _fireButton)
        {
            if (PhotonNetwork.IsConnected)
            {
                Transform trans = GetSpawnTransform();
                GameObject player = PhotonNetwork.Instantiate(playerControllerPrefab.name, trans.position, trans.rotation);
                playerController = player.GetComponent<PlayerController>();
                playerController.SetUpPlayer(healthBar, _rotateJoystick, _moveJoystick, _fireButton);
                this.healthBar = healthBar;
                rotateJoystick = _rotateJoystick;
                moveJoystick = _moveJoystick;
                fireButton = _fireButton;
            }
        }

        private Transform GetSpawnTransform()
        {
            int rand = Random.Range(0, spawnPoints.Length);
            return spawnPoints[rand];
        }

        public void RespawnPlayer()
        {
            GameManager.Instance.UpdateStatEventSend(PhotonNetwork.LocalPlayer.ActorNumber, 1, 1);
            SpawnPlayer(healthBar, rotateJoystick, moveJoystick, fireButton);
        }
    }
}

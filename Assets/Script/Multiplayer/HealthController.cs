using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FPS.Multiplayer
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private float health;
        private Slider healthBarSlider;


        public void SetupHealth(Slider healthdBar)
        {
            healthBarSlider = healthdBar;
            healthBarSlider.minValue = 0;
            healthBarSlider.maxValue = health;
            healthBarSlider.value = health;
        }

        public void DecreamentHealth(float damagePoint, int actor)
        {
            health -= damagePoint;
            health = Mathf.Max(0, health);
            UpdateHealthBar();

            if (health <= 0)
            {
                Photon.Pun.PhotonNetwork.Destroy(gameObject);
                SpawnManager.Instance.RespawnPlayer();
                GameManager.Instance.UpdateStatEventSend(actor, 0, 1);
            }
        }

        private void UpdateHealthBar()
        {
            healthBarSlider.value = health;
        }
    }
}

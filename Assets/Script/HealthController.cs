using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class HealthController : MonoBehaviour
    {
        [SerializeField] private float health;
        [SerializeField] private Slider healthBarSlider;

        private void Start()
        {
            healthBarSlider.minValue = 0;
            healthBarSlider.maxValue = health;
            healthBarSlider.value = health;
        }
      
        public void DecreamentHealth(float damagePoint)
        {
            health -= damagePoint;
            health = Mathf.Max(0, health);
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            healthBarSlider.value = health;
        }

        public float GetHealth()
        {
            return health;
        }

        public void SetHealth(float newHealth)
        {
            health = newHealth;
            healthBarSlider.minValue = 0;
            healthBarSlider.maxValue = health;
            healthBarSlider.value = health;
        }
    }

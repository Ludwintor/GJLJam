using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GJLJam
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Image healthBar;

        public void SetHealth(int health, int maxHealth)
        {
            healthBar.fillAmount = (float)health / maxHealth;
        }
    }
}

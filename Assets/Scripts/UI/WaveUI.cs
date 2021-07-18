using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GJLJam
{
    public class WaveUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI waveText;
        [SerializeField]
        private TextMeshProUGUI enemiesText;

        public void SetWave(int wave)
        {
            waveText.SetText("Wave " + wave.ToString());
        }

        public void SetEnemies(int count)
        {
            enemiesText.SetText("Enemies " + count.ToString());
        }
    }
}

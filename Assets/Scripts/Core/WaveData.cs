using UnityEngine;

namespace GJLJam
{
    [CreateAssetMenu(fileName = "New Wave", menuName = "Game/Wave")]
    public class WaveData : ScriptableObject
    {
        public GameObject[] enemies;
    }
}

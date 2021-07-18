using UnityEngine;

namespace GJLJam
{
    [CreateAssetMenu(fileName = "New Enemy Data", menuName = "Game/Data/Enemy")]
    public class EnemyDataObject : CharacterDataObject
    {
        public int Damage => damage;

        [SerializeField]
        private int damage;
    }
}
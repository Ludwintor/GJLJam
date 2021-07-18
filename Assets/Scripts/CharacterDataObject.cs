using UnityEngine;

namespace GJLJam
{
    public abstract class CharacterDataObject : ScriptableObject
    {
        public int Health => health;
        public float MovementSpeed => movementSpeed;

        [SerializeField]
        private float movementSpeed;
        [SerializeField]
        private int health;
    }
}
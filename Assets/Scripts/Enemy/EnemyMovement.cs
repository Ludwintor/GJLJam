using System.Collections;
using UnityEngine;

namespace GJLJam
{
    public abstract class EnemyMovement : MonoBehaviour
    {
        protected Enemy Enemy { get; private set; }

        private void Awake()
        {
            Enemy = GetComponent<Enemy>();
        }

        public abstract void Move(Vector2 targetPosition, float movementSpeed);
    }
}

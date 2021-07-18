using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public abstract class EnemyAttack : MonoBehaviour
    {
        protected Enemy Enemy { get; private set; }

        private void Awake()
        {
            Enemy = GetComponent<Enemy>();
            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}

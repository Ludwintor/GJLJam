using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    [Serializable]
    public abstract class Stats
    {
        public event Action OnDeath;
        public event Action OnDamageReceive;

        public int MaxHealth { get; private set; }
        public int Health { get; private set; }
        public float MovementSpeed { get; private set; }

        public Stats(int health, float movementSpeed)
        {
            MaxHealth = Health = health;
            MovementSpeed = movementSpeed;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            OnDamageReceive?.Invoke();
            if (Health <= 0)
                OnDeath?.Invoke();
        }
    }
}

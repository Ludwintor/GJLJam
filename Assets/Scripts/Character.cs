using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public abstract class Character : MonoBehaviour
    {
        protected Hurtbox Hurtbox { get; private set; }

        protected virtual void Awake()
        {
            Hurtbox = GetComponentInChildren<Hurtbox>();
        }

        protected virtual void OnEnable()
        {
            Hurtbox.OnHit += OnHit;

        }

        protected virtual void OnDisable()
        {
            Hurtbox.OnHit -= OnHit;
        }

        protected abstract void OnHit(int damage);
    }
}

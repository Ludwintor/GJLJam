using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Hurtbox : MonoBehaviour, IHittable
    {
        public HitCreature HitCreature => hitCreature;

        public event Action<int> OnHit;

        [SerializeField]
        private HitCreature hitCreature;

        public void Hit(int damage)
        {
            OnHit?.Invoke(damage);
        }
    }
}

using System;
using UnityEngine;

namespace GJLJam
{
    public class AttackCircle : MonoBehaviour
    {
        public event Action<IHittable> OnPlayerHit;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IHittable hittable))
            {
                Debug.Log(collision.name);
                if (hittable.HitCreature == HitCreature.Player)
                    OnPlayerHit?.Invoke(hittable);
            }
        }
    }
}

using System;
using UnityEngine;

namespace GJLJam
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class PlayerDetector : MonoBehaviour
    {
        public event Action<Player> PlayerInRange;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IHittable hittable) && hittable.HitCreature == HitCreature.Player)
            {
                Player player = collision.GetComponentInParent<Player>();

                PlayerInRange?.Invoke(player);
            }
 
        }
    }
}

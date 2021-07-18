using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class ContactDamage : MonoBehaviour
    {
        public int Damage { private get; set; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.Hit(Damage);
            }
        }
    }
}

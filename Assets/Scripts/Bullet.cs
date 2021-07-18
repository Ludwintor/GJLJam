using Ludwintor.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class Bullet : MonoBehaviour
    {
        public int Damage { get; private set; }

        private SpriteRenderer spriteRenderer;

        private Rigidbody2D rb;
        private int bounces;

        private ObjectPool BulletPool => Game.Pools.BulletPool;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        public void Setup(Vector2 start, Vector2 velocity, int damage, int bounces, Sprite sprite, bool scaleWithDamage, int mask)
        {
            float angle = LMath.VectorToAngle(velocity);
            transform.SetPositionAndRotation(start, Quaternion.Euler(0, 0, angle));
            float scale = scaleWithDamage ? 1f + Game.GameSettings.ScalePerDamage * damage : 1f;
            transform.localScale = new Vector3(scale, scale, scale);

            Damage = damage;
            this.bounces = bounces;
            spriteRenderer.sprite = sprite;

            gameObject.SetActive(true);
            rb.velocity = velocity;
            gameObject.layer = mask;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IHittable hittable))
            {
                hittable.Hit(Damage);
                BulletPool.Recycle(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (bounces > 0)
            {
                rb.velocity = Vector2.Reflect(rb.velocity, collision.GetContact(0).normal);
                bounces--;
            }
            else
            {
                BulletPool.Recycle(gameObject);
            }
        }
    }
}
